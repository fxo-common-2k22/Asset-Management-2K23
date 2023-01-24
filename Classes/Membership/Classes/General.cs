using FAPP.Model;
using FAPP.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FAPP
{
    public static class General
    {
        public static int GetMaxUserId(OneDbContext db)
        {
            return db.Database.SqlQuery<int?>("SELECT [dbo].[f__GetMaxIDForUserEmpStu] ()").FirstOrDefault() ?? 1;
        }

        public static async Task<int> GetMaxUserIdAsycn(OneDbContext db)
        {
            return await db.Database.SqlQuery<int?>("SELECT [dbo].[f__GetMaxIDForUserEmpStu] ()").FirstOrDefaultAsync() ?? 1;
        }

        public static async Task<bool> UserIdExists(OneDbContext db, int studentId)
        {
            var id = await db.Database.SqlQuery<int?>($@"SELECT 1
FROM (
	SELECT StudentId Id
		, 1 y
	FROM Academics.Students
	
	UNION
	
	SELECT UserId
		, 2
	FROM Membership.Users
	
	UNION
	
	SELECT EmployeeId
		, 3
	FROM HR.Employees
	) grf
WHERE Id = {studentId}").FirstOrDefaultAsync();

            return id.HasValue;
        }

    }

}

namespace FAPP.ViewModel.Common
{


    public static partial class ProceduresModel
    {
        public static string p_mnl_FillGroupRights(OneDbContext db, int? GroupId)
        {
            var result = db.Database.ExecuteSqlCommand("INSERT INTO [Membership].[GroupRights] ( [GroupId], [FormRightId],[Allowed]) SELECT @GroupId AS[GroupId] , [fr].[FormRightId],1 FROM[Membership].[FormRights] AS[fr] EXCEPT SELECT[GroupId] , [FormRightId],1 FROM[Membership].[GroupRights]",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId))
                ).ToString();
            return "";
        }

        public static string p_mnl_FillFormRights(OneDbContext db)
        {
            //			//var result = db.Database.ExecuteSqlCommand("DECLARE @TBL TABLE( [Action] VARCHAR(50)); INSERT INTO @TBL ( [Action] ) SELECT 'Can Create' UNION SELECT 'Can Read' UNION SELECT 'Can Update' UNION  SELECT 'Can Delete'; INSERT INTO[Membership].[FormRights] ([FormRightName], [FormId] ) SELECT[t].[Action], [FormId] FROM @TBL AS[t] CROSS JOIN [Membership].[Forms] EXCEPT SELECT FormRightName, FormId FROM Membership.FormRights;").ToString();
            //			var result = db.Database.ExecuteSqlCommand($@"DECLARE @TBL1 TABLE( FormID int, FormURL VARCHAR(100),FormName VARCHAR(50), ParentForm int, 
            //IsMenuItem bit, MenuText VARCHAR(50), MenuItemPriority int, isActive VARCHAR(5), ShowOnDesktop bit,IsDashboardPart bit,PageDescription varchar(50));

            //INSERT INTO @TBL1
            //select 1,'_PartialShowStats','Stats area',1,0,'ShowStats',0,'Yes',0,1,'col-lg-8'
            //UNION
            //select 1,'_PartialShowWeeklySale','WeeklySale graph',1,0,'ShowWeeklySale',0,'Yes',0,1,'col-lg-8'
            //UNION
            //select 1,'_PartialShowTransactions','Transactions grpah',1,0,'ShowTransactions',0,'Yes',0,1,'col-lg-8'
            //UNION
            //select 1,'_PartialShowDashboardMenus','Dashboard Menus area',1,0,'ShowDashboardMenus',0,'Yes',0,1,'col-lg-8'
            //UNION
            //select 1,'_PartialShowIncomeExpense','Income Expense graph',1,0,'ShowIncomeExpense',0,'Yes',0,1,'col-lg-8'
            //UNION
            //select 1,'_PartialShowQuickLinks','QuickLinks area',1,0,'ShowQuickLinks',0,'Yes',0,1,'col-lg-4'
            //UNION
            //select 1,'_PartialShowSomething','Something area',1,0,'ShowSomething',0,'Yes',0,1,'col-lg-4'
            //UNION
            //select 1,'_PartialShowTopIcons','Top icons area',1,0,'Top icons area',0,'Yes',0,1,''

            //declare @nextFormId int
            //select @nextFormId =Max(FormID) from Membership.Forms

            //insert into Membership.Forms (FormID,FormName, FormURL, ParentForm, IsMenuItem, MenuText, MenuItemPriority, isActive, ShowOnDesktop,ModuleId,IsDashboardPart,PageDescription)
            //SELECT @nextFormId +ROW_NUMBER() OVER(ORDER BY y.FormID) as FormId, y.FormName ,y.FormURL, y.FormID as ParentForm,tb.IsMenuItem, tb.MenuText, tb.MenuItemPriority, tb.isActive, tb.ShowOnDesktop,0,1,tb.PageDescription FROM (
            //select  dm.FormURL, m.FormID, dm.FormName
            //FROM @TBL1 as dm CROSS JOIN  (select FormID from Membership.Forms where FormName='Dashboard') M
            //EXCEPT
            //SELECT FormURL, ParentForm, FormName FROM [Membership].[Forms]
            //) y inner join @TBL1 as tb on tb.FormURL = y.FormURL

            //DECLARE @TBL TABLE( [Action] VARCHAR(50)); INSERT INTO @TBL ( [Action] ) 
            //SELECT 'Can Create' 
            //UNION 
            //SELECT 'Can Read' 
            //UNION 
            //SELECT 'Can Update' 
            //UNION  
            //SELECT 'Can Delete'; 
            //INSERT INTO[Membership].[FormRights] ([FormRightName], [FormId] ) 
            //SELECT[t].[Action], [FormId] FROM @TBL AS[t] CROSS JOIN[Membership].[Forms] 
            //EXCEPT
            //SELECT FormRightName, FormId FROM Membership.FormRights;").ToString();
            //return result;
            return "";
        }

        public class v_mnl_UserBranches_Result
        {
            public int UserBranchId { get; set; }
            public int UserId { get; set; }
            public short BranchId { get; set; }
            public Nullable<bool> Active { get; set; }
            public bool DefaultBranch { get; set; }
            [Display(Name = "Branch Name")]
            public string Name { get; set; }
            [Display(Name = "Branch Code")]
            public short BranchCode { get; set; }
            [Display(Name = "Address")]
            public string AddressLine1 { get; set; }
            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Email")]
            public string EmailAddress { get; set; }
            [Display(Name = "Prefix")]
            public string RegPrefix { get; set; }
            public byte[] BranchLogoMini { get; set; }
            public byte[] BranchLogoSmall { get; set; }
            public byte[] BranchLogoLarge { get; set; }
            [Display(Name = "Company Name")]
            public string CompanyName { get; set; }
            public string NTN { get; set; }
            public string GSTN { get; set; }
            [UIHint("ActiveInactiveIcons")]
            public bool IsMasterBranch { get; set; }
        }

        public static IQueryable<v_mnl_UserBranches_Result> v_mnl_UserBranches(OneDbContext db)
        {
            //var result = db.Database.SqlQuery<v_mnl_UserBranches_Result>("SELECT        Membership.UserBranches.UserBranchId, Membership.UserBranches.UserId, Membership.UserBranches.BranchId, Membership.UserBranches.Active, Membership.UserBranches.DefaultBranch, Company.Branches.Name FROM            Company.Branches INNER JOIN Membership.UserBranches ON Company.Branches.BranchId = Membership.UserBranches.BranchId").ToList();

            var result = db.UserBranches.Select(p => new v_mnl_UserBranches_Result
            {
                UserBranchId = p.UserBranchId,
                Active = p.Active,
                BranchId = p.BranchId,
                UserId = p.UserId,
                DefaultBranch = p.DefaultBranch,
                Name = p.Branch.Name,
                PhoneNumber = p.Branch.PhoneNumber,
                EmailAddress = p.Branch.EmailAddress,
                RegPrefix = p.Branch.RegPrefix,
                BranchCode = p.Branch.BranchCode,
                AddressLine1 = p.Branch.AddressLine1,
                BranchLogoMini = p.Branch.BranchLogoMini,
                BranchLogoSmall = p.Branch.BranchLogoSmall,
                BranchLogoLarge = p.Branch.BranchLogoLarge,
                CompanyName = p.Branch.CompanyName,
                NTN = p.Branch.NTN,
                GSTN = p.Branch.GSTN,
                IsMasterBranch = p.Branch.IsMasterBranch
            });

            return result;
        }

        public class p_mnl_DashboardMenus_Result
        {
            public string MenuText { get; set; }
            public string FormUrl { get; set; }
            public string Icon { get; set; }
            public bool IsDashboardPart { get; set; }
            public string FaIcon { get; set; }
        }

        public static List<p_mnl_DashboardMenus_Result> p_mnl_DashboardMenus(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_DashboardMenus_Result>(@"DECLARE @ParentForm INT

SET @ParentForm = (
		SELECT TOP 1 ParentForm
		FROM Membership.Forms
		WHERE FormUrl LIKE @URL
		)

IF (@ParentForm IS NULL)
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
ELSE
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = @ParentForm
		AND isActive = 'Yes'
		AND Allowed = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }

        public class p_mnl_QuickLinks_Result
        {
            public string MenuText { get; set; }
            public string FormUrl { get; set; }
            public string Icon { get; set; }
            public bool IsDashboardPart { get; set; }
            public string FaIcon { get; set; }
        }

        public static List<p_mnl_QuickLinks_Result> p_mnl_QuickLinks(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_QuickLinks_Result>($@"DECLARE @ParentForm BIGINT

SET @ParentForm = (
		SELECT TOP 1 ParentForm
		FROM Membership.Forms
		WHERE FormUrl LIKE @URL
		)

IF (@ParentForm IS NULL)
BEGIN
	SELECT MenuText, FormUrl,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
ELSE
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = @ParentForm
		AND isActive = 'Yes'
		AND Allowed = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }
        public static List<p_mnl_QuickLinks_Result> p_mnl_QuickLinks_Main_Dashboard(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_QuickLinks_Result>($@"
BEGIN
	SELECT MenuText, FormUrl,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
        AND FormName not like '%Dashboard%'
	ORDER BY MenuItemPriority
END",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }

        #region Membership

        public class FormAndGroupRightsViewModel
        {
            public List<v_mnl_FormRights_Result> FormRights { get; set; }

            public List<v_mnl_FormRights_Result> GroupRights { get; set; }
        }

        public static FormAndGroupRightsViewModel GetFormAndGroupRights(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var vm = new FormAndGroupRightsViewModel();
            vm.FormRights = v_mnl_FormRights(db, groupId, allowed, actionName, url);
            vm.GroupRights = v_mnl_DashboardViews(db, groupId, true, "Can Read", url);
            return vm;
        }

       

        public static List<v_mnl_FormRights_Result> v_mnl_FormRights(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
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
            //string masterMenuCondition = string.Empty;
            //if (!SessionHelper.IsMasterBranch)
            //{
            //    masterMenuCondition = "And IsMasterMenu  != 1";
            //} 

            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
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
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId 
WHERE Membership.UserGroups.UserGroupId = {groupId} { allowedStr } { actionNameStr } {urlStr} ").ToList();
            return result;
        }

        public static IQueryable<v_mnl_FormRights_Result> v_mnl_SingleFormRights(OneDbContext db, int groupId, string url)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
DECLARE @FormId INT
DECLARE @GroupId INT = '{groupId}'
DECLARE @FormURL VARCHAR(4000) = '{url}'

SELECT @FormId = FormId
FROM Membership.Forms
WHERE FormURL = @FormURL

SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
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
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
WHERE (Membership.UserGroups.UserGroupId = @GroupId)
	AND Form.ParentForm = @FormId
	AND FormRightName = 'Can Read'
").AsQueryable();
            return result;
        }


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


        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRights(OneDbContext db, int ModuleId, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
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
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
WHERE Form.ModuleId = @FormID
	AND (
		isActive = 'Yes'
		OR Form.isActive = 'Active'
		)
	AND UserGroupId = @UserGroupId
ORDER BY form.formid
	, MenuItemPriority", new SqlParameter("@FormID", GetDBNullOrValue(ModuleId))
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }

        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRights(OneDbContext db, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
declare @Modules table(ModuleId int, ModuleName varchar(50)) 
insert into @Modules(ModuleId, ModuleName)
select ModuleId, MenuText from Membership.Forms where isActive = 'Yes' and ParentForm = 1

SELECT Form.FormID, Form.MenuText, FormRights.FormRightName, GroupRights.GroupId, Form.ParentForm, Membership.UserGroups.UserGroupName,
GroupRights.Allowed, Form.ControllerName, Form.isActive, Form.FormName, Form.FormURL, FormRights.FormRightId, GroupRights.GroupRightId,
Form.IsMenuItem, Form.MenuItemPriority, Form.Icon, Form.PageType, Form.ModuleId,
(select top 1 '(' + ModuleName + ')' from @Modules as mf where mf.ModuleId = Form.ModuleId) as Module,
Form.IsDashboardPart ,Form.PageDescription,Form.IsAction
FROM Membership.Forms AS Form INNER JOIN
Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN
Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN
Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
where(isActive = 'Yes' or Form.isActive = 'Active') and UserGroupId = @UserGroupId
order by form.formid, MenuItemPriority"
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }

        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRightsQueryable(OneDbContext db, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"declare @Modules table(ModuleId int, ModuleName varchar(50)) 
insert into @Modules(ModuleId, ModuleName)
select ModuleId, MenuText from Membership.Forms where isActive = 'Yes' and ParentForm = 1

SELECT Form.FormID, Form.MenuText, FormRights.FormRightName, GroupRights.GroupId, Form.ParentForm, Membership.UserGroups.UserGroupName,
GroupRights.Allowed, Form.ControllerName, Form.isActive, Form.FormName, Form.FormURL, FormRights.FormRightId, GroupRights.GroupRightId,
Form.IsMenuItem, Form.MenuItemPriority, Form.Icon, Form.PageType, Form.ModuleId,
(select top 1 '(' + ModuleName + ')' from @Modules as mf where mf.ModuleId = Form.ModuleId) as Module,
Form.IsDashboardPart ,Form.PageDescription
FROM Membership.Forms AS Form INNER JOIN
Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN
Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN
Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
where(isActive = 'Yes' or Form.isActive = 'Active') and UserGroupId = @UserGroupId
order by form.formid, MenuItemPriority"
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }

        #endregion



        public static int InsertInMembershipModules(OneDbContext db, Module ex)
        {
            int result = 0;
            var IfExists = db.Modules.Where(s => s.ModuleName == ex.ModuleName).Any();
            if (!IfExists)
            {
                db.Modules.Add(ex);
                result = db.SaveChanges();
            }
            return result;
        }
        public static int InsertInMembershipForms(OneDbContext db, Form ex,short BranchId)
        {
           var result= InsertInMembershipFormsWithStaticFormId(db, ex,BranchId);
            return result;
//            int result = 0;
//            var ismenutext = ex.IsMenuItem == false ? 0 : 1;
//            var isdashboard = ex.IsDashboardPart == false ? 0 : 1;
//            var showondesktop = ex.ShowOnDesktop == false ? 0 : 1;
//            var ismaster = ex.IsMasterMenu == false ? 0 : 1;
//            var isquicklink = ex.IsQuickLink == false ? 0 : 1;
//            var IsAction = ex.IsAction == false ? 0 : 1;
//            if (ex.ParentForm > 0)
//            {
//                result = db.Database.ExecuteSqlCommand($@"
//DECLARE @TBL1 TABLE (
//	FormID INT
//	, FormURL VARCHAR(100)
//	, FormName VARCHAR(50)
//	, ParentForm INT
//	, IsMenuItem BIT
//	, MenuText VARCHAR(50)
//	, MenuItemPriority INT
//	, isActive VARCHAR(5)
//	, ShowOnDesktop BIT
//	, IsDashboardPart BIT
//	, PageDescription VARCHAR(50)
//	);
//--showOnDesktop is 0 when Permission and Isquicklink is also zero and IsDashboard part is 1
//DECLARE @nextFormId INT
//	, @FormName VARCHAR(max)
//	, @ParentId INT
//	, @FormUrl VARCHAR(max)
//	, @MenuText VARCHAR(max)
//	, @IsMenuItem BIT
//	, @MenuItemPriority SMALLINT
//	, @isActive VARCHAR(max)
//	, @IsDashboardPart BIT
//	, @ShowOnDesktop BIT
//	, @PageDescription NVARCHAR(max)
//	, @ModuleId INT
//	, @IsMasterMenu BIT
//	, @IsGroupReportHead BIT
//	, @IsAjaxRequest BIT
//	, @IsHideChilds BIT
//	, @IsAction BIT
//	, @PageType NVARCHAR(max)
//	, @IsQuickLink BIT

            //SELECT @nextFormId = Max(FormID)
            //FROM Membership.Forms

            //SET @nextFormId = @nextFormId + 1
            //SET @FormName = '{ex.FormName}'
            //SET @ParentId = '{ex.ParentForm}'
            //SET @FormUrl = '{ex.FormURL}'
            //SET @MenuText = '{ex.MenuText}'
            //SET @IsMenuItem = '{ismenutext}'
            //SET @MenuItemPriority = '{ex.MenuItemPriority}'
            //SET @isActive = '{ex.isActive}'
            //SET @IsDashboardPart = '{isdashboard}'
            //SET @PageDescription = '{ex.PageDescription}'
            //SET @ShowOnDesktop = '{showondesktop}'
            //SET @ModuleId = {ex.ModuleId}
            //SET @IsMasterMenu = {ismaster}
            //SET @IsGroupReportHead = 0
            //SET @IsAjaxRequest = 0
            //SET @IsHideChilds = 0
            //SET @IsAction = '{IsAction}'
            //SET @PageType = '{ex.PageType}'
            //SET @IsQuickLink = '{isquicklink}'

            //PRINT @nextFormId

            //IF NOT EXISTS (
            //		SELECT FormName
            //		FROM Membership.Forms
            //		WHERE FormName = @FormName
            //			AND ParentForm = @ParentId
            //		)
            //BEGIN TRY
            //	BEGIN TRANSACTION

            //	INSERT INTO Membership.Forms (
            //		FormID
            //		, FormName
            //		, FormURL
            //		, ParentForm
            //		, IsMenuItem
            //		, MenuText
            //		, MenuItemPriority
            //		, isActive
            //		, ShowOnDesktop
            //		, ModuleId
            //		, IsDashboardPart
            //		, PageDescription
            //		, IsMasterMenu
            //		, IsGroupReportHead
            //		, IsAjaxRequest
            //		, IsHideChilds
            //		, IsAction
            //		, PageType
            //		, IsQuickLink
            //		)
            //	VALUES (
            //		@nextFormId
            //		, @FormName
            //		, @FormUrl
            //		, @ParentId
            //		, @IsMenuItem
            //		, @MenuText
            //		, @MenuItemPriority
            //		, @isActive
            //		, @ShowOnDesktop
            //		, @ModuleId
            //		, @IsDashboardPart
            //		, @PageDescription
            //		, @IsMasterMenu
            //		, @IsGroupReportHead
            //		, @IsAjaxRequest
            //		, @IsHideChilds
            //		, @IsAction
            //		, @PageType
            //		, @IsQuickLink
            //		)

            //	DECLARE @TBL TABLE ([Action] VARCHAR(50));

            //	INSERT INTO @TBL ([Action])
            //	SELECT 'Can Create'

            //	UNION

            //	SELECT 'Can Read'

            //	UNION

            //	SELECT 'Can Update'

            //	UNION

            //	SELECT 'Can Delete';

            //	INSERT INTO [Membership].[FormRights] (
            //		[FormRightName]
            //		, [FormId]
            //		)
            //	SELECT [t].[Action]
            //		, [FormId]
            //	FROM @TBL AS [t]
            //	CROSS JOIN [Membership].[Forms]

            //	EXCEPT

            //	SELECT FormRightName
            //		, FormId
            //	FROM Membership.FormRights;

            //	INSERT INTO [Membership].[GroupRights] (
            //		[GroupId]
            //		, [FormRightId]
            //		, [Allowed]
            //		)
            //	SELECT (
            //			SELECT TOP 1 UserGroupId
            //			FROM Membership.UserGroups
            //			WHERE UserGroupName LIKE 'admin'
            //			) AS [GroupId]
            //		, [fr].[FormRightId]
            //		, 1
            //	FROM [Membership].[FormRights] AS [fr]

            //	EXCEPT

            //	SELECT [GroupId]
            //		, [FormRightId]
            //		, 1
            //	FROM [Membership].[GroupRights]

            //	COMMIT
            //END TRY

            //BEGIN CATCH
            //	ROLLBACK
            //END CATCH	 ");
            //            }
            //            return result;
        }
        public static int InsertInMembershipFormsWithStaticFormId(OneDbContext db, Form ex,short BranchId)
        {
            int result = 0;
            var ismenutext = ex.IsMenuItem == true ? 1 : 0;
            var isdashboard = ex.IsDashboardPart == true ? 1 : 0;
            var showondesktop = ex.ShowOnDesktop == true ? 1 : 0;
            var ismaster = ex.IsMasterMenu == true ? 1 : 0;
            var isquicklink = ex.IsQuickLink == true ? 0 : 0;
            var IsAction = ex.IsAction == true ? 1 : 0;
            var isreport = ex.IsReport == true ? 1 : 0;
            var isgroupreport = ex.IsGroupReportHead == true ? 1 : 0;
            if (ex.ParentForm > 0)
            {
                var query = $@"
                --showOnDesktop is 0 when Permission and Isquicklink is also zero and IsDashboard part is 1
                DECLARE @nextFormId BIGINT
	                , @FormName VARCHAR(max)
	                , @ParentId BIGINT
	                , @FormUrl VARCHAR(max)
	                , @MenuText VARCHAR(max)
	                , @IsMenuItem BIT
	                , @MenuItemPriority SMALLINT
	                , @isActive VARCHAR(max)
	                , @IsDashboardPart BIT
	                , @ShowOnDesktop BIT
	                , @PageDescription NVARCHAR(max)
	                , @ModuleId INT
	                , @IsMasterMenu BIT
	                , @IsAjaxRequest BIT
	                , @IsHideChilds BIT
	                , @IsAction BIT
	                , @PageType NVARCHAR(max)
	                , @IsQuickLink BIT
	                , @IsReport BIT
	                , @IsGroupReportHead BIT
                    , @DefaultReportFileName NVARCHAR(max)
                    , @Area nvarchar(50)
                    , @Controller nvarchar(50)
                    , @Action nvarchar(50)";
                if (ex.FormID > 0)
                {
                    query += $@"SET @nextFormId = {ex.FormID}";
                }
                else
                {
                    query += $@"
                                SELECT @nextFormId = ISNULL((
			                                SELECT Max(FormID)
			                                FROM Membership.Forms
			                                WHERE ParentForm = {ex.ParentForm}
			                                ), (
			                                CONCAT (
				                                '{ex.ParentForm}'
				                                , '00'
				                                )
			                                ))

                                SET @nextFormId = @nextFormId + 1

                                PRINT @nextFormId";
                }
                query += $@"
                            SET @FormName = '{ex.FormName}'
                            SET @ParentId = '{ex.ParentForm}'
                            SET @FormUrl = '{ex.FormURL}'
                            SET @MenuText = '{ex.MenuText}'
                            SET @IsMenuItem = '{ismenutext}'
                            SET @MenuItemPriority = '{ex.MenuItemPriority}'
                            SET @isActive = '{ex.isActive}'
                            SET @IsDashboardPart = '{isdashboard}'
                            SET @PageDescription = '{ex.PageDescription}'
                            SET @ShowOnDesktop = '{showondesktop}'
                            SET @ModuleId = {ex.ModuleId}
                            SET @IsMasterMenu = {ismaster}
                            SET @IsAjaxRequest = 0
                            SET @IsHideChilds = 0
                            SET @IsAction = '{IsAction}'
                            SET @IsReport = '{isreport}'
                            SET @IsGroupReportHead = '{isgroupreport}'
                            SET @PageType = '{ex.PageType}'
                            SET @IsQuickLink = '{isquicklink}'
                            SET @DefaultReportFileName = '{ex.DefaultReportFileName}'
                            SET @Area = '{ex.Area}'
                            SET @Controller = '{ex.Controller}'
                            SET @Action = '{ex.Action}'

                            PRINT @nextFormId

                            IF NOT EXISTS (
		                            SELECT FormName
		                            FROM Membership.Forms
		                            WHERE FormName = @FormName
			                            AND ParentForm = @ParentId
		                            )
                            BEGIN TRY
	                            BEGIN TRANSACTION
	                            INSERT INTO Membership.Forms (
		                            FormID
		                            , FormName
		                            , FormURL
		                            , ParentForm
		                            , IsMenuItem
		                            , MenuText
		                            , MenuItemPriority
		                            , isActive
		                            , ShowOnDesktop
		                            , ModuleId
		                            , IsDashboardPart
		                            , PageDescription
		                            , IsMasterMenu
		                            , IsAjaxRequest
		                            , IsHideChilds
		                            , IsAction
		                            , PageType
		                            , IsQuickLink
		                            , IsReport
		                            , IsGroupReportHead
		                            , DefaultReportFileName
		                            , Area
		                            , Controller
		                            , Action
		                            )
	                            VALUES (
		                            @nextFormId
		                            , @FormName
		                            , @FormUrl
		                            , @ParentId
		                            , @IsMenuItem
		                            , @MenuText
		                            , @MenuItemPriority
		                            , @isActive
		                            , @ShowOnDesktop
		                            , @ModuleId
		                            , @IsDashboardPart
		                            , @PageDescription
		                            , @IsMasterMenu
		                            , @IsAjaxRequest
		                            , @IsHideChilds
		                            , @IsAction
		                            , @PageType
		                            , @IsQuickLink
		                            , @IsReport
		                            , @IsGroupReportHead
		                            , @DefaultReportFileName
		                            , @Area
		                            , @Controller
		                            , @Action
		                            )


                                    IF (@IsReport = 1)
                                    BEGIN
	                                    IF NOT EXISTS (
			                                    SELECT FormId
			                                    FROM Membership.Reports
			                                    WHERE FormId = ''
			                                    )
	                                    BEGIN
		                                    INSERT INTO Membership.Reports (
			                                    FormId
			                                    , ReportFileName
                                                ,BranchId
			                                    )
		                                    VALUES (
			                                    @nextFormId
			                                    , @FormName
                                                ,{BranchId}
			                                    )
	                                    END
                                    END              

                            DECLARE @TBL TABLE ([Action] VARCHAR(50));
	                            INSERT INTO @TBL ([Action])
	                            SELECT 'Can Create'
	
	                            UNION
	
	                            SELECT 'Can Read'
	
	                            UNION
	
	                            SELECT 'Can Update'
	
	                            UNION
	
	                            SELECT 'Can Delete';

	                            INSERT INTO [Membership].[FormRights] (
		                            [FormRightName]
		                            , [FormId]
		                            )
	                            SELECT [t].[Action]
		                            , [FormId]
	                            FROM @TBL AS [t]
	                            CROSS JOIN [Membership].[Forms]
	
	                            EXCEPT
	
	                            SELECT FormRightName
		                            , FormId
	                            FROM Membership.FormRights;

	                            INSERT INTO [Membership].[GroupRights] (
		                            [GroupId]
		                            , [FormRightId]
		                            , [Allowed]
		                            )
	                            SELECT (
			                            SELECT TOP 1 UserGroupId
			                            FROM Membership.UserGroups
			                            WHERE UserGroupName LIKE 'admin'
			                            ) AS [GroupId]
		                            , [fr].[FormRightId]
		                            , 1
	                            FROM [Membership].[FormRights] AS [fr]
	
	                            EXCEPT
	
	                            SELECT [GroupId]
		                            , [FormRightId]
		                            , 1
	                            FROM [Membership].[GroupRights]

	                            COMMIT
                            END TRY

                            BEGIN CATCH
	                            ROLLBACK
                            END CATCH
                        ";
                result = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, query);
            }
            return result;
        }
        public static int UpdateMembershipForms(OneDbContext db, Form ex)
        {
            int result = 0;
            var ismenutext = ex.IsMenuItem == false ? 0 : 1;
            var isdashboard = ex.IsDashboardPart == false ? 0 : 1;
            var showondesktop = ex.ShowOnDesktop == false ? 0 : 1;
            var ismaster = ex.IsMasterMenu == false ? 0 : 1;
            var isquicklink = ex.IsQuickLink == false ? 0 : 1;
            var IsAction = ex.IsAction == false ? 0 : 1;
            if (ex.ParentForm > 0)
            {
                result = db.Database.ExecuteSqlCommand($@"
              
                DECLARE @ParentId int,@FormUrl Varchar(max), @MenuText varchar(max)
                ,@IsMenuItem bit,@MenuItemPriority smallint,@isActive varchar(max),@IsDashboardPart bit,@ShowOnDesktop bit
                ,@PageDescription nvarchar(max),@ModuleId int,@IsMasterMenu bit,@IsGroupReportHead bit
                ,@IsAjaxRequest bit,@IsHideChilds bit,@IsAction bit,@PageType nvarchar(max),@IsQuickLink bit

                set @ParentId='{ex.ParentForm}'
                set @FormUrl='{ex.FormURL}'
                set @MenuText='{ex.MenuText}'
                set @IsMenuItem='{ismenutext}'
                set @MenuItemPriority='{ex.MenuItemPriority}'
                set @isActive='{ex.isActive}'
                set @IsDashboardPart='{isdashboard}'
                set @PageDescription=''
                set @ShowOnDesktop='{showondesktop}'
                set @ModuleId={ex.ModuleId}
                set @IsMasterMenu={ismaster}
                set @IsGroupReportHead=0
                set @IsAjaxRequest=0
                set @IsHideChilds=0
                set @IsAction='{IsAction}'
                set @PageType='{ex.PageType}'
                set @IsQuickLink='{isquicklink}'
				Update
				Membership.Forms 
				set 
				FormURL = @FormUrl, 
				ParentForm = @ParentId,
				IsMenuItem = @IsMenuItem,
				MenuText = @MenuText,
				MenuItemPriority = @MenuItemPriority, 
				isActive = @isActive, 
				ShowOnDesktop = @ShowOnDesktop, 
				ModuleId  = @ModuleId, 
				IsDashboardPart = @IsDashboardPart, 
				PageDescription = @PageDescription, 
				IsMasterMenu = @IsMasterMenu,
				IsGroupReportHead = @IsGroupReportHead,
				IsAjaxRequest = @IsAjaxRequest, 
				IsHideChilds = @IsHideChilds, 
				IsAction = @IsAction,
				PageType = @PageType,
				IsQuickLink = @IsQuickLink
	            where FormID = {ex.FormID}");
            }
            return result;
        }

        public static bool DeleteGroupoRightsAndForms(OneDbContext db, int id, bool status)
        {
            if (id > 0)
            {
                var dltrows = db.Database.ExecuteSqlCommand($@"
                delete gr from Membership.GroupRights gr
                join Membership.FormRights fr on fr.FormRightId=gr.FormRightId
                where fr.FormId={id}
                delete Membership.FormRights where FormId={id}
                delete Membership.Forms where FormID={id}
                ");
                if (dltrows > 0)
                {
                    status = true;
                }
            }

            return status;
        }
    }
}