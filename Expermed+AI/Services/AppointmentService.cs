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



        public async Task<DataTable> ListAllAppointmentsAsync(int appointmentStatus, int userProfile, int userId, int? doctorId = null)
        {
            DataTable dtAppointments = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_ListAllAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AppointmentStatus", appointmentStatus);
                        command.Parameters.AddWithValue("@UserProfile", userProfile);
                        command.Parameters.AddWithValue("@UserId", userId);

                        if (doctorId.HasValue)
                        {
                            command.Parameters.AddWithValue("@DoctorId", doctorId.Value);
                        }

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(dtAppointments);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions, e.g., connection issues, syntax errors
                Console.WriteLine("SQL Error: " + sqlEx.Message);
                // Optionally, log the error
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("Error: " + ex.Message);
                // Optionally, log the error
            }

            return dtAppointments;
        }



    }
}
