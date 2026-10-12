using Microsoft.EntityFrameworkCore;
using SusanOsuolaleCV.Models;

namespace SusanOsuolaleCV.Data
{
    public class GalleryDbContext : DbContext
    {
        public GalleryDbContext(DbContextOptions<GalleryDbContext> options) : base(options)
        {
        }

        public DbSet<GalleryImage> GalleryImages { get; set; }
    }
}
