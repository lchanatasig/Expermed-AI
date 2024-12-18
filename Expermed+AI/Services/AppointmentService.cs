using Expermed_AI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Expermed_AI.Services
{
    public class AppointmentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsersService> _logger;
        private readonly ExpermedBDAIContext _dbContext;
       
        //Obtener las horas disponibles
        public async Task<List<TimeSpan>> GetAvailableHoursAsync(int userId, DateTime date)
        {
            var availableHours = new List<TimeSpan>();

            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_GetAvailableHours", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Date", date);

                    try
                    {
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Obtener cada hora disponible como TimeSpan
                                if (!reader.IsDBNull(0))
                                {
                                    availableHours.Add(reader.GetTimeSpan(0));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        throw new Exception($"Error al obtener horas disponibles: {ex.Message}", ex);
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

            return availableHours;
        }
    

    }
}
