using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common
{
    public class UrlSettingVM
    {
        public short UrlId { get; set; }

        [StringLength(50)]
        public string SMSGateway { get; set; }

        [StringLength(50)]
        public string SMSUsername { get; set; }

        [StringLength(50)]
        public string SMSPassword { get; set; }

        [StringLength(50)]
        public string SMSAuth { get; set; }

        public int? SMSCredit { get; set; }

        public string URL { get; set; }

        [StringLength(500)]
        public string URLEmployee { get; set; }

        [StringLength(500)]
        public string URLStudent { get; set; }

        [StringLength(500)]
        public string URLapi { get; set; }
        [StringLength(500)]
        public string URLProduct { get; set; }
        [StringLength(500)]
        public string URLKnowledgeBase { get; set; }
        [StringLength(500)]
        public string URLSupportTicket { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Host { get; set; }
        [UIHint("YesNoRadio")]
        public bool isSecure { get; set; }

        public int? Port { get; set; }

        [StringLength(500)]
        public string BCCEmails { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        [StringLength(50)]
        public string PortNumber { get; set; }

        public short BranchId { get; set; }
    }
}