using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
namespace FAPP.Helpers
{
    public class GoogleDriveHelper
    {
        
        public Google.Apis.Drive.v3.Data.File Getfolder(string FolderName)
        {
            DriveService service = GetService();
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "nextPageToken,files(id, name)";
            listRequest.Q = "mimeType='application/vnd.google-apps.folder'";
            Google.Apis.Drive.v3.Data.File Folder = listRequest.Execute().Files.Where(s => s.Name == FolderName).FirstOrDefault();
            if (Folder == null)
            {
                Folder = CreateFolder(FolderName, service);
            }
            return Folder;
        }
        public static DriveService GetService()
        {
            string[] Scopes = { DriveService.Scope.Drive };
            //string[] Scopes = new string[] { DriveService.Scope.Drive,
            //               DriveService.Scope.DriveAppdata,
            //               //DriveService.Scope.DriveAppsReadonly,
            //               DriveService.Scope.DriveFile,
            //               DriveService.Scope.DriveMetadataReadonly,
            //               DriveService.Scope.DriveReadonly,
            //               DriveService.Scope.DriveScripts };
            string ApplicationName = "TestFileUpload";
            UserCredential credential;
            string ClientSecretJsonPath = HttpContext.Current.Server.MapPath("~/uploads/GoogleDrive/FappClientSecret.json");

            using (var stream =
                new FileStream(ClientSecretJsonPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string FolderPath = HttpContext.Current.Server.MapPath("~/uploads/GoogleDrive/");
                string credPath = Path.Combine(FolderPath, "token.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        public static Google.Apis.Drive.v3.Data.File CreateFolder(string folderName, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = service.Files.Create(fileMetadata);
            request.Fields = "id,name";
            Google.Apis.Drive.v3.Data.File Folder = request.Execute();
            return Folder;
        }
        public UploadResponse UploadsFile(string FolderId, string Description, HttpPostedFileBase file)
        {
            var response = new UploadResponse();
            if (FolderId != null)
            {
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
                    DriveService service = GetService();
                    var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                    FileMetaData.Description = Description;
                    FileMetaData.Name = Path.GetFileName(uniqueFileName);
                    FileMetaData.MimeType = MimeMapping.GetMimeMapping(UploadPath);

                    //We have to assign parent folder id mean in which id file have to save
                    List<string> ParentFolderId = new List<string>();
                    ParentFolderId.Add(FolderId);
                    FileMetaData.Parents = ParentFolderId;
                    FilesResource.CreateMediaUpload request;
                    using (var stream = new System.IO.FileStream(UploadPath, System.IO.FileMode.Open))
                    {
                        request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);

                        request.Fields = "id, webViewLink";
                        request.Upload();
                    }
                    response.Link  = request.ResponseBody.WebViewLink;
                    response.IsUploadedSuccessfully = true;
                    response.Type = "File";
                    response.Provider = "Google Drive";
                }
                catch (Exception ex)
                {
                    response.IsUploadedSuccessfully = false;
                    response.ErrorMessage = ex.Message;
                }
                
            }
            else
            {
                response.IsUploadedSuccessfully = false;
                response.ErrorMessage = "Folder must not be null.";
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

        public void Get_ID_Token_For_Service_Account_Test()
        {
            string FolderPath = HttpContext.Current.Server.MapPath("~/uploads/GoogleDrive/credentials.json");
            using (Stream stream = new FileStream(FolderPath, FileMode.Open, FileAccess.Read))
            {

                ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(stream);
               
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromServiceAccountCredential(credential),
                    ServiceAccountId = "futuretech323-gmail-com@symbolic-grail-279813.iam.gserviceaccount.com",
                });
                var uid = Guid.NewGuid().ToString();
                var additionalClaims = new Dictionary<string, object>
        {
        };
                string customToken = FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid, additionalClaims).Result;
                //string idToken = null; // How to get this? 

                //FirebaseToken token = FirebaseAuth.DefaultInstance.UpdateUserAsync(customToken, CancellationToken.None).Result;
                //Assert.NotNull(token);
                //Assert.True(token.Claims.ContainsKey("dmitry"));
            }
        }

    }
}