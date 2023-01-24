using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FAPP.Model;

namespace FAPP.ViewModel
{
    public class MediaTemplatesViewModel
    {
        public Certificate Certificate { get; set; }
        public string PrintContent { get; set; }
        //public StudentCertificate StudentCertificate { get; set; }
        //public List<StudentCertificate> IssuedCertificatesList { get; set; }
        public List<EmployeeCertificate> IssuedEmployeeCertificatesList { get; set; }
        public TemplateType TemplateType { get; set; }
        public List<TemplateType> TemplateTypeList { get; set; }
        public List<string> ViewFields { get; set; }
        public List<long> SelectedCertificates { get; set; }
        public IssueCertificateViewModel IssueCertificate { get; set; }
        public SearchIssuedCertificates SearchIssuedCertificates { get; set; }
        [DisplayName("Print Header")]
        [UIHint("YesNo")]
        public bool IsPrintHeader { get; set; }
        [DisplayName("Print Footer")]
        [UIHint("YesNo")]
        public bool IsPrintFooter { get; set; }
        public int ModuleId { get; set; }
        public Branch Branch { get; set; }
        [DisplayName("Issue Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime IssueDate { get; set; }
    }
    public class IssueCertificateViewModel
    {
        public Guid GroupId { get; set; }
        public List<Guid> SelectedStudentSessionIds { get; set; }
        public int? DepartmentId { get; set; }
        public int CertificateId { get; set; }
        //public List<StudentSession> Students { get; set; }
        public List<Employee> Employees { get; set; }
        public List<int> SelectedStudents { get; set; }
        public List<int> SelectedEmployees { get; set; }
        public bool IncludeInactive { get; set; }
    }
    public class StudentVM {
        public int StudentId { get; set; }
        public Guid StudentSessionId { get; set; }
    }

    public class SearchIssuedCertificates
    {
        public Guid? GroupId { get; set; }
        public short? DepartmentId { get; set; }
        public string StudentName { get; set; }
        public string EmployeeName { get; set; }
        public string IssueNo { get; set; }
        public string SearchStudent { get; set; }
        public string SearchEmployee { get; set; }
        public int? CertificateTypeId { get; set; }
        public int? RollNo { get; set; }
    }
    public class CertificatesViewModel
    {
        public string Search { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}