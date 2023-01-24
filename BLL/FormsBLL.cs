using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FAPP.Model;
using FAPP.ViewModel.FormModels;

namespace FAPP.BLL
{
    public static class FormsBLL
    {
        public static List<FormsViewModel> GetAllFormsByUserId(OneDbContext db, int UserId, int ApplicationId)
        {
            return db.Database.SqlQuery<FormsViewModel>($@"SELECT frm.FormID
                            , frm.FormURL
                            , frm.FormName
                            , frm.MenuText
                            , frm.MenuItemPriority
                            , frm.ApplicationModuleId
                            , frm.ParentForm
                            , md.ModuleName
                            , frm.IsHideChilds
                            , frm.Icon
                        FROM System.Forms frm 
                        INNER JOIN System.FormGroupRights frmGrpRt ON frm.FormID = frmGrpRt.FormId
	                        AND frmGrpRt.Allowed = 1
                        INNER JOIN Membership.Users usr ON usr.UserGroupId = frmGrpRt.UserGroupId
                        INNER JOIN System.ApplicationModules appMd ON appMd.ApplicationModuleId = frm.ApplicationModuleId
                        INNER JOIN System.Modules md ON md.ModuleId = appMd.ModuleId
                        WHERE usr.UserID = {UserId} And frm.IsMenuItem = 1 AND appMd.ApplicationId = {ApplicationId} AND appMd.IsActive = 1").ToList();
        }
        public static List<FormsViewModel> GetQuickLinks(OneDbContext db, int UserId, string FormURL, int UserGroupId, int ApplicationId)
        {
            return db.Database.SqlQuery<FormsViewModel>($@"DECLARE @ParentFormId BIGINT = (
							SELECT top 1 ParentForm
							FROM [System].[Forms] as f
						    Inner join System.ApplicationModules as am on f.ApplicationModuleId = am.ApplicationModuleId
							WHERE FormURL LIKE '{FormURL}'  and am.ApplicationId = {ApplicationId}
							)
						,@UserId INT = '{UserId}'
						,@AppModuleId INT = (
							SELECT top 1 f.ApplicationModuleId
							FROM [System].[Forms] as f
						Inner join System.ApplicationModules as am on f.ApplicationModuleId = am.ApplicationModuleId
							WHERE FormURL LIKE '{FormURL}'  and am.ApplicationId = {ApplicationId}
							)
						,@UserGroupId INT = {UserGroupId}

					SELECT f.FormName
						,f.MenuText
						,f.FormURL
						,f.ParentForm
						,f.FormId
					FROM [System].[Forms] f
					INNER JOIN [System].[Forms] pf ON (
							pf.ParentForm = f.FormID
							OR pf.ParentForm = 1
							)
						AND pf.ApplicationModuleId = f.ApplicationModuleId
					INNER JOIN System.FormGroupRights frmGrpRt ON f.FormID = frmGrpRt.FormId
						AND frmGrpRt.Allowed = 1
					INNER JOIN Membership.Users usr ON usr.UserGroupId = frmGrpRt.UserGroupId
					INNER JOIN System.ApplicationModules appMd ON appMd.ApplicationModuleId = f.ApplicationModuleId
					WHERE f.ParentForm = @ParentFormId
						AND usr.UserID = @UserId
						AND frmGrpRt.UserGroupId = @UserGroupId
						AND appMd.IsActive = 1
						AND f.ApplicationModuleId = @AppModuleId
                        AND f.IsMenuItem = 1
					GROUP BY f.FormName
						,f.MenuText
						,f.FormURL
						,f.ParentForm
						,f.FormId
						,f.MenuItemPriority
					ORDER BY f.MenuItemPriority").ToList();
        }
    }
}