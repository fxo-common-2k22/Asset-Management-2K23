using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FAPP.Helpers
{
    public class AuditHelpers
    {
        public interface IDescribableEntity
        {
            // Override this method to provide a description of the entity for audit purposes
            string Describe();
        }
        public object[] PrimaryKey(DbEntityEntry dbEntry)
        {
            return (from property in this.GetType().GetProperties()
                    where Attribute.IsDefined(property, typeof(KeyAttribute))
                    orderby ((ColumnAttribute)property.GetCustomAttributes(false).Single(
                        attr => attr is ColumnAttribute)).Order ascending
                    select property.GetValue(this)).ToArray();
        }

        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        public List<Audit> GetAuditRecordsForChange(Model.OneDbContext db, DbEntityEntry dbEntry, int userId)
        {
            List<Audit> result = new List<Audit>();
            List<DeletionLog> deleteResult = new List<DeletionLog>();
            DateTime changeTime = DateTime.Now;
            string tableName = dbEntry.Entity.GetType().Name;
            tableName = tableName.Split('_')[0].Trim();
            //tableName = db.GetTableName<FAPP.Areas.FrontDesk.Models.ReservationDetail>();
            string pvalue = "";

            if (tableName != "Audit")
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                List<string> keyName = new List<string>();
                try
                {
                    keyName = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Select(u => u.Name).ToList();
                    pvalue = dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()) == null ? null : dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()).ToString();
                }
                catch
                { }

                if (dbEntry.State == EntityState.Added)
                {
                    // For Inserts, just add the whole record
                    // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                    using (var userDb = new OneDbContext())
                    {
                        result.Add(new Audit()
                        {
                            //AuditId = Convert.ToInt64(userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) + 1 : 1),
                            AuditDate = DateTime.Now,
                            Operation = "I", // Added
                            TableName = tableName,
                            PrimaryKey = string.Join(",", keyName),
                            ModifiedBy = userId,
                            IP = SessionHelper.IP,
                            Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                            Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                            Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                            //BranchId = (short)SessionHelper.BranchId
                            Url = SessionHelper.CurrentUrl,
                            PrimaryKeyValue = pvalue
                        });
                    }
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                    using (var userDb = new OneDbContext())
                    {
                        result.Add(new Audit()
                        {
                            //AuditId = userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) : 1,
                            AuditDate = DateTime.Now,
                            Operation = "D", // Deleted
                            TableName = tableName,
                            PrimaryKey = string.Join(",", keyName),
                            Col = "",
                            ModifiedBy = userId,
                            IP = SessionHelper.IP,
                            Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                            Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                            Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                            //BranchId = (short)SessionHelper.BranchId
                            Url = SessionHelper.CurrentUrl,
                            PrimaryKeyValue = pvalue
                        });


                    }
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        // For updates, we only want to capture the columns that actually changed
                        if (!object.Equals(dbEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                        {
                            using (var userDb = new OneDbContext())
                            {
                                result.Add(new Audit()
                                {
                                    //AuditId = userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) : 1,
                                    AuditDate = DateTime.Now,
                                    Operation = "U", // UPdated
                                    TableName = tableName,
                                    PrimaryKey = string.Join(",", keyName),
                                    Col = propertyName,
                                    OldVal = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                                    NewVal = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                                    ModifiedBy = userId,
                                    IP = SessionHelper.IP,
                                    Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                                    Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                                    Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                                    //BranchId = (short)SessionHelper.BranchId
                                    Url = SessionHelper.CurrentUrl,
                                    PrimaryKeyValue = pvalue
                                });
                            }
                        }
                    }
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            return result;
        }

        public List<DeletionLog> GetDeleteRecordsForChange(Model.OneDbContext db, DbEntityEntry dbEntry, int userId)
        {
            //List<Audit> result = new List<Audit>();
            List<DeletionLog> deleteResult = new List<DeletionLog>();
            DateTime changeTime = DateTime.Now;
            string tableName = dbEntry.Entity.GetType().Name;

            tableName = tableName.Split('_')[0].Trim();
            //tableName = db.GetTableName<FAPP.Areas.FrontDesk.Models.ReservationDetail>();
            string pvalue = "";

            if (tableName != "Audit")
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                List<string> keyName = new List<string>();
                try
                {
                    keyName = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Select(u => u.Name).ToList();
                    pvalue = dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()) == null ? null : dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()).ToString();
                }
                catch
                { }

                if (dbEntry.State == EntityState.Added)
                {
                    // For Inserts, just add the whole record
                    // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                    //using (var userDb = new OneDbContext())
                    //{
                    //    result.Add(new Audit()
                    //    {
                    //        //AuditId = Convert.ToInt64(userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) + 1 : 1),
                    //        AuditDate = DateTime.Now,
                    //        Operation = "I", // Added
                    //        TableName = tableName,
                    //        PrimaryKey = string.Join(",", keyName),
                    //        ModifiedBy = userId,
                    //        IP = SessionHelper.IP,
                    //        Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                    //        Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                    //        Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                    //        //BranchId = (short)SessionHelper.BranchId
                    //        Url = SessionHelper.CurrentUrl,
                    //        PrimaryKeyValue = pvalue
                    //    });
                    //}
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                    using (var userDb = new OneDbContext())
                    {
                        //result.Add(new Audit()
                        //{
                        //    //AuditId = userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) : 1,
                        //    AuditDate = DateTime.Now,
                        //    Operation = "D", // Deleted
                        //    TableName = tableName,
                        //    PrimaryKey = string.Join(",", keyName),
                        //    Col = "",
                        //    ModifiedBy = userId,
                        //    IP = SessionHelper.IP,
                        //    Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                        //    Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                        //    Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                        //    //BranchId = (short)SessionHelper.BranchId
                        //    Url = SessionHelper.CurrentUrl,
                        //    PrimaryKeyValue = pvalue
                        //});

                        var DeleteLog = new DeletionLog()
                        {
                            DeleteDate = DateTime.Now,
                            UserId = SessionHelper.UserId, // Deleted
                            //Type = tableName + string.Join(",", keyName),
                            Type = DeletionLogTypeDescription(dbEntry),
                            BranchId = (short)SessionHelper.BranchId,
                            Path = SessionHelper.CurrentUrl,
                            Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                            Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                            Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                        };
                        DeleteLog.Description = "Delete By: " + SessionHelper.UserName + " At :" + DeleteLog.DeleteDate + " From :" + DeleteLog.Type;

                        deleteResult.Add(DeleteLog);
                    }
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    //foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    //{
                    //    // For updates, we only want to capture the columns that actually changed
                    //    if (!object.Equals(dbEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    //    {
                    //        using (var userDb = new OneDbContext())
                    //        {
                    //            result.Add(new Audit()
                    //            {
                    //                //AuditId = userDb.Audits.Any() ? userDb.Audits.Max(s => s.AuditId) : 1,
                    //                AuditDate = DateTime.Now,
                    //                Operation = "U", // UPdated
                    //                TableName = tableName,
                    //                PrimaryKey = string.Join(",", keyName),
                    //                Col = propertyName,
                    //                OldVal = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                    //                NewVal = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                    //                ModifiedBy = userId,
                    //                IP = SessionHelper.IP,
                    //                Area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "",
                    //                Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString(),
                    //                Action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString(),
                    //                //BranchId = (short)SessionHelper.BranchId
                    //                Url = SessionHelper.CurrentUrl,
                    //                PrimaryKeyValue = pvalue
                    //            });
                    //        }
                    //    }
                    //}
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            return deleteResult;
        }

        public string DeletionLogTypeDescription(DbEntityEntry dbEntry)
        {

            string tableName = dbEntry.Entity.GetType().Name;

            tableName = tableName.Split('_')[0].Trim();
            string pvalue = "";
            string DescriptionString = "";
            string valueString = "";

            
            if (tableName != "Audit")
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                List<string> keyName = new List<string>();
                try
                {
                   
                    FieldInfo[] myField = dbEntry.Entity.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    PropertyInfo[] propInfos = dbEntry.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    if (tableName.ToLower().EndsWith("Invoi".ToLower()) || tableName.ToLower().EndsWith("Return".ToLower()) || tableName.ToLower().EndsWith("Pa".ToLower()))
                    {
                        for (int i = 0; i < propInfos.Length; i++)
                        {
                            if (propInfos[i] != null && (propInfos[i].Name.ToLower().EndsWith("ReturnId".ToLower() ) 
                                 || propInfos[i].Name.ToLower().EndsWith("InvoiceId".ToLower())
                                ))
                            {
                                valueString += propInfos[i].Name.Replace("Id".ToLower(), "") + ":" + propInfos[i].GetValue(dbEntry.Entity) + ",";
                            }
                        }
                    }
                  
                    else
                    {
                        for (int i = 0; i < propInfos.Length; i++)
                        {
                            if (propInfos[i] != null && (
                                propInfos[i].Name.ToLower().EndsWith("title".ToLower()) ||
                                propInfos[i].Name.ToLower().EndsWith("Name".ToLower())) && !propInfos[i].Name.ToLower().Contains("Voucher".ToLower()))
                            {
                                valueString += propInfos[i].Name.Replace("Id".ToLower(), "") + ":" + propInfos[i].GetValue(dbEntry.Entity) + ",";
                            }
                        } 
                    }
                    if (tableName.ToLower().EndsWith("Invoi".ToLower()))
                    {
                        tableName = tableName.Replace("Invoi".ToLower(), "Invoice");
                    }
                    if (tableName.ToLower().EndsWith("Pa".ToLower()))
                    {
                        tableName = tableName.Replace("Pa", "Payment");

                    }
                    
                    keyName = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Select(u => u.Name).ToList();
                    pvalue = dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()) == null ? null : dbEntry.CurrentValues.GetValue<object>(keyName.Where(u => u != "BranchId").FirstOrDefault()).ToString();
                    
                }
                catch(Exception)
                {

                }
                DescriptionString = tableName +"  "+valueString;
            }


            return DescriptionString;

        }

    }

}





