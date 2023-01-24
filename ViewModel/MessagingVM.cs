using FAPP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using FAPP.ViewModel.Common;

namespace FAPP.ViewModel
{
   
   
    public class StudentViewViewModel
    {
        [Required]
        [StringLength(101)]
        public string FullName { get; set; }
        public int StudentId { get; set; }
        [StringLength(50)]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [StringLength(50)]
        [Display(Name = "Father Mobile No")]
        public string FatherMobileNo { get; set; }
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Roll#")]
        public int? RollNumber { get; set; }
        [StringLength(50)]
        public string RFID { get; set; }
    }
    public class MessagingVM
    {
        //public MessagingVM()
        //{
        //    var notificationSettingObj1 = new NotificationSetting() { SettingName = "AbsenteeSMSSetting",SMSTemplateId = new Guid("A3068E49-76A3-45BC-91C5-04927DC1B2FD"),EmailTemplateId = new Guid("A3068E49-76A3-45BC-91C5-04927DC1B2FD"),ModuleId= 1,Description= "No Description Required." };
        //    NotificationSettingsList.Add(notificationSettingObj1);
        //}
        public Template Template { get; set; }
        public NotificationSetting NotificationSetting { get; set; }
        public int? NotificationSettingId { get; set; }
        public List<NotificationSettingAdminUserVM> NotificationSettingAdminUsers { get; set; }
        public int? UserGroupId { get; set; }
        public List<SelectListItem> UserGroupsDD { get; set; }
        public List<Template> TemplateList { get; set; }
        public List<NotificationSetting> NotificationSettingsList { get; set; }
        public List<SelectListItem> SMSTemplateDD { get; set; }
        public List<SelectListItem> EmailTemplateDD { get; set; }
        public TemplateType TemplateType { get; set; }
        public List<TemplateType> TemplateTypeList { get; set; }
        public IPagedList<SmsBatchVM> SmsBatchList { get; set; }

        public List<string> ViewFields { get; set; }
        public SendMessageVM SendMessage { get; set; }
        public IPagedList<SmsQueue> SmsQuesList { get; set; }
        public List<SentSMSViewModel> SentSMSList { get; set; }
        public SmsQueue SmsQueue { get; set; }
        public Dictionary<string, int> SmsChart { get; set; }
        public SearchMessageVM SearchMessage { get; set; }
        public int? ModuleId { get; set; }
        public long? BatchNo { get; set; }
        public string ModuleName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Command { get; set; }
        private string reservationViewFeilds;
        public string GetReservationViewFeilds()
        {
            return reservationViewFeilds=$@"GuestName,ClientName,AgentName,GuestMobileNumber,ClientMobileNumber,AgentMobileNumber,ReservationId,Discount,ReservationFromDate,ReservationFromTime,ReservationToDate,ReservationToTime,NoOfRoomsRequired,AdvanceDeposit,NoOfGuests,NoOfChildren,AgentCommission,Remarks,ReservationStatus,PaymentStatus,RepresentativeName,RepresentativeEmail,RepresentativePhone,RepresentativeAddress,RepresentativeCNIC,RepresentativeVehicle,TotalAmount,PaidAmount,Balance,ModeOfPayment,CheckOutSMS,CheckInSms,RoomServiceStatus,DiscountAmount,ReservationDate";
        }

        private string reservationpayments;
        public string GetReservationPaymentsFeilds()
        {
            return reservationpayments =$@"MobileNo,InvoicePaymentId,ChequeNo,ChequeDate,Amount,Description,PaymentDate,ClientName,PaymentModeName,TotalAmount,PaidAmount,Balance,GuestName,ReservationFromDate,ReservationFromTime,ReservationToDate,ReservationToTime,ReservationId";
        }
    }
    public class NotificationSettingAdminUserVM {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class SmsBatchVM
    {
        public IEnumerable<SmsBatchVM> SmsBatchVmList { get; set; }
        public long BatchNo { get; set; }
        public DateTime BatchDate { get; set; }
        public string MessageStatus { get; set; }
        public string SMSReference { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
        public int Total { get; set; }
        public int Delivered { get; set; }
        public int Pending { get; set; }
        public int Canceled { get; set; }
        public string SentBy { get; set; }
        public virtual Template Template { get; set; }
        public virtual TemplateType TemplateType { get; set; }
    }

    public class SearchMessageVM
    {
        public short? TemplateTypeId { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
        public long? Batch { get; set; }
        public DateTime? Date { get; set; }
    }

    public class SendMessageVM
    {
        public int? DepartmentId { get; set; }
        public Guid GroupId { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public string MobNos { get; set; }
        public string NotificationTitle { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Remarks { get; set; }

        //public int? ModuledId { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? ScheduledOn { get; set; }
    }

    public class StudentNumUpdateToolViewModel
    {
        public IEnumerable<SelectListItem> GroupsDD { get; set; }
        public Guid? GroupId { get; set; }

        //public async System.Threading.Tasks.Task FillDD(Guid CurrentSessionId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        this.GroupsDD = await db.AcademicGroups
        //            .Where(s => s.SessionId == CurrentSessionId && s.BranchId == s.BranchId)
        //            .OrderBy(s => s.GroupClassOrder)
        //            .ThenBy(s => s.GroupSectionName)
        //            .Select(s => new SelectListItem
        //            {
        //                Text = s.GroupName,
        //                Value = s.GroupId.ToString()
        //            }).ToListAsync();

        //    }
        //}
        //public List<StudentViewViewModel> Students { get; set; }

        //public List<StudentViewViewModel> GetStudents(Guid? GroupId,short BranchId)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        IQueryable<StudentSession> result = db.StudentSessions.Where(u => u.BranchId == BranchId).Include("Student");
        //        if (GroupId != Guid.Empty && GroupId != null)
        //        {
        //            result = result.Where(s =>
        //                s.GroupId == GroupId
        //            ).AsQueryable();
        //        }

        //        return result.Select(v => new StudentViewViewModel
        //        {
        //            StudentId = v.Student.StudentId,
        //            FatherMobileNo = v.Student.FatherMobileNumber,
        //            FullName = v.Student.FullName,
        //            FatherName = v.Student.FatherName,
        //            RegistrationNumber = v.Student.RegistrationNumber,
        //            RollNumber = v.RollNumber,
        //            RFID = v.RFID,

        //        }).OrderBy(s => s.FullName).ToList();



        //    }
        //}

    }
}