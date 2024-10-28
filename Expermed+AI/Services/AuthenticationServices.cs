using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Expermed_AI.Services
{
    public class AuthenticationServices
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticationServices> _logger;
        private readonly ExpermedBDAIContext _dbContext;

        public AuthenticationServices(IHttpContextAccessor httpContextAccessor, ILogger<AuthenticationServices> logger, ExpermedBDAIContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }




        public async Task<User> ValidateUser(string loginUsuario, string claveUsuario, string ipAddress)
        {
            if (string.IsNullOrEmpty(loginUsuario) || string.IsNullOrEmpty(claveUsuario))
            {
                throw new ArgumentException("El login y la clave no pueden estar vacíos.");
            }

            var parameterLoginUsuario = new SqlParameter("@UserLogin", loginUsuario);
            var parameterClaveUsuario = new SqlParameter("@UserPassword", claveUsuario);
            var parameterIpAddress = new SqlParameter("@IpAddress", ipAddress ?? (object)DBNull.Value);

            User user = null;

            try
            {
                using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (var command = new SqlCommand("SP_ValidateUserCredentials", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(parameterLoginUsuario);
                        command.Parameters.Add(parameterClaveUsuario);
                        command.Parameters.Add(parameterIpAddress);

                        using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false) && reader["success"].ToString() == "true")
                            {
                                user = new User
                                {
                                    UsersId = GetValueOrDefault<int>(reader, "users_id"),
                                    UserNames = GetValueOrDefault<string>(reader, "user_names"),
                                    UserSurnames = GetValueOrDefault<string>(reader, "user_surnames"),
                                    UserLogin = GetValueOrDefault<string>(reader, "user_login"),
                                    UserAddress = GetValueOrDefault<string>(reader, "user_address"),
                                    UserPhone = GetValueOrDefault<string>(reader, "user_phone"),
                                    UserEmail = GetValueOrDefault<string>(reader, "user_email"),
                                    UserProfile = new Profile
                                    {
                                        ProfileId = GetValueOrDefault<int>(reader, "profile_id"),
                                        ProfileDescription = GetValueOrDefault<string>(reader, "profile_name")
                                    },
                                    UserSpecialty = new Specialty
                                    {
                                        SpecialityId = GetValueOrDefault<int>(reader, "speciality_id"),
                                        SpecialityName = GetValueOrDefault<string>(reader, "speciality_name")
                                    },
                                    UserEstablishment = new Establishment
                                    {
                                        EstablishmentName = GetValueOrDefault<string>(reader, "establishment_name")
                                    },
                                    UserDigitalsignature = reader.IsDBNull(reader.GetOrdinal("user_digitalsignature")) ? null : (byte[])reader["user_digitalsignature"],
                                    UserQrcode = reader.IsDBNull(reader.GetOrdinal("user_qrcode")) ? null : (byte[])reader["user_qrcode"],
                                    UserProfilephoto = reader.IsDBNull(reader.GetOrdinal("user_profilephoto")) ? null : (byte[])reader["user_profilephoto"]
                                };

                                // Manejo de imágenes nulas
                                user.UserDigitalsignature ??= new byte[0];
                                user.UserQrcode ??= new byte[0];
                                user.UserProfilephoto ??= new byte[0];

                                // Guardar detalles de la sesión
                                var session = _httpContextAccessor.HttpContext.Session;
                                session.SetString("UsuarioNombre", user.UserNames);
                                session.SetInt32("UsuarioId", user.UsersId);
                                session.SetString("UsuarioApellido", user.UserSurnames);
                                session.SetString("UsuarioDescripcion", string.IsNullOrEmpty(user.UserProfile.ProfileDescription) ? "Default Description" : user.UserProfile.ProfileDescription);
                                session.SetString("UsuarioEspecialidad", user.UserSpecialty?.SpecialityName ?? "No specialty");
                                session.SetString("UsuarioEstablecimiento", user.UserEstablishment?.EstablishmentName ?? "No establishment");
                                session.SetString("UsuarioTelefono", user.UserPhone ?? "No phone number");
                                session.SetString("UsuarioEmail", user.UserEmail ?? "No email");
                                session.SetString("UsuarioFotoPerfil", ConvertToBase64(user.UserProfilephoto)); // Guardar imagen en Base64
                                session.SetString("UsuarioFirmaElectronica", ConvertToBase64(user.UserDigitalsignature)); // Firma en Base64
                                session.SetString("UsuarioCodigoQr", ConvertToBase64(user.UserQrcode)); // QR en Base64
                                session.SetString("UsuarioDireccion", user.UserAddress ?? "No address");
                                session.SetString("TokenId", reader["tokenId"].ToString()); // Token de sesión
                                session.SetString("TokenExpiration", reader["tokenExpiration"].ToString());

                                if (user.UserProfile != null)
                                {
                                    session.SetInt32("PerfilId", GetValueOrDefault<int>(reader, "profile_id"));
                                }
                            }
                            else
                            {
                                throw new UnauthorizedAccessException("Credenciales inválidas o usuario inactivo.");
                            }

                            if (user?.UserProfileid == 3) // Verificar si el perfil es de asistente (perfil_id = 3)
                            {
                                user.AssistantDoctorRelationshipAssistantUsers = await _dbContext.AssistantDoctorRelationships
                                    .Where(adr => adr.AssistantUserid == user.UsersId && adr.RelationshipStatus == 1)
                                    .ToListAsync(); // Esto ahora devuelve la colección completa
                            }

                            return user;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Ocurrió un error al validar el usuario.");
                throw new Exception("Ocurrió un error al validar el usuario.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al validar el usuario.");
                throw new Exception("Ocurrió un error inesperado al validar el usuario.", ex);
            }

            return null; // Si llega a este punto, el usuario es nulo
        }



        private T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
                return default(T);
            return (T)reader.GetValue(ordinal);
        }

        private string SafeGetString(SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }
        public string ConvertToBase64(byte[] imageBytes)
        {
            return Convert.ToBase64String(imageBytes);
        }


    }
}
