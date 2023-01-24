using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.Helpers
{
    public class UploadResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string Link { get; set; }
        public bool IsUploadedSuccessfully { get; set; }
        public string ErrorMessage { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
    }
}