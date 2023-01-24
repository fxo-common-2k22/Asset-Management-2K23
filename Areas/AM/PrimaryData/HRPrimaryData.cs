using FAPP.Model;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace FAPP.Areas.AM.ViewModels.PrimaryData
{
    public class HRPrimaryData
    {
        public OneDbContext db = null;

        public HRPrimaryData(OneDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void ImportAll(ref PrimaryDataViewModel vm)
        {
            try
            {
                GetOldDesignations(ref vm);
                ImportDesignationsAsGlobal(ref vm);
                ImportDesignationsAsLocal(ref vm);
                GetOldDepartments(ref vm);
                ImportDepartmentsAsGlobal(ref vm);
                ImportDepartmentsAsLocal(ref vm);
                GetOldShifts(ref vm);
                ImportShifts(ref vm);
                GetOldEmployees(ref vm);
                ImportEmployees(ref vm);
                GetOldBioMetricUsers(ref vm);
                ImportBioMetricUsers(ref vm);
                GetOldLeaveTypes(ref vm);
                ImportLeaveTypes(ref vm);
                GetOldPlacementTypes(ref vm);
                ImportPlacementTypes(ref vm);
                GetOldEmployeeAttendances(ref vm);
                ImportEmployeeAttendances(ref vm);
                GetOldRawAttendanceLogs(ref vm);
                ImportRawAttendanceLogs(ref vm);
                GetOldGuarantors(ref vm);
                ImportGuarantors(ref vm);
                GetOldPlacements(ref vm);
                ImportEmployeePlacements(ref vm);
                GetOldExperiences(ref vm);
                ImportEmployeeWorkExperience(ref vm);
                GetOldEducations(ref vm);
                ImportEmployeeEducations(ref vm);
                var importWizard = db.ImportWizard.FirstOrDefault();
                if (importWizard == null)
                {
                    importWizard = new ImportWizard
                    {
                        HRImported = true,
                    };
                    db.ImportWizard.Add(importWizard);
                }
                else
                {
                    importWizard.HRImported = true;
                }
                db.SaveChanges();
            }
            catch (Exception x)
            {
                vm.Error += x.GetExceptionMessages();
            }
        }

        public void GetOldDesignations(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.Designations";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Designations = DataReaderMapToList<PrimaryDataViewModel.OldDesignations>(reader)
                        .Select(s => new Designation
                        {
                            DesignationId = s.DesignationId,
                            DesignationName = s.DesignationName,
                            GlobalDesignationId = null,
                            IP = s.IP,
                            ModifiedBy = s.ModifiedBy,
                            Prev__ID = s.Prev__ID,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportDesignationsAsGlobal(ref PrimaryDataViewModel vm)
        {
            db.Designations.AddOrUpdate(s => s.DesignationId, vm.Designations.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Designations.Count()} Designations imported successfully.";
        }

        public void ImportDesignationsAsLocal(ref PrimaryDataViewModel vm)
        {
            vm.LocalDesignations = new List<Designation>();
            int i = 1;
            foreach (var branchId in db.Branches.Select(s => s.BranchId).ToList())
            {
                foreach (var desg in vm.Designations)
                {
                    vm.LocalDesignations.Add(new Designation
                    {
                        DesignationId = Convert.ToInt16(vm.Designations.Max(s => s.DesignationId) + i++),
                        DesignationName = desg.DesignationName,
                        GlobalDesignationId = desg.DesignationId,
                        IP = desg.IP,
                        ModifiedBy = desg.ModifiedBy,
                        Prev__ID = desg.Prev__ID,
                        BranchId = branchId,
                    });
                }
            }
            db.Designations.AddOrUpdate(s => s.DesignationId, vm.LocalDesignations.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.LocalDesignations.Count()} Designations imported (Locally/Globally) successfully.";
        }

        public void GetOldDepartments(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.Departments";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Departments = DataReaderMapToList<PrimaryDataViewModel.OldDepartment>(reader)
                        .Select(s => new Department
                        {
                            DepartmentId = s.DepartmentId,
                            DepartmentName = s.DepartmentName,
                            GracePeriod = s.GracePeriod,
                            IP = s.IP,
                            ModifiedBy = s.ModifiedBy,
                            TimeIn = s.TimeIn,
                            TimeOut = s.TimeOut,
                            WorkingHours = s.WorkingHours,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportDepartmentsAsGlobal(ref PrimaryDataViewModel vm)
        {
            db.Departments.AddOrUpdate(s => s.DepartmentId, vm.Departments.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Designations.Count()} Designations imported successfully.";
        }

        public void ImportDepartmentsAsLocal(ref PrimaryDataViewModel vm)
        {
            vm.LocalDepartments = new List<Department>();
            int i = 1;
            foreach (var branchId in db.Branches.Select(s => s.BranchId).ToList())
            {
                foreach (var dept in vm.Departments)
                {
                    vm.LocalDepartments.Add(new Department
                    {
                        DepartmentId = Convert.ToInt16(vm.Departments.Max(s => s.DepartmentId) + i++),
                        DepartmentName = dept.DepartmentName,
                        GracePeriod = dept.GracePeriod,
                        IP = dept.IP,
                        ModifiedBy = dept.ModifiedBy,
                        TimeIn = dept.TimeIn,
                        TimeOut = dept.TimeOut,
                        WorkingHours = dept.WorkingHours,
                        BranchId = branchId,
                        GlobalDepartmentId = dept.DepartmentId
                    });
                }
            }
            db.Departments.AddOrUpdate(s => s.DepartmentId, vm.LocalDepartments.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.LocalDepartments.Count()} Departments imported (Locally/Globally) successfully.";
        }

        public void GetOldShifts(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.Shifts";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Shifts = DataReaderMapToList<PrimaryDataViewModel.OldShift>(reader)
                        .Select(s => new HRShift
                        {
                            ShiftId = s.ShiftId,
                            ShiftName = s.ShiftName,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            Duration = s.Duration,
                            ByDefault = s.ByDefault,
                            BranchId = s.BranchId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportShifts(ref PrimaryDataViewModel vm)
        {
            db.HRShifts.AddOrUpdate(s => s.ShiftId, vm.Shifts.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Shifts.Count()} Shifts imported successfully.";
        }

        public void GetOldEmployees(ref PrimaryDataViewModel vm)
        {
            var localDepts = vm.LocalDepartments;
            var localDesgs = vm.LocalDesignations;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.Employees";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Employees = DataReaderMapToList<PrimaryDataViewModel.OldEmployee>(reader)
                        .Select(s => new Employee
                        {
                            EmployeeId = s.EmployeeId,
                            EmpName = s.EmpName,
                            RegNo = s.RegNo,
                            FatherName = s.FatherName,
                            DOB = s.DOB,
                            NIC = s.NIC,
                            Mobile = s.Mobile,
                            Phone = s.Phone,
                            Email = s.Email,
                            Password = s.Password,
                            NationalityId = s.NationalityId,
                            Gender = s.Gender,
                            ReligionId = s.ReligionId,
                            Active = s.Active,
                            AppointmentDate = s.AppointmentDate,
                            DOJ = s.DOJ,
                            //DepartmentId = s.DepartmentId,
                            //DesignationId = s.DesignationId,
                            DepartmentId = localDepts.Where(w => w.GlobalDepartmentId == s.DepartmentId).Select(x => x.DepartmentId == 0 ? null : (short?)x.DepartmentId).FirstOrDefault(),
                            DesignationId = localDesgs.Where(w => w.GlobalDesignationId == s.DesignationId).Select(x => x.DesignationId == 0 ? null : (short?)x.DesignationId).FirstOrDefault(),
                            CountryId = s.CountryId,
                            StateId = s.StateId,
                            CityId = s.CityId,
                            Address = s.Address,
                            SupervisorId = s.SupervisorId,
                            DeptSupervisorId = s.DeptSupervisorId,
                            MaritalStatus = s.MaritalStatus,
                            ApplicationDate = s.ApplicationDate,
                            ApplicationStatus = s.ApplicationStatus,
                            Photo = s.Photo,
                            LeavingDate = s.LeavingDate,
                            BranchId = s.BranchId,
                            ReasonForLeaving = s.ReasonForLeaving,
                            LeavingType = s.LeavingType,
                            EmployeeAccountId = s.EmployeeAccountId,
                            Salary = s.Salary,
                            PassportNo = s.PassportNo,
                            BankName = s.BankName,
                            BankAccount = s.BankAccount,
                            PersonalEmailAddress = s.PersonalEmailAddress,
                            BloodGroup = s.BloodGroup,
                            EmergencyContactNumber = s.EmergencyContactNumber,
                            EOBINumber = s.EOBINumber,
                            TakafulInsuranceNumber = s.TakafulInsuranceNumber,
                            NewId = s.NewId,
                            OldId = s.OldId,
                            ShiftId = s.ShiftId,
                            Disease = s.Disease,
                            Instructions = s.Instructions,
                            MedicalProblem = s.MedicalProblem,
                            ChronicalMedicalProblems = s.ChronicalMedicalProblems,
                            TBHistory = s.TBHistory,
                            DiabetesHistory = s.DiabetesHistory,
                            EpilepsyHistory = s.EpilepsyHistory,
                            OthersHistory = s.OthersHistory,
                            Allergies = s.Allergies,
                            Medication = s.Medication,
                            TaxSlabId = s.TaxSlabId,
                            ContractType = s.ContractType,
                            EmpNo = s.EmpNo.HasValue ? s.EmpNo.ToString() : string.Empty,
                            ReasonForReactivate = s.ReasonForReactivate,
                            ReactivateDate = s.ReactivateDate,
                            SalaryPaymentModeId = s.SalaryPaymentModeId,
                            SecurityAmount = s.SecurityAmount,
                            RFID = s.RFID,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
                //vm.Employees.ForEach(s =>
                //{
                //    s.DepartmentId = localDepts.Where(w => w.GlobalDepartmentId == s.DepartmentId).Select(x => x.DepartmentId == 0 ? null : (short?)x.DepartmentId).FirstOrDefault();
                //    s.DesignationId = localDesgs.Where(w => w.GlobalDesignationId == s.DesignationId).Select(x => x.DesignationId == 0 ? null : (short?)x.DesignationId).FirstOrDefault();
                //});
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportEmployees(ref PrimaryDataViewModel vm)
        {
            db.Employees.AddOrUpdate(s => s.EmployeeId, vm.Employees.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Employees.Count()} Employees imported successfully.";
        }

        public void GetOldBioMetricUsers(ref PrimaryDataViewModel vm)
        {
            var localDepts = vm.LocalDepartments;
            var localDesgs = vm.LocalDesignations;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from dbo.USERINFO";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.USERINFOes = DataReaderMapToList<PrimaryDataViewModel.OldUSERINFO>(reader)
                        .Select(s => new USERINFO
                        {
                            USERID = s.USERID,
                            BADGENUMBER = s.BADGENUMBER,
                            SSN = s.SSN,
                            NAME = s.NAME,
                            GENDER = s.GENDER,
                            TITLE = s.TITLE,
                            PAGER = s.PAGER,
                            BIRTHDAY = s.BIRTHDAY,
                            HIREDDAY = s.HIREDDAY,
                            STREET = s.STREET,
                            CITY = s.CITY,
                            STATE = s.STATE,
                            ZIP = s.ZIP,
                            OPHONE = s.OPHONE,
                            FPHONE = s.FPHONE,
                            VERIFICATIONMETHOD = s.VERIFICATIONMETHOD,
                            DEFAULTDEPTID = s.DEFAULTDEPTID,
                            SECURITYFLAGS = s.SECURITYFLAGS,
                            ATT = s.ATT,
                            INLATE = s.INLATE,
                            OUTEARLY = s.OUTEARLY,
                            OVERTIME = s.OVERTIME,
                            SEP = s.SEP,
                            HOLIDAY = s.HOLIDAY,
                            MINZU = s.MINZU,
                            PASSWORD = s.PASSWORD,
                            LUNCHDURATION = s.LUNCHDURATION,
                            MVerifyPass = s.MVerifyPass,
                            PHOTO = s.PHOTO,
                            Notes = s.Notes,
                            privilege = s.privilege,
                            InheritDeptSch = s.InheritDeptSch,
                            InheritDeptSchClass = s.InheritDeptSchClass,
                            AutoSchPlan = s.AutoSchPlan,
                            MinAutoSchInterval = s.MinAutoSchInterval,
                            RegisterOT = s.RegisterOT,
                            InheritDeptRule = s.InheritDeptRule,
                            EMPRIVILEGE = s.EMPRIVILEGE,
                            CardNo = s.CardNo,
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                            //DEFAULTDEPTIDParent = s.DEFAULTDEPTIDParent,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportBioMetricUsers(ref PrimaryDataViewModel vm)
        {
            db.USERINFOes.AddOrUpdate(s => s.USERID, vm.USERINFOes.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.USERINFOes.Count()} Biometric Users imported successfully.";
        }

        public void GetOldLeaveTypes(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.LeaveTypes";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.LeaveTypes = DataReaderMapToList<PrimaryDataViewModel.OldLeaveTypes>(reader)
                        .Select(s => new LeaveType
                        {
                            id = s.id,
                            LeaveType1 = s.LeaveType,
                            PaidLeave = s.PaidLeave,
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportLeaveTypes(ref PrimaryDataViewModel vm)
        {
            db.LeaveTypes.AddOrUpdate(s => s.id, vm.LeaveTypes.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.LeaveTypes.Count()} Leave Types imported successfully.";
        }

        public void GetOldPlacementTypes(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.PlacementTypes";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.PlacementTypes = DataReaderMapToList<PrimaryDataViewModel.OldPlacementTypes>(reader)
                        .Select(s => new PlacementType
                        {
                            PlacementTypeId = s.PlacementTypeId,
                            PlacementTypeName = s.PlacementTypeName,
                            Action = s.Action,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportPlacementTypes(ref PrimaryDataViewModel vm)
        {
            db.PlacementTypes.AddOrUpdate(s => s.PlacementTypeId, vm.PlacementTypes.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.PlacementTypes.Count()} Placement Types imported successfully.";
        }

        public void GetOldEmployeeAttendances(ref PrimaryDataViewModel vm)
        {
            var localDepts = vm.LocalDepartments;
            var localDesgs = vm.LocalDesignations;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from dbo.USERINFO";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.EmployeeAttendance = DataReaderMapToList<PrimaryDataViewModel.OldEmployeeAttendance>(reader)
                        .Select(s => new EmployeeAttendance
                        {
                            EAID = s.EAID,
                            EmployeeId = s.EmployeeId,
                            AttendanceDate = s.AttendanceDate,
                            AttendanceStatus = s.AttendanceStatus,
                            Arrival = s.AttendanceDate + s.Arrival,
                            BreakOut = s.BreakOut,
                            BreakIn = s.BreakIn,
                            Departure = s.AttendanceDate + s.Departure,
                            LeaveTypeId = s.LeaveTypeId,
                            PlacementId = s.PlacementId,
                            Hours = s.Hours,
                            Holiday = s.Holiday,
                            OverTime = s.OverTime,
                            Shortage = s.Shortage,
                            DailyHours = s.DailyHours,
                            DepartmentId = localDepts.Where(w => w.GlobalDepartmentId == s.DepartmentId).Select(x => x.DepartmentId).FirstOrDefault(),
                            DesignationId = localDesgs.Where(w => w.GlobalDesignationId == s.DesignationId).Select(x => x.DesignationId).FirstOrDefault(),
                            BranchId = s.BranchId,
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                            HalfDay = s.HalfDay,
                            //Prev__ID = s.Prev__ID,
                            HalfDayLeave = s.HalfDayLeave,
                            Present = s.Present,
                            GracePeriod = s.GracePeriod,
                            ShiftTimeIn = s.ShiftTimeIn,
                            ShiftTimeOut = s.ShiftTimeOut,
                            ShiftId = s.ShiftId,
                            //Version = s.Version,
                            Remarks = s.Remarks,
                            //ArrivalSMSStatus = s.ArrivalSMSStatus,
                            //DepartureSMSStatus = s.DepartureSMSStatus,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportEmployeeAttendances(ref PrimaryDataViewModel vm)
        {
            db.EmployeeAttendances.AddOrUpdate(s => s.EAID, vm.EmployeeAttendance.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.EmployeeAttendance.Count()} Employee Attendances imported successfully.";
        }

        public void GetOldRawAttendanceLogs(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from dbo.checkinout";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.RawAttendanceLogs = DataReaderMapToList<PrimaryDataViewModel.OldCHECKINOUT>(reader)
                        .Select(s => new CHECKINOUT
                        {
                            USERID = s.USERID,
                            CHECKTIME = s.CHECKTIME,
                            CHECKTYPE = s.CHECKTYPE,
                            VERIFYCODE = s.VERIFYCODE,
                            SENSORID = s.SENSORID,
                            Memoinfo = s.Memoinfo,
                            WorkCode = s.WorkCode,
                            sn = s.sn,
                            UserExtFmt = s.UserExtFmt,
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                            //ID = s.ID,
                            Processed = s.Processed,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportRawAttendanceLogs(ref PrimaryDataViewModel vm)
        {
            db.CHECKINOUT.AddRange(vm.RawAttendanceLogs);
            db.SaveChanges();
            vm.Messages += $"{vm.RawAttendanceLogs.Count()} Raw Attendance Log imported successfully.";
        }

        public void GetOldGuarantors(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.EmployeeGuarantors";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.EmployeeGuarantors = DataReaderMapToList<PrimaryDataViewModel.OldEmployeeGuarantor>(reader)
                        .Select(s => new EmployeeGuarantor
                        {
                            EmployeeGuarantorId = s.EmployeeGuarantorId,
                            Name = s.Name,
                            HouseNo = s.HouseNo,
                            StreetAddress = s.StreetAddress,
                            Photo = s.Photo,
                            IDCardNo = s.IDCardNo,
                            Gender = s.Gender,
                            Mobile = s.Mobile,
                            Phone = s.Phone,
                            ModifiedOn = s.ModifiedOn,
                            ModifiedBy = s.ModifiedBy,
                            EmployeeId = s.EmployeeId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportGuarantors(ref PrimaryDataViewModel vm)
        {
            db.EmployeeGuarantors.AddOrUpdate(s => s.EmployeeGuarantorId, vm.EmployeeGuarantors.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.EmployeeGuarantors.Count()} Employee Guarantors Log imported successfully.";
        }

        public void GetOldPlacements(ref PrimaryDataViewModel vm)
        {
            var designations = vm.Designations;
            var departments = vm.Departments;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.EmployeePlacements";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.EmployeePlacements = DataReaderMapToList<PrimaryDataViewModel.OldEmployeePlacement>(reader)
                        .Select(s => new EmployeePlacement
                        {
                            PlacementId = s.PlacementId,
                            EmployeeId = s.EmployeeId,
                            DepartmentId = departments.Where(x => x.GlobalDepartmentId == s.DepartmentId).Select(x => (short?)x.DepartmentId).FirstOrDefault() ?? s.DepartmentId,
                            DesignationId = designations.Where(x => x.GlobalDesignationId == s.DesignationId).Select(x => (short?)x.DesignationId).FirstOrDefault() ?? s.DesignationId,
                            LocationId = s.LocationId,
                            FromDate = s.FromDate,
                            ToDate = s.ToDate,
                            StartingSalary = s.StartingSalary,
                            Active = s.Active,
                            PlacementTypeId = s.PlacementTypeId,
                            Description = s.Description,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportEmployeePlacements(ref PrimaryDataViewModel vm)
        {
            db.EmployeePlacements.AddOrUpdate(s => s.PlacementId, vm.EmployeePlacements.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.EmployeeGuarantors.Count()} Employee Guarantors Log imported successfully.";
        }

        public void GetOldExperiences(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.EmployeeWorkExperience";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.EmployeeWorkExperiences = DataReaderMapToList<PrimaryDataViewModel.OldEmployeeWorkExperience>(reader)
                        .Select(s => new EmployeeWorkExperience
                        {
                            EmployeeWorkExperienceId = s.EmployeeWorkExperienceId,
                            EmployeeId = s.EmployeeId,
                            Organiztion = s.Organiztion,
                            Designation = s.Designation,
                            JobType = s.JobType,
                            FromDate = s.FromDate,
                            ToDate = s.ToDate,
                            ReasonForLeaving = s.ReasonForLeaving,
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportEmployeeWorkExperience(ref PrimaryDataViewModel vm)
        {
            db.EmployeeWorkExperiences.AddOrUpdate(s => s.EmployeeWorkExperienceId, vm.EmployeeWorkExperiences.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.EmployeeWorkExperiences.Count()} Employee Work Experience Log imported successfully.";
        }

        public void GetOldEducations(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from HR.EmployeeEducation";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.EmployeeEducations = DataReaderMapToList<PrimaryDataViewModel.OldEmployeeEducation>(reader)
                        .Select(s => new EmployeeEducation
                        {
                            EmployeeEducationId = s.EmployeeEducationId,
                            EmployeeId = s.EmployeeId,
                            EducationLevelId = s.EducationLevelId,
                            InstituteName = s.InstituteName,
                            Year = s.Year,
                            Specialization = s.Specialization,
                            ModifiedBy = s.ModifiedBy,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportEmployeeEducations(ref PrimaryDataViewModel vm)
        {
            db.EmployeeEducations.AddOrUpdate(s => s.EmployeeEducationId, vm.EmployeeEducations.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.EmployeeEducations.Count()} Employee Educaiton Log imported successfully.";
        }

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
    }
}