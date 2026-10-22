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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile imageFile, string title, string description)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image file.");
                return View();
            }

            var imagesFolder = Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? _env.ContentRootPath,"gallery-images");

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
