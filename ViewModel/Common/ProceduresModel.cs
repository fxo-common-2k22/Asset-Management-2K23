using FAPP.DAL;
using FAPP.Helpers;
using FAPP.Model;
using FAPP.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FAPP.ViewModel.Common
{
    //[Obsolete]
    public static partial class ProceduresModel
    {


        public static List<v_mnl_FormRights_Result> v_mnl_DashboardViews(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var allowedStr = $"AND Allowed = '{ allowed }'";
            if (!allowed.HasValue)
            {
                allowedStr = "";
            }

            var actionNameStr = $"AND FormRightName = '{actionName}'";
            if (string.IsNullOrWhiteSpace(actionName))
            {
                actionNameStr = "";
            }

            var urlStr = $"AND FormURL = '{ url } '";
            if (string.IsNullOrWhiteSpace(url))
            {
                urlStr = "";
            }

            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
DECLARE @parentform bigint
SELECT TOP (1) @parentform = FormID FROM Membership.Forms WHERE 1 = 1 {urlStr}
SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, UserGroup.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.PageDescription
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
	, Form.IsDashboardPart
,Form.IsAction
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups AS UserGroup ON GroupRights.GroupId = UserGroup.UserGroupId
WHERE Form.IsDashboardPart = 1 AND ParentForm = @parentform AND UserGroup.UserGroupId = {groupId} {actionNameStr} {allowedStr}").ToList();
            return result;
        }


        public class p_mnl_dbo_Account__Search_Result
        {
            public string TitleS { get; set; }
            public string ATitle { get; set; }
            public string autokey { get; set; }
            public string LINEAGE { get; set; }
            public string Locked { get; set; }
            public bool IsLocked { get; set; }
            public string IsRoot { get; set; }
            public string OB { get; set; }
            public Nullable<int> Children { get; set; }
            public Nullable<long> ParentId { get; set; }
            public Nullable<short> DEPTH { get; set; }
            public long ACCOUNT_ID { get; set; }
            public Nullable<short> BranchId { get; set; }
            public bool CONTROLACCOUNT { get; set; }
            public string AccountCode { get; set; }
            public string TITLE { get; set; }
            public string AccountTitle { get; set; }
            public bool ISTRANSACTION { get; set; }
            public short ControlAccountRef { get; set; }

        }


        public class p_mnl_Account_GetCashAndBankAccounts_Result
        {
            public Nullable<long> ACCOUNT_ID { get; set; }
            public string TITLE { get; set; }
            public string autokey { get; set; }
            public string Lineage { get; set; }
            public string Locked { get; set; }
            public Nullable<bool> IsLocked { get; set; }
            public string IsRoot { get; set; }
            public string OB { get; set; }
            public Nullable<int> Children { get; set; }
            public Nullable<long> ParentId { get; set; }
            public Nullable<short> Depth { get; set; }
            public Nullable<short> BranchId { get; set; }
            public Nullable<bool> CONTROLACCOUNT { get; set; }
            public Nullable<long> AccountCode { get; set; }
            public string TITLES { get; set; }
            public Nullable<bool> ISTRANSACTION { get; set; }
        }


        public static List<p_mnl_Account_GetCashAndBankAccounts_Result> p_mnl_Account_GetCashAndBankAccounts(OneDbContext db, bool? GetCashAccounts, bool? GetBankAccounts, short? BranchId, bool? ShowOnFrontDeskOnly)
        {
            var result = db.Database.SqlQuery<p_mnl_Account_GetCashAndBankAccounts_Result>($@"
                                DECLARE @TBL TABLE (
	                            [ACCOUNT_ID] BIGINT
	                            ,[TITLE] NVARCHAR(500)
	                            ,[autokey] VARCHAR(36)
	                            ,[Lineage] VARCHAR(500)
	                            ,[Locked] VARCHAR(10)
	                            ,[IsLocked] BIT
	                            ,[IsRoot] VARCHAR(5)
	                            ,[OB] VARCHAR(524)
	                            ,[Children] INT
	                            ,[ParentId] BIGINT
	                            ,[Depth] SMALLINT
	                            ,[BranchId] SMALLINT
	                            ,[CONTROLACCOUNT] BIT
	                            ,[AccountCode] BIGINT
	                            ,[TITLES] NVARCHAR(500)
	                            ,ISTRANSACTION BIT
	                            )

                            IF @GetCashAccounts = 1
                            BEGIN
	                            INSERT INTO @TBL
	                            SELECT ACCOUNT_ID
		                            ,TITLE
		                            ,autokey
		                            ,LINEAGE
		                            ,IsLocked
		                            ,IsLocked
		                            ,0
		                            ,''
		                            ,0
		                            ,ParentId
		                            ,DEPTH
		                            ,BranchId
		                            ,CONTROLACCOUNT
		                            ,ACCOUNT_ID
		                            ,'[' + CAST(ACCOUNT_ID AS VARCHAR(20)) + '] ' + UPPER(TITLE) AS TitleS
		                            ,ISTRANSACTION
	                            FROM Finance.CashAccounts ca
	                            INNER JOIN Finance.Accounts acc ON ca.CashAccountId = acc.autokey
	                            WHERE acc.BranchId = @BranchId
                            END

                            IF @GetBankAccounts = 1
                            BEGIN
	                            INSERT INTO @TBL
	                            SELECT ACCOUNT_ID
		                            ,TITLE
		                            ,autokey
		                            ,LINEAGE
		                            ,IsLocked
		                            ,IsLocked
		                            ,0
		                            ,''
		                            ,0
		                            ,ParentId
		                            ,DEPTH
		                            ,acc.BranchId
		                            ,CONTROLACCOUNT
		                            ,ACCOUNT_ID
		                            ,'[' + CAST(ACCOUNT_ID AS VARCHAR(20)) + '] ' + UPPER(TITLE) AS TitleS
		                            ,ISTRANSACTION
	                            FROM Finance.BankAccounts ca
	                            INNER JOIN Finance.Accounts acc ON ca.AccountId = acc.autokey
	                            WHERE acc.BranchId = @BranchId
                            END
                            SELECT *
                            FROM @TBL [t]",
                         new SqlParameter("@GetCashAccounts", Utilities.GetDBNullOrValue(GetCashAccounts)),
                         new SqlParameter("@GetBankAccounts", Utilities.GetDBNullOrValue(GetBankAccounts)),
                         new SqlParameter("@BranchId", Utilities.GetDBNullOrValue(BranchId)),
                         new SqlParameter("@ShowOnFrontDeskOnly", Utilities.GetDBNullOrValue(ShowOnFrontDeskOnly))
                ).ToList();
            return result;
        }

        public static List<p_mnl_dbo_Account__Search_Result> p_mnl_dbo_Account__Search(OneDbContext db, bool? TreeView, long? ParentId, string Lineage, long? AccountCode, long? branchId, bool transaction = true, bool control = false)
        {
            //--SET @TreeView = ISNULL(@TreeView, 1)
            //--SET @Lineage = ISNULL(@Lineage, '%')
            var sql = $@"SELECT *
						FROM Finance.v__Accounts a
						WHERE a.BranchId = {branchId}
							AND a.ParentId IS NOT NULL
                            AND ISTRANSACTION = '{transaction}'
							AND CONTROLACCOUNT = '{control}'
						";

            if (ParentId != null)
            {
                sql += $" AND a.ParentId = {ParentId})";
            }

            if (!string.IsNullOrWhiteSpace(Lineage))
            {
                sql += $" AND a.Lineage LIKE '{Lineage}%'";
            }

            if (AccountCode != null)
            {
                sql += $" AND a.ACCOUNT_ID = {AccountCode}";
            }

            if (AccountCode != null)
            {
                sql += $" AND a.ACCOUNT_ID = {AccountCode}";
            }

            sql += " ORDER BY Lineage + CAST(a.ACCOUNT_ID AS VARCHAR)";

            var result = db.Database.SqlQuery<p_mnl_dbo_Account__Search_Result>(sql).ToList();
            return result;
        }

        public class p_mnl_CostGroup_Search_Result
        {
            public int CostGroupId { get; set; }
            public string CostGroupName { get; set; }
        }


        public static List<p_mnl_CostGroup_Search_Result> p_mnl_CostGroup_Search(OneDbContext db, short? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl_CostGroup_Search_Result>(@"
                        SELECT CostGroupId
                        ,IsNull(CAST(CostGroupCode AS VARCHAR),' ') + ' ' + CostGroupName AS CostGroupName
                        FROM Finance.CostGroups
                        WHERE (1 = 1)
                        AND (BranchId = @BranchId)
                        AND (ParentId IS NOT NULL)
                        AND (CostGroupType = 2)
                        ",
                         new SqlParameter("@BranchId", Utilities.GetDBNullOrValue(BranchId))
                ).ToList();
            return result;
        }



        //public static bool InvoiceNote_Insert(OneDbContext db, string InvoiceType, string NoteType, long InvoiceId, string StickyNote, short BranchId, int UserId)
        //{
        //    POSInvoiceNote _InvoiceNote;
        //    if (!string.IsNullOrEmpty(StickyNote))
        //    {
        //        _InvoiceNote = new POSInvoiceNote();
        //        _InvoiceNote.InvoiceType = InvoiceType;
        //        _InvoiceNote.NoteType = NoteType;
        //        _InvoiceNote.InvoiceId = InvoiceId;
        //        _InvoiceNote.StickyNote = StickyNote;
        //        _InvoiceNote.BranchId = BranchId;
        //        _InvoiceNote.CreatedBy = UserId;
        //        _InvoiceNote.CreatedOn = DateTime.Now;
        //        db.POSInvoiceNotes.Add(_InvoiceNote);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}

        public static string p_DeleteVoucherDetailsExceptfirst(OneDbContext db, long? VoucherId)
        {
            var result = db.Database.ExecuteSqlCommand($@"--billing
update Finance.BillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
update Finance.SupplierInvoicePayments set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
--pos
update POS.PurchaseInvoiceProducts set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
--inv
update INV.ClearingBillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId =@VoucherId)

update INV.FreightBillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId =@VoucherId)

delete from Finance.VoucherDetails 
where VoucherId=@VoucherId and 
VoucherDetailId<>(select top 1 VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId order by voucherdetailid asc)",
                new SqlParameter("@VoucherId", Utilities.GetDBNullOrValue(VoucherId))).ToString();
            return result;
        }

        public static List<v_mnl_TopTenProducts_Result> v_mnl_WeeklySaleDashboardGraph(OneDbContext db, short? BranchId)
        {
            var result = db.Database.SqlQuery<v_mnl_TopTenProducts_Result>($@"DECLARE @MinDate DATE = DATEADD(day, -6, GETDATE()), @MaxDate DATE = getdate();
Declare @tbl table(SaleInvoiceDate date, NetTotal decimal)
declare @Datetbl table(WeekDate date)

insert into @Datetbl
SELECT  TOP(DATEDIFF(DAY, @MinDate, @MaxDate) + 1) Date = DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate)
FROM    sys.all_objects a

insert into @tbl
select SaleInvoiceDate, SUM(NetTotal) from POS.SaleInvoices
where SaleInvoiceDate in (SELECT  WeekDate from @Datetbl) and BranchId=@BranchId
group by SaleInvoiceDate

insert into @tbl
SELECT WeekDate,0 from @Datetbl
where WeekDate not in(select SaleInvoiceDate from @tbl)

--select SaleInvoiceDate, NetTotal, (
--  DATENAME(dw, CAST(DATEPART(m, Cast(GETDATE() as date)) AS VARCHAR)
--  + '/'
--  + CAST(DATEPART(d, Cast(SaleInvoiceDate as date)) AS VARCHAR)
--  + '/'
--  + CAST(DATEPART(yy, Cast(GETDATE() as date)) AS VARCHAR))
--  ) as Day from @tbl order by SaleInvoiceDate asc
  select SaleInvoiceDate, NetTotal, DATENAME(DW, Cast(SaleInvoiceDate as date))  as Day from @tbl order by SaleInvoiceDate asc",
                         new SqlParameter("@BranchId", Utilities.GetDBNullOrValue(BranchId))
                         ).ToList();
            return result;
        }



        //static short SessionHelper.BranchId  = Convert.ToInt16(SessionHelper.BranchId);


        //public class p_mnl__OpeningStock_Result
        //{
        //    public int OpeningStockId { get; set; }
        //    public int? OpeningStockMasterId { get; set; }
        //    public int ProductId { get; set; }
        //    public decimal? Stock { get; set; }
        //    public int WareHouseId { get; set; }
        //    public short BranchId { get; set; }
        //    public DateTime? CreatedOn { get; set; }
        //    public int? CreatedBy { get; set; }
        //    public DateTime? ExpiryDate { get; set; }
        //    public string ManufacturerProductNo { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public string ProductName { get; set; }
        //    public string ProductCode { get; set; }
        //    public string Barcode { get; set; }
        //    public bool? IsPosted { get; set; }
        //    public string CategoryName { get; set; }
        //    public int CategoryId { get; set; }
        //}

        //public class v_mnl_SaleInvoices_Result
        //{
        //    public long SaleInvoiceId { get; set; }
        //    public int ClientId { get; set; }
        //    public string AMBSDescription { get; set; }
        //    public int? CategoryId { get; set; }
        //    public string CategoryName { get; set; }
        //    public int RecurringType { get; set; }
        //    public DateTime SaleInvoiceDate { get; set; }
        //    public short CurrencyId { get; set; }
        //    public decimal ExchangeRate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public decimal Discount { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? NetTotalMain { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public string AccountTitle { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string PostedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool? IsCheked { get; set; }
        //    public bool? IsAccountPosted { get; set; }
        //    public string AccountPostedName { get; set; }
        //    public string CounterName { get; set; }
        //    public bool IsWarehouseExist { get; set; }
        //    public bool IsInvoiceCreatedFromInvoicing { get; set; }
        //    public bool IsPaid { get; set; }
        //    public string PaidUserName { get; set; }
        //    public bool IsService { get; set; }
        //    public long ServiceId { get; set; }
        //    public DateTime RegDate { get; set; }
        //    public DateTime? NextInvoiceDate { get; set; }
        //    public string ServiceDescription { get; set; }
        //    public string SaleInvoiceIds { get; set; }
        //    public string ServiceTitle { get; set; }
        //    public bool IsActive { get; set; }
        //    public int TotalInvoices { get; set; }
        //    public DateTime? DueDate { get; set; }
        //    public int DueDaysAfterInvoiceDate { get; set; }
        //    public long? InventoryVoucherId { get; set; }
        //    public long? SerialNo { get; set; }
        //}
        //public class v_mnl_SalesReports_Result
        //{
        //    public long SaleInvoiceId { get; set; }
        //    public long SerialNo { get; set; }
        //    public int ClientId { get; set; }
        //    public DateTime SaleInvoiceDate { get; set; }
        //    public short CurrencyId { get; set; }
        //    public decimal ExchangeRate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public decimal Discount { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? InvoiceTotal { get; set; }
        //    public int SaleInvoiceProductId { get; set; }
        //    public int ProductId { get; set; }
        //    public string ManufacturerProductNo { get; set; }
        //    public decimal? OrgWidth { get; set; }
        //    public decimal? OrgLength { get; set; }
        //    public decimal? CalWidth { get; set; }
        //    public decimal? CalLength { get; set; }
        //    public decimal? CalDigit { get; set; }
        //    public decimal Quantity { get; set; }
        //    public decimal? SqFeet { get; set; }
        //    public decimal UnitPrice { get; set; }
        //    public decimal? LineTotal { get; set; }
        //    public decimal Tax { get; set; }
        //    public DateTime? ExpiryDate { get; set; }
        //    public string Warranty { get; set; }
        //    public int? WareHouseId { get; set; }
        //    public string CategoryName { get; set; }
        //    public string ProductName { get; set; }
        //    public int ProductGroupId { get; set; }
        //    public int CategoryId { get; set; }
        //    public int BrandId { get; set; }
        //    public string ProductCode { get; set; }
        //    public decimal? CostPrice { get; set; }
        //    public decimal? SalePrice { get; set; }
        //    public decimal? OpeningStock { get; set; }
        //    public string Size { get; set; }
        //    public string Dimensions { get; set; }
        //    public string Weight { get; set; }
        //    public string Colour { get; set; }
        //    public decimal? RecentUnitPrice { get; set; }
        //    public decimal? UnitsPerPack { get; set; }
        //    public bool MovedToWarehouse { get; set; }
        //    public string IP { get; set; }
        //    public string Barcode { get; set; }
        //    public int? UnitID { get; set; }
        //    public string Type { get; set; }
        //    public decimal? Width { get; set; }
        //    public decimal? Length { get; set; }
        //    public string BrandTitle { get; set; }
        //    public int OrderId { get; set; }
        //    public DateTime OrderDate { get; set; }
        //    public int? WaiterId { get; set; }
        //    public decimal ServiceCharges { get; set; }
        //    public decimal Total { get; set; }
        //    public int OrderDetailId { get; set; }
        //    public int Orders { get; set; }
        //    public int? KitchenId { get; set; }
        //    public string KitchenName { get; set; }
        //    public int? MealCourseId { get; set; }
        //    public string MealCourseTitle { get; set; }
        //    public int? MealCuisineId { get; set; }
        //    public string CuisineTitle { get; set; }
        //}

        public class v_mnl_PurchaseInvoices_Result
        {
            public long PurchaseInvoiceId { get; set; }
            public long? PurchaseOrderId { get; set; }
            public DateTime PurchaseInvoiceDate { get; set; }
            public int? SupplierId { get; set; }
            public string SupplierName { get; set; }
            public decimal Discount { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public DateTime? PostedOn { get; set; }
            public int? PostedBy { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public short? CurrencyId { get; set; }
            public decimal? ExchangeRate { get; set; }
            public bool MovedToWarehouse { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }
            public decimal? ReceivedAmount { get; set; }
            public string PostedName { get; set; }
            public string CancelledName { get; set; }
            public string ModifiedName { get; set; }
            public string CreatedName { get; set; }
            public string ClientName { get; set; }
            public decimal? Paid { get; set; }
            public bool? IsCheked { get; set; }
            public bool? IsAccountPosted { get; set; }
            public DateTime? AccountPostedOn { get; set; }
            public int? AccountPostedBy { get; set; }
            public string AccountPostedName { get; set; }
            public bool IsCreatedFromOpenningStock { get; set; }
            public int? WareHouseId { get; set; }
        }

        //public class v_mnl_PurchaseInvoiceProducts_Result
        //{
        //    public long PurchaseInvoiceId { get; set; }
        //    public int SupplierId { get; set; }
        //    public DateTime PurchaseInvoiceDate { get; set; }
        //    public short CurrencyId { get; set; }
        //    public decimal ExchangeRate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public decimal Discount { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? InvoiceTotal { get; set; }
        //    public int PurchaseInvoiceProductId { get; set; }
        //    public int ProductId { get; set; }
        //    public string ManufacturerProductNo { get; set; }
        //    public decimal? OrgWidth { get; set; }
        //    public decimal? OrgLength { get; set; }
        //    public decimal? CalWidth { get; set; }
        //    public decimal? CalLength { get; set; }
        //    public decimal? CalDigit { get; set; }
        //    public decimal Quantity { get; set; }
        //    public decimal? SqFeet { get; set; }
        //    public decimal UnitPrice { get; set; }
        //    public decimal? LineTotal { get; set; }
        //    public decimal Tax { get; set; }
        //    public DateTime? ExpiryDate { get; set; }
        //    public string Warranty { get; set; }
        //    public int? WareHouseId { get; set; }
        //    public string CategoryName { get; set; }
        //    public string ProductName { get; set; }
        //    public int ProductGroupId { get; set; }
        //    public int CategoryId { get; set; }
        //    public int BrandId { get; set; }
        //    public string ProductCode { get; set; }
        //    public decimal? CostPrice { get; set; }
        //    public decimal? SalePrice { get; set; }
        //    public decimal? OpeningStock { get; set; }
        //    public string Size { get; set; }
        //    public string Dimensions { get; set; }
        //    public string Weight { get; set; }
        //    public string Colour { get; set; }
        //    public decimal? RecentUnitPrice { get; set; }
        //    public decimal? UnitsPerPack { get; set; }
        //    public bool MovedToWarehouse { get; set; }
        //    public string IP { get; set; }
        //    public string Barcode { get; set; }
        //    public int? UnitID { get; set; }
        //    public string Type { get; set; }
        //    public decimal? Width { get; set; }
        //    public decimal? Length { get; set; }
        //}
        //public class v_mnl_TranferMethodsTypes_Result
        //{
        //    public string TransferType { get; set; }
        //    public string TransferMethod { get; set; }
        //}
        //public class v_mnl_PurchaseReturns_Result
        //{
        //    public long PurchaseReturnId { get; set; }
        //    public DateTime PurchaseReturnDate { get; set; }
        //    public int SupplierId { get; set; }
        //    public decimal Discount { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public int? PostedBy { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public short? CurrencyId { get; set; }
        //    public decimal? ExchangeRate { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string PostedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool? IsCheked { get; set; }
        //}
        //public class v_mnl_SaleReturns_Result
        //{
        //    public long SaleReturnId { get; set; }
        //    public int ClientId { get; set; }
        //    public DateTime SaleReturnDate { get; set; }
        //    public short CurrencyId { get; set; }
        //    public decimal ExchangeRate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public decimal Discount { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string PostedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool? IsCheked { get; set; }
        //    public long? InventoryReturnVoucherId { get; set; }
        //}
        //public class v_mnl_Damages_Result
        //{
        //    public int DamageId { get; set; }
        //    public DateTime DamageDate { get; set; }
        //    public string Description { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public long? VoucherId { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string PostedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool? IsCheked { get; set; }
        //}
        public static bool ClearPosCounterSession()
        {
            SessionHelper.ClientIdPOSCounterSession = null;
            SessionHelper.CashAccountIdPOSCounterSession = null;
            SessionHelper.BankAccountIdPOSCounterSession = null;
            SessionHelper.WareHouseIdPOSCounterSession = null;
            SessionHelper.SaleCounterSessionIdPOSCounterSession = null;
            SessionHelper.CounterNamePOSCounterSession = null;
            return true;
        }
        public static bool ClearResCounterSession()
        {
            SessionHelper.ClientIdResCounterSession = null;
            SessionHelper.CashAccountIdResCounterSession = null;
            SessionHelper.BankAccountIdResCounterSession = null;
            SessionHelper.WareHouseIdResCounterSession = null;
            SessionHelper.SaleCounterSessionIdResCounterSession = null;
            SessionHelper.CounterNameResCounterSession = null;
            return true;
        }
        public static bool ClearAMCounterSession()
        {
            SessionHelper.ClientIdAMCounterSession = null;
            SessionHelper.CashAccountIdAMCounterSession = null;
            SessionHelper.BankAccountIdAMCounterSession = null;
            SessionHelper.WareHouseIdAMCounterSession = null;
            SessionHelper.SaleCounterSessionIdAMCounterSession = null;
            SessionHelper.CounterNameAMCounterSession = null;
            return true;
        }
        //public class v_mnl_Tables
        //{
        //    public int TableId { get; set; }
        //    public string TableName { get; set; }
        //    public string SectionName { get; set; }
        //    public int? SectionId { get; set; }
        //    public bool Status { get; set; }
        //    public bool IsClosed { get; set; }
        //    public short? BranchId { get; set; }
        //    public int? OrderId { get; set; }
        //    public int? OrderTableId { get; set; }
        //}
        //public class v_mnl_Orders
        //{
        //    public int OrderId { get; set; }
        //    public int OrderTypeId { get; set; }
        //    public string TypeName { get; set; }
        //    public string TableName { get; set; }
        //}
        //public class v_mnl_ResOrders_Result
        //{
        //    public int OrderId { get; set; }
        //    public string SaleCounterSessionId { get; set; }
        //    public DateTime OrderDate { get; set; }
        //    public int ClientId { get; set; }
        //    public int? TableId { get; set; }
        //    public decimal Total { get; set; }
        //    public decimal Tax { get; set; }
        //    public decimal Discount { get; set; }
        //    public decimal DiscountCoupon { get; set; }
        //    public decimal Received { get; set; }
        //    public decimal ReceivedAmount { get; set; }
        //    public decimal NetTotal { get; set; }
        //    public string OrderType { get; set; }
        //    public string PaymentType { get; set; }
        //    public bool IsClosed { get; set; }
        //    public short NoOfGuests { get; set; }
        //    public string Description { get; set; }
        //    public DateTime? OrderCompletionTime { get; set; }
        //    public string Name { get; set; }
        //    public string MobNo { get; set; }
        //    public string Address { get; set; }
        //    public int? LocationId { get; set; }
        //    public int? SubLocationId { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public long? InventoryVoucherId { get; set; }
        //    public long? VoucherId { get; set; }
        //    public int? WaiterId { get; set; }
        //    public string Suggestion { get; set; }
        //    public string Complaint { get; set; }
        //    public int CreatedBy { get; set; }
        //    public bool IsPosted { get; set; }
        //    public int? PostedBy { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public short BranchId { get; set; }
        //    public int? OrderStatusId { get; set; }
        //    public int? ClientPaymentId { get; set; }
        //    public bool IsAccountPosted { get; set; }
        //    public DateTime? AccountPostedOn { get; set; }
        //    public int? AccountPostedBy { get; set; }
        //    public decimal ServiceCharges { get; set; }
        //    public DateTime OrderTakingTime { get; set; }
        //    public string PostedName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool IsCheked { get; set; }
        //    public string AccountPostedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public int? InvoiceParticularId { get; set; }
        //    public int? RoomId { get; set; }
        //    public DateTime? PostedOn { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public string TableName { get; set; }
        //    public string OrderTypeName { get; set; }
        //    public string UploadedUserName { get; set; }
        //    public string LocalOrderId { get; set; }
        //    public string LocalId { get; set; }
        //    public DateTime? UploadDate { get; set; }
        //}
        //public class v_mnl_WorkOrder_Result
        //{
        //    public long WorkOrderId { get; set; }
        //    public int ClientId { get; set; }
        //    public DateTime WorkOrderDate { get; set; }
        //    public short CurrencyId { get; set; }
        //    public decimal ExchangeRate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public decimal Discount { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? LabourCharges { get; set; }
        //    public decimal? OtherCharges { get; set; }
        //    public decimal? FareCharges { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public short? BranchId { get; set; }
        //    public bool IsApplyTax { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string DchallanCreatedName { get; set; }
        //    public string CancelledName { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public decimal? Paid { get; set; }
        //    public bool? IsCheked { get; set; }
        //    public bool? IsSInvoiceCreated { get; set; }
        //    public bool? IsDChallanCreated { get; set; }
        //    public string SInvoiceCreatedName { get; set; }
        //    public string SaleInvoiceIds { get; set; }
        //    public string DeliverChallanIds { get; set; }
        //}
        //public class v_mnl_DeliveryChallan_Result
        //{
        //    public long DeliveryChallanId { get; set; }
        //    public int ClientId { get; set; }
        //    public DateTime DeliveryChallanDate { get; set; }
        //    public string DealingPerson { get; set; }
        //    public string Description { get; set; }
        //    public bool IsCancelled { get; set; }
        //    public DateTime? CancelledOn { get; set; }
        //    public int? CancelledBy { get; set; }
        //    public int? ModifiedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public DateTime CreatedOn { get; set; }
        //    public int CreatedBy { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public short? BranchId { get; set; }
        //    public string ModifiedName { get; set; }
        //    public string CreatedName { get; set; }
        //    public string ClientName { get; set; }
        //    public long? WorkOrderId { get; set; }
        //    public string SaleInvoiceIds { get; set; }
        //    public string WorkOrderIds { get; set; }
        //    public string CreationMethod { get; set; }
        //    public long? SaleInvoiceId { get; set; }
        //}
        public static void SessionNotes(OneDbContext db)
        {
            var GeneralSettings = db.POSGeneralSettings.ToList();
            if (GeneralSettings.Count() > 0)
            {
                SessionHelper.PurchaseNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper() == "PurchaseInvoiceNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.SaleNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "SaleInvoiceNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.DeliveryChallanNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "DeliveryChallanNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.WorkOrderNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "WorkOrderNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.SaleReturnNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "SaleReturnNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.PurchaseReturnNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "PurchaseReturnNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
            }
        }

        public static string CheckAppConstraints(OneDbContext db, string Module)
        {
            string msg = "";
            var fiscal = db.FiscalYears.Where(u => u.Active == true && u.BranchId == SessionHelper.BranchId).OrderByDescending(u => u.FiscalYearId).FirstOrDefault();
            if (fiscal == null)
            {
                msg = "* Fiscal Year not found <a href='/Finance/Setup/FiscalYears'>Click here</a><br>";
            }
            switch (Module)
            {
                //#region Res
                //case "Res":
                //    var _AccountSettings = db.ResAccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_AccountSettings == null)
                //    {
                //        msg += "* Restaurant setting not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>";
                //    }
                //    else
                //    {
                //        msg += _AccountSettings.ServiceChargesAccountId == null ? "* Service charges account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //        msg += _AccountSettings.TaxAccountId == null ? "* Tax account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //        msg += _AccountSettings.DiscountAccountId == null ? "* Discount account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //    }
                //    var _reswarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_reswarehouse == null)
                //    {
                //        msg += "* Restaurant's warehouse not found <a href='/Res/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                //#region FrontDesk
                //case "FrontDesk":
                //    var _FrontDeskSetting = db.FrontDeskSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_FrontDeskSetting == null)
                //    {
                //        msg += "* FrontDesk setting not found <a href='/FrontDesk/Setup/Setting'>Click here</a><br>";
                //    }
                //    else
                //    {
                //        msg += _FrontDeskSetting.GstAccountId == null ? "* GST account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.TaxParentAccountName == null ? "* Tax parent account not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.DiscountAccountId == null ? "* Discount account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.ServiceAccountHeadName == null ? "* Service parent account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.CommisionAccountId == null ? "* Commision parent account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.RoomServiceId == null ? "* Room service not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.KotServiceId == null ? "* KOT service not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //    }
                //    var _fdwarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_fdwarehouse == null)
                //    {
                //        msg += "* Front Desk's warehouse not found <a href='/FrontDesk/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                #region CashSale
                case "CashSale":
                    msg += SessionHelper.ClientIdResCounterSession == null ? "* Cash Sale Client not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.CashAccountIdResCounterSession == null ? "* Cash account not found in Counter session <a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.BankAccountIdResCounterSession == null ? "* Bank account not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.WareHouseIdResCounterSession == null ? "* Warehouse not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.SaleCounterSessionIdResCounterSession == null ? "* Sale Counter Session not found<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    break;
                #endregion
                #region HRPayroll
                case "HRPayroll":
                    var _AccountSettingss = db.AccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AccountSettingss == null)
                    {
                        msg += "* Human resource setting not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>";
                    }
                    else
                    {
                        msg += _AccountSettingss.EmpParentAccountId == null ? "* Employee parent account not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRBasicSalaryAccountId == null ? "* Basic salary account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRAllowancesParentAccountId == null ? "* Allowances parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRDeductionsParentAccountId == null ? "* Deductions parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpAdvancePaymentParentAccountId == null ? "* Employee Advance payment parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpSecurityParentAccountId == null ? "* Employee Security parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpAdvancePaymentDefaultAccountId == null ? "* Employee Advance payment default account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    }
                    var _AllowancesAndDeductions = db.AllowancesAndDeductions.Where(u => u.BranchId == SessionHelper.BranchId);
                    if (!_AllowancesAndDeductions.Any())
                    {
                        msg += "* Allowances And Deductions not found<br>";
                    }
                    else
                    {
                        foreach (var item in _AllowancesAndDeductions)
                        {
                            msg += item.Account == null ? "* " + item.AllowanceDeductionName + "'s account not found<a href='/HRPayroll/Setup/AllowancesAndDeduction'>Click here</a><br>" : "";
                        }
                    }
                    break;
                #endregion
                //#region Shop
                //case "Shop":
                //    var _warehouse = db.Warehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_warehouse == null)
                //    {
                //        msg += "* Shop's warehouse not found <a href='/Shop/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                #region POS
                case "POS":
                    var _poswarehouse = db.InvWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_poswarehouse == null)
                    {
                        msg += "* POS's warehouse not found <a href='/POS/Setup/ManageWarehouses'>Click here</a><br>";
                    }
                    break;
                #endregion
                #region AM
                case "AM":
                    var _AMAccountSettings = db.AMNatures.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AMAccountSettings == null)
                    {
                        msg += "* Account setting not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>";
                    }
                    else
                    {
                        msg += _AMAccountSettings.InventoryParentAccountId == null ? "* Inventory Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                        msg += _AMAccountSettings.COGIParentAccountId == null ? "* COGI Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                        msg += _AMAccountSettings.DepreciationParentAccountId == null ? "* Depreciation Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                    }
                    var _AMwarehouse = db.AMWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AMwarehouse == null)
                    {
                        msg += "* Warehouse not found <a href='/AM/Setup/ManageWarehouses'>Click here</a><br>";
                    }
                    break;
                    #endregion
                    //#region Contact
                    //case "Contact":
                    //    _AccountSettingss = db.AccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    //    if (_AccountSettingss == null)
                    //        msg += "* Human resource setting not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>";
                    //    else
                    //    {
                    //        msg += _AccountSettingss.EmpParentAccountId == null ? "* Employee parent account not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.HRBasicSalaryAccountId == null ? "* Basic salary account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.HRAllowancesParentAccountId == null ? "* Allowances parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.HRDeductionsParentAccountId == null ? "* Deductions parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.EmpAdvancePaymentParentAccountId == null ? "* Employee Advance payment parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.EmpSecurityParentAccountId == null ? "* Employee Security parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //        msg += _AccountSettingss.EmpAdvancePaymentDefaultAccountId == null ? "* Employee Advance payment default account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    //    }
                    //    break;
                    //    #endregion
            }
            return msg;
        }

        //public class v_mnl_FillCategoryCode
        //{
        //    public int CatgoryCode { get; set; }
        //}

        //public class v_SaleCounterSession
        //{
        //    public string SaleCounterSessionId { get; set; }
        //    public int SaleCounterId { get; set; }
        //    public int UserId { get; set; }
        //    public TimeSpan StartTime { get; set; }
        //    public TimeSpan? EndTime { get; set; }
        //    public decimal StartCash { get; set; }
        //    public decimal? EndCash { get; set; }
        //    public DateTime? CreatedOn { get; set; }
        //    public DateTime? ClosedOn { get; set; }
        //    public bool IsExpired { get; set; }
        //    public short BranchId { get; set; }
        //    public string Title { get; set; }
        //    public string Username { get; set; }
        //    public decimal? BankPayment { get; set; }

        //    public decimal? TotalSale { get; set; }
        //    public decimal? ReceivedAmount { get; set; }

        //    public decimal? CashPayment { get; set; }
        //    public decimal? TotalPayment { get; set; }
        //    public decimal? RemainingCash { get { return (TotalPayment - BankPayment); } }
        //}
        //public static int AccountPosting_POS(OneDbContext db, Areas.InvPharma.Model.SaleModel ex)
        //{
        //    var count = 0;
        //    long[] vocIds = new long[2];
        //    //bool isBalanced = false;
        //    if (ex.v_mnl_SaleInvoiceList != null)
        //    {
        //        VoucherAndReceiptModel vr = new VoucherAndReceiptModel();
        //        using (var trans = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (var item in ex.v_mnl_SaleInvoiceList)
        //                {
        //                    if (item.IsCheked == true)
        //                    {
        //                        var pInvoice = db.PosSaleInvoices.Where(u => u.BranchId == SessionHelper.BranchId).Where(u => u.SaleInvoiceId == item.SaleInvoiceId).FirstOrDefault();
        //                        //var pInvoice = db.Database.SqlQuery<POSSaleInvoice>($@"Select * From Pos.SaleInvoices where SaleInvoiceId = {item.SaleInvoiceId} and BranchId = {SessionHelper.BranchId}").FirstOrDefault();
        //                        if (pInvoice != null)
        //                        {
        //                            if (pInvoice.IsPosted && !pInvoice.IsAccountPosted && !pInvoice.IsCancelled)
        //                            {
        //                                var _Voucher = new Voucher();
        //                                var clientAccount = db.Clients.Where(u => u.ClientId == pInvoice.ClientId).Select(u => u.AccountId).FirstOrDefault();
        //                                _Voucher.VoucherType = "SI";
        //                                _Voucher.TransactionDate = pInvoice.SaleInvoiceDate;
        //                                _Voucher.VoucherStatus = "Posted";
        //                                _Voucher.CurrencyId = pInvoice.CurrencyId;
        //                                _Voucher.ExchangeRate = pInvoice.ExchangeRate;
        //                                _Voucher.CBAccountId = clientAccount;
        //                                _Voucher.Particulars = "Sale Invoice # " + pInvoice.SaleInvoiceId;
        //                                //Voucher
        //                                if (pInvoice.VoucherId == null)
        //                                {
        //                                    _Voucher.VoucherId = ProceduresModel.Voucher_Insert(db, _Voucher);
        //                                    pInvoice.VoucherId = _Voucher.VoucherId;
        //                                }
        //                                else
        //                                {
        //                                    _Voucher.VoucherId = Convert.ToInt64(pInvoice.VoucherId);
        //                                    ProceduresModel.Voucher_Update(db, "Posted", _Voucher);
        //                                }
        //                                //Inventory Voucher
        //                                var _InvVoucher = new Voucher();
        //                                _InvVoucher.VoucherType = "IV";
        //                                _InvVoucher.TransactionDate = pInvoice.SaleInvoiceDate;
        //                                _InvVoucher.VoucherStatus = "Posted";
        //                                _InvVoucher.CurrencyId = pInvoice.CurrencyId;
        //                                _InvVoucher.ExchangeRate = pInvoice.ExchangeRate;
        //                                _InvVoucher.CBAccountId = null;
        //                                _InvVoucher.Particulars = "Sale Invoice # " + pInvoice.SaleInvoiceId;
        //                                if (pInvoice.InventoryVoucherId == null)
        //                                {
        //                                    _InvVoucher.VoucherId = ProceduresModel.JV_Voucher_Insert(db, _InvVoucher);
        //                                    pInvoice.InventoryVoucherId = _InvVoucher.VoucherId;
        //                                }
        //                                else
        //                                {
        //                                    _InvVoucher.VoucherId = _InvVoucher.VoucherId = Convert.ToInt64(pInvoice.InventoryVoucherId);
        //                                    ProceduresModel.JV_Voucher_Update(db, "Posted", _InvVoucher);
        //                                }
        //                                pInvoice.IsAccountPosted = true;
        //                                pInvoice.AccountPostedOn = DateTime.Now;
        //                                pInvoice.AccountPostedBy = SessionHelper.UserID;
        //                                db.Entry(pInvoice).State = EntityState.Modified;
        //                                db.SaveChanges();
        //                                if (_Voucher.VoucherId > 0)
        //                                {
        //                                    //delete all voucher details except first
        //                                    p_DeleteVoucherDetailsExceptfirst(db, _Voucher.VoucherId);
        //                                    //delete all voucher details
        //                                    p_DeleteVoucherDetails(db, _InvVoucher.VoucherId);
        //                                    var pDetails = db.PosSaleInvoiceProducts.Where(u => u.BranchId == SessionHelper.BranchId).Where(u => u.SaleInvoiceId == item.SaleInvoiceId);
        //                                    if (pDetails != null)
        //                                    {
        //                                        var vDetails = new VoucherDetail();
        //                                        var _Salesman = db.Salesmans.Where(u => u.SalesmanId == pInvoice.SalesmanId).FirstOrDefault();
        //                                        if (_Salesman == null)
        //                                        {
        //                                            _Salesman = new Salesman();
        //                                        }
        //                                        foreach (var details in pDetails)
        //                                        {
        //                                            vDetails = new VoucherDetail();
        //                                            vDetails.VoucherId = _Voucher.VoucherId;
        //                                            vDetails.AccountId = details.Product.IncomeAccountId;
        //                                            vDetails.Credit = vDetails.Debit = Convert.ToDecimal(details.NetTotal);
        //                                            vDetails.Narration = _Voucher.Particulars;
        //                                            vDetails.CostGroupId = details.Product?.CostGroupId == null ? details.Product?.Category?.CostGroupId : null;
        //                                            //Income Voucher Entry
        //                                            long VoucherDetailId = 0;
        //                                            if (vDetails.Credit > 0 || vDetails.Debit > 0)
        //                                            {
        //                                                VoucherDetailId = ProceduresModel.VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                            }
        //                                            if (VoucherDetailId > 0)
        //                                            {
        //                                                details.VoucherDetailId = VoucherDetailId;
        //                                                db.Entry(details).State = EntityState.Modified;
        //                                                db.SaveChanges();
        //                                            }
        //                                            //Inventory Voucher Entry
        //                                            //Not for service type
        //                                            if (details.Product.Type != "Service")
        //                                            {
        //                                                v_InventoryEntry(db, details, _InvVoucher, vDetails);
        //                                            }
        //                                            else
        //                                                if (details.Product.CreateCogsEntryInVoucher)
        //                                            {
        //                                                v_InventoryEntry(db, details, _InvVoucher, vDetails);
        //                                            }
        //                                            //COGS Voucher Entry
        //                                            //Not for service type
        //                                            if (details.Product.Type != "Service")
        //                                            {
        //                                                v_CogsEntry(db, details, _InvVoucher, vDetails);
        //                                            }
        //                                            else
        //                                                if (details.Product.CreateCogsEntryInVoucher)
        //                                            {
        //                                                v_CogsEntry(db, details, _InvVoucher, vDetails);
        //                                            }
        //                                            #region Commision entry product base
        //                                            if (_Salesman.CommisionPercentage > 0 &&
        //                                                _Salesman.CommisionOn == false &&
        //                                                !string.IsNullOrEmpty(SessionHelper.CommisionAccountId_POS) &&
        //                                                _Salesman.Employee?.Account != null)
        //                                            {
        //                                                decimal commisionAmount = ((Convert.ToDecimal(details.NetTotal)) * _Salesman.CommisionPercentage) / 100;
        //                                                if (commisionAmount > 0)
        //                                                {
        //                                                    //Commision debit Voucher Entry
        //                                                    vDetails = new VoucherDetail();
        //                                                    vDetails.VoucherId = _Voucher.VoucherId;
        //                                                    vDetails.AccountId = SessionHelper.CommisionAccountId_POS;
        //                                                    //vDetails.AccountId = _Salesman.Employee.EmployeeAccountId;
        //                                                    vDetails.Debit = commisionAmount;
        //                                                    vDetails.Narration = _Voucher.Particulars + ", " + details.Product?.ProductName + " Commision debit entry";
        //                                                    VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                    if (VoucherDetailId > 0)
        //                                                    {
        //                                                        details.CommisionDebitVoucherDetailId = VoucherDetailId;
        //                                                        db.Entry(pInvoice).State = EntityState.Modified;
        //                                                        db.SaveChanges();
        //                                                    }
        //                                                    //Commision Credit Voucher Entry
        //                                                    vDetails = new VoucherDetail();
        //                                                    vDetails.VoucherId = _Voucher.VoucherId;
        //                                                    //vDetails.AccountId = SessionHelper.CommisionAccountId_POS;
        //                                                    vDetails.AccountId = _Salesman.Employee.EmployeeAccountId;
        //                                                    vDetails.Credit = commisionAmount;
        //                                                    vDetails.Narration = _Voucher.Particulars + ", " + details.Product?.ProductName + " Commision credit entry";
        //                                                    VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                    if (VoucherDetailId > 0)
        //                                                    {
        //                                                        details.CommisionCreditVoucherDetailId = VoucherDetailId;
        //                                                        db.Entry(pInvoice).State = EntityState.Modified;
        //                                                        db.SaveChanges();
        //                                                    }
        //                                                }
        //                                            }
        //                                            #endregion
        //                                        }
        //                                        #region GST Tax entry
        //                                        if (SessionHelper.GST_POS > 0 && !string.IsNullOrEmpty(SessionHelper.GstAccountId_POS))
        //                                        {
        //                                            decimal taxAmount = ((Convert.ToDecimal(pInvoice.NetTotal) - pInvoice.Discount) * SessionHelper.GST_POS) / 100;
        //                                            if (taxAmount > 0)
        //                                            {
        //                                                //GST TAX debit Voucher Entry
        //                                                vDetails = new VoucherDetail();
        //                                                vDetails.VoucherId = _Voucher.VoucherId;
        //                                                vDetails.AccountId = clientAccount;
        //                                                vDetails.Debit = taxAmount;
        //                                                vDetails.Narration = _Voucher.Particulars + ", Tax debit entry";
        //                                                var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                if (VoucherDetailId > 0)
        //                                                {
        //                                                    pInvoice.TaxDebitVoucherDetailId = VoucherDetailId;
        //                                                    db.Entry(pInvoice).State = EntityState.Modified;
        //                                                    db.SaveChanges();
        //                                                }
        //                                                //GST TAX Credit Voucher Entry
        //                                                vDetails = new VoucherDetail();
        //                                                vDetails.VoucherId = _Voucher.VoucherId;
        //                                                vDetails.AccountId = SessionHelper.GstAccountId_POS;
        //                                                vDetails.Credit = taxAmount;
        //                                                vDetails.Narration = _Voucher.Particulars + ", Tax credit entry";
        //                                                VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                if (VoucherDetailId > 0)
        //                                                {
        //                                                    pInvoice.TaxCreditVoucherDetailId = VoucherDetailId;
        //                                                    db.Entry(pInvoice).State = EntityState.Modified;
        //                                                    db.SaveChanges();
        //                                                }
        //                                            }
        //                                        }
        //                                        #endregion
        //                                        #region Discount entry
        //                                        if (pInvoice.Discount > 0 && !string.IsNullOrEmpty(SessionHelper.DiscountAccountId_POS))
        //                                        {
        //                                            //Discount Credit Voucher Entry
        //                                            vDetails = new VoucherDetail();
        //                                            vDetails.VoucherId = _Voucher.VoucherId;
        //                                            vDetails.AccountId = clientAccount;
        //                                            vDetails.Credit = pInvoice.Discount;
        //                                            vDetails.Narration = _Voucher.Particulars + ", Discount debit entry";
        //                                            var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                            if (VoucherDetailId > 0)
        //                                            {
        //                                                pInvoice.DiscountDebitVoucherDetailId = VoucherDetailId;
        //                                                db.Entry(pInvoice).State = EntityState.Modified;
        //                                                db.SaveChanges();
        //                                            }
        //                                            //Discount Debit Voucher Entry
        //                                            vDetails = new VoucherDetail();
        //                                            vDetails.VoucherId = _Voucher.VoucherId;
        //                                            vDetails.AccountId = SessionHelper.DiscountAccountId_POS;
        //                                            vDetails.Debit = pInvoice.Discount;
        //                                            vDetails.Narration = _Voucher.Particulars + ", Discount credit entry";
        //                                            VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                            if (VoucherDetailId > 0)
        //                                            {
        //                                                pInvoice.DiscountCreditVoucherDetailId = VoucherDetailId;
        //                                                db.Entry(pInvoice).State = EntityState.Modified;
        //                                                db.SaveChanges();
        //                                            }
        //                                        }
        //                                        #endregion
        //                                        #region Commision entry Invoice base
        //                                        if (_Salesman.CommisionPercentage > 0 &&
        //                                            _Salesman.CommisionOn == true &&
        //                                            !string.IsNullOrEmpty(SessionHelper.CommisionAccountId_POS) &&
        //                                            _Salesman.Employee.Account != null)
        //                                        {
        //                                            decimal commisionAmount = ((Convert.ToDecimal(pInvoice.NetTotal) - pInvoice.Discount) * _Salesman.CommisionPercentage) / 100;
        //                                            if (commisionAmount > 0)
        //                                            {
        //                                                //Commision debit Voucher Entry
        //                                                vDetails = new VoucherDetail();
        //                                                vDetails.VoucherId = _Voucher.VoucherId;
        //                                                vDetails.AccountId = _Salesman.Employee.EmployeeAccountId;
        //                                                vDetails.Debit = commisionAmount;
        //                                                vDetails.Narration = _Voucher.Particulars + ", Commision debit entry";
        //                                                var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                if (VoucherDetailId > 0)
        //                                                {
        //                                                    pInvoice.CommisionDebitVoucherDetailId = VoucherDetailId;
        //                                                    db.Entry(pInvoice).State = EntityState.Modified;
        //                                                    db.SaveChanges();
        //                                                }
        //                                                //Commision Credit Voucher Entry
        //                                                vDetails = new VoucherDetail();
        //                                                vDetails.VoucherId = _Voucher.VoucherId;
        //                                                vDetails.AccountId = SessionHelper.CommisionAccountId_POS;
        //                                                vDetails.Credit = commisionAmount;
        //                                                vDetails.Narration = _Voucher.Particulars + ", Commision credit entry";
        //                                                VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
        //                                                if (VoucherDetailId > 0)
        //                                                {
        //                                                    pInvoice.CommisionCreditVoucherDetailId = VoucherDetailId;
        //                                                    db.Entry(pInvoice).State = EntityState.Modified;
        //                                                    db.SaveChanges();
        //                                                }
        //                                            }
        //                                        }
        //                                        #endregion
        //                                        #region Remove commision previous entries
        //                                        if (_Salesman.CommisionOn == true)//Invoice base commision
        //                                        {
        //                                            var _CommisionVoucherDetailIds = pDetails.Select(u => u.CommisionCreditVoucherDetailId).ToArray().Union(pDetails.Select(u => u.CommisionCreditVoucherDetailId).ToArray());
        //                                            var det = db.VoucherDetails.Where(u => _CommisionVoucherDetailIds.Contains(u.VoucherDetailId));
        //                                            if (det.Any())
        //                                            {
        //                                                db.VoucherDetails.RemoveRange(det);
        //                                                foreach (var items in pDetails)
        //                                                {
        //                                                    items.CommisionDebitVoucherDetailId = null;
        //                                                    items.CommisionCreditVoucherDetailId = null;
        //                                                }
        //                                                db.SaveChanges();
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            long?[] _CommisionVoucherDetailIds = new long?[2];
        //                                            _CommisionVoucherDetailIds[0] = pInvoice.CommisionCreditVoucherDetailId;
        //                                            _CommisionVoucherDetailIds[1] = pInvoice.CommisionDebitVoucherDetailId;
        //                                            var det = db.VoucherDetails.Where(u => _CommisionVoucherDetailIds.Contains(u.VoucherDetailId));
        //                                            if (det.Any())
        //                                            {
        //                                                db.VoucherDetails.RemoveRange(det);
        //                                                pInvoice.CommisionDebitVoucherDetailId = null;
        //                                                pInvoice.CommisionCreditVoucherDetailId = null;
        //                                                db.SaveChanges();
        //                                            }
        //                                        }
        //                                        #endregion
        //                                        //VoucherDetail Trigger
        //                                        ProceduresModel.t_VoucherDetail(db, _Voucher.VoucherId);
        //                                        ProceduresModel.t_VoucherDetail(db, _InvVoucher.VoucherId);
        //                                        count++;
        //                                        //check if debit credit is balanced
        //                                        //vocIds[0] = _Voucher.VoucherId;
        //                                        //vocIds[1] = _InvVoucher.VoucherId;
        //                                        //isBalanced = false;
        //                                        //var _Vouchers = db.Vouchers.Where(u => vocIds.Contains(u.VoucherId));
        //                                        //foreach (var _v in _Vouchers)
        //                                        //{
        //                                        //    if (_v.VoucherDetails.Sum(u => u.Debit) == _v.VoucherDetails.Sum(u => u.Credit))
        //                                        //        isBalanced = true;
        //                                        //    else
        //                                        //    {
        //                                        //        isBalanced = false;
        //                                        //        break;
        //                                        //    }
        //                                        //}
        //                                        //if (isBalanced)
        //                                        //{
        //                                        //    trans.Commit();
        //                                        //    count++;
        //                                        //}
        //                                        //else
        //                                        //{
        //                                        //    ViewBag.Error = "Debit credit must be equal for Invoice # " + pInvoice.SaleInvoiceId;
        //                                        //    trans.Rollback();
        //                                        //}
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                //trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //    return count;
        //}

        public class v_mnl_EmployeeAttendance
        {
            public string EmpName { get; set; }
            public DateTime AttendanceDate { get; set; }
            public DateTime? Arrival { get; set; }
            public DateTime? Departure { get; set; }
            public string AttendanceStatus { get; set; }
        }
        //public class v_mnl_InvoicePayments
        //{
        //    public string EmpName { get; set; }
        //    public DateTime AttendanceDate { get; set; }
        //    public DateTime? Arrival { get; set; }
        //    public DateTime? Departure { get; set; }
        //    public string AttendanceStatus { get; set; }
        //    public int Rows { get; set; }
        //}

        public static void ResetSessionHelper_Branch(OneDbContext db, Branch _branch, int branchId, int userId)
        {
            ClearAMCounterSession();
            ClearPosCounterSession();
            ClearResCounterSession();
            if (_branch == null)
            {
                _branch = db.Branches.Where(u => u.BranchId == branchId).FirstOrDefault();
            }
            else
            {
                _branch = db.Branches.Where(u => u.BranchId == branchId).FirstOrDefault();
            }
            if (_branch != null)
            {
                SessionHelper.BranchId = _branch.BranchId;
                SessionHelper.BranchName = _branch.Name;
                SessionHelper.BranchCode = _branch.BranchCode;
                SessionHelper.BranchAddress = _branch.AddressLine1;
                SessionHelper.BranchPhone = _branch.PhoneNumber;
                SessionHelper.CompanyName = _branch.Info.CompanyName;
                SessionHelper.IsMasterBranch = _branch.IsMasterBranch;
                if (!string.IsNullOrEmpty(_branch.CompanyName))
                {
                    SessionHelper.CompanyName = _branch.CompanyName;
                }
                SessionHelper.NTN = _branch.NTN;
                SessionHelper.GSTN = _branch.GSTN;
                CheckAndCreateLogoImages(db, _branch, branchId);
                SessionHelper.CompanyId = _branch.SettingId;
            }
            //Academics
            //Session session = null;
            //session = (from s in db.Sessions
            //           join us in db.UserSessions on s.SessionId equals us.SessionId
            //           where us.UserId == userId && us.Default == true && us.Assigned == true && s.BranchId == SessionHelper.BranchId
            //           select s).FirstOrDefault();

            //if (session != null)
            //{
            //    SessionHelper.CurrentSession = session?.SessionName ?? string.Empty;
            //    SessionHelper.CurrentSessionId = session?.SessionId ?? Guid.Empty;
            //    SessionHelper.SessionYearEndDate = session.EndTime;
            //    SessionHelper.SessionYearStartDate = session.StartTime;

            //    //var term = db.Terms
            //    //    .Where(s => s.SessionId == session.SessionId && s.BranchId == SessionHelper.BranchId && s.IsActive == true)
            //    //    .Select(s => new
            //    //    {
            //    //        TermName = s.TermName
            //    //    }).FirstOrDefault();

            //    SessionHelper.CurrentTerm = term?.TermName;
            //}
            //else
            //{
            //    SessionHelper.CurrentSession = "";
            //    SessionHelper.CurrentSessionId = Guid.Empty;
            //}
            //Finance
            var fiscal = db.FiscalYears.Where(u => u.Active == true && u.BranchId == branchId).OrderByDescending(u => u.FiscalYearId).FirstOrDefault();
            if (fiscal != null)
            {
                SessionHelper.FiscalYearId = fiscal.FiscalYearId;
                SessionHelper.FiscalYearName = fiscal.FiscalYearName;
                SessionHelper.FiscalStartDate = fiscal.StartDate;
                SessionHelper.FiscalEndDate = fiscal.EndDate;
            }
            //FrontDesk
            FAPP.FrontDesk.Models.FrontDeskSetting _FrontDeskSetting = db.FrontDeskSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            if (_FrontDeskSetting != null)
            {
                SessionHelper.IsRestaurantAutomation = _FrontDeskSetting.IsRestaurantAutomation;
                SessionHelper.GST = _FrontDeskSetting.GST;
                SessionHelper.KotServiceId = _FrontDeskSetting.KotServiceId;
                SessionHelper.RoomServiceId = _FrontDeskSetting.RoomServiceId;
                SessionHelper.GstAccountId_FD = _FrontDeskSetting.GstAccountId;
                SessionHelper.DiscountAccountId_FD = _FrontDeskSetting.DiscountAccountId;
                SessionHelper.CommisionAccountId = _FrontDeskSetting.CommisionAccountId;
                SessionHelper.IsIndividualItemAccounts_FrontDesk = _FrontDeskSetting.IsIndividualItemAccounts;
                if (_FrontDeskSetting.RoomService != null)
                {
                    SessionHelper.TaxForRoomService = _FrontDeskSetting.RoomService.TaxPercentage;
                }
            }
            //POS
            var _AccountSetting = db.AccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            if (_AccountSetting != null)
            {
                SessionHelper.IsIndividualItemAccounts_POS = _AccountSetting.IsIndividualItemAccounts;
                SessionHelper.GST_POS = _AccountSetting.Tax;
                SessionHelper.GstAccountId_POS = _AccountSetting.TaxAccountId;
                SessionHelper.DiscountAccountId_POS = _AccountSetting.DiscountAccountId;
                SessionHelper.CommisionAccountId_POS = _AccountSetting.CommisionAccountId;
                SessionHelper.IsHideHeaderFooterInPrint_POS = _AccountSetting.IsHideHeaderFooterInPrint;
            }
            //Res Setting
            //var ResSetting = db.ResAccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            //if (ResSetting != null)
            //{
            //    SessionHelper.IsIndividualItemAccounts_Res = ResSetting.IsIndividualItemAccounts;
            //    SessionHelper.GstTax_Res = ResSetting.Tax;
            //    SessionHelper.ServiceCharges_Res = ResSetting.ServiceCharges;
            //    SessionHelper.ServiceChargesAccountId_Res = ResSetting.ServiceChargesAccountId;
            //    SessionHelper.GstTaxAccountId_Res = ResSetting.TaxAccountId;
            //    SessionHelper.DiscountAccountId_Res = ResSetting.DiscountAccountId;
            //    SessionHelper.RestaurantInvoicePrintNote = ResSetting.RestaurantInvoicePrintNote;
            //}
            //Shop Setting
            //var ShopSetting = db.ShopAccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            //if (ShopSetting != null)
            //{
            //    SessionHelper.IsIndividualItemAccounts_Shop = ShopSetting.IsIndividualItemAccounts;
            //    SessionHelper.GstTax_Shop = ShopSetting.Tax;
            //    SessionHelper.ServiceCharges_Shop = ShopSetting.ServiceCharges;
            //    SessionHelper.ServiceChargesAccountId_Shop = ShopSetting.ServiceChargesAccountId;
            //    SessionHelper.GstTaxAccountId_Shop = ShopSetting.TaxAccountId;
            //    SessionHelper.DiscountAccountId_Shop = ShopSetting.DiscountAccountId;
            //}
            //AM Setting
            var AMSetting = db.AMNatures.Where(u => u.BranchId == branchId).FirstOrDefault();
            if (AMSetting != null)
            {
                SessionHelper.IsIndividualItemAccounts_AM = AMSetting.CreateItemAccounts;
            }
        }

        internal static void CheckAndCreateLogoImages(OneDbContext db, Branch _branch, int BranchId)
        {
            if (_branch == null)
            {
                _branch = db.Branches.Where(u => u.BranchId == BranchId).FirstOrDefault();
            }
            if (_branch != null)
            {
                if (_branch.BranchLogoLarge != null && _branch.BranchLogoLarge.Length > 0)
                {
                    string targetFolder = HttpContext.Current.Server.MapPath("~/uploads/Logos");
                    string targetPath = System.IO.Path.Combine(targetFolder, "BranchLogo" + _branch.BranchId + ".png");
                    var stream = new System.IO.MemoryStream(_branch.BranchLogoLarge);
                    System.IO.File.WriteAllBytes(targetPath, stream.ToArray());
                }
            }
        }


        //public class v_mnl_FinishgoodVsMaterialIssuance_Result
        //{
        //    public int IssueItemId { get; set; }
        //    public DateTime IssueItemDate { get; set; }
        //    public int OrderId { get; set; }
        //    public DateTime OrderDate { get; set; }
        //    public DateTime FinishGoodVoucherDate { get; set; }
        //    public decimal Quantity { get; set; }
        //    public string ProductName { get; set; }
        //    public decimal productionQuantity { get; set; }
        //    public string ProductGroupName { get; set; }
        //    public decimal EndQty { get; set; }
        //    public int FinishGoodVoucherId { get; set; }
        //    public int MaterialIssuanceId { get; set; }
        //    public DateTime MaterialIssuanceDate { get; set; }
        //    public string UnitTitleName { get; set; }
        //    public int ProductGroupId { get; set; }
        //    public string ParentUnitTitleName { get; set; }
        //    public decimal EquivalentQty { get; set; }
        //}
        //public class v_mnl_OrderPaymentStatusView
        //{
        //    public decimal PaidAmount { get; set; }
        //    public string Status { get; set; }
        //}
        //public static string encode(this string text)
        //{
        //    return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
        //}
        //public static string decode(this string text)
        //{
        //    return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(text));
        //}
        //public class TrailLogViewModel
        //{
        //    [Display(Name = "Audit Date")]
        //    public DateTime AuditDate { get; set; }
        //    public string Column { get; set; }
        //    public string IP { get; set; }
        //    [Display(Name = "User")]
        //    public string ModifiedBy { get; set; }
        //    [Display(Name = "New Value")]
        //    public string NewValue { get; set; }
        //    [Display(Name = "Old Value")]
        //    public string OldValue { get; set; }
        //    public string Operation { get; set; }
        //    [Display(Name = "Table Name")]
        //    public string TableName { get; set; }
        //    public string Area { get; set; }
        //    public string Controller { get; set; }
        //    public string Action { get; set; }
        //    [Display(Name = "Branch Name")]
        //    public string BranchName { get; set; }
        //    public short BranchId { get; set; }
        //    public int UserId { get; set; }
        //}
        //public class v_mnl_AuditLog_Result
        //{
        //    public long AuditId { get; set; }
        //    public DateTime AuditDate { get; set; }
        //    public string TableName { get; set; }
        //    public string Operation { get; set; }
        //    public string Col { get; set; }
        //    public string OldVal { get; set; }
        //    public string NewVal { get; set; }
        //    public int ModifiedBy { get; set; }
        //    public string IP { get; set; }
        //    public string PrimaryKeyValue { get; set; }
        //    public string Url { get; set; }
        //    public string Username { get; set; }
        //}

        //public class v_mnl_SaleInvoices_ProfitCalculationAll
        //{
        //    public long SaleInvoiceId { get; set; }
        //    public int ClientId { get; set; }
        //    public int? CategoryId { get; set; }
        //    public string CategoryName { get; set; }
        //    public DateTime SaleInvoiceDate { get; set; }
        //    public string Description { get; set; }
        //    public decimal Discount { get; set; }
        //    public decimal? TotalAmount { get; set; }
        //    public decimal? NetTotal { get; set; }
        //    public decimal? Received { get; set; }
        //    public decimal? CostPrice { get; set; }
        //    public decimal? SalePrice { get; set; }
        //    public decimal? Quantity { get; set; }
        //    public decimal? TotalProfit { get; set; }
        //    public short? BranchId { get; set; }
        //    public decimal? ReceivedAmount { get; set; }
        //    public string ClientName { get; set; }
        //    public string CounterName { get; set; }
        //    public string SaleInvoiceIds { get; set; }
        //    public int TotalInvoices { get; set; }
        //    public int ProductId { get; set; }
        //    public string ProductName { get; set; }
        //}
        //public static List<v_mnl_SaleInvoices_ProfitCalculationAll> v_mnl_ProfitCalculationAll(OneDbContext db)
        //{
        //    var result = db.Database.SqlQuery<v_mnl_SaleInvoices_ProfitCalculationAll>("SELECT si.SaleInvoiceId,sip.ProductId,p.ProductName,p.CostPrice,p.SalePrice,sip.Quantity,si.SaleInvoiceDate,si.Description,si.Discount,(p.SalePrice - p.CostPrice) as TotalProfit FROM  POS.SaleInvoices AS si left join POS.SaleInvoiceProducts sip on sip.SaleInvoiceId = si.SaleInvoiceId left join pos.Products p on p.ProductId = sip.ProductId order by CostPrice desc").ToList();
        //    return result;
        //}
        public static List<v_mnl_EmployeeAttendance> v_mnl_EmployeeAttendanceList(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_EmployeeAttendance>($@"SELECT TOP(25) emp.EmpName, att.AttendanceDate, att.Arrival, att.AttendanceStatus, att.Departure
FROM            HR.EmployeeAttendances AS att INNER JOIN
						 HR.Employees AS emp ON att.EmployeeId = emp.EmployeeId Order By AttendanceDate Desc").ToList();
            return result;
        }

        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
            return words;
        }
        public static string NumberToWords(double doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)}";
            var afterFloatingPointWord =
                $"{SmallNumberToWord((int)((doubleNumber - beforeFloatingPoint) * 100), "")}";
            if (afterFloatingPointWord == "")
            {
                return $"{beforeFloatingPointWord} Only";
            }
            else
            {
                return $"{beforeFloatingPointWord} Point {afterFloatingPointWord} Only";
            }
        }
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }


        public class v_mnl_Vouchers_Result
        {
            public bool? IsCheked { get; set; }
            public long VoucherId { get; set; }
            public string VoucherType { get; set; }
            public string VoucherName { get; set; }
            public DateTime TransactionDate { get; set; }
            public decimal DebitAmount { get; set; }
            public decimal CreditAmount { get; set; }
            public decimal? Balance { get; set; }
            public string FiscalYear { get; set; }
            public int FiscalYearId { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string Particulars { get; set; }
            public int? _CreatedByUserId { get; set; }
            public int? _ModifiedByUserId { get; set; }
            public string VoucherStatus { get; set; }
            public bool IsPosted { get; set; }
            public bool IsReconciled { get; set; }
            public bool IsCancelled { get; set; }
            public bool IsApproved { get; set; }
            public short? CurrencyId { get; set; }
            public decimal? ExchangeRate { get; set; }
            public string CurrencyName { get; set; }
            public int? _postedByUserId { get; set; }
            public int? _reconciledByUserId { get; set; }
            public int? _approvedByUserId { get; set; }
            public int? _cancelledByUserId { get; set; }
            public int? ApprovedBy { get; set; }
            public int? PostedBy { get; set; }
            public int? CancelledBy { get; set; }
            public string _IsApproved { get; set; }
            public string _IsPosted { get; set; }
            public string _IsReconciled { get; set; }
            public string _IsCancelled { get; set; }
            public string _ApprovedByName { get; set; }
            public string _PostedByName { get; set; }
            public string _ReconciledByName { get; set; }
            public string _CreatedByName { get; set; }
            public string _CancelledByName { get; set; }
            public string _ModifiedByName { get; set; }
            public string CBAccountId { get; set; }
            public string AccountId { get; set; }
            public string CashAccount { get; set; }
            public string BankAccount { get; set; }
            public string VoucherAccounts { get; set; }
            public short BranchId { get; set; }
            public int? ProjectId { get; set; }
            public DateTime? PostedOn { get; set; }
            public DateTime? ReconciledOn { get; set; }
            public DateTime? CancelledOn { get; set; }


        }


    }
}