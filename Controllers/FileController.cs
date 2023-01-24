using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class ImageResult : ActionResult
{
    public string ContentType { get; set; }
    public byte[] ImageBytes { get; set; }
    public string SourceFilename { get; set; }

    //This is used for times where you have a physical location
    public ImageResult(string sourceVirtualFilename, string contentType)
    {
        SourceFilename = sourceVirtualFilename;
        ContentType = contentType;
    }

    //This is used for when you have the actual image in byte form
    //  which is more important for this post.
    public ImageResult(byte[] sourceStream, string contentType)
    {
        ImageBytes = sourceStream;
        ContentType = contentType;
    }

    public override void ExecuteResult(ControllerContext context)
    {
        var response = context.HttpContext.Response;
        response.Clear();
        response.Cache.SetCacheability(HttpCacheability.NoCache);
        response.ContentType = ContentType;

        //Check to see if this is done from bytes or physical location
        //  If you're really paranoid you could set a true/false flag in
        //  the constructor.
        if (ImageBytes != null)
        {
            var stream = new MemoryStream(ImageBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
        else if (!string.IsNullOrWhiteSpace(SourceFilename))
        {
            response.TransmitFile(context.RequestContext.HttpContext.Server.MapPath(SourceFilename));
        }
    }
}

namespace FAPP.Controllers
{
    public class FileController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetFile(int id)
        {
            Model.OneDbContext db = new Model.OneDbContext();
            var _file = db.CRMAttachment.Where(u => u.AttachmentId == id).FirstOrDefault();
            //This is my method for getting the image information
            // including the image byte array from the image column in
            // a database.
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            if (_file.InternalFile != null)
            {
                ImageResult result = new ImageResult(_file.InternalFile, _file.fileType);
                return result;
            }
            else
            {
                ImageResult result = new ImageResult("/Images/no-photo.gif", "image/gif");
                return result;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EmployeeFile(Guid? id)
        {
            Model.OneDbContext db = new Model.OneDbContext();
            var _file = db.EmployeeDocuments.Where(u => u.EntryId == id).FirstOrDefault();
            if (_file.DocumentImage != null)
            {
                ImageResult result = new ImageResult(_file.DocumentImage, _file.DocumentExtension);
                return result;
            }
            else
            {
                ImageResult result = new ImageResult("/Images/no-photo.gif", "image/gif");
                return result;
            }
        }

        public FileResult EFileDownload(Guid? id)
        {
            using (var db = new Model.OneDbContext())
            {
                string ContentType;
                byte[] ImageBytes;
                string SourceFilename;
                var _file = db.EmployeeDocuments.Where(u => u.EntryId == id).FirstOrDefault();
                if (_file.DocumentImage != null)
                {
                    ContentType = _file.DocumentExtension;
                    ImageBytes = _file.DocumentImage;
                    SourceFilename = _file.DocumentName;
                }
                else
                {
                    ContentType = ".gif";
                    ImageBytes = null;
                    SourceFilename = "/Images/no-photo.gif";
                }
                //result = new ImageResult("/Images/no-photo.gif", "image/gif");
                return File(ImageBytes, Service.MimeTypeMap.GetMimeType(ContentType), SourceFilename);

            }
        }

    }
}