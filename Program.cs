using Microsoft.AspNetCore.Authentication.Negotiate;
using RaceStrategyApp.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Authorization;

namespace RaceStrategyApp {
    public class Program {
        public static void Main(string[] args) {
            static IEdmModel GetEdmModel() {
                ODataConventionModelBuilder builder = new();
                builder.EntitySet<Race>("Race");
                builder.EntitySet<RaceSeries>("RaceSeries");
                builder.EntitySet<RaceProgress>("RaceProgress");
                return builder.GetEdmModel();
            }

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddOData(options => options
                .AddRouteComponents("api", GetEdmModel())
                .Select().Filter().OrderBy().Count().Expand());

            builder.Services.AddOpenApi();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build();
            });

            builder.Services.AddDbContext<RaceStrategyContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();
            }

            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.MapControllers();

            app.UseWhen(context => context.Request.Path.Value!.Equals("/api/$metadata", StringComparison.OrdinalIgnoreCase), subApp =>
            {
                subApp.UseRouting();
                app.UseAuthorization();
                subApp.UseEndpoints(endpoints => endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute()));
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
