using FAPP.ViewModel.FormModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace FAPP.DAL
{
    public static class SessionHelper
    {
        public static List<FormsViewModel> MenuList
        {
            get
            {
                return HttpContext.Current.Session["MenuList"] != null ? (List<FormsViewModel>)HttpContext.Current.Session["MenuList"] : new List<FormsViewModel>();
            }
            set { HttpContext.Current.Session["MenuList"] = value; }
        }
        public static string Token
        {
            get
            {
                return HttpContext.Current.Session["Token"] != null ? HttpContext.Current.Session["Token"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["Token"] = value; }
        }
        public static string CurrencySymbol
        {
            get
            {
                return HttpContext.Current.Session["CurrencySymbol"] != null ? HttpContext.Current.Session["CurrencySymbol"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CurrencySymbol"] = value; }
        }
        public static bool ResetSessionVarriables
        {
            get
            {
                if (HttpContext.Current.Session["ResetSessionVarriables"] != null)
                    return bool.Parse(HttpContext.Current.Session["ResetSessionVarriables"].ToString());
                else
                    return false;
            }
            set { HttpContext.Current.Session["ResetSessionVarriables"] = value; }
        }
        public static short BranchId
        {
            get
            {
                return HttpContext.Current.Session["BranchId"] != null ? Convert.ToInt16(HttpContext.Current.Session["BranchId"]) : (short)0;
            }
            set { HttpContext.Current.Session["BranchId"] = value; }
        }
        public static int ModuleID
        {
            get
            {
                return HttpContext.Current.Session["ModuleID"] != null ? Convert.ToInt32(HttpContext.Current.Session["ModuleID"]) : (int)0;
            }
            set { HttpContext.Current.Session["ModuleID"] = value; }
        }
        public static int UserID
        {
            get
            {
                return HttpContext.Current.Session["UserID"] != null ? Convert.ToInt32(HttpContext.Current.Session["UserID"]) : (int)0;
            }
            set { HttpContext.Current.Session["UserID"] = value; }
        }

        public static int UserLogId
        {
            get
            {
                return HttpContext.Current.Session["UserLogId"] != null ? Convert.ToInt32(HttpContext.Current.Session["UserLogId"]) : (int)0;
            }
            set { HttpContext.Current.Session["UserLogId"] = value; }
        }

        public static int CurrentSessionStrength
        {
            get
            {
                return HttpContext.Current.Session["CurrentSessionStrength"] != null ? Convert.ToInt32(HttpContext.Current.Session["CurrentSessionStrength"]) : (int)0;
            }
            set { HttpContext.Current.Session["CurrentSessionStrength"] = value; }
        }

        public static string DomainName
        {
            get
            {
                return HttpContext.Current.Session["DomainName"] != null ? HttpContext.Current.Session["DomainName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DomainName"] = value; }
        }



        public static string Username
        {
            get
            {
                return HttpContext.Current.Session["Username"] != null ? HttpContext.Current.Session["Username"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["Username"] = value; }
        }
        public static string DropboxToken
        {
            get
            {
                return HttpContext.Current.Session["DropboxToken"] != null ? HttpContext.Current.Session["DropboxToken"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DropboxToken"] = value; }
        }

        public static string CompanyCode
        {
            get
            {
                return HttpContext.Current.Session["CompanyCode"] != null ? HttpContext.Current.Session["CompanyCode"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CompanyCode"] = value; }
        }



        public static int? UserGroupId
        {
            get
            {
                if (HttpContext.Current.Session["UserGroupId"] != null)
                    return int.Parse(HttpContext.Current.Session["UserGroupId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["UserGroupId"] = value; }
        }
        public static int? PhoneNoLength
        {
            get
            {
                if (HttpContext.Current.Session["PhoneNoLength"] != null)
                    return int.Parse(HttpContext.Current.Session["PhoneNoLength"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["PhoneNoLength"] = value; }
        }
        public static string DiallingCode
        {
            get
            {
                return HttpContext.Current.Session["DiallingCode"] != null ? HttpContext.Current.Session["DiallingCode"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DiallingCode"] = value; }
        }

        public static string BranchName
        {
            get
            {
                return HttpContext.Current.Session["BranchName"] != null ? HttpContext.Current.Session["BranchName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BranchName"] = value; }
        }

        public static string CompanyName
        {
            get
            {
                return HttpContext.Current.Session["CompanyName"] != null ? HttpContext.Current.Session["CompanyName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CompanyName"] = value; }
        }

        public static bool IsMasterBranch
        {
            get
            {
                return HttpContext.Current.Session["IsMasterBranch"] != null ? (bool)HttpContext.Current.Session["IsMasterBranch"] : false;
            }
            set { HttpContext.Current.Session["IsMasterBranch"] = value; }
        }

        public static bool IsMasterUser
        {
            get
            {
                return HttpContext.Current.Session["IsMasterUser"] != null ? (bool)HttpContext.Current.Session["IsMasterUser"] : false;
            }
            set { HttpContext.Current.Session["IsMasterUser"] = value; }
        }


        public static string BranchAddress
        {
            get
            {
                return HttpContext.Current.Session["BranchAddress"] != null ? HttpContext.Current.Session["BranchAddress"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BranchAddress"] = value; }
        }
        public static string UserGroupName
        {
            get
            {
                return HttpContext.Current.Session["UserGroupName"] != null ? HttpContext.Current.Session["UserGroupName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UserGroupName"] = value; }
        }

        public static int? BranchCode
        {
            get
            {
                if (HttpContext.Current.Session["BranchCode"] != null)
                    return int.Parse(HttpContext.Current.Session["BranchCode"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["BranchCode"] = value; }
        }
        public static int? SupervisorId
        {
            get
            {
                if (HttpContext.Current.Session["SupervisorId"] != null)
                    return int.Parse(HttpContext.Current.Session["SupervisorId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["SupervisorId"] = value; }
        }
        public static int? DeptSupervisorId
        {
            get
            {
                if (HttpContext.Current.Session["DeptSupervisorId"] != null)
                    return int.Parse(HttpContext.Current.Session["DeptSupervisorId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["DeptSupervisorId"] = value; }
        }
        public static string connectionId
        {
            get
            {
                return HttpContext.Current.Session["connectionId"] != null ? HttpContext.Current.Session["connectionId"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["connectionId"] = value; }
        }

        public static string SessionId
        {
            get
            {
                return HttpContext.Current.Session["SessionId"] != null ? HttpContext.Current.Session["SessionId"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SessionId"] = value; }
        }

        public static string SessionName
        {
            get
            {
                return HttpContext.Current.Session["SessionName"] != null ? HttpContext.Current.Session["SessionName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SessionName"] = value; }
        }
        public static int FiscalYearId
        {
            get
            {
                return HttpContext.Current.Session["FiscalYearId"] != null ? Convert.ToInt32(HttpContext.Current.Session["FiscalYearId"]) : (int)0;
            }
            set { HttpContext.Current.Session["FiscalYearId"] = value; }
        }

        public static int? DepartmentId
        {
            get
            {
                if (HttpContext.Current.Session["DepartmentId"] != null)
                    return int.Parse(HttpContext.Current.Session["DepartmentId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["DepartmentId"] = value; }
        }

        public static byte[] UserPhoto
        {
            get
            {
                return HttpContext.Current.Session["UserPhoto"] != null ? (byte[])HttpContext.Current.Session["UserPhoto"] : null;
            }
            set { HttpContext.Current.Session["UserPhoto"] = value; }
        }

        public static byte[] SenderPhoto
        {
            get
            {
                return HttpContext.Current.Session["SenderPhoto"] != null ? (byte[])HttpContext.Current.Session["SenderPhoto"] : null;
            }
            set { HttpContext.Current.Session["SenderPhoto"] = value; }
        }
        public static string TeacherName
        {
            get
            {
                return HttpContext.Current.Session["TeacherName"] != null ? HttpContext.Current.Session["TeacherName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["TeacherName"] = value; }
        }

        public static string ThemeColor
        {
            get
            {
                return HttpContext.Current.Session["ThemeColor"] != null ? HttpContext.Current.Session["ThemeColor"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["ThemeColor"] = value; }
        }
        public static string FiscalYearName
        {
            get
            {
                return HttpContext.Current.Session["FiscalYearName"] != null ? HttpContext.Current.Session["FiscalYearName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["FiscalYearName"] = value; }
        }
        public static DateTime FiscalStartDate
        {
            get
            {
                return HttpContext.Current.Session["StartDate"] != null ? Convert.ToDateTime(HttpContext.Current.Session["StartDate"].ToString()) : DateTime.Now;
            }
            set { HttpContext.Current.Session["StartDate"] = value; }
        }
        public static DateTime FiscalEndDate
        {
            get
            {
                return HttpContext.Current.Session["FiscalEndDate"] != null ? Convert.ToDateTime(HttpContext.Current.Session["FiscalEndDate"].ToString()) : DateTime.Now;
            }
            set { HttpContext.Current.Session["FiscalEndDate"] = value; }
        }


        public static string BranchPhone
        {
            get
            {
                return HttpContext.Current.Session["BranchPhone"] != null ? HttpContext.Current.Session["BranchPhone"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BranchPhone"] = value; }
        }

        public static string Href
        {
            get
            {
                return HttpContext.Current.Session["Href"] != null ? HttpContext.Current.Session["Href"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["Href"] = value; }
        }

        public static string OPDNo
        {
            get
            {
                return HttpContext.Current.Session["OPDNo"] != null ? HttpContext.Current.Session["OPDNo"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["OPDNo"] = value; }
        }

        public static int? ServiceTypeId
        {
            get
            {
                if (HttpContext.Current.Session["ServiceTypeId"] != null)
                    return int.Parse(HttpContext.Current.Session["ServiceTypeId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ServiceTypeId"] = value; }
        }

        public static string SaleCounterSessionIdPOSCounterSession
        {
            get
            {
                return HttpContext.Current.Session["SaleCounterSessionIdPOSCounterSession"] != null ? HttpContext.Current.Session["SaleCounterSessionIdPOSCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SaleCounterSessionIdPOSCounterSession"] = value; }
        }
        public static string CounterNamePOSCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CounterNamePOSCounterSession"] != null ? HttpContext.Current.Session["CounterNamePOSCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CounterNamePOSCounterSession"] = value; }
        }
        public static string CashAccountIdPOSCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CashAccountIdPOSCounterSession"] != null ? HttpContext.Current.Session["CashAccountIdPOSCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CashAccountIdPOSCounterSession"] = value; }
        }
        public static string BankAccountIdPOSCounterSession
        {
            get
            {
                return HttpContext.Current.Session["BankAccountIdPOSCounterSession"] != null ? HttpContext.Current.Session["BankAccountIdPOSCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BankAccountIdPOSCounterSession"] = value; }
        }
        public static int? ClientIdPOSCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["ClientIdPOSCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["ClientIdPOSCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ClientIdPOSCounterSession"] = value; }
        }
        public static int? WareHouseIdPOSCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["WareHouseIdPOSCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["WareHouseIdPOSCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["WareHouseIdPOSCounterSession"] = value; }
        }

        public static int? WareHouseIdResCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["WareHouseIdResCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["WareHouseIdResCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["WareHouseIdResCounterSession"] = value; }
        }
        public static string SaleCounterSessionIdResCounterSession
        {
            get
            {
                return HttpContext.Current.Session["SaleCounterSessionIdResCounterSession"] != null ? HttpContext.Current.Session["SaleCounterSessionIdResCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SaleCounterSessionIdResCounterSession"] = value; }
        }
        public static string CounterNameResCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CounterNameResCounterSession"] != null ? HttpContext.Current.Session["CounterNameResCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CounterNameResCounterSession"] = value; }
        }
        public static string CashAccountIdResCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CashAccountIdResCounterSession"] != null ? HttpContext.Current.Session["CashAccountIdResCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CashAccountIdResCounterSession"] = value; }
        }
        public static string BankAccountIdResCounterSession
        {
            get
            {
                return HttpContext.Current.Session["BankAccountIdResCounterSession"] != null ? HttpContext.Current.Session["BankAccountIdResCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BankAccountIdResCounterSession"] = value; }
        }
        public static int? ClientIdResCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["ClientIdResCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["ClientIdResCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ClientIdResCounterSession"] = value; }
        }

        public static int? WareHouseIdAMCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["WareHouseIdAMCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["WareHouseIdAMCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["WareHouseIdAMCounterSession"] = value; }
        }
        public static string SaleCounterSessionIdAMCounterSession
        {
            get
            {
                return HttpContext.Current.Session["SaleCounterSessionIdAMCounterSession"] != null ? HttpContext.Current.Session["SaleCounterSessionIdAMCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SaleCounterSessionIdAMCounterSession"] = value; }
        }
        public static string CounterNameAMCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CounterNameAMCounterSession"] != null ? HttpContext.Current.Session["CounterNameAMCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CounterNameAMCounterSession"] = value; }
        }
        public static string CashAccountIdAMCounterSession
        {
            get
            {
                return HttpContext.Current.Session["CashAccountIdAMCounterSession"] != null ? HttpContext.Current.Session["CashAccountIdAMCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CashAccountIdAMCounterSession"] = value; }
        }
        public static string BankAccountIdAMCounterSession
        {
            get
            {
                return HttpContext.Current.Session["BankAccountIdAMCounterSession"] != null ? HttpContext.Current.Session["BankAccountIdAMCounterSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["BankAccountIdAMCounterSession"] = value; }
        }
        public static int? ClientIdAMCounterSession
        {
            get
            {
                if (HttpContext.Current.Session["ClientIdAMCounterSession"] != null)
                    return int.Parse(HttpContext.Current.Session["ClientIdAMCounterSession"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ClientIdAMCounterSession"] = value; }
        }

        public static decimal? GstTax_Res
        {
            get
            {
                if (HttpContext.Current.Session["GstTax_Res"] != null)
                    return decimal.Parse(HttpContext.Current.Session["GstTax_Res"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["GstTax_Res"] = value; }
        }

        public static string GstTaxAccountId_Res
        {
            get
            {
                return HttpContext.Current.Session["GstTaxAccountId_Res"] != null ? HttpContext.Current.Session["GstTaxAccountId_Res"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["GstTaxAccountId_Res"] = value; }
        }

        public static string DiscountAccountId_Res
        {
            get
            {
                return HttpContext.Current.Session["DiscountAccountId_Res"] != null ? HttpContext.Current.Session["DiscountAccountId_Res"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DiscountAccountId_Res"] = value; }
        }

        public static decimal? ServiceCharges_Res
        {
            get
            {
                if (HttpContext.Current.Session["ServiceCharges_Res"] != null)
                    return decimal.Parse(HttpContext.Current.Session["ServiceCharges_Res"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ServiceCharges_Res"] = value; }
        }

        public static string ServiceChargesAccountId_Res
        {
            get
            {
                return HttpContext.Current.Session["ServiceChargesAccountId_Res"] != null ? HttpContext.Current.Session["ServiceChargesAccountId_Res"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["ServiceChargesAccountId_Res"] = value; }
        }

        public static string PurchaseNote
        {
            get
            {
                return HttpContext.Current.Session["PurchaseNote"] != null ? HttpContext.Current.Session["PurchaseNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["PurchaseNote"] = value; }
        }

        public static string SaleNote
        {
            get
            {
                return HttpContext.Current.Session["SaleNote"] != null ? HttpContext.Current.Session["SaleNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SaleNote"] = value; }
        }

        public static string DeliveryChallanNote
        {
            get
            {
                return HttpContext.Current.Session["DeliveryChallanNote"] != null ? HttpContext.Current.Session["DeliveryChallanNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DeliveryChallanNote"] = value; }
        }
        public static string WorkOrderNote
        {
            get
            {
                return HttpContext.Current.Session["WorkOrderNote"] != null ? HttpContext.Current.Session["WorkOrderNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["WorkOrderNote"] = value; }
        }

        public static string SaleReturnNote
        {
            get
            {
                return HttpContext.Current.Session["SaleReturnNote"] != null ? HttpContext.Current.Session["SaleReturnNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["SaleReturnNote"] = value; }
        }

        public static string PurchaseReturnNote
        {
            get
            {
                return HttpContext.Current.Session["PurchaseReturnNote"] != null ? HttpContext.Current.Session["PurchaseReturnNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["PurchaseReturnNote"] = value; }
        }

        public static string OrderNote
        {
            get
            {
                return HttpContext.Current.Session["OrderNote"] != null ? HttpContext.Current.Session["OrderNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["OrderNote"] = value; }
        }

        public static bool IsRestaurantAutomation
        {
            get
            {
                return HttpContext.Current.Session["IsRestaurantAutomation"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsRestaurantAutomation"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsRestaurantAutomation"] = value; }
        }

        public static decimal GST
        {
            get
            {
                if (HttpContext.Current.Session["GST"] != null)
                    return decimal.Parse(HttpContext.Current.Session["GST"].ToString());
                else
                    return 0;
            }
            set { HttpContext.Current.Session["GST"] = value; }
        }

        public static decimal TaxForRoomService
        {
            get
            {
                if (HttpContext.Current.Session["TaxForRoomService"] != null)
                    return decimal.Parse(HttpContext.Current.Session["TaxForRoomService"].ToString());
                else
                    return 0;
            }
            set { HttpContext.Current.Session["TaxForRoomService"] = value; }
        }

        public static decimal GST_POS
        {
            get
            {
                if (HttpContext.Current.Session["GST_POS"] != null)
                    return decimal.Parse(HttpContext.Current.Session["GST_POS"].ToString());
                else
                    return 0;
            }
            set { HttpContext.Current.Session["GST_POS"] = value; }
        }

        public static int? RoomServiceId
        {
            get
            {
                if (HttpContext.Current.Session["RoomServiceId"] != null)
                    return Int32.Parse(HttpContext.Current.Session["RoomServiceId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["RoomServiceId"] = value; }
        }

        public static string GstAccountId_FD
        {
            get
            {
                if (HttpContext.Current.Session["GstAccountId_FD"] != null)
                    return HttpContext.Current.Session["GstAccountId_FD"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["GstAccountId_FD"] = value; }
        }
        public static string DiscountAccountId_FD
        {
            get
            {
                if (HttpContext.Current.Session["DiscountAccountId_FD"] != null)
                    return HttpContext.Current.Session["DiscountAccountId_FD"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["DiscountAccountId_FD"] = value; }
        }

        public static string GstAccountId_POS
        {
            get
            {
                if (HttpContext.Current.Session["GstAccountId_POS"] != null)
                    return HttpContext.Current.Session["GstAccountId_POS"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["GstAccountId_POS"] = value; }
        }
        public static string DiscountAccountId_POS
        {
            get
            {
                if (HttpContext.Current.Session["DiscountAccountId_POS"] != null)
                    return HttpContext.Current.Session["DiscountAccountId_POS"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["DiscountAccountId_POS"] = value; }
        }


        public static string PurchaseDiscountAccountId_POS
        {
            get
            {
                if (HttpContext.Current.Session["PurchaseDiscountAccountId_POS"] != null)
                    return HttpContext.Current.Session["PurchaseDiscountAccountId_POS"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["PurchaseDiscountAccountId_POS"] = value; }
        }


        public static string CommisionAccountId_POS
        {
            get
            {
                if (HttpContext.Current.Session["CommisionAccountId_POS"] != null)
                    return HttpContext.Current.Session["CommisionAccountId_POS"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["CommisionAccountId_POS"] = value; }
        }



        public static int? KotServiceId
        {
            get
            {
                if (HttpContext.Current.Session["KotServiceId"] != null)
                    return Int32.Parse(HttpContext.Current.Session["KotServiceId"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["KotServiceId"] = value; }
        }

        public static string CommisionAccountId
        {
            get
            {
                if (HttpContext.Current.Session["CommisionAccountId"] != null)
                    return HttpContext.Current.Session["CommisionAccountId"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["CommisionAccountId"] = value; }
        }
        public static string IP
        {
            get
            {
                if (HttpContext.Current.Session["IP"] != null)
                    return HttpContext.Current.Session["IP"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["IP"] = value; }
        }

        public static string MenuString
        {
            get
            {
                if (HttpContext.Current.Session["MenuString"] != null)
                    return HttpContext.Current.Session["MenuString"].ToString();
                else
                    return null;
            }
            set { HttpContext.Current.Session["MenuString"] = value; }
        }

        public static bool IsIndividualItemAccounts_POS
        {
            get
            {
                return HttpContext.Current.Session["IsIndividualItemAccounts_POS"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsIndividualItemAccounts_POS"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsIndividualItemAccounts_POS"] = value; }
        }

        public static bool IsHideHeaderFooterInPrint_POS
        {
            get
            {
                return HttpContext.Current.Session["IsHideHeaderFooterInPrint_POS"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsHideHeaderFooterInPrint_POS"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsHideHeaderFooterInPrint_POS"] = value; }
        }

        public static bool IsIndividualItemAccounts_FrontDesk
        {
            get
            {
                return HttpContext.Current.Session["IsIndividualItemAccounts_FrontDesk"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsIndividualItemAccounts_FrontDesk"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsIndividualItemAccounts_FrontDesk"] = value; }
        }


        public static bool IsIndividualItemAccounts_Res
        {
            get
            {
                return HttpContext.Current.Session["IsIndividualItemAccounts_Res"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsIndividualItemAccounts_Res"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsIndividualItemAccounts_Res"] = value; }
        }

        public static bool IsIndividualItemAccounts_Shop
        {
            get
            {
                return HttpContext.Current.Session["IsIndividualItemAccounts_Shop"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsIndividualItemAccounts_Shop"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsIndividualItemAccounts_Shop"] = value; }
        }

        public static bool IsIndividualItemAccounts_AM
        {
            get
            {
                return HttpContext.Current.Session["IsIndividualItemAccounts_AM"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsIndividualItemAccounts_AM"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsIndividualItemAccounts_AM"] = value; }
        }
        public static bool ShowExceptionToUser
        {
            get
            {
                return HttpContext.Current.Session["ShowExceptionToUser"] != null ? Convert.ToBoolean(HttpContext.Current.Session["ShowExceptionToUser"].ToString()) : false;
            }
            set { HttpContext.Current.Session["ShowExceptionToUser"] = value; }
        }

        public static decimal? GstTax_Shop
        {
            get
            {
                if (HttpContext.Current.Session["GstTax_Shop"] != null)
                    return decimal.Parse(HttpContext.Current.Session["GstTax_Shop"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["GstTax_Shop"] = value; }
        }

        public static string GstTaxAccountId_Shop
        {
            get
            {
                return HttpContext.Current.Session["GstTaxAccountId_Shop"] != null ? HttpContext.Current.Session["GstTaxAccountId_Shop"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["GstTaxAccountId_Shop"] = value; }
        }

        public static string DiscountAccountId_Shop
        {
            get
            {
                return HttpContext.Current.Session["DiscountAccountId_Shop"] != null ? HttpContext.Current.Session["DiscountAccountId_Shop"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DiscountAccountId_Shop"] = value; }
        }

        public static decimal? ServiceCharges_Shop
        {
            get
            {
                if (HttpContext.Current.Session["ServiceCharges_Shop"] != null)
                    return decimal.Parse(HttpContext.Current.Session["ServiceCharges_Shop"].ToString());
                else
                    return null;
            }
            set { HttpContext.Current.Session["ServiceCharges_Shop"] = value; }
        }

        public static string ServiceChargesAccountId_Shop
        {
            get
            {
                return HttpContext.Current.Session["ServiceChargesAccountId_Shop"] != null ? HttpContext.Current.Session["ServiceChargesAccountId_Shop"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["ServiceChargesAccountId_Shop"] = value; }
        }
        public static string NTN
        {
            get
            {
                return HttpContext.Current.Session["NTN"] != null ? HttpContext.Current.Session["NTN"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["NTN"] = value; }
        }
        public static string GSTN
        {
            get
            {
                return HttpContext.Current.Session["GSTN"] != null ? HttpContext.Current.Session["GSTN"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["GSTN"] = value; }
        }

        public static bool IsAMBSActive
        {
            get
            {
                return HttpContext.Current.Session["IsAMBSActive"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsAMBSActive"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsAMBSActive"] = value; }
        }

        public static bool IsAcademicsActive
        {
            get
            {
                return HttpContext.Current.Session["IsAcademicsActive"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsAcademicsActive"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsAcademicsActive"] = value; }
        }

        public static bool IsSaleInvoiceForBatchActive
        {
            get
            {
                return HttpContext.Current.Session["IsSaleInvoiceForBatchActive"] != null ? Convert.ToBoolean(HttpContext.Current.Session["IsSaleInvoiceForBatchActive"].ToString()) : false;
            }
            set { HttpContext.Current.Session["IsSaleInvoiceForBatchActive"] = value; }
        }



        public static string RestaurantInvoicePrintNote
        {
            get
            {
                return HttpContext.Current.Session["RestaurantInvoicePrintNote"] != null ? HttpContext.Current.Session["RestaurantInvoicePrintNote"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["RestaurantInvoicePrintNote"] = value; }
        }
        public static string CurrentSession
        {
            get
            {
                return HttpContext.Current.Session["CurrentSession"] != null ? HttpContext.Current.Session["CurrentSession"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CurrentSession"] = value; }
        }

        public static string CurrentTerm
        {
            get
            {
                return HttpContext.Current.Session["CurrentTerm"] != null ? HttpContext.Current.Session["CurrentTerm"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CurrentTerm"] = value; }
        }

        public static Guid CurrentSessionId
        {
            get
            {
                return HttpContext.Current.Session["CurrentSessionId"] != null ? new Guid(HttpContext.Current.Session["CurrentSessionId"].ToString()) : Guid.Empty;
            }
            set { HttpContext.Current.Session["CurrentSessionId"] = value; }
        }

        public static int TemplateTypeId
        {
            get { return HttpContext.Current.Session["TemplateTypeId"] != null ? Convert.ToInt32(HttpContext.Current.Session["TemplateTypeId"]) : (int)0; }
            set { HttpContext.Current.Session["TemplateTypeId"] = value; }
        }
        public static int ViewId
        {
            get
            {
                return HttpContext.Current.Session["ViewId"] != null ? Convert.ToInt32(HttpContext.Current.Session["ViewId"]) : (int)0;
            }
            set { HttpContext.Current.Session["ViewId"] = value; }
        }
        public static int ModuleId
        {
            get
            {
                return HttpContext.Current.Session["ModuleId"] != null ? Convert.ToInt32(HttpContext.Current.Session["ModuleId"]) : (int)0;
            }
            set { HttpContext.Current.Session["ModuleId"] = value; }
        }

        public static string FiscalYear
        {
            get
            {
                return HttpContext.Current.Session["FiscalYear"] != null ? HttpContext.Current.Session["FiscalYear"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["FiscalYear"] = value; }
        }


        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static string Email
        {
            get
            {
                return HttpContext.Current.Session["Email"] != null ? HttpContext.Current.Session["Email"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["Email"] = value; }
        }
        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session["UserId"] != null ? Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString()) : (int)0;
            }
            set { HttpContext.Current.Session["UserId"] = value; }
        }


        public static int CompanyId
        {
            get
            {
                return HttpContext.Current.Session["CompanyId"] != null ? (int)HttpContext.Current.Session["CompanyId"] : 0;
            }
            set { HttpContext.Current.Session["CompanyId"] = value; }
        }


        public static string CompanyAddress
        {
            get
            {
                return HttpContext.Current.Session["CompanyAddress"] != null ? HttpContext.Current.Session["CompanyAddress"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["CompanyAddress"] = value; }
        }

        public static string ReturnUrl
        {
            get
            {
                return HttpContext.Current.Session["ReturnUrl"] != null ? HttpContext.Current.Session["ReturnUrl"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["ReturnUrl"] = value; }
        }
        public static string DashboardUrl
        {
            get
            {
                return HttpContext.Current.Session["DashboardUrl"] != null ? HttpContext.Current.Session["DashboardUrl"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["DashboardUrl"] = value; }
        }

        public static string CurrentUrl { get; set; }

        public static DateTime SessionYearStartDate
        {
            get
            {
                return HttpContext.Current.Session["SessionYearStartDate"] != null ? Convert.ToDateTime(HttpContext.Current.Session["SessionYearStartDate"]) : DateTime.Now;
            }

            set
            {
                HttpContext.Current.Session["SessionYearStartDate"] = value;
            }
        }

        public static DateTime SessionYearEndDate
        {
            get
            {
                return HttpContext.Current.Session["SessionYearEndDate"] != null ? Convert.ToDateTime(HttpContext.Current.Session["SessionYearEndDate"]) : DateTime.Now;
            }

            set
            {
                HttpContext.Current.Session["SessionYearEndDate"] = value;
            }
        }

        public static bool UppercaseStudentData
        {
            get
            {
                if (HttpContext.Current.Session["UppercaseStudentData"] != null)
                    return bool.Parse(HttpContext.Current.Session["UppercaseStudentData"].ToString());
                else
                    return false;
            }
            set { HttpContext.Current.Session["UppercaseStudentData"] = value; }
        }

        public static bool? SaveBranchId { get; set; }

        public static string UrlStudent
        {
            get
            {
                return HttpContext.Current.Session["UrlStudent"] != null ? HttpContext.Current.Session["UrlStudent"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UrlStudent"] = value; }
        }
        public static string UrlEmployee
        {
            get
            {
                return HttpContext.Current.Session["UrlEmployee"] != null ? HttpContext.Current.Session["UrlEmployee"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UrlEmployee"] = value; }
        }
        public static string Urlapi
        {
            get
            {
                return HttpContext.Current.Session["Urlapi"] != null ? HttpContext.Current.Session["Urlapi"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["Urlapi"] = value; }
        }
        public static string UrlProduct
        {
            get
            {
                return HttpContext.Current.Session["UrlProduct"] != null ? HttpContext.Current.Session["UrlProduct"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UrlProduct"] = value; }
        }
        public static string UrlKnowledgeBase
        {
            get
            {
                return HttpContext.Current.Session["UrlKnowledgeBase"] != null ? HttpContext.Current.Session["UrlKnowledgeBase"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UrlKnowledgeBase"] = value; }
        }
        public static string UrlSupportTicket
        {
            get
            {
                return HttpContext.Current.Session["UrlSupportTicket"] != null ? HttpContext.Current.Session["UrlSupportTicket"].ToString() : string.Empty;
            }
            set { HttpContext.Current.Session["UrlSupportTicket"] = value; }
        }
    }
}