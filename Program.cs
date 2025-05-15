using Microsoft.AspNetCore.Authentication.Negotiate;
using RaceStrategyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RaceStrategyApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();
            builder.Services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);
            builder.Services.AddDbContext<RaceStrategyContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();
            }

            // Fixed middleware order:
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();

            app.Run();
        }
    }
}
