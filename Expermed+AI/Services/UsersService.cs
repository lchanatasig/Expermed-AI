using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

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
                // Ejecuta el procedimiento almacenado sp_ListAllUser
                var users = await _dbContext.Users
                    .FromSqlRaw("EXEC sp_ListAllUser")
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios.");
                throw;  // O manejar el error de forma más específica si es necesario
            }
        }




        // Método para obtener un usuario por su ID
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var users = await _dbContext.Users
                    .FromSqlRaw("EXEC sp_ListUserById @user_id = {0}", userId)
                    .ToListAsync();

                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario por ID.");
                throw;
            }
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


        public async Task<int> CreateUserAsync(User usuario)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("SP_CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros requeridos
                    command.Parameters.AddWithValue("@DocumentNumber", usuario.UserDocumentNumber);
                    command.Parameters.AddWithValue("@Names", usuario.UserNames);
                    command.Parameters.AddWithValue("@Surnames", usuario.UserSurnames);
                    command.Parameters.AddWithValue("@Phone", usuario.UserPhone);
                    command.Parameters.AddWithValue("@Email", usuario.UserEmail);
                    command.Parameters.AddWithValue("@Address", usuario.UserAddress);
                    command.Parameters.AddWithValue("@Login", usuario.UserLogin);
                    command.Parameters.AddWithValue("@Password", usuario.UserPassword);
                    command.Parameters.AddWithValue("@ProfileId", usuario.UserProfileid);

                    // Parámetros opcionales
                    command.Parameters.Add(new SqlParameter("@DigitalSignature", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserDigitalsignature ?? (object)DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter("@QRCode", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserQrcode ?? (object)DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter("@ProfilePhoto", SqlDbType.VarBinary)
                    {
                        Value = usuario.UserProfilephoto ?? (object)DBNull.Value
                    });
                    command.Parameters.AddWithValue("@ProfilePhoto64", usuario.UserPrfilephoto64 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SenecytCode", usuario.UserSenecytcode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XKeyTaxo", usuario.UserXkeytaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@XPassTaxo", usuario.UserXpasstaxo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SequentialBilling", usuario.UserSequentialBilling ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EstablishmentId", usuario.UserEstablishmentid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SpecialtyId", usuario.UserSpecialtyid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", usuario.UserCountryid ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DoctorIds", usuario. ?? (object)DBNull.Value);

                    // Parámetros del horario
                    command.Parameters.AddWithValue("@StartTime", usuario.user ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndTime", usuario.EndTime ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@WorkDays", usuario.WorkDays ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AppointmentInterval", usuario.AppointmentInterval ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Parsear el resultado JSON devuelto
                                var result = new
                                {
                                    Success = reader.GetBoolean(reader.GetOrdinal("success")),
                                    Message = reader.GetString(reader.GetOrdinal("message")),
                                    UserId = reader.IsDBNull(reader.GetOrdinal("userId"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("userId"))
                                };

                                if (!result.Success)
                                {
                                    throw new Exception(result.Message);
                                }

                                return result.UserId ?? 0;
                            }
                        }

                        throw new Exception("No se recibió una respuesta válida del procedimiento almacenado.");
                    }
                    catch (SqlException ex)
                    {
                        // Manejo de excepciones personalizadas
                        throw new Exception($"Error al ejecutar SP_CreateUser: {ex.Message}", ex);
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
