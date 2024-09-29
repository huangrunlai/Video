using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using VideoUploader.Models;

namespace VideoUploader.Controllers
{
    [Route("api/video")]
    [ApiController]
    public class VideoApiController : ControllerBase
    {
        private readonly string _mediaFolderPath;

        public VideoApiController(IWebHostEnvironment env)
        {
            _mediaFolderPath = Path.Combine(env.WebRootPath, "media");
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            if (!Directory.Exists(_mediaFolderPath))
            {
                return Ok(new List<VideoFile>());
            }

            var videos = Directory.GetFiles(_mediaFolderPath, "*.mp4")
                                  .Select(f => new VideoFile
                                  {
                                      FileName = Path.GetFileName(f),
                                      FileSize = new FileInfo(f).Length
                                  }).ToList();

            return Ok(videos);
        }

        [HttpPost("upload")]
        public IActionResult Upload(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ".mp4")
                {
                    var filePath = Path.Combine(_mediaFolderPath, file.FileName);

                    // Rename the file if a file with the same name exists
                    int count = 1;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath);
                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(_mediaFolderPath, $"{fileNameWithoutExtension}({count++}){extension}");
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            return Ok();
        }
    }
}
