using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FAPP.Classes
{
    public class FireBaseApp
    {
        public void StartApp()
        {
            string FolderPath = HttpContext.Current.Server.MapPath("~/uploads/Firebase/");
            string credPath = Path.Combine(FolderPath, "key.json");

            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(credPath),
            });
            Console.WriteLine(defaultApp.Name); // "[DEFAULT]"
        }
    }
}