
using System.Text;
using ClinicBookingSystem.Api.MainExtintion;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistence.Data;
using Presistence.Repository;
using Service;
using Sevice.Abstactions;
using Shared.IdentityDtos;

namespace ClinicBookingSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Services
            builder.Services.AddApplicationServices(builder.Configuration);


            var app = builder.Build();
            // Create Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "User", "Admin", "Doctor", "Patient" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }
            }
            // Middleware
            app.UseApplicationPipeline();
            
            app.Run();
            
        }
    }
}
