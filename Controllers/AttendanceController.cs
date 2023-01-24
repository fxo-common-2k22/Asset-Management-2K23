using FAPP.Model;
using FAPP.ViewModel;
using FAPP.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace FAPP.Controllers
{
    public class AttendanceController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult DownloadConstraints(short? id)
        {
            if (!Authenticated(Request.Headers))
                return Unauthorized();

            if (!id.HasValue)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Branch id not found"));
            }
            try
            {
                var UserInfoes = db.USERINFOes
                   .Select(u => new
                   {
                       u.USERID,
                       u.BADGENUMBER,
                       u.SSN,
                       u.NAME,
                       u.GENDER,
                       u.TITLE,
                       u.PAGER,
                       u.BIRTHDAY,
                       u.HIREDDAY,
                       u.STREET,
                       u.CITY,
                       u.STATE,
                       u.ZIP,
                       u.OPHONE,
                       u.FPHONE,
                       u.VERIFICATIONMETHOD,
                       u.DEFAULTDEPTID,
                       u.SECURITYFLAGS,
                       u.ATT,
                       u.INLATE,
                       u.OUTEARLY,
                       u.OVERTIME,
                       u.SEP,
                       u.HOLIDAY,
                       u.MINZU,
                       u.PASSWORD,
                       u.LUNCHDURATION,
                       u.MVerifyPass,
                       //u.PHOTO,
                       //u.Notes,
                       u.privilege,
                       u.InheritDeptSch,
                       u.InheritDeptSchClass,
                       u.AutoSchPlan,
                       u.MinAutoSchInterval,
                       u.RegisterOT,
                       u.InheritDeptRule,
                       u.EMPRIVILEGE,
                       u.CardNo,
                       u.ModifiedBy,
                       u.IP,
                       u.RegNo
                   }).ToList();

                var Departments = db.DEPARTMENTS_dbo
                   .Select(u => new
                   {
                       u.DEPTID,
                       u.DEPTNAME,
                       u.SUPDEPTID,
                       u.InheritParentSch,
                       u.InheritDeptSch,
                       u.InheritDeptSchClass,
                       u.AutoSchPlan,
                       u.InLate,
                       u.OutEarly,
                       u.InheritDeptRule,
                       u.MinAutoSchInterval,
                       u.RegisterOT,
                       u.DefaultSchId,
                       u.ATT,
                       u.Holiday,
                       u.OverTime,
                       u.ModifiedBy,
                       u.IP
                   }).ToList();


                var data = new
                {
                    USERINFOList = UserInfoes,
                    DEPARTMENTList = Departments
                };
                return Ok(data);
            }
            catch (Exception exc)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc.GetExceptionMessages()));
            }
        }

        [HttpPost]
        public IHttpActionResult UploadAttendance([FromBody] AttendanceAPIVM data)
        {
            if (!Authenticated(Request.Headers))
                return Unauthorized();
            if (data == null || data.CHECKINOUTlist == null || data.CHECKINOUTlist?.Count() == 0)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No data found"));
            }
            List<CheckInOutMapping> uploadedorders = new List<CheckInOutMapping>();
            //try
            //{
                using (var trans = db.Database.BeginTransaction())
                {
                    foreach (var item in data.CHECKINOUTlist)
                    {
                        uploadedorders.Add(CheckInOut_Insert(item));
                    }
                    trans.Commit();
                    var edata = new
                    {
                        UploadedIds = uploadedorders
                    };
                    return Ok(edata);
                }
            //}
            //catch (Exception exc)
            //{

            //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc.GetExceptionMessages()));
            //}
        }

        public CheckInOutMapping CheckInOut_Insert(CHECKINOUT objCHECKINOUT)
        {
            CHECKINOUT _CHECKINOUT = new CHECKINOUT();
            _CHECKINOUT.USERID = objCHECKINOUT.USERID;
            if (!string.IsNullOrEmpty(objCHECKINOUT.CHECKDATETIME))
                _CHECKINOUT.CHECKTIME = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(objCHECKINOUT.CHECKDATETIME);
            else
                _CHECKINOUT.CHECKTIME = objCHECKINOUT.CHECKTIME;
            _CHECKINOUT.CHECKTYPE = objCHECKINOUT.CHECKTYPE;
            _CHECKINOUT.VERIFYCODE = objCHECKINOUT.VERIFYCODE;
            _CHECKINOUT.SENSORID = objCHECKINOUT.SENSORID;
            _CHECKINOUT.Memoinfo = objCHECKINOUT.Memoinfo;
            _CHECKINOUT.WorkCode = objCHECKINOUT.WorkCode;
            _CHECKINOUT.sn = objCHECKINOUT.sn;
            _CHECKINOUT.UserExtFmt = objCHECKINOUT.UserExtFmt;
            _CHECKINOUT.ModifiedBy = objCHECKINOUT.ModifiedBy;
            _CHECKINOUT.IP = objCHECKINOUT.IP;
            _CHECKINOUT.ID = objCHECKINOUT.ID;
            _CHECKINOUT.Processed = objCHECKINOUT.Processed;
            var isexist = db.CHECKINOUT.Where(u => u.USERID == objCHECKINOUT.USERID && u.CHECKTIME == _CHECKINOUT.CHECKTIME).Any();
            if (!isexist)
            {
                db.CHECKINOUT.Add(_CHECKINOUT);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return new CheckInOutMapping() { USERID = _CHECKINOUT.USERID, CHECKTIME = _CHECKINOUT.CHECKTIME };
        }
    }
}
