using Microsoft.EntityFrameworkCore;
using SusanOsuolaleCV.Data;

namespace SusanOsuolaleCV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Get connection string from configuration. In production (Azure) this must be provided by App Service settings.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                if (builder.Environment.IsDevelopment())
                {
                    // For local development only, fall back to LocalDB
                    connectionString = "Server=(localdb)\\mssqllocaldb;Database=GalleryDb;Trusted_Connection=True;MultipleActiveResultSets=true";
                }
                else
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured. In production, set the DefaultConnection in App Service connection strings.");
                }
            }

            builder.Services.AddDbContext<GalleryDbContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Ensure wwwroot/images exists
            var imagesPath = Path.Combine(app.Environment.WebRootPath ?? "wwwroot", "images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
