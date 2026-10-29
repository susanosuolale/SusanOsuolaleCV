using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SusanOsuolaleCV.Data;
using SusanOsuolaleCV.Hubs;
using SusanOsuolaleCV.Models;

namespace SusanOsuolaleCV.Controllers
{
    public class CvController : Controller
    {
        private const string CvFileName = "Susan_Osuolale_Backend_Developer_CV.pdf";
        private readonly GalleryDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<CvDownloadHub> _hubContext;

        public CvController(
            GalleryDbContext dbContext,
            IWebHostEnvironment environment,
            IHubContext<CvDownloadHub> hubContext)
        {
            _dbContext = dbContext;
            _environment = environment;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Download()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "files", CvFileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("The CV file is not available.");
            }

            var download = new CvDownload { DownloadedAtUtc = DateTime.UtcNow };
            _dbContext.CvDownloads.Add(download);
            await _dbContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("cvDownloaded", download.DownloadedAtUtc);

            return PhysicalFile(filePath, "application/pdf", CvFileName);
        }
    }
}
