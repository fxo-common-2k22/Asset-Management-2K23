using System;

namespace FAPP.dbviews
{
    public class v__Family
    {
        public string FamilyId { get; set; }
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
        public string GuardianName { get; set; }
        public string GuardianLastName { get; set; }
        public string GuardianMobileNumber { get; set; }
        public string GuardianEmail { get; set; }
        public string GuardianIdNumber { get; set; }
        public string GuardianQualification { get; set; }
        public short? GuardianProfessionId { get; set; }
        public int? GaurdianRelationId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short BranchId { get; set; }
        public string Password { get; set; }

    }
}