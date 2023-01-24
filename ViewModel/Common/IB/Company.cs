using FAPP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common.IB
{
    public class Company
    {
        public short NoOfBranchesAllowed { get; set; }
        public bool ShowExceptionToUser { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string GSTN { get; set; }
        public string NTN { get; set; }
        public bool? ApplyGST { get; set; }
        public string Fax { get; set; }
        public decimal GST { get; set; }
        public string ShortName { get; set; }
        public string ProfileFormat { get; set; }
        public string Facebook { get; set; }
        public string IP { get; set; }
        public SoftwareTypeEnum SoftwareType { get; set; }
        public int? ModifiedBy { get; set; }
        public string Website { get; set; }
        public string RegPrefix { get; set; }
        public string Email { get; set; }
        public byte[] WebLogoFull { get; set; }
        public byte[] WebLogoMini { get; set; }
        public byte[] LogoFull { get; set; }
        public byte[] Logo { get; set; }
        public string Phone { get; set; }
        public string Organization { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeAuthType { get; set; }
        public string ParentAuthType { get; set; }
        public string OwnerAuthType { get; set; }
        public short SettingId { get; set; }
        public string ProductKey { get; set; }
    }
}