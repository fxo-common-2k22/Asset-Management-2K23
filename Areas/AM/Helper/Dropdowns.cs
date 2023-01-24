using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace FAPP.Areas.AM.Helper
{
    public class SelectListItem2
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Tag { get; set; }
    }
    public class Dropdowns
    {
        private Guid _SessionId = SessionHelper.CurrentSessionId;
        private short _BranchId = SessionHelper.BranchId;
        public IEnumerable<SelectListItem> GetPassFailDD()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Value = true.ToString(), Text = "Pass"},
                new SelectListItem() { Value = false.ToString(), Text = "Fail"},
            };
        }

        //public async Task<List<SelectListItem>> GetDeactivationTypesDD(OneDbContext db)
        //{
        //    return await db.DeactivationTypes.OrderBy(s => s.DeactivationTypeName)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.DeactivationTypeName,
        //            Value = s.DeactivationTypeId.ToString()
        //        }).ToListAsync();
        //}

        //public async Task<List<SelectListItem>> GetFeeTypeGroupsGlobalDD(OneDbContext db)
        //{
        //    return await db.FeeTypeGroups.Where(s => s.BranchId == null)
        //        .OrderBy(s => s.Name)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.Name,
        //            Value = s.FeeTypeGroupId.ToString()
        //        }).ToListAsync();
        //}

        //public async Task<List<SelectListItem>> GetFeeTypeGroupsLocalsDD(OneDbContext db)
        //{
        //    return await db.FeeTypeGroups.Where(s => s.BranchId == SessionHelper.BranchId)
        //        .OrderBy(s => s.Name)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.Name,
        //            Value = s.FeeTypeGroupId.ToString()
        //        }).ToListAsync();
        //}

        //public async Task<List<SelectListItem>> GetTermsByGroupId(Guid groupId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        //trm.Term.SessionId == _SessionId && trm.Term.BranchId == _BranchId &&
        //        return await (from trm in db.TermsStages.Include(w => w.Term)
        //                      join gr in db.AcademicGroups.Include(s => s.Class) on trm.StageId equals gr.Class.StageId
        //                      where gr.GroupId == groupId
        //                      select new SelectListItem
        //                      {
        //                          Text = trm.Term.TermName + " (" + trm.Term.Weightage + ")",
        //                          Value = trm.ExamTermId.ToString()
        //                      }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetBussesDD(OneDbContext db)
        //{
        //    return await (from v in db.Vehicles2.Include(s => s.VehicleType)
        //                  where v.BranchId == SessionHelper.BranchId
        //                  select new SelectListItem
        //                  {
        //                      Text = (v.VehicleType != null ? v.VehicleType.VehicleTypeName : "") + " - " + v.VechileRegNo,
        //                      Value = v != null ? v.VehicleId.ToString() : "",
        //                  }).ToListAsync();
        //}

        //public async Task<List<SelectListItem>> GetActiveTermsByGroupId(Guid groupId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await (from trm in db.TermsStages.Include(w => w.Term)
        //                      join gr in db.AcademicGroups.Include(s => s.Class) on trm.StageId equals gr.Class.StageId
        //                      where trm.Term.SessionId == _SessionId && trm.Term.BranchId == _BranchId && gr.GroupId == groupId && trm.Term.IsActive == true
        //                      select new SelectListItem
        //                      {
        //                          Text = trm.Term.TermName + " (" + trm.Term.Weightage + ")",
        //                          Value = trm.ExamTermId.ToString()
        //                      }).ToListAsync();
        //    }
        //}

        public async Task<List<SelectListItem>> GetApplicationPortalsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.ApplicationPortals
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.ApplicationPortalId.ToString(),
                }).ToListAsync();
            }
        }

        public async Task<List<SelectListItem>> GetFormsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Forms.Select(s => new SelectListItem
                {
                    Text = s.FormName + " [" + s.FormURL + "]",
                    Value = s.FormURL,
                }).ToListAsync();
            }
        }
        public async Task<List<SelectListItem>> GetUserGroupsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.UserGroups.Select(s => new SelectListItem
                {
                    Text = s.UserGroupName,
                    Value = s.UserGroupId.ToString()
                }).ToListAsync();
            }
        }

        public async Task<List<SelectListItem>> GetUsersDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Users
                    .Where(p => p.UserGroup.UserGroupName != "Student" || p.UserGroup.UserGroupName != "Employee")
                    .Select(s => new SelectListItem
                    {
                        Text = s.Username,
                        Value = s.UserID.ToString()
                    }).ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetExamTypesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Types1.Where(s => s.BranchId == SessionHelper.BranchId).OrderBy(s => s.DisplayPriority)
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.ExamTypeId.ToString(),
        //            Text = s.ExamType,
        //        }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetGlobalExamTypesDD(OneDbContext db)
        //{
        //    return await db.Types1.Where(s => s.BranchId == null).OrderBy(s => s.DisplayPriority)
        //    .Select(s => new SelectListItem
        //    {
        //        Value = s.ExamTypeId.ToString(),
        //        Text = s.ExamType,
        //    }).ToListAsync();
        //}

        //public async Task<List<SelectListItem>> GetSubjectsDDByStageIdLocal(Guid stageId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Subjects
        //            .Where(s => s.BranchId == _BranchId)
        //            .OrderBy(s => s.SubjectCode)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SubjectName,
        //                Value = s.ExamSubjectId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<List<SelectListItem>> GetSubjectsDDByStageIdForIB(Guid stageId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Subjects
        //            .Where(s => s.BranchId == null)
        //            .OrderBy(s => s.SubjectCode)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SubjectName,
        //                Value = s.ExamSubjectId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<List<SelectListItem>> GetSubjectsDDForIB()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Subjects
        //            .Where(s => s.BranchId == null)
        //            .OrderBy(s => s.SubjectCode)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SubjectName,
        //                Value = s.ExamSubjectId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<List<SelectListItem>> GetGlobalSubSubjects()
        {
            using (var db = new OneDbContext())
            {
                return await db.Database.SqlQuery<SelectListItem>($@"select Cast (ExamSubSubjectId AS varchar(MAX)) AS Value,Name AS Text
                                                                from ER.SubSubjects
                                                                where GlobalSubSubjectId is null and BranchId is null
                                                                and ExamSubSubjectId not in (select GlobalSubSubjectId from ER.SubSubjects where GlobalSubSubjectId is not null AND BranchId = {SessionHelper.BranchId})").ToListAsync();
            }
        }
        //public async Task<List<SelectListItem>> GetSubjectsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Subjects
        //            .Where(s => s.BranchId == _BranchId)
        //            .OrderBy(s => s.SubjectCode)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SubjectName,
        //                Value = s.ExamSubjectId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<List<SelectListItem>> GetGlobalSubjectsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Database.SqlQuery<SelectListItem>($@"select Cast (ExamSubjectId AS varchar(MAX)) AS Value,SubjectName AS Text
                                                                from ER.Subjects
                                                                where GlobalSubjectId is null and BranchId is null
                                                                and ExamSubjectId not in (select GlobalSubjectId from ER.Subjects where GlobalSubjectId is not null AND BranchId = {SessionHelper.BranchId})").ToListAsync();
            }
        }
        //public async Task<IEnumerable<SelectListItem>> GetExamTermsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Terms.Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId).OrderBy(s => s.TermName)
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.ExamTermId.ToString(),
        //            Text = s.TermName,
        //        }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetGroupsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups
        //            .Include(s => s.Class)
        //            .Include(s => s.Section)
        //            .Where(s => s.SessionId == _SessionId && s.BranchId == _BranchId && s.Active == true)
        //            .OrderBy(s => s.Class.ClassOrder)
        //            .ThenBy(s => s.Section.SectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.GroupName,
        //                Value = s.GroupId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> FeeVoucherAutoCreationSettingsDD(Guid SessionId, Guid? ClassId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var query = db.FeeVoucherAutoCreationSettings
        //            .Include(s => s.Class)
        //            .Where(s => s.SessionId == _SessionId && s.BranchId == _BranchId);
        //        if (ClassId != Guid.Empty && ClassId != null)
        //        {
        //            query = query.Where(s => s.ClassId == ClassId);
        //        }
        //        return await query
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.DurationTitle,
        //                Value = s.EntryId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetStudentSessionDD(Guid groupId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.StudentSessions
        //            .Where(s => s.GroupId == groupId && s.Active == true)
        //            .OrderBy(s => s.Student.FullName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Student.FullName,
        //                Value = s.StudentId.ToString(),
        //            }).ToListAsync();
        //    }
        //}


        public async Task<IEnumerable<SelectListItem>> GetReportingTemplatesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Templates11

                    .Select(s => new SelectListItem
                    {
                        Text = s.TemplateTitle,
                        Value = s.ReportTemplateId.ToString()
                    }).ToListAsync();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetCertificatesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Certificates.Select(s => new SelectListItem { Text = s.Title, Value = s.CertificateId.ToString() }).ToListAsync();
            }
        }

        //public IEnumerable<SelectListItem> GetFilesDDByPath(string path)
        //{
        //    DirectoryInfo dinfo = new DirectoryInfo(path);
        //    FileInfo[] Files = dinfo.GetFiles("*.cshtml");
        //    var dropdown = new List<SelectListItem>();
        //    for (int i = 0; i < Files.Count(); i++)
        //    {
        //        FileInfo file = Files[i];
        //        dropdown.Add(new SelectListItem()
        //        {
        //            Text = file.Name.Replace(".cshtml", ""),
        //            Value = $"style{i + 1}",
        //        });
        //    }
        //    return dropdown;
        //}

        public IEnumerable<SelectListItem> GetFilesDDByPathV2(string path)
        {
            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("*.cshtml");
            var dropdown = new List<SelectListItem>();
            for (int i = 0; i < Files.Count(); i++)
            {
                FileInfo file = Files[i];
                dropdown.Add(new SelectListItem()
                {
                    Text = file.Name.Replace(".cshtml", "").Replace("_", " "),
                    Value = file.Name.Replace(".cshtml", ""),
                });
            }
            return dropdown;
        }

        //public async Task<IEnumerable<SelectListItem>> GetSubSubjectsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.SubSubjects
        //            .Where(s => s.BranchId == _BranchId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Name + "-" + s.ShortForm,
        //                Value = s.ExamSubSubjectId.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetSubExamsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ExamSubTypes
        //            .Where(s => s.BranchId == _BranchId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Name + "-" + s.Name,
        //                Value = s.Id.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetSchoolHousesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.SchoolHouses.Where(s => s.BranchId == _BranchId)
        //            .OrderBy(s => s.HouseOrder)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.HouseName,
        //            Value = s.HouseId.ToString(),
        //        }).ToListAsync();
        //    }
        //}


        public async Task<IEnumerable<SelectListItem>> GetStatesDD(short? countryId)
        {
            using (var db = new OneDbContext())
            {
                return await db.States.Where(s => s.CountryId == (countryId.HasValue ? countryId.Value : s.CountryId))
                    .Select(s => new SelectListItem { Text = s.StateName, Value = s.StateId.ToString() })
                    .ToListAsync();
            }
        }


        public async Task<IEnumerable<SelectListItem>> GetCitiesDD(short? stateId)
        {
            using (var db = new OneDbContext())
            {
                return await db.Cities.Where(s => s.StateId == (stateId.HasValue ? stateId.Value : s.StateId))
                    .Select(s => new SelectListItem { Text = s.CityName, Value = s.CityId.ToString() })
                    .ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetFeeHeadsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.FeeTypes
        //            .Where(s => s.BranchId == _BranchId)
        //            .OrderBy(s => s.Priority).Select(s => new SelectListItem
        //            {
        //                Text = s.FeeTypeName,
        //                Value = s.FeeTypeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}


        //public async Task<IEnumerable<SelectListItem>> GetFineHeadsDD(OneDbContext db)
        //{
        //    return await db.FeeTypes
        //        .Where(s => s.BranchId == _BranchId && s.IsFineHead == true)
        //        .OrderBy(s => s.Priority).Select(s => new SelectListItem
        //        {
        //            Text = s.FeeTypeName,
        //            Value = s.FeeTypeId.ToString(),
        //        }).ToListAsync();
        //}

        //public async Task<IEnumerable<SelectListItem>> GetGlobalFeeHeadsForDiscountDD(OneDbContext db, Guid? id = null)//id = DiscountId
        //{
        //    //var discountIds = db.Discounts.Where(s => s.BranchId == null && s.DiscountHeads.Count() > 0);

        //    //if (id.HasValue && id.Value != Guid.Empty)
        //    //{
        //    //    discountIds = discountIds.Where(s => s.AccountsFeeDiscountId != id.Value);
        //    //}

        //    //var feeTypeIds = db.DiscountHeads
        //    //                    .Where(s => discountIds.Select(x => x.AccountsFeeDiscountId).Contains(s.FeeDiscountId))
        //    //                    .Select(x => x.FeeTypeId);

        //    //var query = db.FeeTypes
        //    //    .Where(s => s.Discountable == true && s.BranchId == null); //!feeTypeIds.Contains(s.FeeTypeId) && 

        //    //return await query
        //    //    .OrderBy(s => s.Priority)
        //    //    .Select(s => new SelectListItem
        //    //    {
        //    //        Text = s.FeeTypeName,
        //    //        Value = s.FeeTypeId.ToString(),
        //    //    }).ToListAsync();
        //}

        //public async Task<IEnumerable<SelectListItem>> GetLocalFeeHeadsForDiscountDD(OneDbContext db, Guid? id)//id = DiscountId
        //{
        //    //var discountIds = db.Discounts.Where(s => s.BranchId == SessionHelper.BranchId && s.DiscountHeads.Count() > 0);

        //    //if (id.HasValue && id.Value != Guid.Empty)
        //    //{
        //    //    discountIds = discountIds.Where(s => s.AccountsFeeDiscountId != id.Value);
        //    //}

        //    //var feeTypeIds = db.DiscountHeads
        //    //                    .Where(s => discountIds.Select(x => x.AccountsFeeDiscountId).Contains(s.FeeDiscountId))
        //    //                    .Select(x => x.FeeTypeId);

        //    var query = db.FeeTypes
        //        .Where(s => s.Discountable == true && s.BranchId == SessionHelper.BranchId); //!feeTypeIds.Contains(s.FeeTypeId) && 

        //    return await query
        //        .OrderBy(s => s.Priority)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.FeeTypeName,
        //            Value = s.FeeTypeId.ToString(),//dont use global here
        //        }).ToListAsync();
        //}

        //public async Task<IEnumerable<SelectListItem>> GetFineableGlobalFeeHeadsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.FeeTypes.Where(s => s.BranchId == null && s.Fineable == true).OrderBy(s => s.FeeTypeName).Select(s => new SelectListItem
        //        {
        //            Text = s.FeeTypeName,
        //            Value = s.FeeTypeId.ToString(),
        //        }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetPaymentModesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.PaymentModes.OrderBy(s => s.PaymentModeName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.PaymentModeName,
                        Value = s.PaymentModeId.ToString(),
                    }).ToListAsync();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetBankAndCashAccountsDD()
        {
            using (var db = new OneDbContext())
            {
                var query = (from a in db.Accounts
                             where a.BranchId == _BranchId && db.BankAccounts.Select(s => s.AccountId).Contains(a.autokey)
                             select new SelectListItem
                             {
                                 Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                 Value = a.autokey.ToString(),
                             }).Union(
                                from a in db.Accounts
                                where a.BranchId == _BranchId && db.CashAccounts.Select(s => s.CashAccountId).Contains(a.autokey)
                                select new SelectListItem
                                {
                                    Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                    Value = a.autokey.ToString(),
                                });

                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetBankAccountsDD()
        {
            using (var db = new OneDbContext())
            {
                return await (from a in db.Accounts
                              where a.BranchId == _BranchId && db.BankAccounts.Select(s => s.AccountId).Contains(a.autokey)
                              select new SelectListItem
                              {
                                  Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                  Value = a.autokey.ToString(),
                              }).ToListAsync();
            }
        }


        public async Task<IEnumerable<SelectListItem>> GetCashAccountsDD()
        {
            using (var db = new OneDbContext())
            {
                return await (from a in db.Accounts
                              where a.BranchId == _BranchId && db.CashAccounts.Select(s => s.CashAccountId).Contains(a.autokey)
                              select new SelectListItem
                              {
                                  Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                  Value = a.autokey.ToString(),
                              }).ToListAsync();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetAccountsDD()
        {
            using (var db = new OneDbContext())
            {
                return await (from a in db.Accounts
                              where a.BranchId == _BranchId
                              select new SelectListItem
                              {
                                  Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                  Value = a.autokey.ToString(),
                              }).ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetBankAndCashAdvanPaymentAccountsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var query = (from a in db.Accounts
        //                     where a.BranchId == _BranchId && db.BankAccounts.Select(s => s.AccountId).Contains(a.autokey)
        //                     select new SelectListItem
        //                     {
        //                         Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
        //                         Value = a.autokey.ToString(),
        //                     })
        //                     .Union(
        //                        from a in db.Accounts
        //                        where a.BranchId == _BranchId && db.CashAccounts.Select(s => s.CashAccountId).Contains(a.autokey)
        //                        select new SelectListItem
        //                        {
        //                            Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
        //                            Value = a.autokey.ToString(),
        //                        })
        //                        .Union((from b in db.FeeAccountSettings.Include(s => s.AdvancePaymentAccount)
        //                                where b.BranchId == _BranchId
        //                                select new SelectListItem
        //                                {
        //                                    Text = "[" + (b.AdvancePaymentAccount.CONTROLACCOUNT == true ? "C" : "T") + "] [" + b.AdvancePaymentAccount.ACCOUNT_ID + "] " + b.AdvancePaymentAccount.TITLE,
        //                                    Value = b.AdvancePaymentAccount.autokey.ToString(),
        //                                }));

        //        return await query.ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetAccountsDDByPaymentModeId(int paymentModeId)
        {
            using (var db = new OneDbContext())
            {
                var paymentMode = await db.PaymentModes.FindAsync(paymentModeId);
                if (paymentMode.PaymentModeName.Contains("Cash"))
                {
                    return await (from a in db.Accounts
                                  where a.BranchId == _BranchId && db.CashAccounts.Select(s => s.CashAccountId).Contains(a.autokey)
                                  select new SelectListItem
                                  {
                                      Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                      Value = a.autokey.ToString(),
                                  }).ToListAsync();
                }
                if (paymentMode.PaymentModeName.Contains("Bank"))
                {
                    return await (from a in db.Accounts
                                  where a.BranchId == _BranchId && db.BankAccounts.Select(s => s.AccountId).Contains(a.autokey)
                                  select new SelectListItem
                                  {
                                      Text = "[" + (a.CONTROLACCOUNT == true ? "C" : "T") + "] [" + a.ACCOUNT_ID + "] " + a.TITLE,
                                      Value = a.autokey.ToString(),
                                  }).ToListAsync();
                }

                //if (paymentMode.PaymentModeName.Contains("Credit"))
                //{
                //    return await (from b in db.FeeAccountSettings.Include(s => s.AdvancePaymentAccount)
                //                  where b.BranchId == _BranchId
                //                  select new SelectListItem
                //                  {
                //                      Text = "[" + (b.AdvancePaymentAccount.CONTROLACCOUNT == true ? "C" : "T") + "] [" + b.AdvancePaymentAccount.ACCOUNT_ID + "] " + b.AdvancePaymentAccount.TITLE,
                //                      Value = b.AdvancePaymentAccount.autokey.ToString(),
                //                  }).ToListAsync(); ;
                //}
                return new List<SelectListItem>();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetCurrentSessionMonthsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var session = await db.Sessions.Where(s => s.SessionId == _SessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefaultAsync();
        //        var list = new List<SelectListItem>();
        //        foreach (var item in MonthNames(session.StartTime, session.EndTime))
        //        {
        //            list.Add(new SelectListItem
        //            {
        //                Value = item.ToString(),
        //                Text = string.Format("{0: MMMM/yyyy}", item)
        //            });
        //        }
        //        return list;
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetCurrentSessionMonthsForVoucherAutoCreationDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var session = await db.Sessions.Where(s => s.SessionId == _SessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefaultAsync();
        //        var list = new List<SelectListItem>();
        //        foreach (var item in MonthNames(session.StartTime, session.EndTime))
        //        {
        //            list.Add(new SelectListItem
        //            {
        //                Value = string.Format("{0: MMM/yyyy}", item),
        //                Text = string.Format("{0: MMM/yyyy}", item)
        //            });
        //        }
        //        return list;
        //    }
        //}

        public List<DateTime> MonthNames(DateTime date1, DateTime date2)
        {
            date1 = new DateTime(date1.Year, date1.Month, 1);
            var monthList = new List<DateTime>();

            while (date1 < date2)
            {
                monthList.Add(date1);
                date1 = date1.AddMonths(1);
            }
            return monthList;
        }

        public List<SelectListItem> GetCancelledYesNoDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = true.ToString(), Text = "Yes"},
                new SelectListItem(){Value = false.ToString(), Text = "No"},
            };
        }
        public List<SelectListItem> GetYesNoDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = true.ToString(), Text = "Yes"},
                new SelectListItem(){Value = false.ToString(), Text = "No"},
            };
        }
        public List<SelectListItem> GetCancelledDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = true.ToString(), Text = "Yes"},
                new SelectListItem(){Value = false.ToString(), Text = "No"},
            };
        }
        public List<SelectListItem> GetValidInvalidDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = true.ToString(), Text = "Yes"},
                new SelectListItem(){Value = false.ToString(), Text = "No"},
            };
        }
        public List<SelectListItem> GetDueOverdueDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = true.ToString(), Text = "Yes"},
                new SelectListItem(){Value = false.ToString(), Text = "No"},
            };
        }

        public List<SelectListItem> GetGenderDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Male", Text = "Male"},
                new SelectListItem(){Value = "Female", Text = "Female"},
            };

        }


        public List<SelectListItem> PaidUnpaidDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Paid", Text = "Paid"},
                new SelectListItem(){Value = "Unpaid", Text = "Unpaid"},
                new SelectListItem(){Value = "PartiallyPaid", Text = "Partially Paid"},
            };
        }

        public List<SelectListItem> GetPostedDraftDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Posted", Text = "Posted"},
                new SelectListItem(){Value = "Draft", Text = "Draft"},
            };
        }

        public List<SelectListItem> PaidUnpaidOnlyDD()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Paid", Text = "Paid"},
                new SelectListItem(){Value = "Unpaid", Text = "Unpaid"},
            };
        }

        //public async Task<IEnumerable<SelectListItem>> GetExamSubTypesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ExamSubTypes
        //            .Where(s => s.BranchId == SessionHelper.BranchId)
        //            .OrderBy(s => s.DisplayPriority)
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.Id.ToString(),
        //            Text = s.Name,
        //        }).ToListAsync();
        //    }
        //}


        //public async Task<List<SelectListItem>> GetClassBooksDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.ClassBooksAndStationeries
        //            .Include(s => s.BooksAndStationery)
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.BooksAndStationery.Type.Name == "Book")
        //            .OrderBy(s => s.BooksAndStationery.ItemName)
        //            .ThenBy(s => s.Class.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.BooksAndStationery.ItemName + " for " + s.Class.ClassName,
        //                Value = s.Id.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetGlobalClassBook()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ClassBooksAndStationeries.Where(s => s.SessionId == null && s.BranchId == null && s.BooksAndStationery.Type.Name == "Book")
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.BooksAndStationery.ItemName,
        //                Value = s.Id.ToString(),
        //            }).ToListAsync();
        //    }
        //}


        //public async Task<List<SelectListItem>> GetClassBooksDDs()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ClassBooksAndStationeries.Where(s => s.BooksAndStationery.Type.Name.ToLower().Contains("Book")).Select(s => new SelectListItem
        //        {
        //            Text = s.BooksAndStationery.ItemName,
        //            Value = s.Id.ToString(),
        //        }).ToListAsync();
        //    }
        //}


        //public async Task<List<SelectListItem>> GetBookTopicsByBookId(Guid bookId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ClassBookTopics.Where(s => s.ClassBooksAndStationeryId == bookId).Select(s => new SelectListItem
        //        {
        //            Text = s.TopicNo + " - " + s.Topic,
        //            Value = s.Id.ToString(),
        //        }).ToListAsync();
        //    }
        //}


        //public async Task<IEnumerable<SelectListItem>> GetAcademicGroupsWithStagesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups.Where(s => s.SessionId == _SessionId && s.BranchId == _BranchId)
        //            .OrderBy(s => s.Class.Stage.StageOrder)
        //            .ThenBy(s => s.GroupClassOrder)
        //            .ThenBy(s => s.GroupSectionName).Select(s => new SelectListItem
        //            {
        //                Text = s.Class.Stage.StageName + " - " + s.GroupName,
        //                Value = s.GroupId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetBookAndStationaryTypes()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.BooksAndStationeryTypes
        //            .OrderBy(s => s.Name)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Name,
        //                Value = s.TypeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetBooksAndStationeryLocal()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.BooksAndStationeries
        //            .Where(s => s.BranchId == _BranchId/* && s.SessionId == _SessionId*/)
        //            .OrderBy(s => s.ItemName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ItemName,
        //                Value = s.BooksAndStationaryId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetBooksAndStationeryGlobal()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.BooksAndStationeries
        //            .Where(s => s.BranchId == null /*&& s.SessionId == null*/)
        //            .OrderBy(s => s.ItemName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ItemName,
        //                Value = s.BooksAndStationaryId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetOnlyBooksDDLocal()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.BooksAndStationeries
        //            .Where(s => s.BranchId == _BranchId /*&& s.SessionId == _SessionId*/ && s.Type.Name == "Book")
        //            .OrderBy(s => s.ItemName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ItemName,
        //                Value = s.BooksAndStationaryId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetClassBooksDDLocal()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.ClassBooksAndStationeries
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.BooksAndStationery.Type.Name == "Book")
        //            .OrderBy(s => s.BooksAndStationery.ItemName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.BooksAndStationery.ItemName + " For Class " + s.Class.ClassName,
        //                Value = s.Id.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetOnlyBooksDDGlobal()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await
        //            db.BooksAndStationeries
        //            .Where(s => s.BranchId == null /*&& s.SessionId == null*/ && s.Type.Name == "Book")
        //            .OrderBy(s => s.ItemName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ItemName,
        //                Value = s.BooksAndStationaryId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetTeacherByGroupAndSubject(Guid subjectId, Guid groupId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var timeTable = await db.TimeTables.Where(s => s.SessionId == _SessionId).Select(s => s.TimeTableId).ToListAsync();
        //        return await db.TimeTableDetails.Where(s => timeTable.Contains(s.TimeTableId) && s.SubjectId == subjectId && s.EmployeeId != null)
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.Employee.EmpName,
        //            Value = s.EmployeeId.ToString(),
        //        }).Distinct().ToListAsync();
        //    }

        //}

        //public async Task<IEnumerable<SelectListItem>> GetTeachingStaffsByClassAndSubject(Guid classId, Guid subjectId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var query = db.TeachingStaffSubjects.Where(s => s.ClassId == classId && s.ExamSubjectId == subjectId && s.Status == true && s.BranchId == SessionHelper.BranchId).Select(s => s.TeachingStaffId);
        //        return await db.TeachingStaffs.Include(s => s.Employee).Where(s => query.Contains(s.TeachingStaffId))
        //        .Select(s => new SelectListItem
        //        {
        //            Text = s.Employee.EmpName,
        //            Value = s.EmployeeId.ToString(),
        //        }).Distinct().ToListAsync();
        //    }

        //}


        public async Task<IEnumerable<SelectListItem>> GetDaysDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.WeekDays
                    .Select(s => new SelectListItem
                    {
                        Text = s.DayName,
                        Value = s.DayId.ToString(),
                    }).ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetTeachingStaff()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.TeachingStaffs.Where(t => t.SessionId == SessionHelper.CurrentSessionId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Employee.EmpName,
        //                Value = s.EmployeeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetTermsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Terms.Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.TermName,
        //                Value = s.ExamTermId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetTermsByClassIdDD(Guid classId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var stageId = await db.AcademicGroups
        //            .Include(s => s.Class)
        //            .Where(s => s.ClassId == classId && s.SessionId == _SessionId && s.BranchId == _BranchId)
        //            .Select(s => s.Class.StageId).FirstOrDefaultAsync();
        //        return await db.Terms
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.TermsStages.Select(w => w.StageId).Contains(stageId))
        //            .OrderBy(s => s.TermName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.TermName,
        //                Value = s.ExamTermId.ToString(),
        //            }).ToListAsync();
        //    }
        //}


        //public async Task<IEnumerable<SelectListItem>> GetTermsByStageIdDD(Guid stageId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Terms.Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.TermsStages.Select(w => w.StageId).Contains(stageId))
        //            .OrderBy(s => s.TermName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.TermName,
        //                Value = s.ExamTermId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetExamSubTypesDDByExamId(Guid id)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ExamSubTypes
        //            .Where(s => s.BranchId == SessionHelper.BranchId && s.TypeSubTypes.Where(w => w.IsActive == true && w.ExamTypeId == id).Any())
        //            .OrderBy(s => s.DisplayPriority)
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.Id.ToString(),
        //            Text = s.Name,
        //        }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetSessionsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sessions.Where(s => s.BranchId == SessionHelper.BranchId)
        //            .OrderBy(s => s.SessionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Value = s.SessionId.ToString(),
        //                Text = s.SessionName,
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetAllOtherSessionsDD(Guid CurrentSessionId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sessions.Where(s => s.BranchId == SessionHelper.BranchId && s.SessionId != CurrentSessionId)
        //            .OrderBy(s => s.SessionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Value = s.SessionId.ToString(),
        //                Text = s.SessionName,
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetActiveAdmissionSessionsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sessions.Where(s => s.BranchId == SessionHelper.BranchId && s.IsAdmissionOpen == true)
        //            .OrderBy(s => s.SessionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Value = s.SessionId.ToString(),
        //                Text = s.SessionName,
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetHouseTypesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.HouseTypes
                    .OrderBy(s => s.HouseTypeName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.HouseTypeName,
                        Value = s.HouseTypeId.ToString()
                    }).ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetSectionsByClassIdDD(Guid classId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.ClassId == classId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Section.SectionName,
        //                Value = s.Section.SectionId.ToString(),
        //            })
        //            .ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetSectionsDD(short branchId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sections
        //            .Where(p => p.BranchId == branchId)
        //            .OrderBy(s => s.SectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SectionName,
        //                Value = s.SectionId.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetSectionsDDForIB()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sections
        //            .Where(s => s.BranchId == null && s.GlobalSectionId == null)
        //            .OrderBy(s => s.SectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SectionName,
        //                Value = s.SectionId.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetSectionsDDByBranch()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sections.Where(s => s.BranchId == _BranchId && s.Active == true)
        //            .OrderBy(s => s.SectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SectionName,
        //                Value = s.SectionId.ToString()
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetDiscountsDD(short branchId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Discounts
        //            .Where(p => p.BranchId == branchId)
        //            .OrderBy(s => s.DiscountName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.DiscountName,
        //                Value = s.AccountsFeeDiscountId.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetDiscountsDDForIB()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Discounts
        //            .Where(s => s.BranchId == null && s.GlobalDiscountId == null)
        //            .OrderBy(s => s.DiscountName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.DiscountName,
        //                Value = s.AccountsFeeDiscountId.ToString()
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem2>> GetSectionsDDWithStage()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Sections
        //            .OrderBy(s => s.SectionName)
        //            .Select(s => new SelectListItem2
        //            {
        //                Text = s.SectionName,
        //                Value = s.SectionId.ToString(),
        //                Tag = s.StageId.ToString()
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetBranchesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Branches

                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.BranchId.ToString(),
                    }).ToListAsync();
            }
        }
        //public async Task<IEnumerable<SelectListItem>> GetClassesDD(short? branchId = null)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Classes
        //            .Where(s => s.BranchId == (branchId ?? _BranchId))
        //            .OrderBy(s => s.Stage.StageOrder)
        //            .OrderBy(s => s.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ClassName,
        //                Value = s.ClassId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetGradingTemplates(short? branchId = null)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.GradingTemplates
        //            .Where(s => s.BranchId == (branchId ?? _BranchId))
        //            .OrderBy(s => s.TemplateName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.TemplateName,
        //                Value = s.GradingTemplateId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetSubjectGroups(short? branchId = null)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.SubjectGroups
        //            .Where(s => s.BranchId == (branchId ?? _BranchId))
        //            .OrderBy(s => s.SubjectGroupName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.SubjectGroupName,
        //                Value = s.SubjectGroupId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetClassesDDForIB()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Classes
        //            .Where(s => s.BranchId == null && s.GlobalClassId == null)
        //            .OrderBy(s => s.Stage.StageOrder)
        //            .OrderBy(s => s.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ClassName,
        //                Value = s.ClassId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public List<SelectListItem> GetMonthsDD()
        {
            List<string> monthsList = new List<string>();
            monthsList.Add("Jan");
            monthsList.Add("Feb");
            monthsList.Add("Mar");
            monthsList.Add("Apr");
            monthsList.Add("May");
            monthsList.Add("Jun");
            monthsList.Add("Jul");
            monthsList.Add("Aug");
            monthsList.Add("Sep");
            monthsList.Add("Oct");
            monthsList.Add("Nov");
            monthsList.Add("Dec");
            return monthsList.Select(s => new SelectListItem()
            {
                Value = s,
                Text = s,
            }).ToList();
        }
        public List<SelectListItem> GetReportTypesForSibling()
        {
            List<string> monthsList = new List<string>();
            monthsList.Add("All");
            monthsList.Add("Siblings");
            monthsList.Add("Non-Siblings");

            return monthsList.Select(s => new SelectListItem()
            {
                Value = s,
                Text = s,
            }).ToList();
        }

        //public async Task<IEnumerable<SelectListItem>> GetCurrentSessionClassesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.Active == true)
        //            .Include(s => s.Class)
        //            .GroupBy(s => new { s.Class.ClassName, s.Class.ClassId, s.Class.ClassOrder })
        //            .OrderBy(s => s.Key.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Key.ClassName,
        //                Value = s.Key.ClassId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetCurrentSessionSectionsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups
        //            .Where(s => s.BranchId == _BranchId && s.SessionId == _SessionId && s.Active == true)
        //            .Include(s => s.Section)
        //            .GroupBy(s => new { s.Section.SectionName, s.Section.SectionId })
        //            .OrderBy(s => s.Key.SectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Key.SectionName,
        //                Value = s.Key.SectionId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetCountriesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Countries
                    .OrderBy(s => s.CountryName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.CountryName,
                        Value = s.CountryId.ToString()
                    }).ToListAsync();
            }
        }

        public IEnumerable<SelectListItem> GetFamilyHeadTypesDD()
        {
            var List = new List<SelectListItem>();
            List.Add(new SelectListItem { Value = "Father", Text = "Father" });
            List.Add(new SelectListItem { Value = "Mother", Text = "Mother" });
            List.Add(new SelectListItem { Value = "Guardian", Text = "Guardian" });
            return List;
        }
        public async Task<IEnumerable<SelectListItem>> GetReligionsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Religions
                    .OrderBy(s => s.ReligionName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.ReligionName,
                        Value = s.ReligionId.ToString()
                    }).ToListAsync();
            }
        }
        //public async Task<IEnumerable<SelectListItem>> GetFamiliesDDL()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var list = await db.Families
        //            .OrderBy(s => s.FatherName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.FatherName,
        //                Value = s.FamilyRef.ToString()
        //            }).ToListAsync();
        //        return list;
        //    }
        //}

        //public async Task<List<SelectListItem>> GetStructureTypesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.StructureTypes
        //            .Where(s => s.BranchId == _BranchId)
        //            .OrderBy(s => s.StructureTypeName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.StructureTypeName,
        //                Value = s.StructureTypeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        public async Task<IEnumerable<SelectListItem>> GetCitiesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Cities
                    .OrderBy(s => s.CityName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.CityName,
                        Value = s.CityId.ToString()
                    }).ToListAsync();
            }
        }
        //public async Task<IEnumerable<SelectListItem>> GetGlobalClassesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Classes
        //            .Where(s => s.BranchId == null)
        //            .OrderBy(s => s.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ClassName,
        //                Value = s.ClassId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetGlobalResultGradesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ResultGrades.Where(g => g.BranchId == null && g.SessionId == null)
        //          .Select(s => new SelectListItem
        //          {
        //              Text = s.GradeName,
        //              Value = s.GradeId.ToString(),
        //          }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetEmployeesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Employees.Where(s => s.BranchId == _BranchId && s.Active == true)
                    .Select(s => new SelectListItem
                    {
                        Text = s.EmpName,
                        Value = s.EmployeeId.ToString(),
                    }).ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetCurrentSessionEmployeesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.TeachingStaffs.Include(t => t.Employee).Where(s => s.SessionId == SessionHelper.CurrentSessionId)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Employee.EmpName,
        //                Value = s.EmployeeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetProfessionsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Professions.OrderBy(s => s.ProfessionName).Select(s => new SelectListItem
                {
                    Text = s.ProfessionName,
                    Value = s.ProfessionId.ToString()
                }).ToListAsync();
            }
        }


        public async Task<IEnumerable<SelectListItem>> GetRoomsDD()
        {
            using (var db = new OneDbContext())
            {
                return await (from b in db.Branches
                              join bu in db.CompanyBuildings on b.BranchId equals bu.BranchId
                              join f in db.CompanyFloors on bu.BuildingId equals f.BuildingId
                              join r in db.CompanyRooms on f.FloorId equals r.FloorId
                              where r.BranchId == _BranchId
                              select new SelectListItem
                              {
                                  Text = bu.BuildingName + ", " + f.FloorName + ", " + r.RoomCode,
                                  Value = r.RoomId.ToString(),
                              }).ToListAsync();
            }
        }


        //public async Task<IEnumerable<SelectListItem>> GetGlobalStagesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Stages.OrderBy(s => s.StageOrder)
        //            .Where(s => s.BranchId == null)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.StageName,
        //                Value = s.StageId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetStagesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Stages.Where(s => s.BranchId == _BranchId && s.GlobalStageId != null)
        //            .OrderBy(s => s.StageOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.StageName,
        //                Value = s.StageId.ToString(),
        //            }).ToListAsync();
        //    }
        //}
        //public async Task<IEnumerable<SelectListItem>> GetBranchScholarshipsDD(short? BranchId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.ScholarshipTypes.Where(s => s.BranchId == _BranchId && s.GlobalScholarshipTypeId != null)
        //            .OrderBy(s => s.ScholarshipTypeName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ScholarshipTypeName,
        //                Value = s.ScholarshipTypeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}



        //public async Task<IEnumerable<SelectListItem>> GetSessionSettingsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.SessionDefaultSettings
        //        .Select(s => new SelectListItem
        //        {
        //            Text = (string.IsNullOrEmpty(s.SessionName) ? "" : s.SessionName) + " " + s.StartDay + "/" + s.StartMonth + " - " + s.EndDay + "/" + s.EndMonth,
        //            Value = s.AutoKey.ToString()
        //        }).ToListAsync();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetClassesByStageDD(Guid stageId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.Classes.Where(s => s.StageId == stageId && s.BranchId == SessionHelper.BranchId).OrderBy(s => s.ClassOrder)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.ClassName,
        //                Value = s.ClassId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetDepartmentsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Departments.Where(w => w.BranchId == SessionHelper.BranchId && w.GlobalDepartmentId != null).OrderBy(s => s.DepartmentName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.DepartmentName,
                        Value = s.DepartmentId.ToString(),
                    }).ToListAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetDepartmentsByBranchDD(short? BranchId)
        {
            using (var db = new OneDbContext())
            {
                return await db.Departments.Where(d => d.BranchId == BranchId && d.GlobalDepartmentId != null).OrderBy(s => s.DepartmentName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.DepartmentName,
                        Value = s.DepartmentId.ToString(),
                    }).ToListAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetDesignationsDD(short? BranchId)
        {
            using (var db = new OneDbContext())
            {
                return await db.Designations.Where(d => d.BranchId == BranchId && d.GlobalDesignationId != null).OrderBy(s => s.DesignationName)
                    .Select(s => new SelectListItem
                    {
                        Text = s.DesignationName,
                        Value = s.DesignationId.ToString(),
                    }).ToListAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetTemplateTypesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.TemplateTypes
                    .Include(m => m.Module)
                    .OrderBy(m => m.Module.ModuleName)
                    .ThenBy(m => m.TemplateTypeName)
                    .Select(s =>
                    new SelectListItem
                    {
                        Text = s.Module.ModuleName + "(" + s.TemplateTypeName + ")",
                        Value = s.TemplateTypeId.ToString()
                    }).ToListAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetTemplateTypesDD(int? id)
        {
            using (var db = new OneDbContext())
            {
                return await db.TemplateTypes
                    .Include(m => m.Module)
                    .OrderBy(m => m.Module.ModuleName)
                    .ThenBy(m => m.TemplateTypeName)
                    .Where(k => k.ModuleId == id)
                    .Select(s =>
                    new SelectListItem
                    {
                        Text = s.Module.ModuleName + "(" + s.TemplateTypeName + ")",
                        Value = s.TemplateTypeId.ToString()
                    }).ToListAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetSmsQueuesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.SmsQueues
                    .GroupBy(q => q.Batch)
                    .Select(s => new SelectListItem
                    {
                        Text = s.Key.ToString(),
                        Value = s.Key.ToString()
                    }).ToListAsync();
            }
        }

        public IEnumerable<SelectListItem> GetPendingSentFailedDD()
        {
            using (var db = new OneDbContext())
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Pending", Value = "Pending" },
                    new SelectListItem { Text = "Sent", Value = "Sent"       },
                    new SelectListItem { Text = "Failed", Value = "Failed"   },
                };
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetTermTypesByTermId(Guid examTermId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await (from tt in db.TermTypes
        //                      join ty in db.Types1 on tt.ExamTypeId equals ty.ExamTypeId
        //                      where tt.ExamTermId == examTermId
        //                      select new SelectListItem
        //                      {
        //                          Text = ty.ExamType,
        //                          Value = tt.ExamTypeId.ToString()
        //                      }).ToListAsync();
        //    }
        //}
        public async Task<IEnumerable<SelectListItem>> GetClassSubjectDDByClassId(Guid classId)
        {
            using (var db = new OneDbContext())
            {
                return await db.Database.SqlQuery<SelectListItem>($@"select 
                cast(s.ExamSubjectId as varchar(500))Value,s.SubjectName as Text 
                from ER.ClassSubjects cls
                Inner JOIN ER.Subjects s ON cls.SubjectId = s.ExamSubjectId
                where cls.ClassId = '{classId}' 
                AND s.BranchId = {SessionHelper.BranchId}  AND cls.IsActive = 1
                Group By s.ExamSubjectId,s.SubjectName
                order by s.SubjectName").ToListAsync();
            }
        }

        //public async Task<IEnumerable<SelectListItem>> GetSubjectsDDByGroupId(Guid id)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var groupS = await db.AcademicGroups.Include(s => s.Class).Where(s => s.GroupId == id).FirstOrDefaultAsync();
        //        var query = from sb in db.Subjects
        //                    join cs in db.ClassSubjects on sb.ExamSubjectId equals cs.SubjectId
        //                    where cs.ClassId == groupS.ClassId &&
        //                    //cs.SectionId == groupS.SectionId &&
        //                    sb.BranchId == SessionHelper.BranchId &&
        //                    cs.IsActive == true &&
        //                    cs.SessionId == SessionHelper.CurrentSessionId
        //                    orderby sb.SubjectCode
        //                    select new SelectListItem
        //                    {
        //                        Text = sb.SubjectName,
        //                        Value = sb.ExamSubjectId.ToString()
        //                    };
        //        return await query.Distinct().ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetTimetableSubjectsDDByGroupId(Guid id, int day)
        {
            using (var db = new OneDbContext())
            {
                var query = $@"select Concat(sb.SubjectName,', ', att.PeriodNumber,' , (', ltrim(right(convert(varchar(25), cast(att.StartTime as datetime), 100), 7)),' - ',ltrim(right(convert(varchar(25), cast(att.EndTime as datetime), 100), 7)),') , ',emp.EmpName)Text,
                            cast(att.TimeTableDetailId as varchar(50)) as Value
                            from Academics.TimeTableDetails att
                            INNER JOIN Academics.TimeTable ttbl ON att.TimeTableId = ttbl.TimeTableId
                            INNER JOIN ER.Subjects sb on sb.ExamSubjectId = att.SubjectId
                            Left JOIN HR.Employees emp on emp.EmployeeId = att.EmployeeId 
                            where GroupId = '{id}'
                            AND ttbl.IsActive = 1  AND att.DayId = {day}
                            order by sb.SubjectCode";
                return await db.Database.SqlQuery<SelectListItem>(query).ToListAsync();
            }
        }
        //public async Task<IEnumerable<SelectListItem>> GetSubjectsByGroup(Guid groupId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var group = await db.AcademicGroups.Include(s => s.Class).Where(s => s.GroupId == groupId).FirstOrDefaultAsync();
        //        var subjectIds = await db.Subjects.Where(s => s.StageId == group.Class.StageId).Select(s => s.ExamSubjectId).ToListAsync();
        //        return await db.ClassSubjects
        //            .Where(s => s.IsActive == true && s.ClassId == group.ClassId && s.SectionId == group.SectionId && subjectIds.Contains(s.SubjectId))
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.Subject.SubjectName,
        //                Value = s.SubjectId.ToString(),
        //            }).ToListAsync();
        //    }

        //}

        //public async Task<IEnumerable<SelectListItem>> GetTeachingStaffsDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var subQuery = db.TeachingStaffs
        //            .Where(s => s.SessionId == _SessionId)
        //            .Select(s => s.EmployeeId);

        //        return await db.Employees
        //            .Where(s => s.BranchId == _BranchId && s.Active == true && subQuery.Contains(s.EmployeeId))
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.EmpName,
        //                Value = s.EmployeeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public async Task<List<SelectListItem>> GetTeachingStaffsBySubjectDD(Guid? groupId, Guid subjectId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var classId = await db.AcademicGroups.Where(s => s.GroupId == groupId).Select(s => s.ClassId).FirstOrDefaultAsync();

        //        //getting teaching staff subjects
        //        var teachingStaffSubjects = db.TeachingStaffSubjects.Where(s => s.ClassId == classId && s.ExamSubjectId == subjectId && s.Status == true && s.BranchId == _BranchId).Select(s => s.TeachingStaffClassId);

        //        //getting teaching staff classes
        //        var teachingStaffClasses = db.TeachingStaffClasses.Where(s => teachingStaffSubjects.Contains(s.TeachingStaffClassId)).Select(s => s.TeachingStaffId);

        //        //getting teaching staffs
        //        var subQuery = db.TeachingStaffs.Where(s => teachingStaffClasses.Contains(s.TeachingStaffId) && s.SessionId == _SessionId).Select(s => s.EmployeeId);

        //        //getting Employees
        //        return await db.Employees
        //            .Where(s => s.BranchId == _BranchId && s.Active == true && subQuery.Contains(s.EmployeeId))
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.EmpName,
        //                Value = s.EmployeeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        //public List<SelectListItem> IQGetTeachingStaffsBySubjectDD(Guid? groupId, Guid subjectId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var classId = db.AcademicGroups.Where(s => s.GroupId == groupId).Select(s => s.ClassId).FirstOrDefault();

        //        //getting teaching staff subjects
        //        var teachingStaffSubjects = db.TeachingStaffSubjects.Where(s => s.ClassId == classId && s.ExamSubjectId == subjectId && s.Status == true && s.BranchId == _BranchId).Select(s => s.TeachingStaffClassId);

        //        //getting teaching staff classes
        //        var teachingStaffClasses = db.TeachingStaffClasses.Where(s => teachingStaffSubjects.Contains(s.TeachingStaffClassId)).Select(s => s.TeachingStaffId);

        //        //getting teaching staffs
        //        var subQuery = db.TeachingStaffs.Where(s => teachingStaffClasses.Contains(s.TeachingStaffId) && s.SessionId == _SessionId).Select(s => s.EmployeeId);

        //        //getting Employees
        //        return db.Employees
        //            .Where(s => s.BranchId == _BranchId && s.Active == true && subQuery.Contains(s.EmployeeId))
        //            .Select(x => new SelectListItem
        //            {
        //                Text = x.EmpName,
        //                Value = x.EmployeeId.ToString()
        //            }).ToList();
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetTeachingStaffsByClassDD(Guid classId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        //&& s.SessionId == _SessionId
        //        //getting teaching staff classes
        //        var teachingStaffClasses = db.TeachingStaffClasses.Where(s => s.ClassId == classId && s.BranchId == _BranchId && s.Status == true).Select(s => s.TeachingStaffId);

        //        //getting teaching staffs
        //        var subQuery = db.TeachingStaffs.Where(s => teachingStaffClasses.Contains(s.TeachingStaffId)).Select(s => s.EmployeeId);

        //        //getting Employees
        //        return await db.Employees
        //            .Where(s => s.BranchId == _BranchId && s.Active == true && subQuery.Contains(s.EmployeeId))
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.EmpName,
        //                Value = s.EmployeeId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public IEnumerable<SelectListItem> GetSkillLevelsDD()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Text = "1", Value = 1.ToString()},
                new SelectListItem{ Text = "2", Value = 2.ToString()},
                new SelectListItem{ Text = "3", Value = 3.ToString()},
                new SelectListItem{ Text = "4", Value = 4.ToString()},
                new SelectListItem{ Text = "5", Value = 5.ToString()}
            };
        }
        public IEnumerable<SelectListItem> GetSeniorityTypeDD()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Value = "Class", Text = "Class Wise"},
                new SelectListItem{ Value = "AdmissionWise", Text = "Admission Wise"},
            };
        }

        //public async Task<IEnumerable<SelectListItem>> GetGroupsWithStagesDD()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        return await db.AcademicGroups.Where(s => s.SessionId == _SessionId && s.BranchId == _BranchId)
        //            .OrderBy(s => s.Class.Stage.StageOrder)
        //            .ThenBy(s => s.GroupClassOrder)
        //            .ThenBy(s => s.GroupSectionName).Select(s => new SelectListItem
        //            {
        //                Text = s.Class.Stage.StageName + " - " + s.GroupName,
        //                Value = s.GroupId.ToString(),
        //            }).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<SelectListItem>> GetStatesDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.States.Select(s => new SelectListItem()
                {
                    Value = s.StateId.ToString(),
                    Text = s.StateName
                }).ToListAsync();
            }

        }



        public async Task<IEnumerable<SelectListItem>> GetDistrictsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.Districts.Select(s => new SelectListItem()
                {
                    Value = s.DistrictId.ToString(),
                    Text = s.DistrictName
                }).ToListAsync();
            }

        }


        #region NewsEvents
        public async Task<IEnumerable<SelectListItem>> GetNewsEventsDD()
        {
            using (var db = new OneDbContext())
            {
                return await db.NewsEvents

                    .Select(s => new SelectListItem
                    {
                        Text = s.Title,
                        Value = s.News_Id.ToString(),
                    }).ToListAsync();
            }
        }
        #endregion
    }
}