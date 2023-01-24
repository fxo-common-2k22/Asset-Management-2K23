using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common
{
    //MessagingViewModel Same
    public class SentSMSViewModel
    {
        public string Sentby { get; set; }
        public string ReceiverMobile { get; set; }
        public string MessageBody { get; set; }
        public string ScheduledOn { get; set; }
        public string ModuleName { get; set; }
        public string SentOn { get; set; }
        public string ProfileId { get; set; }
        public string MessageStatus { get; set; }
        public string Remarks { get; set; }
        public string TemplateTypeName { get; set; }
        public string StudentName { get; set; }
        public string GroupName { get; set; }
        public int SmsQueueId { get; set; }
        public int? ModuleId { get; set; }
        public long Batch { get; set; }
        public bool? DeliveredStatus { get; set; }
        public short BranchId { get; set; }
        public short? TemplateTypeId { get; set; }
        public Guid? TemplateId { get; set; }
    }
}