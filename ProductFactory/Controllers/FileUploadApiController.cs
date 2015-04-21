using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ProductFactory.Models;

namespace ProductFactory.Controllers
{
    public class FileUploadApiController : ApiController
    {
		public async Task<List<string>> PostAsync()
		{
			if (Request.Content.IsMimeMultipartContent())
			{
				string uploadPath = HttpContext.Current.Server.MapPath("~/App_Data");

				MyStreamProvider streamProvider = new MyStreamProvider(uploadPath);

				await Request.Content.ReadAsMultipartAsync(streamProvider);

				List<string> messages = new List<string>();
				foreach (var file in streamProvider.FileData)
				{
					FileInfo fi = new FileInfo(file.LocalFileName);
					messages.Add("File uploaded as " + fi.FullName + " (" + fi.Length + " bytes)");
				}

				return messages;
			}
			else
			{
				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
				throw new HttpResponseException(response);
			}
		}
    }
}
