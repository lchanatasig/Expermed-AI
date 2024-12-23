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



        public bool CreateAppointment(Appointment model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateAppointment", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pasar parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@appointment_createdate", model.AppointmentCreatedate);
                        cmd.Parameters.AddWithValue("@appointment_modifydate", model.AppointmentModifydate);
                        cmd.Parameters.AddWithValue("@appointment_createuser", model.AppointmentCreateuser);
                        cmd.Parameters.AddWithValue("@appointment_modifyuser", model.AppointmentModifyuser);
                        cmd.Parameters.AddWithValue("@appointment_date", model.AppointmentDate);
                        cmd.Parameters.AddWithValue("@appointment_hour", model.AppointmentHour);
                        cmd.Parameters.AddWithValue("@appointment_patientid", model.AppointmentPatientid);
                        cmd.Parameters.AddWithValue("@appointment_status", model.AppointmentStatus);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                return true; // Cita creada exitosamente
            }
            catch (Exception ex)
            {
                // En caso de error, se puede registrar o manejar el error aquí
                return false; // Retorna falso si ocurre un error
            }
        }

    }
}
