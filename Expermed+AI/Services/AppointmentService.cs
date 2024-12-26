using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Expermed_AI.Services
{
    public class AppointmentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;
        private readonly ExpermedBDAIContext _dbContext;

        public AppointmentService(IHttpContextAccessor httpContextAccessor, ILogger<AppointmentService> logger, ExpermedBDAIContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public class AvailableHour
        {
            public TimeSpan AvailableTime { get; set; }
        }

        public async Task<(List<Appointment>, string)> GetAppointments(int appointmentStatus, int userProfile, int userId, int? doctorId = null)
        {
            try
            {
                // Verificar que userProfile y userId no sean nulos o inválidos (mayores que 0)
                if (userProfile <= 0 || userId <= 0)
                {
                    return (new List<Appointment>(), "Parámetros de consulta no válidos.");
                }

                // Definir los parámetros para la consulta
                var parameters = new[]
                {
            new SqlParameter("@AppointmentStatus", appointmentStatus),  // Permitimos appointmentStatus en 0
            new SqlParameter("@UserProfile", userProfile),
            new SqlParameter("@UserId", userId),
            new SqlParameter("@DoctorId", (object)doctorId ?? DBNull.Value) // Si doctorId es nulo, usar DBNull.Value
        };

                // Ejecutar el procedimiento almacenado y obtener las citas
                var result = await _dbContext.Set<Appointment>()
                    .FromSqlRaw("EXEC sp_ListAllAppointment @AppointmentStatus, @UserProfile, @UserId, @DoctorId", parameters)
                    .ToListAsync(); // Convertir en lista de manera asincrónica

                // Verificar si no se encontraron citas
                if (result == null || !result.Any())
                {
                    return (new List<Appointment>(), "No se encontraron citas con el estado especificado.");
                }

                // Cargar manualmente las relaciones necesarias si no se cargan automáticamente
                foreach (var appointment in result)
                {
                    // Cargar la relación AppointmentPatient
                    await _dbContext.Entry(appointment).Reference(a => a.AppointmentPatient).LoadAsync();

                    // Cargar la relación AppointmentCreateuserNavigation y luego UserSpecialty
                    if (appointment.AppointmentCreateuserNavigation != null)
                    {
                        await _dbContext.Entry(appointment.AppointmentCreateuserNavigation).Reference(a => a.UserSpecialty).LoadAsync();
                    }
                }

                // Retornar las citas encontradas
                return (result, null);
            }
            catch (Exception ex)
            {
                // Registrar el error y devolver un mensaje
                Console.WriteLine($"Error: {ex.Message}\nStack: {ex.StackTrace}");
                // Puedes agregar un logger si es necesario
                // _logger.LogError(ex, "Error al obtener las citas.");

                // Retornar un mensaje de error y una lista vacía
                return (new List<Appointment>(), $"Error interno: {ex.Message}");
            }
        }


       

        //Obtener horas disponibles por medico
        public List<string> GetAvailableHours(int userId, DateTime date)
        {
            List<string> availableHours = new List<string>();

            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetAvailableHours", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = userId;
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date)).Value = date;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            availableHours.Add(reader["AvailableTime"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching available hours", ex);
                }
            }

            return availableHours;
        }

        //Obtener cita por ID
        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment appointment = null;

            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("sp_GetAppointmentById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@appointment_id", appointmentId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            appointment = new Appointment
                            {
                                AppointmentId = reader.GetInt32(reader.GetOrdinal("appointment_id")),
                                AppointmentCreatedate = reader.GetDateTime(reader.GetOrdinal("appointment_createdate")),
                                AppointmentModifydate = reader.GetDateTime(reader.GetOrdinal("appointment_modifydate")),
                                AppointmentCreateuser = reader.GetInt32(reader.GetOrdinal("appointment_createuser")),
                                AppointmentModifyuser = reader.GetInt32(reader.GetOrdinal("appointment_modifyuser")),
                                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("appointment_date")),
                                AppointmentHour = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("appointment_hour"))),
                                AppointmentPatientid = reader.GetInt32(reader.GetOrdinal("appointment_patientid")),
                                AppointmentStatus = reader.GetInt32(reader.GetOrdinal("appointment_status"))
                            };
                        }
                    }
                }
            }

            return appointment;
        }

        //CREAR UNA NUEVA CITA
        public async Task CreateAppointmentAsync(Appointment appointmentDto)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_CreateAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@appointment_createdate", DateTime.Now);
                    command.Parameters.AddWithValue("@appointment_modifydate", DateTime.Now);
                    command.Parameters.AddWithValue("@appointment_createuser", appointmentDto.AppointmentCreateuser);
                    command.Parameters.AddWithValue("@appointment_modifyuser", appointmentDto.AppointmentModifyuser);
                    command.Parameters.AddWithValue("@appointment_date", appointmentDto.AppointmentDate);
                    command.Parameters.AddWithValue("@appointment_hour", appointmentDto.AppointmentHour);
                    command.Parameters.AddWithValue("@appointment_patientid", appointmentDto.AppointmentPatientid);
                    command.Parameters.AddWithValue("@appointment_status", appointmentDto.AppointmentStatus);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException("Error al crear la cita: " + ex.Message);
                    }
                }
            }
        }

        //MODIFICAR UNA CITA
        public async Task ModifyAppointmentAsync(Appointment appointmentDto)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_ModifyAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros para el procedimiento almacenado
                    command.Parameters.AddWithValue("@appointment_id", appointmentDto.AppointmentId);
                    command.Parameters.AddWithValue("@appointment_modifydate", DateTime.Now);
                    command.Parameters.AddWithValue("@appointment_modifyuser", appointmentDto.AppointmentModifyuser);
                    command.Parameters.AddWithValue("@appointment_date", appointmentDto.AppointmentDate);
                    command.Parameters.AddWithValue("@appointment_hour", appointmentDto.AppointmentHour);
                    command.Parameters.AddWithValue("@appointment_patientid", appointmentDto.AppointmentPatientid);
                    command.Parameters.AddWithValue("@appointment_status", appointmentDto.AppointmentStatus);

                    try
                    {
                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        // Manejo de errores, si ocurre algún problema al ejecutar el SP
                        throw new ApplicationException("Error al modificar la cita: " + ex.Message);
                    }
                }
            }
        }


        //Cancelar una Cita
        public void DesactivateAppointment(int appointmentId, int modifiedBy)
        {
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Crear el comando para ejecutar el SP
                    using (SqlCommand cmd = new SqlCommand("sp_DesactiveAppointment", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        cmd.Parameters.Add(new SqlParameter("@AppointmentId", SqlDbType.Int)).Value = appointmentId;
                        cmd.Parameters.Add(new SqlParameter("@ModifiedBy", SqlDbType.Int)).Value = modifiedBy;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine($"Error al desactivar la cita: {ex.Message}");
                }
            }
        }

        //Obtener cita por dia 
        public async Task<DataTable> GetAppointmentsForToday(int userProfile, int userId)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    // Abrimos la conexión a la base de datos
                    await connection.OpenAsync();

                    // Creamos el comando para ejecutar el SP
                    using (var command = new SqlCommand("sp_GetAppointmentsForToday", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@user_profile", SqlDbType.Int) { Value = userProfile });
                        command.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int) { Value = userId });

                        // Ejecutamos el SP y obtenemos el resultado
                        var dataTable = new DataTable();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }

                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw new Exception("Error al obtener citas", ex);
                }
            }
        }

    }
}
