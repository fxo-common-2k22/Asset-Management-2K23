//using System;

//namespace FAPP.dbviews
//{
//    public partial class v__Students
//    {
//        public Nullable<int> StudentId { get; set; }
//        public string RegistrationNo { get; set; }
//        public string Name { get; set; }
//        public Nullable<System.DateTime> RegistrationDate { get; set; }
//        public string SessionName { get; set; }
//        public string ClassName { get; set; }
//        public string SectionName { get; set; }
//        public Nullable<int> RollNumber { get; set; }
//        public Nullable<bool> ActiveInClass { get; set; }
//        public Nullable<System.Guid> StudentSessionId { get; set; }
//        public Nullable<System.Guid> SessionId { get; set; }
//        public Nullable<System.Guid> ClassId { get; set; }
//        public Nullable<System.Guid> SectionId { get; set; }
//        public string MobileNumber { get; set; }
//        public Nullable<System.DateTime> DateOfBirth { get; set; }
//        public string Address { get; set; }
//        public string FatherMobileNumber { get; set; }
//        public string FatherIdNumber { get; set; }
//        public string FatherEmail { get; set; }
//        public Nullable<bool> Active { get; set; }
//        public string RequestedClassName { get; set; }
//        public Nullable<System.DateTime> SchoolLeavingDate { get; set; }
//        public string ReasonForLeaving { get; set; }
//        public string MotherName { get; set; }
//        public string MotherMobileNumber { get; set; }
//        public string MotherIdNumber { get; set; }
//        public string MotherEmail { get; set; }
//        public string GuardianName { get; set; }
//        public string GuardianMobileNumber { get; set; }
//        public string GuardianIdNumber { get; set; }
//        public string GuardianEmail { get; set; }
//        public string BloodGroup { get; set; }
//        public string Type { get; set; }
//        public string BranchName { get; set; }
//        public short BranchId { get; set; }
//        public System.DateTime AdmissionDate { get; set; }
//        public bool ActiveInBranch { get; set; }
//        public Nullable<System.DateTime> DateOfAssignment { get; set; }
//        public Nullable<short> ClassOrder { get; set; }
//        public string Password { get; set; }
//        public string ProfileId { get; set; }
//        public string GRNo { get; set; }
//        public Nullable<int> GaurdianRelationId { get; set; }
//        public Nullable<bool> SessionActive { get; set; }
//        public Nullable<bool> IsOrphan { get; set; }
//        public Nullable<System.Guid> GroupId { get; set; }
//        public string School { get; set; }
//        public string Branch { get; set; }
//        public string Disease { get; set; }
//        public string Instructions { get; set; }
//        public Nullable<bool> MedicalProblem { get; set; }
//        public Nullable<bool> ChronicalMedicalProblems { get; set; }
//        public string TBHistory { get; set; }
//        public string DiabetesHistory { get; set; }
//        public string EpilespsyHistory { get; set; }
//        public string OthersHistory { get; set; }
//        public string Allergies { get; set; }
//        public string Medication { get; set; }
//        public string Email { get; set; }
//        public string FatherName { get; set; }
//        public Nullable<bool> Gender { get; set; }
//        public Nullable<decimal> FatherAnnualIncome { get; set; }
//        public string DiscountName { get; set; }
//        public Nullable<decimal> DiscountRate { get; set; }
//        public Nullable<decimal> StudentSecurityFeeAmount { get; set; }
//        public Nullable<long> StudentSecurityFeeVoucherId { get; set; }
//        public Nullable<long> EmployeeId { get; set; }
//        public bool StaffChild { get; set; }
//        public Nullable<int> HouseTypeId { get; set; }
//        public string AdmissionClass { get; set; }
//        public string RFID { get; set; }
//        public bool SendPresentSMSAuto { get; set; }
//        public bool SendAbsentSMSAuto { get; set; }
//        public string LastSchoolAttended { get; set; }
//        public string ReligionName { get; set; }
//        public byte[] Photo { get; set; }
//    }
//}