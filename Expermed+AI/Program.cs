using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Expermed_AI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ExpermedBDAIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // Registrar IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();
            // Registrar el servicio de autenticación
            builder.Services.AddScoped<AuthenticationServices>();
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddHttpClient(); // Registrar HttpClient

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
}
