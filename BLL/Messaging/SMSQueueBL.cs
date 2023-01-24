using FAPP.dbviews;
using FAPP.Model;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace FAPP.BLL.Messaging
{
    public class SMSQueueBL
    {
        public long GetNextBatchNo(OneDbContext db)
        {
            var batch = db.SmsQueues.Max(q => (long?)q.Batch) ?? 0;
            batch++;
            return batch;
        }

        public void Insert(OneDbContext db, Template template, long batch, DateTime batchDate, string MobileNumber, int userId, string profileId, string Remarks, string body, short branchId, DateTime? ScheduledOn, string RefId, string Type, string ProfileType = null)
        {
            var smsQue = new SmsQueue();
            smsQue.Batch = batch;
            smsQue.BatchDate = batchDate;
            smsQue.TemplateId = template.TemplateId;
            smsQue.TemplateTypeId = template.TemplateTypeId;
            smsQue.ReceiverMobile = MobileNumber;
            smsQue.MessageBody = body;
            smsQue.UserId = userId;
            smsQue.ModuleId = template.TemplateType.ModuleId;
            smsQue.MessageStatus = "Pending";
            smsQue.DeliveredStatus = false;
            smsQue.SmsReference = Remarks;
            smsQue.ProfileId = profileId;
            smsQue.SentOn = null;
            smsQue.MessageId = null;
            if (ScheduledOn != null)
            {
                smsQue.ScheduledOn = ScheduledOn;
                smsQue.ScheduledOnDate = ScheduledOn.Value.Date;

            }
            else {
                smsQue.ScheduledOn = DateTime.Now;
                smsQue.ScheduledOnDate = DateTime.Today;
            }
            smsQue.EAID = null;
            smsQue.CreatedOn = DateTime.Now;
            smsQue.ModifiedOn = null;
            smsQue.BranchId = branchId;
            smsQue.RefId = RefId;
            smsQue.Type = Type;
            smsQue.ProfileType = ProfileType;
            db.SmsQueues.Add(smsQue);
        }

        public string PrepareSMSBody(Template temp, string id)
        {
            var content = temp.SMSTemplate;
            var content1 = Regex.Replace(content, "{ ", "{");
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} where {2}='{3}'", temp.TemplateType.ViewField, temp.TemplateType.TemplateView, temp.TemplateType.TemplateViewKey,id);
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {
                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    content1 = Smart.Format(content1, dict1);
                    break;
                }
                _dr.Close();
            }
            return content1;
        }
    }
}