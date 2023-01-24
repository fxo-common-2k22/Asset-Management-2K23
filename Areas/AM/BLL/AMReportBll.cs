using FAPP.AM.Models;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FAPP.Areas.AM.BLL
{
    public static class AMReportBll
    {
        static short branch_ID = SessionHelper.BranchId;

        public static List<IssuanceVM> GetItemRegisterData(OneDbContext db, ReportModel ex, short branch_ID)
        {

            var newQuery = $@"SELECT 
                        --isnull(trans.TransferHistoryId, 0) TransferHistoryId,
                        itemReg.ItemRegisterId
                       	,itemReg.ItemCode
                        ,itemReg.ProductId
						,cat.CategoryId
						,cat.CategoryName
                       	,pro.ProductName
                       	,itemReg.Qty Quantity
                       	,itemReg.[Value]
                       	,itemReg.DateOfEntry AS IssueDate
                        ,itemReg.[Status]
                       	,ISNULL(itemReg.CurrentdepartmentId, 0) DepartmentId
						,isnull(dep.DepartmentName,'') DepartmentName
                       	,ISNULL(itemReg.EmployeeId, 0) EmployeeId
						,isnull(emp.EmpName,'') EmployeeName
                       	,ISNULL(itemReg.CurrentLocationId, 0) LocationId
						,isnull(loc.RoomDoorNo,'') LocationName
                       	,isnull(trans.[Description], '') [Description]
                        ,'' [Description]
                       	,isnull(trans.ConditionTypeId, itemReg.ConditionTypeId) ConditionTypeId
                       FROM am.ItemRegister itemReg
                       INNER JOIN inv.Products pro on pro.ProductId=itemReg.ProductId
					   LEFT JOIN inv.Categories cat on cat.CategoryId=pro.CategoryId
                       LEFT JOIN am.TransferHistory trans ON trans.ItemRegisterId = itemReg.ItemRegisterId and trans.BranchId=itemReg.BranchId
					   LEFT JOIN Hr.Departments dep on itemReg.CurrentdepartmentId=dep.DepartmentId
					   LEFT JOIN Hr.Employees emp on itemReg.EmployeeId=emp.EmployeeId
					   LEFT JOIN Company.Rooms loc on itemReg.CurrentLocationId=loc.RoomId


                       WHERE 
                       	 itemReg.[Status] = @StatusId
                       	AND itemReg.BranchId=@BranchId

                        GROUP BY itemReg.ItemRegisterId
                    	,itemReg.ItemCode
                        ,itemReg.ProductId
						,cat.CategoryId
						,cat.CategoryName
                    	,pro.ProductName
                    	,itemReg.Qty
                    	,itemReg.[Value]
                    	,itemReg.DateOfEntry
                    	,itemReg.[Status]
                    	,itemReg.CurrentdepartmentId
						,dep.DepartmentName
						,emp.EmpName
						,loc.RoomDoorNo
                    	,itemReg.EmployeeId
                    	,itemReg.CurrentLocationId
                    	,trans.Description
                    	,trans.ConditionTypeId
                    	,itemReg.ConditionTypeId";

            var oldQuery = $@"SELECT itemReg.ItemRegisterId
                                                    	,itemReg.ItemCode
                                                    	,pro.ProductName
                                                    	,itemReg.Qty Quantity
                                                    	,itemReg.[Value]
                                                    	,itemReg.DateOfEntry AS IssueDate
                                                    	,itemReg.[Status]
                                                    	,ISNULL(trans.DepartmentId, 0) DepartmentId
                                                    	,ISNULL(dep.DepartmentName, 0) DepartmentName
                                                    	,ISNULL(trans.EmployeeId, 0) EmployeeId
                                                    	,isnull(emp.EmpName, '') EmployeeName
                                                    	,ISNULL(trans.LocationId, 0) LocationId
                                                    	,ISNULL(loc.RoomDoorNo, 0) LocationName
                                                    	,isnull(trans.[Description], '') [Description]
                                                    	,isnull(trans.ConditionTypeId, itemReg.ConditionTypeId) ConditionTypeId
                                                    FROM am.ItemRegister itemReg
                                                    INNER JOIN inv.Products pro ON pro.ProductId = itemReg.ProductId
                                                    LEFT JOIN am.TransferHistory trans ON trans.ItemRegisterId = itemReg.ItemRegisterId
                                                    	AND trans.BranchId = itemReg.BranchId
                                                    LEFT JOIN hr.Employees emp ON emp.EmployeeId = trans.EmployeeId
                                                    LEFT JOIN hr.Departments dep ON dep.DepartmentId = trans.DepartmentId
                                                    LEFT JOIN Company.Rooms loc ON loc.RoomId = trans.LocationId
                                                    WHERE itemReg.[Status] = @StatusId
                                                    	AND itemReg.BranchId = @BranchId
                                                    GROUP BY itemReg.ItemRegisterId
                                                    	,itemReg.ItemCode
                                                    	,pro.ProductName
                                                    	,itemReg.Qty
                                                    	,itemReg.[Value]
                                                    	,itemReg.DateOfEntry
                                                    	,itemReg.[Status]
                                                    	,trans.DepartmentId
                                                    	,trans.EmployeeId
                                                    	,trans.LocationId
                                                    	,trans.[Description]
                                                    	,trans.ConditionTypeId
                                                    	,itemReg.ConditionTypeId
                                                    	,dep.DepartmentName
                                                    	,emp.EmpName
                                                    	,loc.RoomDoorNo";
            var query = db.Database.SqlQuery<IssuanceVM>(newQuery,
                                                        new SqlParameter("@BranchId", AMProceduresModel.GetDBNullOrValue(branch_ID))
                                                        , new SqlParameter("@StatusId", AMProceduresModel.GetDBNullOrValue(ex.StatusId)));

            ex.IssuanceList = query.ToList();
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.IssuanceList = ex.IssuanceList.Where(s => s.ProductName.ToLower().Contains(ex.Search.ToLower()) ||
                s.ItemCode.ToLower().Contains(ex.Search.ToLower()) ||
                s.CategoryName.ToLower().Contains(ex.Search.ToLower())).ToList();
            }

            if (ex.FromDateTime != DateTime.MinValue && ex.ToDateTime != DateTime.MinValue)
            {

                ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate >= ex.FromDateTime && u.IssueDate <= ex.ToDateTime).ToList();
            }
            else
            {
                if (ex.FromDateTime != DateTime.MinValue)
                {
                    var date = Convert.ToDateTime(ex.FromDateTime);
                    ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate >= ex.FromDateTime).ToList();
                }
                //if (!string.IsNullOrEmpty(ex.ToDate))
                if (ex.ToDateTime != DateTime.MinValue)
                {
                    //var date = Convert.ToDateTime(ex.ToDate);
                    ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate <= ex.ToDateTime).ToList();
                }
            }

            if (ex.StatusId != null)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.Status == ex.StatusId).ToList();
            }
            if (ex.DepartmentId.HasValue)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.DepartmentId == ex.DepartmentId).ToList();
            }
            if (ex.EmployeeId.HasValue)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.EmployeeId == ex.EmployeeId).ToList();
            }
            if (ex.LocationId.HasValue)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.LocationId == ex.LocationId).ToList();
            }
            if (ex.CategoryId.HasValue)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.CategoryId == ex.CategoryId).ToList();
            }
            if (ex.ProductId.HasValue)
            {
                ex.IssuanceList = ex.IssuanceList.Where(u => u.ProductId == ex.ProductId).ToList();
            }


            ex.IssuanceList = ex.IssuanceList.OrderBy(x => x.IssueDate).ToList();
            return ex.IssuanceList;
        }

        public static List<SummarizeData> GetSummarizeData(OneDbContext db, ReportModel ex, short branch_ID)
        {

            var newQuery = $@"SELECT ProductId
                            	,ProductName
                            	,CategoryId
                            	,CategoryName
                            	,sum(Available) Available
                            	,sum(Issued) Issued
                            	,Sum(Damaged) Damaged
                            	,sum(Available - (Issued + Damaged)) AS InStock
                            FROM (
                            	SELECT pr.ProductId
                            		,pr.ProductName
                            		,itemReg.ItemCode
                            		,pr.CategoryId
                            		,Inv.Categories.CategoryName
                            		,count(*) AS Available
                            		,CASE 
                            			WHEN itemReg.[Status] = 2
                            				THEN count(*)
                            			ELSE 0
                            			END AS Issued
                            		,CASE 
                            			WHEN itemReg.[Status] = 3
                            				THEN count(*)
                            			ELSE 0
                            			END AS Damaged
                            	FROM Am.ItemRegister itemReg
                            	JOIN Inv.products AS pr ON itemReg.ProductId = pr.ProductId
                            	JOIN Inv.Categories ON pr.CategoryId = Inv.Categories.CategoryId
                            	WHERE pr.IsFixedAsset = 1
                            		AND itemReg.BranchId = @branchId
                            		AND itemReg.DateOfEntry BETWEEN cast(@datefrom as date)
                            			AND cast(@dateto as date)
                                    AND (itemReg.CurrentdepartmentId=@departmentId OR @departmentId is null)
			                        AND ( itemReg.EmployeeId=@employeeId OR @employeeId is null)
			                        AND (itemReg.CurrentLocationId=@locationId OR @locationId is null)
                            	GROUP BY pr.ProductId
                            		,itemReg.ItemCode
                            		,pr.ProductName
                            		,pr.CategoryId
                            		,Inv.Categories.CategoryName
                            		,itemReg.[Status]
                            	) t
                            GROUP BY ProductId
                            	,ProductName
                            	,CategoryId
                            	,CategoryName";
            var query = db.Database.SqlQuery<SummarizeData>(newQuery,
                                                        new SqlParameter("@BranchId", AMProceduresModel.GetDBNullOrValue(branch_ID))
                                                        , new SqlParameter("@datefrom", AMProceduresModel.GetDBNullOrValue(ex.FromDateTime.ToddMMMyyyy()))
                                                        , new SqlParameter("@dateto", AMProceduresModel.GetDBNullOrValue(ex.ToDateTime.ToddMMMyyyy()))
                                                        , new SqlParameter("@departmentId", AMProceduresModel.GetDBNullOrValue(ex.DepartmentId))
                                                        , new SqlParameter("@employeeId", AMProceduresModel.GetDBNullOrValue(ex.EmployeeId))
                                                        , new SqlParameter("@locationId", AMProceduresModel.GetDBNullOrValue(ex.LocationId))
                                                        );

            ex.SummarizeReport = query.ToList();
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.SummarizeReport = ex.SummarizeReport.Where(s => s.ProductName.ToLower().Contains(ex.Search.ToLower()) ||
                s.CategoryName.ToLower().Contains(ex.Search.ToLower())).ToList();
            }

            //if (ex.FromDateTime != DateTime.MinValue && ex.ToDateTime != DateTime.MinValue)
            //{

            //    ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate >= ex.FromDateTime && u.IssueDate <= ex.ToDateTime).ToList();
            //}
            //else
            //{
            //    if (ex.FromDateTime != DateTime.MinValue)
            //    {
            //        var date = Convert.ToDateTime(ex.FromDateTime);
            //        ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate >= ex.FromDateTime).ToList();
            //    }
            //    //if (!string.IsNullOrEmpty(ex.ToDate))
            //    if (ex.ToDateTime != DateTime.MinValue)
            //    {
            //        //var date = Convert.ToDateTime(ex.ToDate);
            //        ex.IssuanceList = ex.IssuanceList.Where(u => u.IssueDate <= ex.ToDateTime).ToList();
            //    }
            //}

            //if (ex.StatusId != null)
            //{
            //    ex.IssuanceList = ex.IssuanceList.Where(u => u.Status == ex.StatusId).ToList();
            //}
            //if (ex.DepartmentId.HasValue)
            //{
            //    ex.IssuanceList = ex.IssuanceList.Where(u => u.DepartmentId == ex.DepartmentId).ToList();
            //}
            //if (ex.EmployeeId.HasValue)
            //{
            //    ex.IssuanceList = ex.IssuanceList.Where(u => u.EmployeeId == ex.EmployeeId).ToList();
            //}
            //if (ex.LocationId.HasValue)
            //{
            //    ex.IssuanceList = ex.IssuanceList.Where(u => u.LocationId == ex.LocationId).ToList();
            //}
            if (ex.CategoryId.HasValue)
            {
                ex.SummarizeReport = ex.SummarizeReport.Where(u => u.CategoryId == ex.CategoryId).ToList();
            }
            if (ex.ProductId.HasValue)
            {
                ex.SummarizeReport = ex.SummarizeReport.Where(u => u.ProductId == ex.ProductId).ToList();
            }


            ex.SummarizeReport = ex.SummarizeReport.ToList();
            return ex.SummarizeReport;
        }

        public static List<TransferHistory> GetAssetLog(OneDbContext db, ReportModel ex, out string Error)
        {
            try
            {
                var list = db.TransferHistory.Include(x => x.ItemRegister).Where(x => x.BranchId == branch_ID);

                if (!string.IsNullOrEmpty(ex.Search))
                {
                    list = list.Where(s => s.ItemRegister.Product.ProductName.ToLower().Contains(ex.Search.ToLower()) ||
                    s.ItemRegister.ItemCode.ToLower().Contains(ex.Search.ToLower()) ||
                    s.ItemRegister.Product.Category.CategoryName.ToLower().Contains(ex.Search.ToLower()) ||
                    s.ItemRegister.Department.DepartmentName.ToLower().Contains(ex.Search.ToLower()) ||
                    s.ItemRegister.Employee.EmpName.ToLower().Contains(ex.Search.ToLower()));
                }
                if (ex.FromDateTime != DateTime.MinValue && ex.ToDateTime != DateTime.MinValue)
                {

                    ex.TransferHistory = list.Where(u => u.Date >= ex.FromDateTime && u.Date <= ex.ToDateTime).ToList();
                }
                else
                {
                    if (ex.FromDateTime != DateTime.MinValue)
                    {
                        var date = Convert.ToDateTime(ex.FromDateTime);
                        ex.TransferHistory = ex.TransferHistory.Where(u => u.Date >= ex.FromDateTime).ToList();
                    }
                    //if (!string.IsNullOrEmpty(ex.ToDate))
                    if (ex.ToDateTime != DateTime.MinValue)
                    {
                        //var date = Convert.ToDateTime(ex.ToDate);
                        ex.TransferHistory = ex.TransferHistory.Where(u => u.Date <= ex.ToDateTime).ToList();
                    }
                }

                //if (ex.StatusId != null)
                //{
                //    ex.TransferHistory = ex.TransferHistory.Where(u => u.sta == ex.StatusId).ToList();
                //}
                if (ex.DepartmentId.HasValue)
                {
                    ex.TransferHistory = ex.TransferHistory.Where(u => u.DepartmentId == ex.DepartmentId).ToList();
                }
                if (ex.EmployeeId.HasValue)
                {
                    ex.TransferHistory = ex.TransferHistory.Where(u => u.EmployeeId == ex.EmployeeId).ToList();
                }
                if (ex.LocationId.HasValue)
                {
                    ex.TransferHistory = ex.TransferHistory.Where(u => u.LocationId == ex.LocationId).ToList();
                }
                if (ex.ProductId.HasValue)
                {
                    ex.TransferHistory = ex.TransferHistory.Where(u => u.ItemRegister.ProductId == ex.ProductId).ToList();
                }
                if (ex.CategoryId.HasValue)
                {
                    ex.TransferHistory = ex.TransferHistory.Where(u => u.ItemRegister.Product.CategoryId == ex.CategoryId).ToList();
                }
                ex.TransferHistory = ex.TransferHistory.OrderBy(x => x.Date).ToList();
                Error = "";
                return ex.TransferHistory;
            }
            catch (Exception exc)
            {

                Error = exc.GetExceptionMessages();
                return null;
            }
        }
    }
}