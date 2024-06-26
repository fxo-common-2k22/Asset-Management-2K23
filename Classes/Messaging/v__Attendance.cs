//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAPP.Classes.Messaging.DTOS
{
    using System;
    using System.Collections.Generic;
    
    public partial class v__Attendance
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RegNo { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string GRNo { get; set; }
        public System.DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.TimeSpan> Arrival { get; set; }
        public Nullable<System.TimeSpan> Departure { get; set; }
        public Nullable<bool> HalfDay { get; set; }
    }

    public partial class v__Receipts
    {
        public long FeeVoucherId { get; set; }
        public short PaymentModeId { get; set; }
        public string ChequeNumber { get; set; }
        public string AccountId { get; set; }
        public string PaymentModeName { get; set; }
        public string FullName { get; set; }
        public string GroupName { get; set; }
        public string RegistrationNo { get; set; }
        public string AccountTitle { get; set; }
        public long FeeReceiptId { get; set; }
        public System.DateTime ReceivedOn { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string GRNo { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.Guid ClassId { get; set; }
        public System.Guid SectionId { get; set; }
        public short ClassOrder { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public System.DateTime IssueDate { get; set; }
        public short BranchId { get; set; }
        public Nullable<System.Guid> SessionId { get; set; }
        public string Name { get; set; }
        public string VoucherDuration { get; set; }
        public string FatherName { get; set; }
        public Nullable<decimal> VoucherAmount { get; set; }
        public Nullable<decimal> RemainingAmount { get; set; }
    }

    public partial class v__Clients
    {
        public int ClientId { get; set; }
        public string PresentableName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ID_Card_No { get; set; }
        public string Passport_No { get; set; }
        public string Email { get; set; }
        public Nullable<short> Nationality { get; set; }
        public Nullable<short> CountryId { get; set; }
        public Nullable<short> CityId { get; set; }
        public Nullable<int> Client_Type { get; set; }
        public string AccountId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public bool IsSupplier { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public byte[] Photo { get; set; }
        public string TITLE { get; set; }
        public string ClientGroupName { get; set; }
        public string ProfessionName { get; set; }
        public Nullable<long> AccountCode { get; set; }
        public bool IsClient { get; set; }
        public bool IsAgent { get; set; }
        public short BranchId { get; set; }
        public string Remarks { get; set; }
        public string VehicleRegistrationNo { get; set; }
        public Nullable<int> ClientTypeId { get; set; }
        public string NIC { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string ClientTypeName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public Nullable<short> ProfessionId { get; set; }
        public Nullable<short> NationalityId { get; set; }
        public string AccountTITLE { get; set; }
        public string NationalityName { get; set; }
        public Nullable<decimal> MonthlyIncome { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PermanentAddress { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> VisaExpiryDate { get; set; }
        public string WorkPhone { get; set; }
        public string WorkAddress { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string HusbandName { get; set; }
        public string FatherName { get; set; }
        public bool ByDefault { get; set; }
    }
}
