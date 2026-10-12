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
        public async Task<IActionResult> Upload(IFormFile imageFile, string title)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image file.");
                return View();
            }

            //creates the path to wwwroot/images where the images will be stored by taking
            //the two pieces of text combining them with a slash in the middle
            //_env.WebRootPath finds the path were the project is saved in the computer
            var imagesFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");

            // ensure images folder exists
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            //creates a random string of letters and gets the file extensiion like .jpg, joins them together to
            //to give the file a unique name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, uniqueFileName);

            // creates stream that works with files and connects it to the file path
            //then the file is copied into the stream which writes the bytes into the file path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Save record to database
            var relativePath = Path.Combine("/images", uniqueFileName).Replace("\\", "/");

            var galleryImage = new GalleryImage
            {
                Title = title ?? Path.GetFileNameWithoutExtension(imageFile.FileName),
                FilePath = relativePath
            };

            _context.GalleryImages.Add(galleryImage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var image = await _context.GalleryImages.FindAsync(id);
            if (image == null) return NotFound();
            return View(image);
        }
    }
}
