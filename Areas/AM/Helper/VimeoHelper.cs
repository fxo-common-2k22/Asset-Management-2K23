using FAPP.Areas.AM.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VimeoDotNet;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace FAPP.Areas.AM.Helpers
{
    public class VimeoHelper
    {
        private VimeoClient _Client;
        //private IHostingEnvironment _env;

        // authorizationClient
        public VimeoHelper()
        {
            // This is Secret Token 5132dcb0269e253ccc222a7adb5e22a4sdsd333e84352 latter will be saved in to database
            _Client = new VimeoClient("da6ea2bdf587d8448f19f106163a30c6");
        }


        public async Task<UploadResponse> UploadVideo(string Description, HttpPostedFileBase file)
        {
            var response = new UploadResponse();
            try
            {
                var RootPath = Path.Combine(HttpContext.Current.Server.MapPath("~/uploads/TempFiles"));
                var uniqueFileName = GetUniqueFileName(Path.GetFileName(file.FileName));
                var UploadPath = Path.Combine(RootPath + "\\" + uniqueFileName);
                using (FileStream Upload = new FileStream(Path.Combine(UploadPath), FileMode.Create))
                {
                    file.InputStream.CopyTo(Upload);
                    Upload.Dispose();
                    Upload.Close();
                }
                long length;
                using (var files = new BinaryContent(UploadPath))
                {

                    files.ContentType = MimeMapping.GetMimeMapping(UploadPath);
                    length = files.Data.Length;
                    files.OriginalFileName = uniqueFileName;

                    var t = await _Client.UploadEntireFileAsync(files, 1048576, null);
                    VideoUpdateMetadata metadata = new VideoUpdateMetadata();
                    metadata.Description = "Uploaded From ezBiz";
                    metadata.Name = uniqueFileName;
                    await _Client.UpdateVideoMetadataAsync(t.ClipId.Value, metadata);
                    response.IsUploadedSuccessfully = true;
                    response.id = t.ClipId.ToString();
                    //await client.Result.DeleteVideoAsync(completedRequest.ClipId.Value);
                }
            }
            catch (Exception ex)
            {
                response.IsUploadedSuccessfully = false;
                response.ErrorMessage = ex.Message;
            }
            return response;

        }
        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 10)
                      + Path.GetExtension(fileName);
        }
    }
}