using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
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


        //Metodo para  insertar un nuevo usuario



    public async Task<int> CreateUserAsync(UserViewModel usuario, int? idMedicoAsociado = null)
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
                if (usuario.AssistantDoctorRelationshipAssistantUsers != null && usuario.AssistantDoctorRelationshipAssistantUsers.Any())
                {
                    string doctorIds = string.Join(",", usuario.AssistantDoctorRelationshipAssistantUsers.Select(relationship => relationship.DoctorUserid));
                    command.Parameters.AddWithValue("@DoctorIds", doctorIds);
                }
                else
                {
                    command.Parameters.AddWithValue("@DoctorIds", DBNull.Value);
                }
                    command.Parameters.AddWithValue("@StartTime", usuario.StartTime == TimeOnly.MinValue ? DateTime.MinValue : DateTime.Today.Add(usuario.StartTime.ToTimeSpan()));

                    command.Parameters.AddWithValue("@EndTime", usuario.EndTime == TimeOnly.MinValue ? DateTime.MinValue : DateTime.Today.Add(usuario.EndTime.ToTimeSpan()));
                  
                    command.Parameters.AddWithValue("@AppointmentInterval", usuario.AppointmentInterval);

                    command.Parameters.AddWithValue("@WorkDays", usuario.WorksDays);


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



}
}
