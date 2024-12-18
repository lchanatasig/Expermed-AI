using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using System.Text.Json;

namespace Expermed_AI.Services
{
    public class UsersService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsersService> _logger;
        private readonly ExpermedBDAIContext _dbContext;

        public UsersService(IHttpContextAccessor httpContextAccessor, ILogger<UsersService> logger, ExpermedBDAIContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // Método para obtener todos los usuarios
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllUser y obtiene la lista de usuarios
                var users = await _dbContext.Users
                    .FromSqlRaw("EXEC sp_ListAllUser")  // Llama al procedimiento almacenado
                    .ToListAsync();  // Convierte a lista

                // Incluye la relación UserEstablishment después de obtener los usuarios
                foreach (var user in users)
                {
                    await _dbContext.Entry(user)
                        .Reference(u => u.UserEstablishment)  // Carga la relación
                        .LoadAsync();
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios.");
                throw;  // O manejar el error de forma más específica si es necesario
            }
        }


        // Método para obtener un usuario por su ID
        public async Task<UserWithDetails> GetUserDetailsAsync(int userId)
        {
            UserWithDetails userDetails = null;
            List<DoctorDto> doctors = new List<DoctorDto>();

            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    // Abrir la conexión
                    await connection.OpenAsync();

                    // Configurar el comando para ejecutar el procedimiento almacenado
                    using (var command = new SqlCommand("sp_ListUserById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        // Ejecutar el comando y leer los resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Mapear los datos del usuario
                                userDetails = new UserWithDetails
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("users_id")),
                                    DocumentNumber = reader.GetString(reader.GetOrdinal("user_document_number")),
                                    Names = reader.GetString(reader.GetOrdinal("user_names")),
                                    Surnames = reader.GetString(reader.GetOrdinal("user_surnames")),
                                    Phone = reader.GetString(reader.GetOrdinal("user_phone")),
                                    Email = reader.GetString(reader.GetOrdinal("user_email")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("user_creationdate")),
                                    ModificationDate = reader.IsDBNull(reader.GetOrdinal("user_modificationdate"))
                                                       ? (DateTime?)null
                                                       : reader.GetDateTime(reader.GetOrdinal("user_modificationdate")),
                                    Address = reader.GetString(reader.GetOrdinal("user_address")),
                                    ProfilePhoto = reader["user_profilephoto"] != DBNull.Value ? (byte[])reader["user_profilephoto"] : null,
                                    ProfilePhoto64 = reader["user_profilephoto"] != DBNull.Value
                                        ? "data:image/png;base64," + Convert.ToBase64String((byte[])reader["user_profilephoto"])
                                        : "assets/images/users/avatar-1.jpg", // Ruta por defecto, // Ruta por defecto
                                    SenecytCode = reader.IsDBNull(reader.GetOrdinal("user_senecytcode")) ? null : reader.GetString(reader.GetOrdinal("user_senecytcode")),
                                    XKeyTaxo = reader.IsDBNull(reader.GetOrdinal("user_xkeytaxo")) ? null : reader.GetString(reader.GetOrdinal("user_xkeytaxo")),
                                    XPassTaxo = reader.IsDBNull(reader.GetOrdinal("user_xpasstaxo")) ? null : reader.GetString(reader.GetOrdinal("user_xpasstaxo")),
                                    SequentialBilling = reader.GetInt32(reader.GetOrdinal("user_sequential_billing")),
                                    Login = reader.GetString(reader.GetOrdinal("user_login")),
                                    Status = reader.GetInt32(reader.GetOrdinal("user_status")),
                                    profileSelect = reader.GetInt32(reader.GetOrdinal("user_profileid")),
                                    UserCountryid = reader.GetInt32(reader.GetOrdinal("user_countryid")),
                                    UserDescription = reader.GetString(reader.GetOrdinal("user_description")) ?? "Sin especificar",                                    ProfileName = reader.GetString(reader.GetOrdinal("profile_name")),
                                    EstablishmentName = reader.GetString(reader.GetOrdinal("establishment_name")),
                                    SpecialtyName = reader.GetString(reader.GetOrdinal("speciality_name")),
                                    CountryName = reader.GetString(reader.GetOrdinal("country_name")),
                                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("start_time")),
                                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("end_time")),
                                    AppointmentInterval = reader.GetInt32(reader.GetOrdinal("appointment_interval")),
                                    WorkDays = reader.GetString(reader.GetOrdinal("works_days")) ?? "Sin días especificados",
                                    Doctors = doctors


                                };
                            }

                            // Si es un asistente (profile_id = 3), obtener los médicos relacionados
                            if (userDetails != null && userDetails.ProfileName == "Asistente")
                            {
                                // Reabrir el lector para obtener los médicos relacionados
                                if (await reader.NextResultAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        doctors.Add(new DoctorDto
                                        {
                                            DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                            DoctorNames = reader.GetString(reader.GetOrdinal("doctor_names")),
                                            DoctorSurnames = reader.GetString(reader.GetOrdinal("doctor_surnames")),
                                            DoctorSpecialtyId = reader.GetInt32(reader.GetOrdinal("doctor_specialtyid")),
                                            DoctorSpecialtyName = reader.GetString(reader.GetOrdinal("doctor_specialty_name"))
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores, loguear el error si es necesario
                    throw new Exception("Error al obtener los detalles del usuario", ex);
                }
            }

            return userDetails;
        }


        // Método para activar o desactivar al usuario
        public async Task<(bool success, string message)> DesactiveOrActiveUserAsync(int userId, int status)
        {
            try
            {
                // Crear la conexión
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    // Crear el comando para ejecutar el SP
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SP_DesactiveOrActiveUser";
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros del procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                        command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = status });

                        // Ejecutar el comando y obtener la respuesta
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                string success = reader["success"].ToString();
                                string message = reader["message"].ToString();

                                if (success == "true")
                                {
                                    return (true, message);
                                }
                                else
                                {
                                    return (false, message);
                                }
                            }
                            else
                            {
                                return (false, "No se recibió respuesta válida del procedimiento.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar/desactivar el usuario.");
                return (false, "Ocurrió un error al procesar la solicitud.");
            }

        }


        //Metodo para  insertar un nuevo usuario

        public async Task<int> CreateUserAsync(UserViewModel usuario, List<int>? associatedDoctorIds = null, List<string>? workDays = null)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("SP_CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros de datos personales
                    command.Parameters.Add(new SqlParameter("@ProfilePhoto", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserProfilephoto ?? (object)DBNull.Value
                    });
                    command.Parameters.AddWithValue("@ProfilePhoto64", usuario.UserPrfilephoto64 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProfileId", usuario.UserProfileid);
                    command.Parameters.AddWithValue("@DocumentNumber", usuario.UserDocumentNumber);
                    command.Parameters.AddWithValue("@Names", usuario.UserNames);
                    command.Parameters.AddWithValue("@Surnames", usuario.UserSurnames);
                    command.Parameters.AddWithValue("@Address", usuario.UserAddress);
                    command.Parameters.AddWithValue("@SenecytCode", usuario.UserSenecytcode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", usuario.UserPhone);
                    command.Parameters.AddWithValue("@Email", usuario.UserEmail);
                    command.Parameters.AddWithValue("@SpecialtyId", usuario.UserSpecialtyid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", usuario.UserCountryid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Login", usuario.UserLogin);
                    command.Parameters.AddWithValue("@Password", usuario.UserPassword);

                    // Agregar los parámetros de Taxo
                    command.Parameters.AddWithValue("@EstablishmentId", usuario.UserEstablishmentid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@VatPercentageId", usuario.UserVatpercentageid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XKeyTaxo", usuario.UserXkeytaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XPassTaxo", usuario.UserXpasstaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SequentialBilling", usuario.UserSequentialBilling ?? (object)DBNull.Value);
                    command.Parameters.Add(new SqlParameter("@DigitalSignature", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserDigitalsignature ?? (object)DBNull.Value
                    });

                    // Agregar los médicos asociados
                    if (associatedDoctorIds != null && associatedDoctorIds.Any())
                    {
                        string doctorIds = string.Join(",", associatedDoctorIds);
                        command.Parameters.AddWithValue("@DoctorIds", doctorIds);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DoctorIds", DBNull.Value);
                    }

                    // Agregar los días de trabajo
                    if (workDays != null && workDays.Any())
                    {
                        string workDaysStr = string.Join(",", workDays);
                        command.Parameters.AddWithValue("@WorkDays", workDaysStr);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@WorkDays", DBNull.Value);
                    }

                    // Otros parámetros
                    command.Parameters.AddWithValue("@StartTime", usuario.StartTime == TimeOnly.MinValue ? DateTime.MinValue : DateTime.Today.Add(usuario.StartTime.ToTimeSpan()));
                    command.Parameters.AddWithValue("@EndTime", usuario.EndTime == TimeOnly.MinValue ? DateTime.MinValue : DateTime.Today.Add(usuario.EndTime.ToTimeSpan()));
                    command.Parameters.AddWithValue("@AppointmentInterval", usuario.AppointmentInterval);
                    command.Parameters.AddWithValue("@Description", usuario.UserDescription ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();

                        // Ejecutar y leer el resultado JSON
                        string jsonResult = null;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jsonResult = reader.GetString(0);
                            }
                        }

                        if (string.IsNullOrEmpty(jsonResult))
                        {
                            throw new Exception("Error inesperado: No se obtuvo ningún resultado del procedimiento almacenado.");
                        }

                        // Deserializar el resultado JSON
                        using (JsonDocument document = JsonDocument.Parse(jsonResult))
                        {
                            var root = document.RootElement;

                            // Validar el resultado
                            if (root.TryGetProperty("success", out var success) && success.GetInt32() == 1)
                            {
                                if (root.TryGetProperty("userId", out var userId))
                                {
                                    return userId.GetInt32();
                                }
                                else
                                {
                                    throw new Exception("El campo 'userId' no se encuentra en el resultado.");
                                }
                            }
                            else
                            {
                                string errorMessage = root.TryGetProperty("message", out var message)
                                    ? message.GetString()
                                    : "Error al crear el usuario.";
                                throw new Exception(errorMessage);
                            }
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }
        }



        //Metodo para actualizar un usuario


        public async Task UpdateUserAsync(int userId, UserViewModel usuario, List<int>? associatedDoctorIds = null, List<string>? workDays = null)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("SP_UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar el parámetro del ID del usuario
                    command.Parameters.AddWithValue("@UserId", userId);

                    // Agregar los parámetros de datos personales
                    command.Parameters.Add(new SqlParameter("@ProfilePhoto", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserProfilephoto ?? (object)DBNull.Value
                    });
                    command.Parameters.AddWithValue("@ProfilePhoto64", usuario.UserPrfilephoto64 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProfileId", usuario.UserProfileid);
                    command.Parameters.AddWithValue("@DocumentNumber", usuario.UserDocumentNumber);
                    command.Parameters.AddWithValue("@Names", usuario.UserNames);
                    command.Parameters.AddWithValue("@Surnames", usuario.UserSurnames);
                    command.Parameters.AddWithValue("@Address", usuario.UserAddress);
                    command.Parameters.AddWithValue("@SenecytCode", usuario.UserSenecytcode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", usuario.UserPhone);
                    command.Parameters.AddWithValue("@Email", usuario.UserEmail);
                    command.Parameters.AddWithValue("@SpecialtyId", usuario.UserSpecialtyid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", usuario.UserCountryid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Login", usuario.UserLogin);
                    command.Parameters.AddWithValue("@Password", usuario.UserPassword);

                    // Agregar los parámetros de Taxo
                    command.Parameters.AddWithValue("@EstablishmentId", usuario.UserEstablishmentid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@VatPercentageId", usuario.UserVatpercentageid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XKeyTaxo", usuario.UserXkeytaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XPassTaxo", usuario.UserXpasstaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SequentialBilling", usuario.UserSequentialBilling ?? (object)DBNull.Value);
                    command.Parameters.Add(new SqlParameter("@DigitalSignature", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserDigitalsignature ?? (object)DBNull.Value
                    });

                    // Agregar los médicos asociados
                    if (associatedDoctorIds != null && associatedDoctorIds.Any())
                    {
                        string doctorIds = string.Join(",", associatedDoctorIds);
                        command.Parameters.AddWithValue("@DoctorIds", doctorIds);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DoctorIds", DBNull.Value);
                    }

                    // Agregar los días de trabajo
                    if (workDays != null && workDays.Any())
                    {
                        string workDaysStr = string.Join(",", workDays);
                        command.Parameters.AddWithValue("@WorkDays", workDaysStr);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@WorkDays", DBNull.Value);
                    }

                    // Otros parámetros
                    command.Parameters.AddWithValue("@StartTime", usuario.StartTime == TimeOnly.MinValue ? DBNull.Value : DateTime.Today.Add(usuario.StartTime.ToTimeSpan()));
                    command.Parameters.AddWithValue("@EndTime", usuario.EndTime == TimeOnly.MinValue ? DBNull.Value : DateTime.Today.Add(usuario.EndTime.ToTimeSpan()));
                    command.Parameters.AddWithValue("@AppointmentInterval", usuario.AppointmentInterval);
                    command.Parameters.AddWithValue("@Description", usuario.UserDescription ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();

                        // Ejecutar y leer el resultado JSON
                        string jsonResult = null;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jsonResult = reader.GetString(0);
                            }
                        }

                        if (string.IsNullOrEmpty(jsonResult))
                        {
                            throw new Exception("Error inesperado: No se obtuvo ningún resultado del procedimiento almacenado.");
                        }

                        // Deserializar el resultado JSON
                        using (JsonDocument document = JsonDocument.Parse(jsonResult))
                        {
                            var root = document.RootElement;

                            // Validar el resultado
                            if (root.TryGetProperty("success", out var success) && success.GetInt32() == 1)
                            {
                                // Actualización exitosa
                                return;
                            }
                            else
                            {
                                string errorMessage = root.TryGetProperty("message", out var message)
                                    ? message.GetString()
                                    : "Error al actualizar el usuario.";
                                throw new Exception(errorMessage);
                            }
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }
        }


    }
}
