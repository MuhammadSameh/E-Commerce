using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly IBaseRepository<Media> repo;

        public MediaController(IBaseRepository<Media> repo)
        {
            this.repo = repo;
        }
        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.ContentType is null || !Request.ContentType.Contains("multipart/form-data"))
            {
                return BadRequest(new { Err = "Wrong content type" });
            }
            var files = Request.Form.Files;
            if (!files.Any()) { return BadRequest(new { Err = "There's no files" }); }

            var allowedExtensions = new string[] { ".jpg", ".svg", ".png" };
            List<string> urls = new List<string>();

            foreach (var file in files) {

                if (!allowedExtensions.Any(ext => file.FileName.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return BadRequest(new { Err = "Not valid extension" });
                }

                if (file.Length > 5_000_000)
                {
                    return BadRequest(new { Err = "Max size exceeded" });
                }

                if (file.Length <= 0)
                {
                    return BadRequest(new { Err = "Empty file" });
                }

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var fullFilePath = Directory.GetCurrentDirectory() + @"\Assets\Images\" + fileName;

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var url = $"{Request.Scheme}://{Request.Host}/Assets/Images/{fileName}";
                urls.Add(url);
            }
            
            
            return Ok(urls);
        }

        [HttpPost]
        [Route("add")]
        public async  Task<ActionResult> AddMedia(Media media)
        {
            if(media== null) { return BadRequest(new { Error = "Invalid Data" });}
            await repo.Add(media);
            return Ok(media.Id);
        }
    }
}
