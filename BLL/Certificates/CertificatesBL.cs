using FAPP.Classes.Messaging.DTOS;
using FAPP.DAL;
using FAPP.dbviews;
using FAPP.Model;
using FAPP.Service;
using FAPP.ViewModel;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FAPP.BLL.Certificates
{
    public class CertificatesBL
    {
        public async Task<int> LastIssueCertificateNumber(OneDbContext db, Certificate certificate, int? ModuleId)
        {
            if (ModuleId == 9)
            {
                var issuedCertificates = await db.EmployeeCertificates.Where(c => c.CertificateTypeId == certificate.CertificateId)
                   .ToListAsync();
                int lastNo = 0;
                if (issuedCertificates != null)
                {
                    lastNo = issuedCertificates.Count;
                }

                return lastNo;
            }
            else
            {
                var issuedCertificates = await db.StudentCertificates.Where(c => c.CertificateTypeId == certificate.CertificateId)
                    .ToListAsync();
                int lastNo = 0;
                if (issuedCertificates != null)
                {
                    lastNo = issuedCertificates.Count;
                }

                return lastNo;
            }
        }
        public void PrepareCertificate(OneDbContext db, MediaTemplatesViewModel vm, Certificate certificate, int lastNo, int item, int moduleId,Guid? StudentSessionId = null)
        {
            string issueNumFull = certificate.RefNoFormat.ToUpper();
            //+ "/" + DateTime.Now.Year.ToString().Substring(2,2) + "" + DateTime.Now.Month.ToString().PadLeft(2, '0') +   "" + issueNoLast;
            issueNumFull = issueNumFull.Replace("[YYYY]", String.Format("{0:yyyy}", DateTime.Now));
            issueNumFull = issueNumFull.Replace("[YY]", String.Format("{0:yy}", DateTime.Now));
            issueNumFull = issueNumFull.Replace("[MMMM]", String.Format("{0:MMMM}", DateTime.Now));
            issueNumFull = issueNumFull.Replace("[MMM]", String.Format("{0:MMM}", DateTime.Now));
            issueNumFull = issueNumFull.Replace("[MM]", String.Format("{0:MM}", DateTime.Now));
            issueNumFull = issueNumFull.Replace("[000000]", (lastNo).ToString().PadLeft(6, '0'));
            issueNumFull = issueNumFull.Replace("[00000]", (lastNo).ToString().PadLeft(5, '0'));
            issueNumFull = issueNumFull.Replace("[0000]", (lastNo).ToString().PadLeft(4, '0'));
            var certContent = certificate.CertificateContent;
            if (moduleId == 9)
            {
                var employeeCert = new EmployeeCertificate();
                employeeCert.EmployeeId = item;
                employeeCert.IssueNo = issueNumFull;
                CertificatesBL certificatesBL = new CertificatesBL();
                employeeCert.CertificateContent = certificatesBL.PrepareEmployeeCertificate(certificate, item);
                employeeCert.IssuedOn = vm.IssueDate;
                employeeCert.IssuedBy = SessionHelper.UserId;
                employeeCert.CertificateType = certificate.Title;
                employeeCert.CertificateTypeId = certificate.CertificateId;
                db.EmployeeCertificates.Add(employeeCert);

            }
            else
            {
                var studentCert = new StudentCertificate();
                studentCert.StudentId = db.StudentSessions.Where(s => s.StudentSessionId == StudentSessionId).Select(s => s.StudentId).FirstOrDefault();
                studentCert.IssueNo = issueNumFull;
                CertificatesBL certificatesBL = new CertificatesBL();
                studentCert.Certificate = certificatesBL.PrepareStudentCertificate(certificate, item, vm.IssueCertificate.GroupId,StudentSessionId);
                studentCert.IssuedOn = vm.IssueDate;
                studentCert.StudentSessionId = StudentSessionId;
                studentCert.GroupId = vm.IssueCertificate.GroupId;
                studentCert.IssuedBy = SessionHelper.UserId;
                studentCert.CertificateType = certificate.Title;
                studentCert.CertificateTypeId = certificate.CertificateId;
                db.StudentCertificates.Add(studentCert);
            }
        }

        public string PrepareEmployeeCertificate(Certificate cert, int id)
        {
            var content = cert.CertificateContent;
            var connection =
                System.Configuration.ConfigurationManager.
                    ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} where {2}={3}", cert.TemplateType.ViewField, cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, id);
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {

                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        if (fieldName == "Photo")
                        {
                            OneDbContext db = new OneDbContext();
                            string _sql1 = string.Format("SELECT Photo FROM {0} where {1}={2}", cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, id);
                            var imgpath = db.Database.SqlQuery<v__Students>(_sql1).FirstOrDefault();
                            string template = " <img  src=data:image;base64,{0}>";
                            string ImageSource = string.Empty;
                            if (imgpath.Photo != null)
                            {
                                ImageSource = string.Format(template, Convert.ToBase64String(imgpath.Photo));
                            }
                            fieldValue = ImageSource;
                        }
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    if (content != null)
                    {
                        content = Smart.Format(content, dict1);
                    }
                    else
                    {
                        content = Smart.Format("", dict1);

                    }
                    break;
                }
                _dr.Close();
            }
            return content;

        }

        public string PrepareStudentCertificate(Certificate cert, int id, Guid? GroupId = null,Guid? StudentSessionId = null)
        {
            var content = cert.CertificateContent;
            var connection =
                System.Configuration.ConfigurationManager.
                    ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Empty;
                if (GroupId != null && StudentSessionId != null)
                {
                     _sql = string.Format("SELECT {0} FROM {1} where StudentSessionId='{3}' AND GroupId = '{4}'", cert.TemplateType.ViewField, cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, StudentSessionId, GroupId);
                }
                else {
                     _sql = string.Format("SELECT {0} FROM {1} where {2}={3} AND ", cert.TemplateType.ViewField, cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, id);
                }
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {

                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        if (fieldName == "Photo")
                        {
                            OneDbContext db = new OneDbContext();
                            string _sql1 = string.Empty;
                            if (GroupId != null && StudentSessionId != null) {
                                 _sql1 = string.Format("SELECT Photo FROM {0} where StudentSessionId='{3}'", cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, id, StudentSessionId);

                            }
                            else
                            {
                                _sql1 = string.Format("SELECT Photo FROM {0} where {1}={2}", cert.TemplateType.TemplateView, cert.TemplateType.TemplateViewKey, id);
                            }
                            var imgpath = db.Database.SqlQuery<v__Students>(_sql1).FirstOrDefault();
                            string template = " <img  src=data:image;base64,{0}>";
                            string ImageSource = string.Empty;
                            if (imgpath.Photo != null)
                            {
                                ImageSource = string.Format(template, Convert.ToBase64String(imgpath.Photo));
                            }
                            fieldValue = ImageSource;
                        }
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    if (content != null)
                    {
                        content = Smart.Format(content, dict1);
                    }
                    else
                    {
                        content = Smart.Format("", dict1);

                    }
                    break;
                }
                _dr.Close();
            }
            return content;
        }

        public MediaTemplatesViewModel PrepareCertificateForPrint(OneDbContext db, Certificate certificate, int lastNo, int item)
        {
            var vm = new MediaTemplatesViewModel();
            CertificatesBL certificatesBL = new CertificatesBL();
            vm.PrintContent = certificatesBL.PrepareStudentCertificate(certificate, item);
            return vm;
        }
    }
}