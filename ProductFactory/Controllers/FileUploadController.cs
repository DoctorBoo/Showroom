using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ProductFactory.Controllers
{
	[System.Web.Mvc.Authorize]
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

    }
}