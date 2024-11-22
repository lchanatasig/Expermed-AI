using Expermed_AI.Models;
using Microsoft.EntityFrameworkCore;

namespace Expermed_AI.Services
{
    public class SelectsService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsersService> _logger;
        private readonly ExpermedBDAIContext _dbContext;

        public SelectsService(IHttpContextAccessor httpContextAccessor, ILogger<UsersService> logger, ExpermedBDAIContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        // Método para obtener todos los perfiles
        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllProfiles
                var profiles = await _dbContext.Profiles
                    .FromSqlRaw("EXEC sp_ListAllProfiles")
                    .ToListAsync();

                return profiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los perfiles.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }
         
        // Método para obtener todas las especialidades
        public async Task<List<Specialty>> GetAllSpecialtiesAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var specialties = await _dbContext.Specialties
                    .FromSqlRaw("EXEC sp_ListAllSpecialities")
                    .ToListAsync();

                return specialties;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las especialidades.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }

        // Método para obtener todas los establecimientos de la base de datos 
        public async Task<List<Establishment>> GetAllEstablishmentsAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var establishments = await _dbContext.Establishments
                    .FromSqlRaw("EXEC sp_ListAllEstablishment")
                    .ToListAsync();

                return establishments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los establecimientos.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }


        // Método para obtener todos los Medicos
        public async Task<List<User>> GetAllMedicsAsync()
       {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var medics = await _dbContext.Users
                    .FromSqlRaw("EXEC sp_ListAllMedics")
                    .ToListAsync();

                return medics;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las Medicos.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }


    }
}
