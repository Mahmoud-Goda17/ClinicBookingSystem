using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Api.MainExtintion
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApplicationPipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();



            return app;
        }

            
        
    }
}
