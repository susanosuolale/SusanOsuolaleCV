
using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Data;
using SusanOsuolaleCV.Models;

namespace SusanOsuolaleCV.Controllers
{
    public class GalleryController : Controller
    {
        private readonly GalleryDbContext _context;
        private readonly IWebHostEnvironment _env;

        public GalleryController(GalleryDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var images = _context.GalleryImages.ToList();
            return View(images);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult GetImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                Path.GetFileName(fileName) != fileName)
            {
                return BadRequest();
            }

            var home = Environment.GetEnvironmentVariable("HOME");

            var imagesFolders = new[]
            {
                Path.Combine(home ?? _env.ContentRootPath, "gallery-images"),
                Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "images")
            };

            foreach (var imagesFolder in imagesFolders)
            {
                var filePath = Path.Combine(imagesFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".webp" => "image/webp",
                        _ => "application/octet-stream"
                    };

                    return PhysicalFile(filePath, contentType);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile imageFile, string title, string description)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image file.");
                return View();
            }

            var imagesFolder = Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? _env.ContentRootPath, "gallery-images");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, uniqueFileName);

            Console.WriteLine($"Images folder: {imagesFolder}");
            Console.WriteLine($"File path: {filePath}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var galleryImage = new GalleryImage
            {
                FileName = uniqueFileName,
                Title = title ?? Path.GetFileNameWithoutExtension(imageFile.FileName),
                Description = description ?? string.Empty
            };

            _context.GalleryImages.Add(galleryImage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var image = await _context.GalleryImages.FindAsync(id);
            if (image == null) return NotFound();
            return View(image);
        }
    }
}
