using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SusanOsuolaleCV.Data;
using SusanOsuolaleCV.Models;
using System.Data.Common;
using SusanOsuolaleCV.Services;

namespace SusanOsuolaleCV.Controllers
{
    public class GalleryController : Controller
    {
        private readonly GalleryDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IGalleryStorage _galleryStorage;

        public GalleryController(GalleryDbContext context, IWebHostEnvironment env, IGalleryStorage galleryStorage)
        {
            _context = context;
            _env = env;
            _galleryStorage = galleryStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GalleryImage> images;

            try
            {
                images = await _context.GalleryImages.AsNoTracking().ToListAsync();
            }
            catch (Exception ex) when (ex is DbException || ex is RetryLimitExceededException)
            {
                images = Array.Empty<GalleryImage>();
                ViewBag.DatabaseMessage = "We couldn't load the photo album right now. Please try again in a moment!";
                ViewBag.DatabaseError = ex.GetBaseException().Message;
            }

            return View(images);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        public async Task<IActionResult> GetImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                Path.GetFileName(fileName) != fileName)
            {
                return BadRequest();
            }

            var blobStream = await _galleryStorage.OpenReadAsync(fileName);
            if (blobStream is not null)
            {
                return File(blobStream, GetContentType(fileName));
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
                    return PhysicalFile(filePath, GetContentType(fileName));
                }
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var image = await _context.GalleryImages.FindAsync(id);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrWhiteSpace(image.FileName) &&
                Path.GetFileName(image.FileName) == image.FileName)
            {
                var deletedFromBlob = await _galleryStorage.DeleteIfExistsAsync(image.FileName);
                if (!deletedFromBlob)
                {
                    DeleteLegacyFile(image.FileName);
                }
            }

            _context.GalleryImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

        private void DeleteLegacyFile(string fileName)
        {
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
                    System.IO.File.Delete(filePath);
                    break;
                }
            }
        }

        private static string GetContentType(string fileName) =>
            Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var image = await _context.GalleryImages.FindAsync(id);
            if (image == null) return NotFound();
            return View(image);
        }
    }
}
