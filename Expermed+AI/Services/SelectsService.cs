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

        // Método para obtener todas las Nacionalidades
        public async Task<List<Country>> GetAllCountriesAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var countries = await _dbContext.Countries
                    .FromSqlRaw("EXEC sp_ListAllCountries")
                    .ToListAsync();

                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las nacionalidades.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }
        // Método para obtener todas los porcentajes de iva
        public async Task<List<VatBilling>> GetAllVatPercentageAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var pencentage = await _dbContext.VatBillings
                    .FromSqlRaw("EXEC sp_ListAllPercentage")
                    .ToListAsync();

                return pencentage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los porcentajes.");
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
        public async Task<List<Province>> GetAllProvinceAsync()
        {
            try
            {
                // Ejecuta el procedimiento almacenado sp_ListAllSpecialities
                var province = await _dbContext.Provinces
                    .FromSqlRaw("EXEC sp_ListAllProvinces")
                    .ToListAsync();

                return province;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las Provincias.");
                throw; // O manejar el error de forma más específica si es necesario
            }
        }



        //Metodo para obtener los tipos de genero de la tabla catalogo
        public async Task<List<Catalog>> GetGenderTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "GENERO")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de sangre de la tabla catalogo
        public async Task<List<Catalog>> GetBloodTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "TIPO DE SANGRE")
                .ToListAsync();
        }

        //Metodo para obtener los tipos de documentos de la tabla catalogo
        public async Task<List<Catalog>> GetDocumentTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "TIPO DE DOCUMENTO")
                .ToListAsync();
        }

        //Metodo para obtener los tipos de estado civil de la tabla catalogo
        public async Task<List<Catalog>> GetCivilTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "ESTADO CIVIL")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de formacion de la tabla catalogo
        public async Task<List<Catalog>> GetProfessionaltrainingTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "FORMACION PROFESIONAL")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de seguros de salud de la tabla catalogo
        public async Task<List<Catalog>> GetSureHealtTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "SEGUROS DE SALUD")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de Parentesco de la tabla catalogo
        public async Task<List<Catalog>> GetRelationshipTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "PARENTESCO")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de Antedecentes familiares de la tabla catalogo
        public async Task<List<Catalog>> GetFamiliarTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "ANTECEDENTES FAMILIARES")
                .ToListAsync();
        }

        //Metodo para obtener los tipos de Alergias de la tabla catalogo
        public async Task<List<Catalog>> GetAllergiesTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "ALERGIAS")
                .ToListAsync();
        }
        //Metodo para obtener los tipos de Cirugias de la tabla catalogo
        public async Task<List<Catalog>> GetSurgeriesTypeAsync()
        {
            // Asumiendo que _dbContext es tu contexto de base de datos inyectado
            return await _dbContext.Catalogs
                .Where(c => c.CatalogCategory == "CIRUGIAS")
                .ToListAsync();
        }

        //Metodo para obtener los tipos de provincias de la tabla catalogo

    }
}
