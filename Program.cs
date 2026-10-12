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

            // Configure EF Core with SQL Server
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=GalleryDb;Trusted_Connection=True;MultipleActiveResultSets=true";
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
