using FAPP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FAPP.Areas.AM.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "Current Password")]
        [Required]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }

    public class PrimaryDataViewModel
    {
        public Info Info { get; set; }
        public List<User> Users { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public List<FormVM> Forms { get; set; }
        public CredentialVM Credential { get; set; }
        public List<Branch> Branches { get; set; }
        public List<UserBranch> UserBranches { get; internal set; }
        public List<Account> Accounts { get; internal set; }
        public List<StatementSetup> StatementSetups { get; internal set; }
        public List<Currency> Currencies { get; internal set; }
        public List<CurrencyValue> CurrencyValues { get; internal set; }
        public List<CostGroup> CostGroups { get; internal set; }
        public List<FiscalYear> FiscalYears { get; internal set; }
        public List<Designation> Designations { get; set; }
        public string Error { get; internal set; }
        public string Messages { get; internal set; }
        public List<Department> Departments { get; internal set; }
        public List<HRShift> Shifts { get; internal set; }
        public List<Employee> Employees { get; internal set; }
        public List<Department> LocalDepartments { get; internal set; }
        public List<Designation> LocalDesignations { get; internal set; }
        public List<Country> Countries { get; internal set; }
        public List<State> States { get; internal set; }
        public List<City> Cities { get; internal set; }
        public List<Nationality> Nationalities { get; internal set; }
        public List<DepriciationType> DepriciationTypes { get; internal set; }
        public List<DocumentType> DocumentTypes { get; internal set; }
        public List<Profession> Professions { get; internal set; }
        public List<Region> Regions { get; internal set; }
        public List<Religion> Religions { get; internal set; }
        public List<TaxSlab> TaxSlabs { get; internal set; }
        public List<USERINFO> USERINFOes { get; internal set; }
        public List<EmployeeAttendance> EmployeeAttendance { get; internal set; }
        public List<LeaveType> LeaveTypes { get; internal set; }
        public List<PlacementType> PlacementTypes { get; internal set; }
        public List<CHECKINOUT> RawAttendanceLogs { get; internal set; }
        public List<Salary> Salaries { get; internal set; }
        public List<SalaryDetail> SalaryDetails { get; internal set; }
        public List<Payment> Payments { get; internal set; }
        //public List<Session> Sessions { get; internal set; }
        //public List<Stage> Stages { get; internal set; }
        //public List<Stage> GlobalStages { get; internal set; }
        //public List<Class> Classes { get; internal set; }
        //public List<Class> GlobalClasses { get; internal set; }
        //public List<Section> Sections { get; internal set; }
        //public List<AcademicGroup> Groups { get; internal set; }
        //public List<Subject> Subjects { get; internal set; }
        //public List<SubSubject> SubSubjects { get; internal set; }
        //public List<SubSubject> GlobalSubSubjects { get; internal set; }
        //public List<Term> Terms { get; internal set; }
        //public List<Types1> ExamTypes { get; internal set; }
        //public List<Types1> GlobalExamTypes { get; internal set; }
        //public List<Result> Results { get; internal set; }
        //public List<ResultItem> ResultItems { get; internal set; }
        //public List<Student> Students { get; internal set; }
        //public List<StudentSession> StudentSessions { get; internal set; }
        //public List<StudentAdmission> StudentAdmissions { get; internal set; }
        //public List<Subject> GlobalSubjects { get; internal set; }
        //public List<Subject> LocalSubjects { get; internal set; }
        //public List<ExamDateSheet> ExamDateSheets { get; internal set; }
        //public List<DateSheet> DateSheets { get; internal set; }
        //public List<DateSheetDetail> DateSheetDetails { get; internal set; }
        //public List<Term> LocalTerms { get; internal set; }
        //public List<FeeType> FeeHeads { get; internal set; }
        //public List<FeeType> GlobalFeeHeads { get; internal set; }
        //public List<Discount> FeeDiscounts { get; internal set; }
        //public List<LateFine> LateFines { get; internal set; }
        //public List<FeeSetting> FeeSettings { get; internal set; }
        //public List<Discount> GlobalFeeDiscounts { get; internal set; }
        //public List<LateFine> GlobalLateFines { get; internal set; }
        //public List<FeeSetting> GlobalFeeSettings { get; internal set; }
        //public List<FeeVoucher> FeeVouchers { get; internal set; }
        //public List<FeeVoucherDetail> FeeVoucherDetails { get; internal set; }
        //public List<ReceiptItem> ReceiptItems { get; internal set; }
        //public List<Receipt> Receipts { get; internal set; }
        //public List<Structure> FeeStructures { get; internal set; }
        //public List<Structure> GlobalFeeStructures { get; internal set; }
        //public List<Stage> LocalStages { get; internal set; }
        //public List<SessionDefaultSetting> SessionSettings { get; internal set; }
        //public List<StudentStructure> StudentStructures { get; internal set; }
        //public List<StructureType> StructureTypes { get; internal set; }
        //public StructureType GlobalStructureType { get; internal set; }
        public List<Voucher> Vouchers { get; internal set; }
        public List<VoucherDetail> VoucherDetails { get; internal set; }
        public List<VoucherType> VoucherTypes { get; internal set; }
        //public List<StudentAttendance> StudentAttendances { get; internal set; }
        // public List<YearlyBalance> YearlyBalances { get; internal set; }
        public List<BankAccount> BankAccounts { get; internal set; }
        public List<EmployeeGuarantor> EmployeeGuarantors { get; internal set; }
        public List<EmployeePlacement> EmployeePlacements { get; internal set; }
        public List<EmployeeWorkExperience> EmployeeWorkExperiences { get; internal set; }
        public List<EmployeeEducation> EmployeeEducations { get; internal set; }
        //public List<CollectorPerson> CollectorPersons { get; internal set; }
        //public List<StudentCollector> StudentCollectors { get; internal set; }
        //public List<Zones2> TransportZones { get; internal set; }
        //public List<Village> TransportVillages { get; internal set; }
        //public List<Driver1> TransportDrivers { get; internal set; }
        //public List<Bus> TransportBuses { get; internal set; }
        //public List<Vehicles2> TransportVehicles { get; internal set; }
        //public List<HostelBuilding> HostelBuildings { get; internal set; }
        //public List<HostelFloor> HostelFloors { get; internal set; }
        //public List<HostelRoom> HostelRooms { get; internal set; }
        //public List<HostelBed> HostelBeds { get; internal set; }

        public class FormVM
        {
            public long FormId { get; set; }
            public bool IsChecked { get; set; }
            public string FormName { get; set; }
        }

        public class CredentialVM
        {
            [Required(ErrorMessage = "This field is required.")]
            public string Server { get; set; }
            [Required(ErrorMessage = "This field is required.")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "This field is required.")]
            public string Password { get; set; }
            [Required(ErrorMessage = "This field is required.")]
            public string Database { get; set; }
        }

        public class OldInfo
        {
            public short SettingId { get; set; }

            public string CompanyName { get; set; }

            public string CompanyAddress { get; set; }

            public string Organization { get; set; }

            public string Phone { get; set; }

            public byte[] Logo { get; set; }

            public byte[] LogoFull { get; set; }

            public byte[] WebLogoMini { get; set; }

            public byte[] WebLogoFull { get; set; }

            public string Email { get; set; }

            public string RegPrefix { get; set; }

            public string Website { get; set; }

            public string ProductKey { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public string Facebook { get; set; }

            public string ProfileFormat { get; set; }

            public string ShortName { get; set; }

            public decimal GST { get; set; }

            public string Fax { get; set; }

            public bool? ApplyGST { get; set; }

            public string NTN { get; set; }

            public string GSTN { get; set; }

            public decimal? KOTGST { get; set; }

            public DateTime? ModifiedOn { get; set; }

            public bool ShareHR { get; set; }

        }

        public class OldUser
        {
            public int UserID { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            public string Email { get; set; }

            public bool IsEmailVerified { get; set; }

            public string MobileNo { get; set; }

            public bool IsMobileVerified { get; set; }

            public int UserGroupId { get; set; }

            public DateTime? ExpiresOn { get; set; }

            public bool isActive { get; set; }

            public DateTime? LastLoginDate { get; set; }

            public DateTime? LastActivityDate { get; set; }

            public DateTime? LastPasswordChangedDate { get; set; }

            public short? FailedPasswordAttemptCount { get; set; }

            public DateTime CreationDate { get; set; }

            public int? CreatedBy { get; set; }

            public DateTime ModifiedDate { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public string Theme { get; set; }

            public int? NewId { get; set; }

            public string ThemeColor { get; set; }

            public DateTime? CustomDateTime { get; set; }

            public string LoginAllowedIPs { get; set; }

            public TimeSpan? LoginFromTime { get; set; }

            public TimeSpan? LoginToTime { get; set; }

            public bool Monday { get; set; }

            public bool Tuesday { get; set; }

            public bool Wednesday { get; set; }

            public bool Thursday { get; set; }

            public bool Friday { get; set; }

            public bool Saturday { get; set; }

            public bool Sunday { get; set; }

            public byte[] Photo { get; set; }

        }

        public class OldUserGroup
        {
            public int UserGroupId { get; set; }

            public string UserGroupName { get; set; }

            public short BranchId { get; set; }

            public bool ByDefault { get; set; }

            public int? DashboardId { get; set; }

            public bool Hidden { get; set; }
        }

        public class OldBranch
        {
            public short BranchId { get; set; }

            public string Name { get; set; }

            public short BranchCode { get; set; }

            public string AddressLine1 { get; set; }

            public string AddressLine2 { get; set; }

            public short? CityId { get; set; }

            public short? CountryId { get; set; }

            public string PhoneNumber { get; set; }

            public string EmailAddress { get; set; }

            public string RegPrefix { get; set; }

            public short SettingId { get; set; }

            public TimeSpan? BranchStartingTime { get; set; }

            public TimeSpan? BranchEndingTime { get; set; }

            public short CurrencyId { get; set; }

            public short? StateId { get; set; }

            public bool HasBiometricAttendance { get; set; }

            public short? WarehouseId { get; set; }

            public bool AppendFineToNextVoucher { get; set; }

            public bool FillVoucherByAllFeeTypes { get; set; }

            public bool MergeUnpaidVoucher { get; set; }

            public decimal UnpaidVoucherFine { get; set; }

            public bool ShowVoucherStatus { get; set; }

            public bool AllowMultipleVouchersInAMonth { get; set; }

            public string VoucherNote { get; set; }

            public string VoucherHeaderText { get; set; }

            public bool? HasStudentAttendance { get; set; }

            public byte[] Logo { get; set; }

            public byte[] LogoFull { get; set; }

            public byte[] WebLogoMini { get; set; }

            public byte[] WebLogoFull { get; set; }

            public byte[] FeeVoucherRepLogoFull { get; set; }

            public byte[] FeeVoucherWebLogoFull { get; set; }

            //public UNKNOWN_geography Location { get; set; }

            public double? Longitude { get; set; }

            public double? Latitude { get; set; }

            public double? RadiusInMeter { get; set; }

        }

        public class OldUserBranch
        {
            public int UserBranchId { get; set; }

            public int UserId { get; set; }

            public short BranchId { get; set; }

        }

        public class OldAccount
        {
            public string autokey { get; set; }

            public string TITLE { get; set; }

            public long ACCOUNT_ID { get; set; }

            public short? BranchId { get; set; }

            public string ParentAccountId { get; set; }

            public long? ParentId { get; set; }

            public string LINEAGE { get; set; }

            public short? DEPTH { get; set; }

            public string AccountCode { get; set; }

            public bool? Credit { get; set; }

            public decimal? OpeningBalance { get; set; }

            public DateTime? OBDate { get; set; }

            public long? OBVoucherId { get; set; }

            public bool CONTROLACCOUNT { get; set; }

            public bool ISTRANSACTION { get; set; }

            public short? CurrencyId { get; set; }

            public decimal? ExchangeRate { get; set; }

            public bool BYDEFAULT { get; set; }

            public string DESCN { get; set; }

            public bool? DBSTATUS { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public long? Prev__ID { get; set; }

            public long? OldAccountCode { get; set; }

            public bool IsLocked { get; set; }

            public byte GroupNo { get; set; }

            public short ControlAccountRef { get; set; }

            public byte? StatementSetupId { get; set; }

            public long? AccountGroupId { get; set; }

            public string GlobalAccountId { get; set; }

        }

        public class OldStatementSetup
        {
            public byte StatementSetupId { get; set; }

            public byte? ReportType { get; set; }

            public byte? HeadingReportOrder { get; set; }

            public byte? HeadingNature { get; set; }

            public string HeadingName { get; set; }

        }

        public class OldCurreny
        {
            public short CurrencyId { get; set; }

            public string CurrencyName { get; set; }

            public string CurrencySymbol { get; set; }
        }

        public class OldCurrencyValue
        {
            public int CurrencyValueId { get; set; }

            public short CurrencyId { get; set; }

            public decimal Value { get; set; }

            public DateTime RecordedOn { get; set; }

        }

        public class OldCostGroup
        {
            public int CostGroupId { get; set; }

            public long? CostGroupCode { get; set; }

            public string CostGroupName { get; set; }

            public short? Depth { get; set; }

            public string Lineage { get; set; }

            public byte CostGroupType { get; set; }

            public short BranchId { get; set; }

            public int? ParentId { get; set; }

            public long? ParentCode { get; set; }

        }

        public class OldFiscalYear
        {
            public int FiscalYearId { get; set; }

            public string FiscalYearName { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public bool Active { get; set; }

            public bool Closed { get; set; }

            public short BranchId { get; set; }

            public long? Prev__ID { get; set; }
        }

        public class OldDesignations
        {
            public short DesignationId { get; set; }

            public string DesignationName { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public long? Prev__ID { get; set; }

        }

        public class OldDepartment
        {
            public short DepartmentId { get; set; }

            public string DepartmentName { get; set; }

            public TimeSpan? TimeIn { get; set; }

            public TimeSpan? TimeOut { get; set; }

            public decimal? WorkingHours { get; set; }

            public byte? GracePeriod { get; set; }

            public DateTime? CreatedOn { get; set; }

            public int? CreatedBy { get; set; }

            public string CreatedByIP { get; set; }

            public DateTime? ModifiedOn { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

        }

        public class OldShift
        {
            public int ShiftId { get; set; }

            public string ShiftName { get; set; }

            public TimeSpan StartTime { get; set; }

            public TimeSpan EndTime { get; set; }

            public decimal? Duration { get; set; }

            public bool ByDefault { get; set; }

            public short? BranchId { get; set; }
        }

        public class OldEmployee
        {
            public int EmployeeId { get; set; }

            public string EmpName { get; set; }

            public string RegNo { get; set; }

            public string FatherName { get; set; }

            public DateTime? DOB { get; set; }

            public string NIC { get; set; }

            public string Mobile { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public short? NationalityId { get; set; }

            public bool? Gender { get; set; }

            public short? ReligionId { get; set; }

            public bool? Active { get; set; }

            public DateTime? AppointmentDate { get; set; }

            public DateTime? DOJ { get; set; }

            public short? DepartmentId { get; set; }

            public short? DesignationId { get; set; }

            public short? CountryId { get; set; }

            public short? StateId { get; set; }

            public short? CityId { get; set; }

            public string Address { get; set; }

            public int? SupervisorId { get; set; }

            public int? DeptSupervisorId { get; set; }

            public string MaritalStatus { get; set; }

            public DateTime? ApplicationDate { get; set; }

            public string ApplicationStatus { get; set; }

            public byte[] Photo { get; set; }

            public DateTime? LeavingDate { get; set; }

            public short BranchId { get; set; }

            public string ReasonForLeaving { get; set; }

            public string LeavingType { get; set; }

            public string EmployeeAccountId { get; set; }

            public decimal? Salary { get; set; }

            public string PassportNo { get; set; }

            public string BankName { get; set; }

            public string BankAccount { get; set; }

            public string PersonalEmailAddress { get; set; }

            public string BloodGroup { get; set; }

            public string EmergencyContactNumber { get; set; }

            public string EOBINumber { get; set; }

            public string TakafulInsuranceNumber { get; set; }

            public DateTime? DateCreated { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public int? NewId { get; set; }

            public int? OldId { get; set; }

            public int? ShiftId { get; set; }

            public string Disease { get; set; }

            public string Instructions { get; set; }

            public bool? MedicalProblem { get; set; }

            public bool? ChronicalMedicalProblems { get; set; }

            public string TBHistory { get; set; }

            public string DiabetesHistory { get; set; }

            public string EpilepsyHistory { get; set; }

            public string OthersHistory { get; set; }

            public string Allergies { get; set; }

            public string Medication { get; set; }

            public int? TaxSlabId { get; set; }

            public string ContractType { get; set; }

            public long? EmpNo { get; set; }

            public string ReasonForReactivate { get; set; }

            public DateTime? ReactivateDate { get; set; }

            public short? SalaryPaymentModeId { get; set; }

            public decimal SecurityAmount { get; set; }

            public string RFID { get; set; }

        }

        public class OLDCountry
        {
            public short CountryId { get; set; }

            public string CountryName { get; set; }

            public string CallingCode { get; set; }

            public string ISO3166Code { get; set; }

            public string IdCardNoVE { get; set; }

            public string MobileNoVE { get; set; }

        }

        public class OldState
        {
            public short StateId { get; set; }

            public string StateName { get; set; }

            public short CountryId { get; set; }

        }

        public class OldCity
        {
            public short CityId { get; set; }

            public string CityName { get; set; }

            public short CountryId { get; set; }

            public short CityPriority { get; set; }

            public short? StateId { get; set; }

        }

        public class OldNationality
        {
            public short NationalityId { get; set; }

            public string Nationality { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

        }

        public class OldRelation
        {
            public int RelationId { get; set; }

            public string RelationName { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

        }

        public class OldReligion
        {
            public short ReligionId { get; set; }

            public string ReligionName { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

        }

        public class OldDocumentType
        {
            public short DocumentTypeId { get; set; }

            public string DocumentTypeName { get; set; }

            public string DocumentTypeFor { get; set; }

        }

        public class OldProfession
        {
            public short ProfessionId { get; set; }

            public string ProfessionName { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public long? Prev__ID { get; set; }

        }

        public class OldRegion
        {
            public int RegionId { get; set; }

            public string RegionName { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

        }

        public class OldTaxSlab
        {
            public int TaxSlabId { get; set; }

            public string TaxSlabName { get; set; }

            public decimal FromAmount { get; set; }

            public decimal? ToAmount { get; set; }

            public decimal FixedAmount { get; set; }

            public decimal TaxPercentage { get; set; }

            public int FiscalYearId { get; set; }
        }

        public class OldUSERINFO
        {
            public int USERID { get; set; }

            public string BADGENUMBER { get; set; }

            public string SSN { get; set; }

            public string NAME { get; set; }

            public string GENDER { get; set; }

            public string TITLE { get; set; }

            public string PAGER { get; set; }

            public DateTime? BIRTHDAY { get; set; }

            public DateTime? HIREDDAY { get; set; }

            public string STREET { get; set; }

            public string CITY { get; set; }

            public string STATE { get; set; }

            public string ZIP { get; set; }

            public string OPHONE { get; set; }

            public string FPHONE { get; set; }

            public short? VERIFICATIONMETHOD { get; set; }

            public short? DEFAULTDEPTID { get; set; }

            public short? SECURITYFLAGS { get; set; }

            public short ATT { get; set; }

            public short INLATE { get; set; }

            public short OUTEARLY { get; set; }

            public short OVERTIME { get; set; }

            public short SEP { get; set; }

            public short HOLIDAY { get; set; }

            public string MINZU { get; set; }

            public string PASSWORD { get; set; }

            public short LUNCHDURATION { get; set; }

            public string MVerifyPass { get; set; }

            public byte[] PHOTO { get; set; }

            public byte[] Notes { get; set; }

            public int? privilege { get; set; }

            public short? InheritDeptSch { get; set; }

            public short? InheritDeptSchClass { get; set; }

            public short? AutoSchPlan { get; set; }

            public int? MinAutoSchInterval { get; set; }

            public short? RegisterOT { get; set; }

            public short? InheritDeptRule { get; set; }

            public short? EMPRIVILEGE { get; set; }

            public string CardNo { get; set; }

            public int? ModifiedBy { get; set; }

            public string IP { get; set; }

            public short? DEFAULTDEPTIDParent { get; set; }

        }

        public class OldEmployeeAttendance
        {
            public Guid EAID { get; set; }
            public int EmployeeId { get; set; }
            public DateTime AttendanceDate { get; set; }
            public string AttendanceStatus { get; set; }
            public TimeSpan? Arrival { get; set; }
            public TimeSpan? BreakOut { get; set; }
            public TimeSpan? BreakIn { get; set; }
            public TimeSpan? Departure { get; set; }
            public bool Leave { get; set; }
            public short? LeaveTypeId { get; set; }
            public int? PlacementId { get; set; }
            public decimal? Hours { get; set; }
            public bool Holiday { get; set; }
            public decimal? OverTime { get; set; }
            public decimal? Shortage { get; set; }
            public decimal DailyHours { get; set; }
            public short? DepartmentId { get; set; }
            public short? DesignationId { get; set; }
            public short BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public bool? HalfDay { get; set; }
            public long? Prev__ID { get; set; }
            public decimal? LeaveAmount { get; set; }
            public bool HalfDayLeave { get; set; }
            public bool Present { get; set; }
            public short GracePeriod { get; set; }
            public TimeSpan? ShiftTimeIn { get; set; }
            public TimeSpan? ShiftTimeOut { get; set; }
            public int? ShiftId { get; set; }
            public long Version { get; set; }
            public string Remarks { get; set; }
            public bool ArrivalSMSStatus { get; set; }
            public bool DepartureSMSStatus { get; set; }
        }

        public class OldLeaveTypes
        {
            public short id { get; set; }
            public string LeaveType { get; set; }
            public bool? PaidLeave { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
        }

        public class OldPlacementTypes
        {
            public short PlacementTypeId { get; set; }
            public string PlacementTypeName { get; set; }
            public string Action { get; set; }
        }

        public class OldCHECKINOUT
        {
            public int USERID { get; set; }
            public DateTime CHECKTIME { get; set; }
            public string CHECKTYPE { get; set; }
            public int? VERIFYCODE { get; set; }
            public string SENSORID { get; set; }
            public string Memoinfo { get; set; }
            public int? WorkCode { get; set; }
            public string sn { get; set; }
            public short? UserExtFmt { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long ID { get; set; }
            public bool? Processed { get; set; }
        }

        public class OldSalary
        {
            public Guid SalaryId { get; set; }
            public DateTime SalaryMonth { get; set; }
            public DateTime IssueDate { get; set; }
            public long? VoucherId { get; set; }
            public short BranchId { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public int? FiscalYearId { get; set; }
        }

        public class OldSalaryDetail
        {
            public Guid SalaryDetailId { get; set; }
            public Guid SalaryId { get; set; }
            public int EmployeeId { get; set; }
            public decimal Salary { get; set; }
            public bool Posted { get; set; }
            public long? VoucherDetailId { get; set; }
            public string VoucherType { get; set; }
            public decimal? TotalGross { get; set; }
            public decimal? TotalDebit { get; set; }
            public decimal? TotalCredit { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public decimal PaidSalary { get; set; }
        }

        public class OldPayment
        {
            public Guid PaymentId { get; set; }
            public DateTime PaymentDate { get; set; }
            public string AccountId { get; set; }
            public string ChequeNo { get; set; }
            public string Particulars { get; set; }
            public Guid SalaryDetailId { get; set; }
            public decimal PaidAmount { get; set; }
            public bool Posted { get; set; }
            public long? VoucherId { get; set; }
        }

        public class OldSession
        {
            public Guid SessionId { get; set; }
            public string SessionName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool Active { get; set; }
            public short BranchId { get; set; }
            public string UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldStage
        {
            public Guid StageId { get; set; }
            public string StageName { get; set; }
            public short? BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public short StageOrder { get; set; }
        }

        public class OldClass
        {
            public Guid ClassId { get; set; }
            public string ClassName { get; set; }
            public short ClassOrder { get; set; }
            public Guid StageId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public bool Active { get; set; }
            public int? GroupCapacity { get; set; }
            public int? ClassCapacity { get; set; }
            public string ShortName { get; set; }
        }

        public class OldSection
        {
            public Guid SectionId { get; set; }
            public string SectionName { get; set; }
            public bool Active { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
        }

        public class OldGroup
        {
            public Guid GroupId { get; set; }
            public string GroupName { get; set; }
            public Guid SessionId { get; set; }
            public Guid ClassId { get; set; }
            public Guid SectionId { get; set; }
            public short BranchId { get; set; }
            public int? EmployeeId { get; set; }
            public bool Active { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public string CreatedByIP { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public int? Capacity { get; set; }
            public string GroupSessionName { get; set; }
            public string GroupClassName { get; set; }
            public string GroupSectionName { get; set; }
            public short? GroupClassOrder { get; set; }
            public short? GroupStageOrder { get; set; }
            public string GroupBranchName { get; set; }
            public Guid? GroupStageId { get; set; }
            public string GroupStageName { get; set; }
        }

        public class OldSubject
        {
            public Guid ExamSubjectId { get; set; }
            public string SubjectName { get; set; }
            public string SubjectCode { get; set; }
            public Guid? StageId { get; set; }
            public short? BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public string ShortFormSubject { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldSubsubject
        {
            public Guid ExamSubSubjectId { get; set; }
            public string Name { get; set; }
            public string ShortForm { get; set; }
            public short? BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldTerm
        {
            public Guid ExamTermId { get; set; }
            public string TermName { get; set; }
            public Guid SessionId { get; set; }
            public Guid StageId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Weightage { get; set; }
            public short BranchId { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public string CreatedByIP { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldType
        {
            public Guid ExamTypeId { get; set; }
            public string ExamType { get; set; }
            public short DisplayPriority { get; set; }
            public bool IsLocked { get; set; }
            public short? BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldResult
        {
            public Guid ExamId { get; set; }
            public Guid GroupId { get; set; }
            public Guid ExamTermId { get; set; }
            public Guid ExamTypeId { get; set; }
            public Guid ExamSubjectId { get; set; }
            public Guid SubSubjectId { get; set; }
            public DateTime ExamDate { get; set; }
            public decimal MaxMarks { get; set; }
            public bool Published { get; set; }
            public DateTime? DeclarationDate { get; set; }
            public decimal? HighestPercentage { get; set; }
            public decimal? LowestPercentage { get; set; }
            public short? ExamNo { get; set; }
            public string Remarks { get; set; }
            public int? TeacherId { get; set; }
            public Guid? TopicId { get; set; }
            public bool? IncludeInFinalTerm { get; set; }
            public short? Status { get; set; }
            public decimal SubjectPassMarks { get; set; }
            public decimal SubjectPassMarksPercentage { get; set; }
            public decimal MarksPercentage { get; set; }
            public decimal PassPercentage { get; set; }
            public string TermName { get; set; }
            public string TypeName { get; set; }
            public string SubjectName { get; set; }
            public string SubSubjectName { get; set; }
            public string BranchName { get; set; }
            public string SubjectCode { get; set; }
            public Guid? OldExamTermId { get; set; }
            public Guid? OldExamSubjectId { get; set; }
        }

        public class OldResultItem
        {
            public Guid ExamDetailId { get; set; }
            public Guid ExamId { get; set; }
            public int StudentId { get; set; }
            public string RegistrationNumber { get; set; }
            public decimal ObtainedMarks { get; set; }
            public decimal? RIMaxMarks { get; set; }
            public string AttendanceStatus { get; set; }
            public string ResultStatus { get; set; }
            public string Remarks { get; set; }
            public bool? Itr { get; set; }
            public decimal? Percentage { get; set; }
            public string Grade { get; set; }
            public short? Position { get; set; }
            public string SubjectGrade { get; set; }
            public string GradeRemarks { get; set; }
            public string SubjectGradeRemarks { get; set; }
            public bool IsPass { get; set; }
            public string StudentName { get; set; }
            public string StudentRegNo { get; set; }
            public string GrNo { get; set; }
            public string FatherName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
            public int? ClassRollNo { get; set; }
        }

        public class OldStudent
        {
            public int StudentId { get; set; }
            public string FullName { get; set; }
            public short BranchId { get; set; }
            public string ProfileId { get; set; }
            public string RegistrationNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string PlaceOfBirth { get; set; }
            public string Gender { get; set; }
            public short? ReligionId { get; set; }
            public string PhoneNo { get; set; }
            public short? CountryId { get; set; }
            public short? StateId { get; set; }
            public short? CityId { get; set; }
            public string Address { get; set; }
            public string MobileNumber { get; set; }
            public DateTime? RegistrationDate { get; set; }
            public Guid? RequestedClassId { get; set; }
            public string LastSchoolAttended { get; set; }
            public bool? Active { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string FatherName { get; set; }
            public string FatherLastName { get; set; }
            public string FatherMobileNumber { get; set; }
            public string FatherEmail { get; set; }
            public string FatherIdNumber { get; set; }
            public string FatherQualification { get; set; }
            public short? FatherProfessionId { get; set; }
            public decimal? FatherAnnualIncome { get; set; }
            public string MotherName { get; set; }
            public string MotherLastName { get; set; }
            public string MotherMobileNumber { get; set; }
            public string MotherEmail { get; set; }
            public string MotherIdNumber { get; set; }
            public string MotherQualification { get; set; }
            public short? MotherProfessionId { get; set; }
            public bool? IsOrphan { get; set; }
            public string OrphanId { get; set; }
            public string GuardianName { get; set; }
            public string GuardianLastName { get; set; }
            public string GuardianMobileNumber { get; set; }
            public string GuardianEmail { get; set; }
            public string GuardianIdNumber { get; set; }
            public string GuardianQualification { get; set; }
            public short? GuardianProfessionId { get; set; }
            public string GRNo { get; set; }
            public Guid? AssignedClassId { get; set; }
            public Guid? AssignedSectionId { get; set; }
            public int? RollNumber { get; set; }
            public Guid? FeeDiscountId { get; set; }
            public string OldRegistrationNumber { get; set; }
            public byte[] Photo { get; set; }
            public string BloodGroup { get; set; }
            public string Disease { get; set; }
            public string Instructions { get; set; }
            public bool? MedicalProblem { get; set; }
            public bool? ChronicalMedicalProblems { get; set; }
            public string TBHistory { get; set; }
            public string DiabetesHistory { get; set; }
            public string EpilespsyHistory { get; set; }
            public string OthersHistory { get; set; }
            public string Allergies { get; set; }
            public string Medication { get; set; }
            public int? GaurdianRelationId { get; set; }
            public string Hostel { get; set; }
            public string StudentName { get; set; }
            public string FahterName { get; set; }
            public int? NewId { get; set; }
            public DateTime? FeeDiscountValidaty { get; set; }
            public byte[] Version { get; set; }
            public decimal? StudentSecurityFeeAmount { get; set; }
            public long? StudentSecurityFeeVoucherId { get; set; }
            public long? EmployeeId { get; set; }
            public bool StaffChild { get; set; }
            public int? HouseTypeId { get; set; }
            public string StudentIdNo { get; set; }
            public int? FeeGroupId { get; set; }
            public DateTime? FeeDiscountValidatyFrom { get; set; }
            public string FeeDiscountRemarks { get; set; }
            public string CurrentSessionName { get; set; }
            public string CurrentClassName { get; set; }
            public string CurrentSectionName { get; set; }
            public bool SendPresentSMSAuto { get; set; }
            public bool SendAbsentSMSAuto { get; set; }
            public string FatherOccupation { get; set; }
            public string StaffChildType { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public byte CreationMethod { get; set; }
            public DateTime? SchoolLeavingDate { get; set; }
            public int? DeactivationTypeId { get; set; }
            public string ReasonForLeaving { get; set; }
            public int? OldId { get; set; }
            public string VehicleRegNo { get; set; }
        }

        public class OldStudentSession
        {
            public Guid StudentSessionId { get; set; }
            public int StudentId { get; set; }
            public Guid GroupId { get; set; }
            public int RollNumber { get; set; }
            public string Type { get; set; }
            public DateTime DateOfAssignment { get; set; }
            public DateTime? DateOfDeactivation { get; set; }
            public bool Active { get; set; }
            public decimal? OpeningBalance { get; set; }
            public DateTime? OpeningBalanceDate { get; set; }
            public bool OpeningBalanceAdded { get; set; }
            public decimal PendingFine { get; set; }
            public DateTime? PendingFineMonth { get; set; }
            public short BranchId { get; set; }
            public int? AdmissionNo { get; set; }
            public Guid SessionId { get; set; }
            public Guid ClassId { get; set; }
            public Guid SectionId { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string CreatedByIP { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public int? UserLogID { get; set; }
            public long? Prev__ID { get; set; }
            public string RFID { get; set; }
        }

        public class OldStudentAdmission
        {
            public Guid AdmissionId { get; set; }
            public int StudentId { get; set; }
            public Guid? SessionId { get; set; }
            public Guid ClassId { get; set; }
            public short BranchId { get; set; }
            public string RegistrationNo { get; set; }
            public DateTime AdmissionDate { get; set; }
            public bool Active { get; set; }
            public DateTime? LeaveDate { get; set; }
            public string LeaveReason { get; set; }
            public Guid? GroupId { get; set; }
            public int? RollNo { get; set; }
            public string OldRegistrationNo { get; set; }
            public string Password { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string CreatedByIP { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public int? DeactivationTypeId { get; set; }
            public int? Method { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldExamDateSheet
        {
            public Guid DateSheetId { get; set; }
            public Guid SessionId { get; set; }
            public Guid ClassId { get; set; }
            public Guid SectionId { get; set; }
            public Guid TermId { get; set; }
            public Guid ExamTypeId { get; set; }
            public DateTime ExamDate { get; set; }
            public Guid SubjectId { get; set; }
            public Guid SubSubjectId { get; set; }
            public short RoomId { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public Guid? ExamPaperId { get; set; }
            public short BranchId { get; set; }
            public string SubjectName { get; set; }
            public string SubSubjectName { get; set; }
            public string TermName { get; set; }
            public string TypeName { get; set; }
            public string SessionName { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }
            public string RoomName { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public string CreatedByIP { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public Guid? OldTermId { get; set; }
            public Guid? OldSubjectId { get; set; }
        }

        public class OldFeeHead
        {
            public Guid FeeTypeId { get; set; }
            public string FeeTypeName { get; set; }
            public string FeeTypeNameStyled { get; set; }
            public short? BranchId { get; set; }
            public string AccountId { get; set; }
            public string Repeat { get; set; }
            public bool? Refundable { get; set; }
            public bool Discountable { get; set; }
            public int? UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public int? Priority { get; set; }
            public long? Prev__ID { get; set; }
            public bool? IsLocked { get; set; }
            public int? OldAccountId { get; set; }
            public bool ShowInSiblings { get; set; }
            public bool Fineable { get; set; }
            public string Months { get; set; }
            public decimal? Royalty { get; set; }
            public decimal? MinAmount { get; set; }
            public decimal? MaxAmount { get; set; }
            public Guid? GlobalFeeHeadId { get; set; }
        }

        public class OldDiscount
        {
            public Guid AccountsFeeDiscountId { get; set; }
            public string DiscountNameStyled { get; set; }
            public string DiscountName { get; set; }
            public decimal DiscountRate { get; set; }
            public string Type { get; set; }
            public short BranchId { get; set; }
            public int? UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public bool ByDefault { get; set; }
        }

        public class OldLateFine
        {
            public Guid LateFeeFineId { get; set; }
            public decimal FineRatio { get; set; }
            public string RatioType { get; set; }
            public short Frequency { get; set; }
            public short ForNoOfDays { get; set; }
            public short BranchId { get; set; }
            public int? UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public string FineType { get; set; }
        }

        public class OldFeeSetting
        {
            public short BranchId { get; set; }
            public bool PrintZeroAmountVoucher { get; set; }
            public bool AppendFineToNextVoucher { get; set; }
            public bool FillVoucherByAllFeeTypes { get; set; }
            public bool MergeUnpaidVoucher { get; set; }
            public bool ShowVoucherStatus { get; set; }
            public bool AllowMultipleVouchersInAMonth { get; set; }
            public string VoucherNote { get; set; }
            public string VoucherHeaderText { get; set; }
            public bool PartiallyPaidInDefaulterList { get; set; }
            public bool ShowPrevBalanceInVoucherPrint { get; set; }
            public bool ApplyFineAutomatically { get; set; }
            public bool MonthwiseFinancialVoucher { get; set; }
            public decimal UnpaidVoucherFine { get; set; }
            public string UnpaidVoucherFineRatioType { get; set; }
            public byte IssueDay { get; set; }
            public byte DueDay { get; set; }
            public byte ValidityDay { get; set; }
            public byte AutoCreationDay { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public string AdmissionVoucherNote { get; set; }
        }

        public class OldFeeVoucher
        {
            public long FeeVoucherId { get; set; }
            public int StudentId { get; set; }
            public Guid GroupId { get; set; }
            public DateTime IssueDate { get; set; }
            public DateTime DueDate { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal FineAmount { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal? NetAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal? PayableAmount { get; set; }
            public string VoucherStatus { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string IP { get; set; }
            public short BranchId { get; set; }
            public string AccVoucherId { get; set; }
            public string DiscountAccVoucherId { get; set; }
            public DateTime? LastPaymentOn { get; set; }
            public DateTime? ValidityDate { get; set; }
            public string Duration { get; set; }
            public bool IsCancelled { get; set; }
            public bool Viewed { get; set; }
            public long? OldVoucherId { get; set; }
            public long? NewVoucherId { get; set; }
            public bool? EmailSent { get; set; }
            public bool? VoucherEntry { get; set; }
            public byte[] HBC { get; set; }
            public string DiscountRemarks { get; set; }
            public DateTime? CancelledOn { get; set; }
            public long? CancelledVoucherId { get; set; }
            public int? Batch { get; set; }
            public string Note { get; set; }
            public Guid? DiscountId { get; set; }
            public string DiscountName { get; set; }
            public Guid? SessionId { get; set; }
            public Guid? ClassId { get; set; }
            public Guid? SectionId { get; set; }
            public string StudentName { get; set; }
            public string RegNo { get; set; }
            public string GRNo { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }
            public DateTime? ExtendedDueDate { get; set; }
            public string DefaulterRemarks { get; set; }
            public string StudentFatherName { get; set; }
            public string StudentFatherIdNumber { get; set; }
            public bool IsPosted { get; set; }
            public DateTime? FirstPaymentOn { get; set; }
            public bool AutoPost { get; set; }
            public int? AutoPostUserId { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
        }

        public class OldFeeVoucherDetail
        {
            public Guid FeeVoucherDetailsID { get; set; }
            public long FeeVoucherId { get; set; }
            public Guid FeeTypeId { get; set; }
            public DateTime FeeMonth { get; set; }
            public decimal Amount { get; set; }
            public decimal Discount { get; set; }
            public decimal Paid { get; set; }
            public string AccVoucherId { get; set; }
            public int? ModifiedBy { get; set; }
            public int? StudentId { get; set; }
            public bool IsCancelled { get; set; }
            public decimal? ActualFee { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public long? VoucherId { get; set; }
            public long? AccVoucherItemId { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public long? PrevVoucherId { get; set; }
            public string FVIStudentName { get; set; }
            public short? FVIBranchId { get; set; }
        }

        public class OldReceipt
        {
            public long FeeReceiptId { get; set; }
            public long FeeVoucherId { get; set; }
            public decimal TotalAmount { get; set; }
            public DateTime ReceivedOn { get; set; }
            public short PaymentModeId { get; set; }
            public string ChequeNumber { get; set; }
            public DateTime? ChequeDate { get; set; }
            public string AccountId { get; set; }
            public long? VoucherId { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public short? BranchId { get; set; }
            public Guid? SessionId { get; set; }
            public int? StudentId { get; set; }
            public Guid? GroupId { get; set; }
            public string GroupName { get; set; }
            public string VoucherDuration { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
        }

        public class OldReceiptItem
        {
            public long FeeReceiptItemId { get; set; }
            public long FeeReceiptId { get; set; }
            public Guid? FeeVoucherItemId { get; set; }
            public decimal Amount { get; set; }
            public long? VoucherDetailId { get; set; }
            public Guid? FeeTypeId { get; set; }
            public string FeeTypeName { get; set; }
            public int? FeeTypePriority { get; set; }
        }

        public class OldStructure
        {
            public Guid FeeStructureId { get; set; }
            public Guid SessionId { get; set; }
            public Guid ClassId { get; set; }
            public Guid SectionId { get; set; }
            public Guid FeeTypeId { get; set; }
            public decimal Amount { get; set; }
            public bool Active { get; set; }
            public short BranchId { get; set; }
            public int? UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldStudentStructure
        {
            public long StudentStructureId { get; set; }
            public Guid StudentSessionId { get; set; }
            public Guid FeeTypeId { get; set; }
            public DateTime FeeMonth { get; set; }
            public decimal ActualFee { get; set; }
            public decimal? DiscountedFee { get; set; }
            public decimal? ModifiedFee { get; set; }
            public decimal? InstallmentFee { get; set; }
            public short BranchId { get; set; }
            public decimal? DiscountAmount { get; set; }
            public long? FeeVoucherId { get; set; }
            public int? StudentId { get; set; }
        }

        public class OldVoucher
        {
            public long VoucherId { get; set; }
            public string VoucherType { get; set; }
            public string VoucherName { get; set; }
            public DateTime TransactionDate { get; set; }
            public decimal DebitAmount { get; set; }
            public decimal CreditAmount { get; set; }
            public decimal? Balance { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string Particulars { get; set; }
            public string VoucherStatus { get; set; }
            public bool IsApproved { get; set; }
            public int? ApprovedBy { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public bool IsCancelled { get; set; }
            public int? CancelledBy { get; set; }
            public short? CurrencyId { get; set; }
            public decimal? ExchangeRate { get; set; }
            public short BranchId { get; set; }
            public string CBAccountId { get; set; }
            public int? ProjectId { get; set; }
            public string IP { get; set; }
            public DateTime? PostedOn { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int FiscalYearId { get; set; }
            public long? TFeeVoucherId { get; set; }
            public DateTime? TFeeMonth { get; set; }
            public long? TFeeReceiptId { get; set; }
        }
        public class OldVoucherDetail
        {
            public long VoucherDetailId { get; set; }
            public long VoucherId { get; set; }
            public short TransactionId { get; set; }
            public string AccountId { get; set; }
            public string TransactionType { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public string ChequeNo { get; set; }
            public DateTime? ChequeDate { get; set; }
            public DateTime? ChequeClearDate { get; set; }
            public string Narration { get; set; }
            public int? CostGroupId { get; set; }
            public string VoucherTypeABBR { get; set; }
            public short? VDBranchId { get; set; }
            public long? TIFeeVoucherId { get; set; }
            public DateTime? TIFeeMonth { get; set; }
            public Guid? TIFeeTypeId { get; set; }
        }

        public class OldVoucherType
        {
            public string VoucherTypeId { get; set; }
            public string VoucherTypeName { get; set; }
            public string VoucherTypeNo { get; set; }
            public bool AutoPost { get; set; }
        }

        public class OldStudentAttendance
        {
            public Guid EAID { get; set; }
            public int StudentId { get; set; }
            public DateTime AttendanceDate { get; set; }
            public string AttendanceStatus { get; set; }
            public Guid GroupId { get; set; }
            public TimeSpan? Arrival { get; set; }
            public TimeSpan? Departure { get; set; }
            public bool? HalfDay { get; set; }
            public string ArrivalSMSStatus { get; set; }
            public string DepartureSMSStatus { get; set; }
            public string Remarks { get; set; }
            public short? BranchId { get; set; }
            public Guid? TermId { get; set; }
        }

        public class OldYearlyBalance
        {
            public int YearlyBalanceId { get; set; }
            public int FiscalYearId { get; set; }
            public string AccountId { get; set; }
            public decimal OBDebitAmount { get; set; }
            public decimal OBCreditAmount { get; set; }
            public decimal TransactionDebitAmount { get; set; }
            public decimal TransactionCreditAmount { get; set; }
            public short BranchId { get; set; }
        }

        public class OldBankAccount
        {
            public string AccountId { get; set; }
            public string AccountTitle { get; set; }
            public string AccountNo { get; set; }
            public string BankName { get; set; }
            public string BranchName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
            public string PDCReceivable { get; set; }
            public string PDCPayable { get; set; }
            public int? LastChequeNo { get; set; }
            public byte? ChequeNoLength { get; set; }
        }

        public class OldEmployeeGuarantor
        {
            public int EmployeeGuarantorId { get; set; }
            public string Name { get; set; }
            public string HouseNo { get; set; }
            public string StreetAddress { get; set; }
            public byte[] Photo { get; set; }
            public string IDCardNo { get; set; }
            public string Gender { get; set; }
            public string Mobile { get; set; }
            public string Phone { get; set; }
            public DateTime ModifiedOn { get; set; }
            public int ModifiedBy { get; set; }
            public int? EmployeeId { get; set; }
        }

        public class OldEmployeePlacement
        {
            public int PlacementId { get; set; }
            public int EmployeeId { get; set; }
            public short DepartmentId { get; set; }
            public short DesignationId { get; set; }
            public int? LocationId { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public decimal? StartingSalary { get; set; }
            public bool Active { get; set; }
            public short PlacementTypeId { get; set; }
            public string Description { get; set; }
        }
        public class OldEmployeeWorkExperience
        {
            public Guid EmployeeWorkExperienceId { get; set; }
            public int EmployeeId { get; set; }
            public string Organiztion { get; set; }
            public string Designation { get; set; }
            public string JobType { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string ReasonForLeaving { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
        }
        public class OldEmployeeEducation
        {
            public Guid EmployeeEducationId { get; set; }
            public int EmployeeId { get; set; }
            public short EducationLevelId { get; set; }
            public string InstituteName { get; set; }
            public DateTime Date { get; set; }
            public string Specialization { get; set; }
            public int? UserLogId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public string Year { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldCollectorPerson
        {
            public Guid PersonId { get; set; }
            public string PersonName { get; set; }
            public string Gender { get; set; }
            public string NIC { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public byte[] Picture { get; set; }
            public DateTime? IssueDate { get; set; }
            public bool Active { get; set; }
            public short BranchId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldStudentCollector
        {
            public Guid EntryId { get; set; }
            public int StudentId { get; set; }
            public Guid CollectorPersonId { get; set; }
            public int RelationId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
            public string RelationName { get; set; }
        }

        public class OldTransportZone
        {
            public Guid ZoneId { get; set; }
            public string ZoneName { get; set; }
            public string Description { get; set; }
            public decimal TransportFee { get; set; }
            public decimal Discount { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public short BranchId { get; set; }
        }

        public class OldTransportVillage
        {
            public Guid VillageId { get; set; }
            public string VillageName { get; set; }
            public string Description { get; set; }
            public Guid? ZoneId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public short? BranchId { get; set; }
        }

        public class OldTransportDriver
        {
            public int DriverId { get; set; }
            public string DriverName { get; set; }
            public string IDCardNo { get; set; }
            public string LicenseNo { get; set; }
            public byte[] Photo { get; set; }
            public string Mobile { get; set; }
            public bool Active { get; set; }
            public string HouseNo { get; set; }
            public string Address { get; set; }
            public short? BranchId { get; set; }
        }
        public class OldTransportBus
        {
            public Guid BusId { get; set; }
            public string BusName { get; set; }
            public string Description { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public string VanNo { get; set; }
            public long? Prev__ID { get; set; }
            public string DriverName { get; set; }
        }

        public class OldTransportVehicle
        {
            public int VehicleId { get; set; }
            public string VehicleType { get; set; }
            public string VechileRegNo { get; set; }
            public int DriverId { get; set; }
            public int BranchId { get; set; }
            public long? SeatingCapacity { get; set; }
            public long? OccupiedSeats { get; set; }
        }

        public class OldHostelBuilding
        {
            public Guid BuildingId { get; set; }
            public string BuildingName { get; set; }
            public short? BranchId { get; set; }
            public decimal? BedFee { get; set; }
            public string Type { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }
        public class OldHostelFloor
        {
            public Guid FloorId { get; set; }
            public string FloorName { get; set; }
            public Guid? BuildingId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }

        public class OldHostelRoom
        {
            public Guid RoomId { get; set; }
            public string RoomNo { get; set; }
            public Guid? FloorId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }
        public class OldHostelBed
        {
            public Guid BedId { get; set; }
            public string BedNo { get; set; }
            public Guid? RoomId { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long? Prev__ID { get; set; }
        }
        public class AccountVM
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

    }
}