using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VideoUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env; 
        private readonly string _mediaFolderPath;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
            _mediaFolderPath = Path.Combine(_env.WebRootPath, "media");
            if (!Directory.Exists(_mediaFolderPath))
            {
                Directory.CreateDirectory(_mediaFolderPath);
            }
        }

        [HttpPost]
        [RequestSizeLimit(200 * 1024 * 1024)] // 200 MB limit
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            foreach (var file in files)
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
                {
                    return BadRequest("Only MP4 files are allowed.");
                }

                // **Check if a file with the same name exists in the destination**
                var fileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(_mediaFolderPath, fileName);

                // **Rename file if it already exists**
                if (System.IO.File.Exists(fullPath))
                {
                    // **Append a timestamp or GUID to create a unique file name**
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var extension = Path.GetExtension(fileName);
                    fileName = $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{extension}";
                    fullPath = Path.Combine(_mediaFolderPath, fileName);
                }

                // Save the file to the server's media folder
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok("File(s) uploaded successfully.");
        }
    }

}
