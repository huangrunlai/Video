using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VideoUploader.Models;
using System.IO;
using System.Linq;

namespace VideoUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string _mediaFolderPath;

        public HomeController(IWebHostEnvironment env, ILogger<HomeController> logger)
        {
            _env = env;
            _mediaFolderPath = Path.Combine(_env.WebRootPath, "media");
            _logger = logger;

            // Ensure the media folder exists, if not, create it
            if (!Directory.Exists(_mediaFolderPath))
            {
                Directory.CreateDirectory(_mediaFolderPath);
            }
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        private List<VideoFile> GetVideoFiles()
        {
            var videoFiles = new List<VideoFile>();
            var files = Directory.GetFiles(_mediaFolderPath, "*.mp4");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                videoFiles.Add(new VideoFile
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length
                });
            }
            return videoFiles;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
