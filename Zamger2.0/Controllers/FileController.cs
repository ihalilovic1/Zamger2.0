using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zamger2._0.Data;

namespace Zamger2._0.Controllers
{
    [Authorize(Roles = "profesor")]
    public class FileController : Controller
    {
        private readonly ApplicationDbContext context;

        public FileController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("/File/Get/{fileName}")]
        public IActionResult Get(string fileName)
        {
            var document = context.Documents.FirstOrDefault(d => d.Name == Path.GetFileNameWithoutExtension(fileName));

            if(document == null)
            {
                return NotFound();
            }

            var cd = new ContentDispositionHeaderValue("inline");
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());
            return File(document.Data, document.ContentType, document.Name + document.Extension);
        }
    }
}
