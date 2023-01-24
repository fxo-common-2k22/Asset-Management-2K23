using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
//using FAPP.Filters;
//using FAPP.Models;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FAPP.Areas.AM.ViewModels.PrimaryData;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
using FAPP.Areas.AM.PrimaryData;
using FAPP.Areas.AM.BLL;
using System.IdentityModel.Tokens.Jwt;
using static FAPP.Areas.AM.ViewModels.PrimaryDataViewModel;
using System.Web.Configuration;
using FAPP.Classes;
//using FAPP.ViewModel;
using System.Globalization;

namespace FAPP.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public string Error { get; set; }
        public string Warnings { get; set; }
        public string Messages { get; set; }
        public PrimaryDataViewModel.CredentialVM ServerCredential
        {
            get
            {
                System.Web.HttpContext.Current.Application.UnLock();
                var credential = (PrimaryDataViewModel.CredentialVM)System.Web.HttpContext.Current.Application["Credential"];
                System.Web.HttpContext.Current.Application.Lock();
                return credential;
            }
            set
            {
                System.Web.HttpContext.Current.Application.UnLock();
                System.Web.HttpContext.Current.Application["Credential"] = value;
                System.Web.HttpContext.Current.Application.Lock();
            }
        }
        public AccountController()
        {
            System.Web.HttpContext.Current.Application.UnLock();
            CreateNewData = (bool?)System.Web.HttpContext.Current.Application["CreateNewData"];
            System.Web.HttpContext.Current.Application.Lock();
        }
        private string url = "/Home/Index";
        private string lblError = "";
        private bool? CreateNewData = null;
        private string generatedToken = null;
        string JwtKey = WebConfigurationManager.AppSettings["SecretKey"];
        string Issuer = WebConfigurationManager.AppSettings["Issuer"];
        private const int ApplicationId = 16;
        private readonly OneDbContext db = new OneDbContext();

        //Get Build DateTime 
        public static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            //ViewBag.Build = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var buildTime = GetLinkerTime(Assembly.GetExecutingAssembly());
            ViewBag.Build = "Last Modified On: " + buildTime;

            string viewName = "Start";
            if (SessionHelper.UserName != "")
            {
                returnUrl = "/Home/Index";
                return Redirect(returnUrl);
            }
            if (!await db.Info.AnyAsync())
            {
                if (CreateNewData.HasValue)
                {
                    TempData["Warning"] = "Company not found. Please create a company.";
                    viewName = CreateNewData.Value == true ? "CreateCompany" : "ImportCompany";
                    var vm = new PrimaryDataViewModel();
                    //vm.Forms = new PrimaryData.MenuLinks(db).GetParentForms();
                    if (viewName == "ImportCompany")
                    {
                        //vm.Credential = (PrimaryDataViewModel.CredentialVM)System.Web.HttpContext.Current.Application["Credential"];
                        vm.Credential = ServerCredential;
                    }
                    return View(viewName, vm);
                }
                return View(viewName);
            }

            if (!await db.Users.AnyAsync())
            {
                if (CreateNewData.HasValue)
                {
                    TempData["Warning"] = "User not found. Please create a User.";
                    ViewBag.UserGroups = db.UserGroups.Select(u => new SelectListItem { Text = u.UserGroupName, Value = u.UserGroupId.ToString() }).ToList();
                    viewName = CreateNewData.Value == true ? "CreateUser" : "ImportUser";
                    var vm = new PrimaryDataViewModel();
                    if (viewName == "ImportUser")
                    {
                        //vm.Credential = (PrimaryDataViewModel.CredentialVM)System.Web.HttpContext.Current.Application["Credential"];
                        vm.Credential = ServerCredential;
                        return View(viewName, vm);
                    }
                    return View(viewName);
                }
                return View(viewName);
            }

            if (!await db.Branches.AnyAsync())
            {
                if (CreateNewData.HasValue)
                {
                    TempData["Warning"] = "Branch not found. Please create a Branch.";
                    viewName = CreateNewData.Value == true ? "CreateBranch" : "";
                }
                return View(viewName);
            }

            var importWizard = await db.ImportWizard.FirstOrDefaultAsync();
            if (importWizard == null)
            {
                importWizard = new ImportWizard();
                db.ImportWizard.Add(importWizard);
                db.SaveChanges();
            }

            if (CreateNewData.HasValue && CreateNewData.Value == false)
            {
                viewName = "";
                if (!importWizard.DataImported)
                {
                    return View("ImportData");
                }

                if (!importWizard.FinanceImported)
                {
                    return View("ImportFinance");
                }

                if (!importWizard.HRImported)
                {
                    return View("ImportHR");
                }

                if (!importWizard.PRImported)
                {
                    return View("ImportPR");
                }

                if (!importWizard.AcademicsImported)
                {
                    return View("ImportAcademics");
                }

                if (!importWizard.FeeImported)
                {
                    return View("ImportFee");
                }

                if (!importWizard.TransportImported)
                {
                    return View("ImportTransport");
                }

                if (!importWizard.HostelImported)
                {
                    return View("ImportHostel");
                }
                //import finance
                return View(viewName);
            }

            ViewBag.ReturnUrl = returnUrl;
            try
            {
                //ViewBag.LogoImage = dc.Infoes.Select(u => u.WebLogoFull).FirstOrDefault();
                ViewBag.LogoImage = db.Branches.Where(u => u.BranchLogoLarge != null).Select(u => u.BranchLogoLarge).FirstOrDefault();
                //ViewBag.CompanyName = db.Info.Select(u => u.CompanyName).FirstOrDefault();
                var UrlSetting = db.UrlSettings.Select(s => new { s.URLapi, s.URLKnowledgeBase, s.URLProduct, s.URLSupportTicket }).FirstOrDefault();
                if (UrlSetting != null)
                {
                    SessionHelper.Urlapi = UrlSetting.URLapi;
                    SessionHelper.UrlKnowledgeBase = UrlSetting.URLKnowledgeBase;
                    SessionHelper.UrlSupportTicket = UrlSetting.URLSupportTicket;
                    SessionHelper.UrlProduct = UrlSetting.URLProduct;
                    ViewBag.Announcements = GetAnnouncements();
                    ViewBag.ProductInfo = GetProductInfo();
                }
                SessionHelper.CompanyName = db.Info.Select(s => s.CompanyName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string Password, string returnUrl)
        {
            EncryptionDecryption _encryptionDecryption = new EncryptionDecryption();
            ViewBag.LogoImage = db.Info.Select(u => u.WebLogoFull).FirstOrDefault();
            ViewBag.CompanyName = db.Info.Select(u => u.CompanyName).FirstOrDefault();
            SessionHelper.ResetSessionVarriables = false;
            string username = UserName.ToLower();
            //string EncryptPass = FTC.Classes.Security.Encrypt("admin", XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
            string pass = FTC.Classes.Security.Encrypt(Password, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
            //string pass = Password;

            var _users = db.Users.Include(s => s.UserGroup)
                         .Where(u => (u.Username == username) && u.Password == pass)
                         .Select(u => new
                         {
                             UserID = u.UserID,
                             Email = u.Email,
                             Username = u.Username,
                             UserGroupId = u.UserGroupId,
                             UserGroupName = u.UserGroup.UserGroupName,
                             ExpiresOn = u.ExpiresOn,
                             BranchId = 0,
                             BranchName = "",
                             BranchCode = "",
                             BranchAddress = "",
                             //RegPrefix = u.UserGroup.Branch.Info.RegPrefix,
                             //ProductKey = u.UserGroup.Branch.Info.ProductKey,
                             //u.UserGroup.Branch.Info.ShortName,
                             //PhoneNumber = u.UserGroup.Branch.PhoneNumber,
                             //GST = u.UserGroup.Branch.Info.GST,
                             //CompanyName = u.UserGroup.Branch.Info.CompanyName,
                             LastLoginDate = u.LastLoginDate,
                             Theme = u.Theme,
                             ThemeColor = u.ThemeColor,
                             u.MobileNo,
                             u.IsEmailVerified,
                             u.IsMobileVerified,
                             HomepageUrl = u.UserGroup.Form != null ? u.UserGroup.Form.FormURL : "",
                             CustomDateTime = u.CustomDateTime,
                             Active = u.isActive,
                             //Photo = null, u.Photo, do not include like this
                             StartUpPage = db.UserGroupApplications.Where(x => x.ApplicationId == ApplicationId && x.UserGroupId == u.UserGroupId).Select(x => x.DashboardUrl).FirstOrDefault(),
                             IsMasterUser = u.IsMasterUser,
                         }).FirstOrDefault();

            if (_users != null)
            {
                AccountVM accountVM = new AccountVM();
                accountVM.UserName = UserName;
                accountVM.Password = Password;
                generatedToken = JwtManager.GenerateToken(JwtKey, Issuer, accountVM);
                generatedToken = _encryptionDecryption.EncodePasswordToBase64(generatedToken);
                SessionHelper.Token = generatedToken;
                var user = _users;
                if (user != null)
                {
                    //---------Temporarily---------
                    db.Database.ExecuteSqlCommand($@"
                    if exists(select FormID from Membership.Forms where FormURL like '%/HP/%')
                    begin
                    update f
                    set FormURL=REPLACE(f.FormURL,'/HP/','/HRPayroll/')
                    from Membership.Forms f
                    where FormURL like '%/HP/%'
                    end
                    ");
                    //---------------------------
                    SessionHelper.UserID = user.UserID;
                    SessionHelper.Username = user.Username;
                    SessionHelper.IsMasterUser = user.IsMasterUser;
                    SessionHelper.UserGroupId = user.UserGroupId;
                    SessionHelper.UserGroupName = user.UserGroupName;
                    SessionHelper.ShowExceptionToUser = db.Info.Select(u => u.ShowExceptionToUser).FirstOrDefault();
                    var branch = db.UserBranches.Include(b => b.Branch.Currency).Where(k => k.UserId == SessionHelper.UserID && k.DefaultBranch == true && k.Active == true).Select(k => new { k.BranchId, k.Branch.IsMasterBranch, k.Branch.Currency.CurrencySymbol }).FirstOrDefault();
                    SessionHelper.BranchId = branch.BranchId;
                    SessionHelper.CurrencySymbol = branch.CurrencySymbol;
                    var UrlSetting = db.UrlSettings.Select(s => s.URLapi).FirstOrDefault();
                    SessionHelper.Urlapi = UrlSetting;

                    SessionHelper.IsMasterBranch = branch.IsMasterBranch;
                    if (SessionHelper.BranchId > 0)
                    {
                        ProceduresModel.ResetSessionHelper_Branch(db, null, SessionHelper.BranchId, SessionHelper.UserId);
                    }
                    else
                    {
                        Session.Abandon();
                        ViewBag.Error = "Branch not found";
                        lblError = "Branch not found";
                        return Json(new { response = "error", title = "Error", message = lblError });
                    }
                    //SessionHelper.UserPhoto = user.Photo;
                    SessionHelper.ThemeColor = user.ThemeColor;

                    ProceduresModel.SessionNotes(db);
                    UserLog _UserLog = new UserLog();
                    _UserLog.UserId = SessionHelper.UserID;
                    _UserLog.LoginTime = DateTime.Now;
                    SessionHelper.IP = _UserLog.IP = HttpContext.Request.UserHostAddress;
                    _UserLog.BranchId = Convert.ToInt16(SessionHelper.BranchId);

                    db.UserLogs.Add(_UserLog);
                    db.SaveChanges();
                    SessionHelper.UserLogId = _UserLog.UserLogId;

                    if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl != "/Account/Logout" && returnUrl != "/")
                    {
                        return Json(new { response = "success", title = "Success", message = "Login Successfully", url = returnUrl });
                    }
                    else if (!string.IsNullOrEmpty(_users.StartUpPage))
                    {
                        return Json(new { response = "success", title = "Success", message = "Login Successfully", url = _users.StartUpPage });

                    }
                    else
                    {
                        return Json(new { response = "success", title = "Success", message = "Login Successfully", url = url });

                    }
                }
                else
                {
                    lblError = "The user name or password provided is incorrect.";
                }
            }
            else
            {
                lblError = "The user name or password provided is incorrect.";
            }

            ViewBag.Error = lblError;
            return Json(new { response = "error", title = "Error", message = lblError });
        }

        public ActionResult Logout(string returnUrl)
        {
            using (var db = new OneDbContext())
            {
                UserLog _UserLog = new UserLog();
                var res = db.UserLogs.Where(u => u.UserId == SessionHelper.UserID).OrderByDescending(u => u.LoginTime).Take(1).ToList();
                foreach (var item in res)
                {
                    _UserLog = item;
                    _UserLog.LogoutTime = DateTime.Now;
                }
                if (_UserLog.UserLogId > 0)
                {
                    db.Entry(_UserLog).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            Session.Abandon();
            Session["BranchId"] = null;
            return Redirect("/Account/Login");
        }

        public List<Announcement> GetAnnouncements()
        {
            var url = $@"{SessionHelper.UrlProduct}/api/cms/GetAnnouncements?WebsiteId=11&TotalRecords=20&pageNo=1";
            string jsonString;
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format(url));
                WebReq.Method = "GET";
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Console.WriteLine(WebResp.StatusCode);
                Console.WriteLine(WebResp.Server);
                if (WebResp.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    var announcements = JsonConvert.DeserializeObject<List<Announcement>>(jsonString);
                    return announcements;
                }
                else
                {

                    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    var announcements = JsonConvert.DeserializeObject<List<Announcement>>(jsonString);
                    return announcements;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var inner = ex.InnerException;
                return null;
            }
        }
        public ProductInfo GetProductInfo()
        {
            var url = $@"{SessionHelper.UrlProduct}/api/cms/GetWebsiteInfo?WebsiteId=11";
            string jsonString;
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format(url));
                WebReq.Method = "GET";
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Console.WriteLine(WebResp.StatusCode);
                Console.WriteLine(WebResp.Server);
                if (WebResp.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    var productInfo = JsonConvert.DeserializeObject<ProductInfo>(jsonString);
                    return productInfo;
                }
                else
                {
                    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    var productInfo = JsonConvert.DeserializeObject<ProductInfo>(jsonString);
                    return productInfo;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var inner = ex.InnerException;
                return null;
            }
        }


        public JsonResult VerifyCaptcha(string response)
        {
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + "6Lcq6RkUAAAAAIDNPpPn02bRA7N21dWtqVFW-p5i" + "&response=" + response;
            string res = (new WebClient()).DownloadString(url);
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public async Task<ActionResult> CreateCompany(PrimaryDataViewModel vm)
        {
            return await Create(vm);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBranch(Branch branch)
        {
            return await Create(branch);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            return await Create(user);
        }

        public async Task<ActionResult> Create<T>(T entity)
        {
            if (ModelState.IsValid)
            {
                using (var db = new OneDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (typeof(T).Name == "PrimaryDataViewModel")
                            {
                                PrimaryDataViewModel vm = entity as PrimaryDataViewModel;
                                db.Info.Add(vm.Info);
                                //IMPORTING FORMS
                                var formMenus = new MenuLinks(db);
                                formMenus.AddSelectedFormChildren(vm.Forms.Where(s => s.IsChecked == true).Select(s => s.FormId).ToArray());
                                await formMenus.SaveChangesAsync();
                                //IMPORTING DEFAULT USERGROUP THAT IS ADMIN
                                formMenus.AddUpdateDefaultUserGroup();
                                await formMenus.SaveChangesAsync();
                                var userGroupId = db.UserGroups.Where(s => s.UserGroupName == "Admin").Select(s => s.UserGroupId).FirstOrDefault();
                                if (userGroupId > 0)
                                {
                                    //ADDING RIGHTS TO ADDED FORMS
                                    formMenus.AddGroupRights(userGroupId);
                                }
                                SessionHelper.CompanyId = vm.Info.SettingId;
                                //ADDING WEEK DAYS, APPLICATION PORTAL DEFFAULT DATA
                                var pd = new ApplicationPrimaryData(db);
                                pd.AddUpdateWeekDays();
                                pd.AddUpdateApplicationPortal();
                                pd.AddUpdatePaymentModes();
                                pd.AddUpdateAppModule();
                                pd.AddUpdateClientTypes();
                                await pd.SaveChangesAsync();
                                pd.AddUpdateTemplateTypes();
                                await pd.SaveChangesAsync();
                                //pd.CreateTemplateTypeSqlView();
                            }
                            else if (typeof(T).Name == "Branch")
                            {
                                var branch = entity as Branch;
                                branch.BranchLogoLarge = GetBytesByFiles(System.Web.HttpContext.Current.Request.Files["LargeLogo"]);
                                branch.BranchLogoLarge = GetBytesByFiles(System.Web.HttpContext.Current.Request.Files["SmallLogo"]);
                                branch.BranchLogoMini = GetBytesByFiles(System.Web.HttpContext.Current.Request.Files["MiniLogo"]);
                                //branch.BranchId = branch.;
                                //branch.BranchCode = 1;// Convert.ToInt16(branch.BranchId.ToString().Substring(branch.BranchId.ToString().Length - 2));
                                branch.HasBiometricAttendance = false;
                                branch.AppendFineToNextVoucher = false;
                                branch.FillVoucherByAllFeeTypes = false;
                                branch.MergeUnpaidVoucher = false;
                                branch.UnpaidVoucherFine = 0;
                                branch.ShowVoucherStatus = false;
                                branch.AllowMultipleVouchersInAMonth = false;
                                branch.SettingId = SessionHelper.CompanyId > 0 ? (short)SessionHelper.CompanyId : await db.Info.Select(s => s.SettingId).FirstOrDefaultAsync();
                                db.Branches.Add(branch);
                                await db.SaveChangesAsync();

                                UserBranch userBranch = new UserBranch();
                                userBranch.UserBranchId = (db.UserBranches.Max(s => (int?)s.UserBranchId) ?? 0) + 1;
                                userBranch.UserId = SessionHelper.UserID > 0 ? SessionHelper.UserID : await db.Users.Select(s => s.UserID).FirstOrDefaultAsync();
                                userBranch.BranchId = branch.BranchId;
                                userBranch.Active = true;
                                userBranch.DefaultBranch = false;
                                //userBranch.UserBranchId = 1;
                                userBranch.DefaultBranch = true;
                                db.UserBranches.Add(userBranch);
                                await db.SaveChangesAsync();

                                //ADDING CHART OF ACCOUNTS
                                var coa = new ChartOfAccounts(db);
                                coa.AddAllDefault();
                                coa.AddAllDefaultWithBranch(branch.BranchId);
                                coa.AddAllDefaultAccunts();
                                await coa.SaveChanges();
                                //adding other appilcation primary data
                                var pd = new ApplicationPrimaryData(db);
                                pd.AddUpdateAMConditionType();
                                pd.AddUpdateAMRequestStatus();
                                pd.AddUpdateAppModule();
                                pd.AddUpdateHouseType();
                                pd.AddUpdatePlacementType();
                                //pd.AddUpdateResItemOrderStatuses();
                                //pd.AddUpdateResOrderStatuses();
                                pd.AddUpdateResOrderType();
                                pd.AddUpdateRoomStatus();
                                pd.AddUpdateVoucherType();
                                await pd.SaveChangesAsync();
                            }
                            else if (typeof(T).Name == "User")
                            {
                                User user = entity as User;
                                string username = user.Username.ToLower();
                                string EncryptPass = FTC.Classes.Security.Encrypt("admin", XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                                string pass = FTC.Classes.Security.Encrypt(user.Password, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                                user.Password = pass;
                                user.Username = username;
                                user.CreationDate = DateTime.Now;
                                user.ModifiedDate = DateTime.Now;
                                user.UserID = 1;
                                user.isActive = true;
                                SessionHelper.UserID = user.UserID;
                                db.Users.Add(user);
                            }
                            await db.SaveChangesAsync();
                            TempData["Success"] = $"Data has been created successfully.";
                            trans.Commit();
                            return RedirectToAction("Login");
                        }
                        catch (Exception x)
                        {
                            string error = x.Message;
                            trans.Rollback();
                            TempData["Error"] = $"Something went wrong while creating {typeof(T).Name}.";
                            throw;
                        }
                    }
                }
            }
            else
            {
                TempData["Warning"] = GetModelStateErrors();
                throw new Exception(GetModelStateErrors());
            }
            //return RedirectToAction("Login", new { msg = GetModelStateErrors() });
        }

        public async Task<ActionResult> GetOldCompany(PrimaryDataViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            if (ModelState.IsValid)
            {
                string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
                SqlConnection cnn = new SqlConnection(connetionString);
                string query = $@"Select * from Company.Info";
                try
                {
                    cnn.Open();
                    SqlCommand command = new SqlCommand(query, cnn);
                    PrimaryDataViewModel.OldInfo oldInfo = null;
                    using (IDataReader reader = await command.ExecuteReaderAsync())
                    {
                        oldInfo = DataReaderMapToList<PrimaryDataViewModel.OldInfo>(reader).FirstOrDefault();
                    }
                    command.Dispose();
                    cnn.Close();
                    //System.Web.HttpContext.Current.Application.UnLock();
                    //System.Web.HttpContext.Current.Application["Credential"] = vm.Credential;
                    //System.Web.HttpContext.Current.Application.Lock();
                    ServerCredential = vm.Credential;
                    if (oldInfo != null)
                    {
                        vm.Info = new Info
                        {
                            ApplyGST = oldInfo.ApplyGST,
                            CompanyAddress = oldInfo.CompanyAddress,
                            CompanyName = oldInfo.CompanyName,
                            Email = oldInfo.Email,
                            Facebook = oldInfo.Facebook,
                            Fax = oldInfo.Fax,
                            GST = oldInfo.GST,
                            GSTN = oldInfo.GSTN,
                            IP = oldInfo.IP,
                            //KOTGST = oldInfo.KOTGST,
                            Logo = oldInfo.Logo,
                            LogoFull = oldInfo.LogoFull,
                            NTN = oldInfo.NTN,
                            Organization = oldInfo.Organization,
                            Phone = oldInfo.Phone,
                            ProductKey = oldInfo.ProductKey,
                            ProfileFormat = oldInfo.ProfileFormat,
                            RegPrefix = oldInfo.RegPrefix,
                            SettingId = oldInfo.SettingId,
                            //ShareHR = oldInfo.ShareHR,
                            ShortName = oldInfo.ShortName,
                            WebLogoFull = oldInfo.WebLogoFull,
                            WebLogoMini = oldInfo.WebLogoMini,
                            Website = oldInfo.Website,

                        };
                        message = $"Company info loaded successfully.";
                    }
                }
                catch (Exception ex)
                {
                    error = ex.GetExceptionMessages();
                }
            }
            else
            {
                error = GetModelStateErrors();
            }
            vm.Forms = new MenuLinks(new OneDbContext()).GetParentForms();
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_CompanyForm", vm),
                Messages = message,
                Error = error,
                GridId = "companyFormDivId",
                Reset = "false",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]//IMPORT COMPANY FROM PREVIOUS DATABASE 
        public async Task<ActionResult> ImportCompany(PrimaryDataViewModel vm)
        {
            using (var db = new OneDbContext())
            {
                //using (var trans = db.Database.BeginTransaction())
                //{
                try
                {
                    db.Info.AddOrUpdate(s => s.SettingId, vm.Info);
                    db.SaveChanges();
                    //IMPORTING FORMS
                    var formMenus = new MenuLinks(db);
                    formMenus.AddSelectedFormChildren(vm.Forms.Where(s => s.IsChecked == true).Select(s => s.FormId).ToArray());
                    await formMenus.SaveChangesAsync();
                    //GETTING  USERGROUP FROM PREVIOUSE DATA BASE.
                    GetOldUserGroups(ref vm);
                    //IMPORTING FETCHED USERGROUPS INTO CURRENT DATA BASE.
                    ImportUserGroups(ref vm);
                    //GETTING  Users FROM PREVIOUSE DATA BASE.
                    GetOldUsers(ref vm);
                    //IMPORTING FETCHED Users INTO CURRENT DATA BASE.
                    ImportUsers(ref vm);
                    //GETTING  Branches FROM PREVIOUSE DATA BASE.
                    GetOldBranches(ref vm);
                    //IMPORTING FETCHED Branches INTO CURRENT DATA BASE.
                    ImportBranches(ref vm);
                    //GETTING  User Branches FROM PREVIOUSE DATA BASE.
                    GetOldUserBranches(ref vm);
                    //IMPORTING FETCHED User Branches INTO CURRENT DATA BASE.
                    ImportUserBranches(ref vm);

                    var userGroupId = db.UserGroups.Where(s => s.UserGroupName == "Admin").Select(s => s.UserGroupId).FirstOrDefault();
                    //ADDING RIGHTS TO ADDED FORMS
                    if (userGroupId > 0)
                    {
                        //ADDING RIGHTS TO ADDED FORMS 
                        formMenus.AddGroupRights(userGroupId);
                        formMenus.SaveChanges();
                    }
                    SessionHelper.CompanyId = vm.Info.SettingId;
                    //ADDING WEEK DAYS, APPLICATION PORTAL DEFFAULT DATA
                    var pd = new ApplicationPrimaryData(db);
                    pd.AddUpdateWeekDays();
                    pd.AddUpdateApplicationPortal();
                    pd.AddUpdatePaymentModes();
                    //IMPORT TEMPLATE TYPES FROM PREVIOUS DATABASE 
                    //PENDING
                    await pd.SaveChangesAsync();
                    //trans.Commit();
                }
                catch (Exception x)
                {
                    Error += x.GetExceptionMessages();
                    //trans.Rollback();
                }
                //}
            }
            TempData["Error"] = Error;
            TempData["Success"] = Messages;
            TempData["Warnings"] = Warnings;

            return RedirectToAction("Login");
        }

        private void GetOldUserGroups(ref PrimaryDataViewModel vm)
        {
            vm.Credential = ServerCredential;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Membership.UserGroups";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.UserGroups = DataReaderMapToList<PrimaryDataViewModel.OldUserGroup>(reader)
                        .Select(s => new UserGroup
                        {
                            UserGroupId = s.UserGroupId,
                            ByDefault = s.UserGroupName == "Admin" ? true : false,
                            DashboardId = null,
                            UserGroupName = s.UserGroupName,
                            BranchId = s.BranchId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
                //System.Web.HttpContext.Current.Application.UnLock();
                //System.Web.HttpContext.Current.Application["Credential"] = vm.Credential;
                //System.Web.HttpContext.Current.Application.Lock();
                ServerCredential = vm.Credential;
            }
            catch (Exception ex)
            {
                Error = ex.GetExceptionMessages();
            }
        }

        private void ImportUserGroups(ref PrimaryDataViewModel vm)
        {
            if (vm.UserGroups != null && vm.UserGroups.Count() > 0)
            {
                var formMenus = new MenuLinks(new OneDbContext());
                formMenus.ImportUserGroups(vm.UserGroups.ToArray());
                formMenus.SaveChanges();
                Messages = $"Company, User Groups, Menus etc imported successfully.";
            }
        }

        private void GetOldUsers(ref PrimaryDataViewModel vm)
        {
            vm.Credential = ServerCredential;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Membership.Users";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Users = DataReaderMapToList<PrimaryDataViewModel.OldUser>(reader).Select(s => new User
                    {
                        UserID = s.UserID,
                        Username = s.Username,
                        Password = s.Password,
                        Email = s.Email,
                        IsEmailVerified = s.IsEmailVerified,
                        MobileNo = s.MobileNo,
                        IsMobileVerified = s.IsMobileVerified,
                        UserGroupId = s.UserGroupId,
                        ExpiresOn = s.ExpiresOn,
                        isActive = s.isActive,
                        LastLoginDate = s.LastLoginDate,
                        LastActivityDate = s.LastActivityDate,
                        LastPasswordChangedDate = s.LastPasswordChangedDate,
                        FailedPasswordAttemptCount = s.FailedPasswordAttemptCount,
                        CreationDate = DateTime.Now,
                        CreatedBy = SessionHelper.UserId,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = SessionHelper.UserId,
                        IP = s.IP,
                        Theme = s.Theme,
                        NewId = s.NewId,
                        ThemeColor = s.ThemeColor,
                        CustomDateTime = s.CustomDateTime,
                        LoginAllowedIPs = s.LoginAllowedIPs,
                        LoginFromTime = s.LoginFromTime,
                        LoginToTime = s.LoginToTime,
                        Monday = s.Monday,
                        Tuesday = s.Tuesday,
                        Wednesday = s.Wednesday,
                        Thursday = s.Thursday,
                        Friday = s.Friday,
                        Saturday = s.Saturday,
                        Sunday = s.Sunday,
                        Photo = s.Photo,
                    }).ToList();
                }
                command.Dispose();
                cnn.Close();
                ServerCredential = vm.Credential;
            }
            catch (Exception ex)
            {
                Error = ex.GetExceptionMessages();
            }
        }

        public void ImportUsers(ref PrimaryDataViewModel vm)
        {
            db.Users.AddOrUpdate(s => s.UserID, vm.Users.ToArray());
            db.SaveChanges();
            Messages += $"{vm.UserGroups.Count()} Users imported. ";
        }

        private void GetOldBranches(ref PrimaryDataViewModel vm)
        {
            vm.Credential = ServerCredential;
            var info = vm.Info;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Company.Branches";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Branches = DataReaderMapToList<PrimaryDataViewModel.OldBranch>(reader).Select(s => new Branch
                    {
                        BranchId = s.BranchId,
                        Name = s.Name,
                        BranchCode = s.BranchCode,
                        AddressLine1 = s.AddressLine1,
                        AddressLine2 = s.AddressLine2,
                        CityId = s.CityId,
                        CountryId = s.CountryId,
                        PhoneNumber = s.PhoneNumber,
                        EmailAddress = s.EmailAddress,
                        RegPrefix = s.RegPrefix,
                        SettingId = info.SettingId,
                        BranchStartingTime = s.BranchStartingTime,
                        BranchEndingTime = s.BranchEndingTime,
                        CurrencyId = s.CurrencyId,
                        StateId = s.StateId,
                        HasBiometricAttendance = s.HasBiometricAttendance,
                        WarehouseId = s.WarehouseId,
                        AppendFineToNextVoucher = s.AppendFineToNextVoucher,
                        FillVoucherByAllFeeTypes = s.FillVoucherByAllFeeTypes,
                        MergeUnpaidVoucher = s.MergeUnpaidVoucher,
                        UnpaidVoucherFine = s.UnpaidVoucherFine,
                        ShowVoucherStatus = s.ShowVoucherStatus,
                        AllowMultipleVouchersInAMonth = s.AllowMultipleVouchersInAMonth,
                        VoucherNote = s.VoucherNote,
                        VoucherHeaderText = s.VoucherHeaderText,
                        HasStudentAttendance = s.HasStudentAttendance,
                        //Logo = s.Logo,
                        BranchLogoSmall = s.LogoFull,
                        BranchLogoMini = s.WebLogoMini,
                        BranchLogoLarge = s.WebLogoFull,
                        //FeeVoucherRepLogoFull = s.FeeVoucherRepLogoFull,
                        //FeeVoucherWebLogoFull = s.FeeVoucherWebLogoFull,
                        //Location = s.Location,
                        //Longitude = s.Longitude,
                        //Latitude = s.Latitude,
                        //RadiusInMeter = s.RadiusInMeter,
                    }).ToList();
                }
                command.Dispose();
                cnn.Close();
                ServerCredential = vm.Credential;
            }
            catch (Exception ex)
            {
                Error = ex.GetExceptionMessages();
            }
        }

        public void ImportBranches(ref PrimaryDataViewModel vm)
        {
            db.Branches.AddOrUpdate(s => s.BranchId, vm.Branches.ToArray());
            db.SaveChanges();
            Messages += $"{vm.Branches.Count()} Branches imported successfully. ";
        }

        private void GetOldUserBranches(ref PrimaryDataViewModel vm)
        {
            vm.Credential = ServerCredential;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Membership.UserBranches";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.UserBranches = DataReaderMapToList<PrimaryDataViewModel.OldUserBranch>(reader).Select(s => new UserBranch
                    {
                        BranchId = s.BranchId,
                        UserId = s.UserId,
                    }).ToList();
                }
                command.Dispose();
                cnn.Close();
                ServerCredential = vm.Credential;
            }
            catch (Exception ex)
            {
                Error = ex.GetExceptionMessages();
            }
        }

        public void ImportUserBranches(ref PrimaryDataViewModel vm)
        {
            if (vm.UserBranches.Count() > 0)
            {
                db.UserBranches.AddOrUpdate(s => s.UserBranchId, vm.UserBranches.ToArray());
                db.SaveChanges();
            }
            else
            {
                var userGroups = vm.UserGroups.Where(s => s.BranchId.HasValue);
                var maxId = (db.UserBranches.Max(s => (int?)s.UserBranchId) ?? 0) + 1;

                vm.UserBranches = vm.Users.Select(s => new UserBranch
                {
                    BranchId = userGroups.Where(x => x.UserGroupId == s.UserGroupId).Select(x => x.BranchId.Value).FirstOrDefault(),
                    Active = true,
                    DefaultBranch = true,
                    UserBranchId = maxId++,
                    UserId = s.UserID,
                }).ToList();

                db.UserBranches.AddOrUpdate(s => s.UserBranchId, vm.UserBranches.ToArray());
                db.SaveChanges();
            }
            Messages += $"{vm.UserBranches.Count()} Users Branches imported successfully. ";
        }

        public ActionResult ImportData()
        {
            PrimaryDataViewModel vm = new PrimaryDataViewModel();
            vm.Credential = ServerCredential;
            ApplicationPrimaryData obj = new ApplicationPrimaryData(db);
            obj.ImportAllOld(ref vm);
            Messages += vm.Messages;
            Error += vm.Error;
            TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
            TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
            return RedirectToAction("Login");
        }

        public ActionResult ImportFinance()
        {
            if (ServerCredential != null)
            {
                PrimaryDataViewModel vm = new PrimaryDataViewModel();
                vm.Credential = ServerCredential;
                FinancePrimaryData obj = new FinancePrimaryData(db);
                obj.ImportAll(ref vm);

                Messages += vm.Messages;
                Error += vm.Error;
                TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
                TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
            }
            return RedirectToAction("Login");
        }

        public ActionResult ImportHR()
        {
            if (ServerCredential != null)
            {
                PrimaryDataViewModel vm = new PrimaryDataViewModel();
                vm.Credential = ServerCredential;
                HRPrimaryData obj = new HRPrimaryData(db);
                obj.ImportAll(ref vm);
                Error += vm.Error;
                Messages += vm.Messages;
            }

            TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
            TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
            return RedirectToAction("Login");
        }

        public ActionResult ImportPR()
        {
            if (ServerCredential != null)
            {
                PrimaryDataViewModel vm = new PrimaryDataViewModel();
                vm.Credential = ServerCredential;
                var obj = new PRPrimaryData(db);
                obj.ImportAll(ref vm);
                Error += vm.Error;
                Messages += vm.Messages;
            }

            TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
            TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
            return RedirectToAction("Login");
        }

        //public ActionResult ImportAcademics()
        //{
        //    if (ServerCredential != null)
        //    {
        //        PrimaryDataViewModel vm = new PrimaryDataViewModel();
        //        vm.Credential = ServerCredential;
        //        var obj = new AcademicsPrimaryData(db);
        //        obj.ImportAll(ref vm);
        //        Error += vm.Error;
        //        Messages += vm.Messages;
        //    }

        //    TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
        //    TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
        //    return RedirectToAction("Login");
        //}

        //public ActionResult ImportFee()
        //{
        //    if (ServerCredential != null)
        //    {
        //        PrimaryDataViewModel vm = new PrimaryDataViewModel();
        //        vm.Credential = ServerCredential;
        //        var obj = new FeePrimaryData(db);
        //        obj.ImportAll(ref vm);
        //        Error += vm.Error;
        //        Messages += vm.Messages;
        //    }

        //    TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
        //    TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
        //    return RedirectToAction("Login");
        //}

        public ActionResult ImportTransport()
        {
            if (ServerCredential != null)
            {
                PrimaryDataViewModel vm = new PrimaryDataViewModel();
                vm.Credential = ServerCredential;
                var obj = new TransportPrimaryData(db);
                obj.ImportAll(ref vm);
                Error += vm.Error;
                Messages += vm.Messages;
            }

            TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
            TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
            return RedirectToAction("Login");
        }

        //public ActionResult ImportHostel()
        //{
        //    if (ServerCredential != null)
        //    {
        //        PrimaryDataViewModel vm = new PrimaryDataViewModel();
        //        vm.Credential = ServerCredential;
        //        var obj = new HostelPrimaryData(db);
        //        obj.ImportAll(ref vm);
        //        Error += vm.Error;
        //        Messages += vm.Messages;
        //    }

        //    TempData["Error"] = string.IsNullOrEmpty(Error) ? null : Error.Replace(".", ".</br>");
        //    TempData["Success"] = string.IsNullOrEmpty(Messages) ? null : Messages.Replace(".", ".</br>");
        //    return RedirectToAction("Login");
        //}

        //convert datareader to object
        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public string GetModelStateErrors()
        {
            return string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
        }

        public ActionResult ChooseAction(bool id)
        {
            System.Web.HttpContext.Current.Application.UnLock();
            System.Web.HttpContext.Current.Application["CreateNewData"] = id;
            System.Web.HttpContext.Current.Application.Lock();
            return RedirectToAction("Login");
        }

        private byte[] GetBytesByFiles(HttpPostedFile file)
        {
            if (file != null)
            {
                var reader = new System.IO.BinaryReader(file.InputStream);
                return reader.ReadBytes(file.ContentLength);
            }
            return null;
        }

        public ActionResult GetLogo(string id)
        {
            string logotype = "WebLogoMini";
            string contentType = "image/jpeg";

            if (!string.IsNullOrWhiteSpace(id))
            {
                switch (id)
                {
                    case "RLF": logotype = "LogoFull"; break;
                    case "RLM": logotype = "Logo"; break;
                    case "WLM": logotype = "WebLogoMini"; break;
                    case "WLF": logotype = "WebLogoFull"; break;
                    default: logotype = "WebLogoMini"; break;
                }
            }

            string filename = Server.MapPath("~/uploads/Logos/" + logotype + ".png");
            if (System.IO.File.Exists(filename))
            {
                return File(filename, contentType);
            }
            else
            {
                var comp = db.Info.FirstOrDefault();

                try
                {
                    byte[] logo;
                    switch (id)
                    {
                        case "RLF":
                            logo = comp.LogoFull.ToArray();
                            break;
                        case "RLM":
                            logo = comp.Logo.ToArray();
                            break;
                        case "WLM":
                            logo = comp.WebLogoMini.ToArray();
                            break;
                        default:
                            logo = comp.WebLogoFull.ToArray();
                            break;
                    }
                    using (var fs = new System.IO.FileStream(filename, System.IO.FileMode.CreateNew))
                    {
                        fs.Write(logo, 0, logo.Length);
                        fs.Close();
                    }
                    return File(filename, contentType);
                }
                catch
                {
                    using (var rectangleFont = new Font("Tahoma", 24, FontStyle.Bold))
                    using (var bitmap = new Bitmap(600, 64, PixelFormat.Format32bppRgb))
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        g.TextContrast = 3;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        var backgroundColor = Color.White;
                        g.Clear(backgroundColor);

                        var text = comp.CompanyName;
                        // Measure string.
                        SizeF stringSize = new SizeF();
                        stringSize = g.MeasureString(text, rectangleFont);

                        g.DrawString(text, rectangleFont, SystemBrushes.WindowText, new PointF(10, 32 - (stringSize.Height / 2)));

                        contentType = "image/png";
                        return File(filename, contentType);
                    }
                }
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var dc = new OneDbContext())
                {
                    var user = await dc.Users.FindAsync(SessionHelper.UserId);
                    if (user != null)
                    {
                        var currentPassword = FTC.Classes.Security.Encrypt(model.CurrentPassword, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                        if (user.Password == currentPassword)
                        {
                            user.Password = FTC.Classes.Security.Encrypt(model.NewPassword, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
                            await dc.SaveChangesAsync();
                            TempData["Success"] = "Password changed sucessfully";
                        }
                        else
                        {
                            TempData["Error"] = "Incorrect current password.";
                        }
                    }
                }
                return View();
            }
            return View();
        }


        public ActionResult GetUserApps()
        {
            var UserGroupId = SessionHelper.UserGroupId;
            var menu = db.Database.SqlQuery<FAPP.ViewModel.UserAppVM>($@"select app.ApplicationTitle, app.AppUrl, app.ShortName, AppIcon from System.UserGroupApplications ugp
                           join System.Applications app on app.ApplicationId=ugp.ApplicationId where ugp.Usergroupid={UserGroupId} and ugp.isactive=1 and app.ApplicationId not in ({ApplicationId})").ToList();
            return PartialView("_UserApps", menu);
        }
        public ActionResult sso(string q)
        {
            SessionHelper.Token = q;
            EncryptionDecryption _encryptionDecryption = new EncryptionDecryption();
            Settings _settings = new Settings();
            q = _encryptionDecryption.DecodeFrom64(q);
            string UserName = _settings.GetUserNameByToken(q);
            string Password = _settings.GetUserPasswordByToken(q);
            string returnUrl = null;
            ViewBag.LogoImage = db.Info.Select(u => u.WebLogoFull).FirstOrDefault();
            ViewBag.CompanyName = db.Info.Select(u => u.CompanyName).FirstOrDefault();

            string username = UserName.ToLower();
            //string EncryptPass = FTC.Classes.Security.Encrypt("admin", XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
            string pass = FTC.Classes.Security.Encrypt(Password, XCore.INIT_VECTOR, XCore.PASS_PHRASE, XCore.KEY_SIZE);
            //string pass = Password;
            var _users = db.Users.Include(s => s.UserGroup).Include(s => s.Branch.Country)
                         .Where(u => (u.Username == username) && u.Password == pass)
                         .Select(u => new
                         {
                             UserID = u.UserID,
                             Email = u.Email,
                             Username = u.Username,
                             UserGroupId = u.UserGroupId,
                             UserGroupName = u.UserGroup.UserGroupName,
                             ExpiresOn = u.ExpiresOn,
                             BranchId = 0,
                             BranchName = "",
                             BranchCode = "",
                             BranchAddress = "",

                             //RegPrefix = u.UserGroup.Branch.Info.RegPrefix,
                             //ProductKey = u.UserGroup.Branch.Info.ProductKey,
                             //u.UserGroup.Branch.Info.ShortName,
                             //PhoneNumber = u.UserGroup.Branch.PhoneNumber,
                             //GST = u.UserGroup.Branch.Info.GST,
                             //CompanyName = u.UserGroup.Branch.Info.CompanyName,
                             LastLoginDate = u.LastLoginDate,
                             Theme = u.Theme,
                             ThemeColor = u.ThemeColor,
                             u.MobileNo,
                             u.IsEmailVerified,
                             u.IsMobileVerified,
                             HomepageUrl = u.UserGroup.Form != null ? u.UserGroup.Form.FormURL : "",
                             CustomDateTime = u.CustomDateTime,
                             Active = u.isActive,
                             //Photo = null, u.Photo, do not include like this
                             StartUpPage = db.UserGroupApplications.Where(x => x.ApplicationId == ApplicationId && x.UserGroupId == u.UserGroupId).Select(x => x.DashboardUrl).FirstOrDefault(),
                             IsMasterUser = u.IsMasterUser,
                             CallingCode = (u.Branch.CountryId != null ? u.Branch.Country.CallingCode : ""),
                             PhoneNoLength = (u.Branch.CountryId != null ? u.Branch.Country.PhoneNoLength : 0)
                         }).FirstOrDefault();

            if (_users != null)
            {
                var user = _users;
                if (user != null)
                {

                    var branch = db.UserBranches.Include(b => b.Branch).Where(k => k.UserId == user.UserID && k.DefaultBranch == true && k.Active == true).Select(k => new { k.BranchId, k.Branch.IsMasterBranch }).FirstOrDefault();
                    if (branch != null)
                    {


                        //---------------------------
                        SessionHelper.UserID = user.UserID;
                        SessionHelper.Username = user.Username;
                        SessionHelper.IsMasterUser = user.IsMasterUser;
                        SessionHelper.UserGroupId = user.UserGroupId;
                        SessionHelper.DiallingCode = user.CallingCode == "" ? null : user.CallingCode;
                        SessionHelper.PhoneNoLength = user.PhoneNoLength;
                        SessionHelper.DashboardUrl = user.StartUpPage;
                        SessionHelper.UserGroupName = user.UserGroupName;
                        SessionHelper.ShowExceptionToUser = db.Info.Select(u => u.ShowExceptionToUser).FirstOrDefault();
                        SessionHelper.CompanyCode = db.Info.Select(u => u.CompanyCode).FirstOrDefault();



                        SessionHelper.BranchId = branch.BranchId;
                        SessionHelper.IsMasterBranch = branch.IsMasterBranch;
                        if (SessionHelper.BranchId > 0)
                        {
                            var UrlSetting = new UrlSetting();
                            UrlSetting = db.UrlSettings.FirstOrDefault();
                            if (UrlSetting == null)
                            {
                                UrlSetting = db.Database.SqlQuery<UrlSetting>($@"
                                            INSERT INTO Company.UrlSettings (
	                                            BranchId
	                                            , isSecure
	                                            )
                                            VALUES (
	                                            {SessionHelper.BranchId}
	                                            , 1
	                                            )

                                            SELECT *
                                            FROM Company.UrlSettings").FirstOrDefault();
                                SessionHelper.Urlapi = UrlSetting.URLapi;
                                SessionHelper.UrlStudent = UrlSetting.URLStudent;
                                SessionHelper.UrlEmployee = UrlSetting.URLEmployee;
                            }
                            ProceduresModel.ResetSessionHelper_Branch(db, null, SessionHelper.BranchId, SessionHelper.UserId);
                        }
                        else
                        {
                            Session.Abandon();
                            ViewBag.Error = "Branch not found";
                            return View();
                        }
                        //SessionHelper.UserPhoto = user.Photo;
                        SessionHelper.ThemeColor = user.ThemeColor;

                        //CurrentComment

                        // ProceduresModel.SessionNotes(cmdb);
                        UserLog _UserLog = new UserLog();
                        _UserLog.UserId = SessionHelper.UserID;
                        _UserLog.LoginTime = DateTime.Now;
                        SessionHelper.IP = _UserLog.IP = HttpContext.Request.UserHostAddress;
                        _UserLog.BranchId = Convert.ToInt16(SessionHelper.BranchId);

                        db.UserLogs.Add(_UserLog);
                        db.SaveChanges();
                        //var res = db.UserLogs.Where(u => u.UserId == SessionHelper.UserID).OrderByDescending(u => u.LoginTime).Take(1).FirstOrDefault();
                        SessionHelper.UserLogId = _UserLog.UserLogId;

                        //session = (from s in db.Sessions
                        //           join us in db.UserSessions on s.SessionId equals us.SessionId
                        //           where us.UserId == _users.UserID && us.Default == true && us.Assigned == true && s.BranchId == SessionHelper.BranchId
                        //           select s).FirstOrDefault();

                        //if (session != null)
                        //{
                        //    SessionHelper.CurrentSession = session?.SessionName ?? string.Empty;
                        //    SessionHelper.CurrentSessionId = session?.SessionId ?? Guid.Empty;
                        //    SessionHelper.SessionYearEndDate = session.EndTime;
                        //    SessionHelper.SessionYearStartDate = session.StartTime;
                        //    var term = db.Terms
                        //        .Where(s => s.SessionId == session.SessionId && s.BranchId == SessionHelper.BranchId && s.IsActive == true)
                        //        .Select(s => new
                        //        {
                        //            TermName = s.TermName
                        //        }).FirstOrDefault();
                        //    SessionHelper.CurrentTerm = term?.TermName;

                        //    //nullify outdated reg nos
                        //    db.Database.ExecuteSqlCommand(@"
                        //    --no admission or admission removed due to some reason
                        //    UPDATE Academics.Students
                        //    SET RegistrationNumber = NULL, ProfileId = NULL
                        //    FROM Academics.Students
                        //    LEFT OUTER JOIN Academics.StudentAdmission ON Academics.StudentAdmission.StudentId = Academics.Students.StudentId
                        //    WHERE (Academics.StudentAdmission.AdmissionId IS NULL)
                        //     AND (Academics.Students.RegistrationNumber IS NOT NULL)

                        //    --admission changed but reg no not updated in student table
                        //    UPDATE Academics.Students
                        //    SET RegistrationNumber = NULL
                        //    FROM Academics.Students
                        //    INNER JOIN Academics.StudentAdmission ON Academics.StudentAdmission.StudentId = Academics.Students.StudentId
                        //     AND Academics.StudentAdmission.RegistrationNo <> Academics.Students.RegistrationNumber
                        //     AND Academics.Students.RegistrationNumber IS NOT NULL
                        //     --AND StudentAdmission.Active = 1 --later

                        //    UPDATE Academics.Students
                        //    SET RegistrationNumber = Academics.StudentAdmission.RegistrationNo
                        //    FROM Academics.StudentAdmission
                        //    INNER JOIN Academics.Students ON Academics.StudentAdmission.StudentId = Academics.Students.StudentId
                        //    WHERE (Academics.StudentAdmission.Active = 1)
                        //     AND RegistrationNumber IS NULL");
                        //}

                        //var setting = db.AcademicsSettings.Find(SessionHelper.BranchId);
                        //if (setting == null)
                        //{
                        //    setting = new AcademicsSetting
                        //    {
                        //        BranchId = SessionHelper.BranchId,
                        //        UppercaseStudentData = false,
                        //        AutoApplyDiscount = false
                        //    };
                        //    db.AcademicsSettings.Add(setting);
                        //    db.SaveChanges();
                        //}

                        //    SessionHelper.UppercaseStudentData = setting.UppercaseStudentData;

                        if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl != "/Account/Logout" && returnUrl != "/")
                        {
                            return Redirect(returnUrl);
                        }
                        else if (!string.IsNullOrEmpty(_users.StartUpPage))
                        {
                            return Redirect(_users.StartUpPage);
                        }
                        else if (!db.Forms.Any())
                        {
                            return Redirect("/Setup/Applications/Manage");
                        }
                        else
                        {
                            return Redirect(url);
                        }
                    }
                    else
                    {
                        lblError = "User must have a default branch.";
                    }
                }
                else
                {
                    lblError = "The user name or password provided is incorrect.";
                }
            }
            else
            {
                lblError = "The user name or password provided is incorrect.";
            }
            //ModelState.AddModelError("Errors", lblError);
            ViewBag.Error = lblError;
            return View();
        }


    }

    public class EncryptionDecryption
    {
        public string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch
            {
                return "error";
            }
        }
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}


public class Settings
{
    public string GetUserNameByToken(string _bearer_token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(_bearer_token);
        var tokenS = handler.ReadToken(_bearer_token) as JwtSecurityToken;
        var CheckToken = tokenS.Claims.Where(claim => claim.Type.Contains("name")).FirstOrDefault();
        if (CheckToken != null)
        {
            return CheckToken.Value;
        }
        else
        {
            return null;
        }
    }

    public string GetUserPasswordByToken(string _bearer_token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(_bearer_token);
        var tokenS = handler.ReadToken(_bearer_token) as JwtSecurityToken;
        var CheckToken = tokenS.Claims.Where(claim => claim.Type.Contains("role")).FirstOrDefault();
        if (CheckToken != null)
        {
            return CheckToken.Value;
        }
        else
        {
            return null;
        }
    }
}
