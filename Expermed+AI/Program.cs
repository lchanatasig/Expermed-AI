using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expermed_AI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    // Agregar convertidores personalizados para TimeOnly
                    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                });

            builder.Services.AddDbContext<ExpermedBDAIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // Registrar IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Registrar servicios personalizados
            builder.Services.AddScoped<AuthenticationServices>();
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<SelectsService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<AppointmentService>();
            builder.Services.AddHttpClient(); // Registrar HttpClient
            builder.Services.AddLogging(); // Asegúrate de que la inyección de dependencias de logging esté habilitada

            // Agregar servicio de sesión
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true; // La cookie solo se puede acceder a través de HTTP
                options.Cookie.IsEssential = true; // Hace que la cookie sea esencial
                options.Cookie.SameSite = SameSiteMode.None; // Permite el uso de la cookie en solicitudes de diferentes sitios
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Asegúrate de que la cookie sea segura
            });

            builder.Services.AddLogging();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Usar el middleware de sesión antes de cualquier otro middleware que lo requiera
            app.UseSession(); // Asegúrate de que esto esté aquí

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=SignInBasic}/{id?}");
            });

            app.Run();
        }
    }

    // Conversor JSON para TimeOnly
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string TimeFormat = "HH:mm";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!, TimeFormat);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat));
        }
    }
}
