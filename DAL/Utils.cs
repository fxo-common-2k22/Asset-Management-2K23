using System;
using System.Web;
using System.Net.Mail;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using System.Linq;
using FSTM;
using System.Data.Entity;
using FAPP.DAL;
using System.Threading.Tasks;

namespace FAPP
{

    public static class Utils
    {
        private static string user = "";
        private static string pass = "";
        private static string from = "";
        public static SMS.Gateways smsGateway;
        public static string smsUsername = "";
        public static string smsPassword = "";
        public static string smsAuth = "";
        public static bool hasSMSPackage = false;
        private static SmtpClient emailClient = new SmtpClient("");
        private static System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(user, pass);

        public static void GetMailParams(short branchId)
        {
            using (var dc = new Model.OneDbContext())
            {
                var row = dc.UrlSettings.FirstOrDefault(p => p.BranchId == branchId);

                if (row == null)
                {
                    row = dc.UrlSettings.FirstOrDefault();
                }

                if (row != null)
                {
                    //SMS Settings
                    try
                    {
                        System.Enum.TryParse(FTC.Classes.Security.Decrypt(row.SMSGateway, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE), out smsGateway);
                        smsUsername = row.SMSUsername;
                        smsPassword = FTC.Classes.Security.Decrypt(row.SMSPassword, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                        smsAuth = FTC.Classes.Security.Decrypt(row.SMSAuth, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                        hasSMSPackage = true;
                    }
                    catch
                    {
                        hasSMSPackage = false;
                    }

                    //Email Settings
                    if (!string.IsNullOrWhiteSpace(row.Host) && row.Port.HasValue)
                    {
                        emailClient.Host = row.Host;
                        emailClient.Port = row.Port.Value;
                        emailClient.EnableSsl = row.isSecure;

                        user = row.EmailAddress;
                        pass = row.Password;
                        from = row.EmailAddress;
                        SMTPUserInfo = new System.Net.NetworkCredential(user, FTC.Classes.Security.Decrypt(pass, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE));
                    }
                }
            }
        }

        public static async Task GetMailParamsAsync(short branchId)
        {
            using (var dc = new Model.OneDbContext())
            {
                var row = await dc.UrlSettings.FirstOrDefaultAsync(p => p.BranchId == branchId);

                if (row == null)
                {
                    row = await dc.UrlSettings.FirstOrDefaultAsync();
                }

                if (row != null)
                {
                    //SMS Settings
                    try
                    {
                        System.Enum.TryParse(FTC.Classes.Security.Decrypt(row.SMSGateway, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE), out smsGateway);
                        smsUsername = row.SMSUsername;
                        smsPassword = FTC.Classes.Security.Decrypt(row.SMSPassword, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                        smsAuth = FTC.Classes.Security.Decrypt(row.SMSAuth, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                        hasSMSPackage = true;
                    }
                    catch
                    {
                        hasSMSPackage = false;
                    }

                    //Email Settings
                    if (!string.IsNullOrWhiteSpace(row.Host) && row.Port.HasValue)
                    {
                        emailClient.Host = row.Host;
                        emailClient.Port = row.Port.Value;
                        emailClient.EnableSsl = row.isSecure;

                        user = row.EmailAddress;
                        pass = row.Password;
                        from = row.EmailAddress;
                        SMTPUserInfo = new System.Net.NetworkCredential(user, FTC.Classes.Security.Decrypt(pass, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE));
                    }
                }
            }
        }

        public static bool SendEmail(string to, string subject, string body, NameValueCollection Parameters, Attachment attach, short branchId)
        {
            if (string.IsNullOrWhiteSpace(from))
                GetMailParams(branchId);

            if (Parameters != null)
            {
                foreach (string item in Parameters.AllKeys)
                {
                    body = body.Replace(item, Parameters[item]);
                }
            }

            MailMessage message = new MailMessage(from, to, subject, body);

            message.IsBodyHtml = true;

            if (attach != null)
                message.Attachments.Add(attach);

            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = SMTPUserInfo;

            try
            {
                emailClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Send error
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        //public static bool SendErrorEmail(string to, string subject, NameValueCollection param)
        //{
        //    var user = "alert@ezschoolerp.com";
        //    var pass = "Q6b8g@b5";
        //    emailClient.Host = "demo.ezschoolerp.com";
        //    emailClient.Port = 25;
        //    emailClient.EnableSsl = false;

        //    MailMessage message = new MailMessage(user, to, subject, "");
        //    message.Body = GetTemplate("/ErrorPages/ErrorTemplate.html", param);

        //    message.IsBodyHtml = true;
        //    //message.CC.Add("ezschoolerp@futuresoltech.com");
        //    SMTPUserInfo = new System.Net.NetworkCredential(user, pass);
        //    emailClient.UseDefaultCredentials = false;
        //    emailClient.Credentials = SMTPUserInfo;
        //    try
        //    {
        //        emailClient.Send(message);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public static bool SendErrorEmail(string fromDisplayName, string to, string subject, NameValueCollection param)
        {
            var user = "alert@ezschoolerp.com";
            var pass = "Q6b8g@b5";
            SMTPUserInfo = new System.Net.NetworkCredential(user, pass);
            emailClient = new SmtpClient
            {
                Host = "demo.ezschoolerp.com",
                Port = 25,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = SMTPUserInfo
            };

            MailMessage message = new MailMessage(user, to, subject, "")
            {
                Body = GetTemplate("/ErrorPages/ErrorTemplate.html", param),
                IsBodyHtml = true,
                From = new MailAddress(user, fromDisplayName)
            };

            try
            {
                emailClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private static string GetTemplate(string template, NameValueCollection param)
        {
            string body = "";
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(template));
            if (sr != null)
            {
                body = sr.ReadToEnd();
                foreach (string item in param.AllKeys)
                {
                    body = body.Replace(item, param[item]);
                }
            }
            return body;
        }

        public static string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string HashData(string data, string salt)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            byte[] value = System.Text.Encoding.UTF8.GetBytes(data + salt);
            byte[] hash = hashAlg.ComputeHash(value);
            return Convert.ToBase64String(hash);
        }
        public static bool ThumbnailCallback()
        {
            return false;
        }
        public static string CreateThumbnail(string path, string filename, int tsize)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            int width, height, thumbnailSize = tsize;

            Image photo = Image.FromFile(path + filename);
            string temp = string.Format("{0}{1}/{2}", path, tsize, filename);

            if (photo != null)
            {
                if (photo.Width > photo.Height)
                {
                    width = thumbnailSize;
                    height = photo.Height * thumbnailSize / photo.Width;
                }
                else
                {
                    width = photo.Width * thumbnailSize / photo.Height;
                    height = thumbnailSize;
                }

                Image target = photo.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                target.Save(temp, photo.RawFormat);
                target.Dispose();
            }
            photo.Dispose();
            return "";
        }
    }
}