using System;

namespace FAPP.Areas.AM.Helpers
{
    public class MessageKey
    {
        public Guid? GroupId { get; set; }
        public int? ExamTermId { get; set; }
        public int? ExamTypeId { get; set; }
    }
}