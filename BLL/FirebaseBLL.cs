using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FirebaseAdmin.Messaging;
using FAPP.Classes;

namespace FAPP.BLL
{
    public static class FirebaseBLL
    {
        static FirebaseBLL()
        {
            var FireBaseApp = new FireBaseApp();
            FireBaseApp.StartApp();
        }

        public static async Task<string> NotificationToList(List<string> TokensList, string NotificationTitle, string NotificationBody)
        {
            string error = string.Empty;
            //Get All Token List From Database .... 
            //Static List Here
            //List<string> TokenList = new List<string>();
            //TokenList.Add("fT1P9AJtQuqP29VXEz0T6j:APA91bFbUns-fyTIUYmQBj_MZwAH4j2pnlg-kF7J0Fh2LyNKzA9Bu00NXUZ5L_c6cAYM94prt2ImwGHvRZ91R6pyWFBmImp6lYddoKTr0yQ4GeiuNDh-Fp6CoYv0gsCnLrWwEzc7vzi4");
            //TokenList.Add("crEh1MWcTpq5_SNe20kMG8:APA91bHc2ddctZJjmda3YJAj7q-00-pDb4OnQQUQDMymvFDZNBfxMf__I47RXzB5YFNzcgDjERXbuj8yJJN1lITQBumIzXV2vU601dFc4h9DEkZbEix0euU5hMTF4qA_S7c_y7d6GvV3");

            if (TokensList.Count() > 0 && TokensList != null)
            {
                foreach (var item in TokensList)
                {
                    try
                    {
                        var message = new Message()
                        {
                            Notification = new Notification
                            {
                                Title = NotificationTitle,
                                Body = NotificationBody,
                                //ImageUrl = NotificationImageUrl
                            },
                            Token = item
                            //Topic = "news"
                        };
                        var messaging = FirebaseMessaging.DefaultInstance;
                        var result = await messaging.SendAsync(message);

                    }
                    catch (Exception)
                    {
                        error = "Exception Occured!";
                    }
                }
                //error =  "Notifications Sent Successfully!";
            }
            return error;
        }


        public static async Task<string> PushSingleNotification(string NotificationTitle, string NotificationBody, string Token)
        {
            string error = string.Empty;
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitle,
                        Body = NotificationBody,
                        //ImageUrl = NotificationImageUrl
                    },
                    Token = Token
                    //Topic = "news"
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);
                //error = "Notification Sent Successfully!";

            }
            catch (Exception ex)
            {
                error = "Something Went Wrong!";
            }
            return error;
        }
        public static List<string> GetDeviceIdsBYGroup(Model.OneDbContext db, Guid GroupId)
        {

            if (GroupId != Guid.Empty)
            {
                string query = $@" SELECT 
		                Distinct(t2.DeviceID)
                FROM Academics.Students t1
                INNER JOIN Academics.StudentSessions t3 ON t1.StudentId = t3.StudentId
	                AND t3.Active = 1
	                AND t3.SessionActive = 1
                INNER JOIN Academics.Families t2 ON t1.FamilyId = t2.FamilyId
                WHERE t3.GroupId = '{GroupId}'
	                --AND t3.BranchId = 1001
	                AND t1.Active = 1 AND t2.FamilyMobile is not null AND LEN(t2.FamilyMobile) = 12 AND t2.DeviceID is not null";
                var res = db.Database.SqlQuery<string>(query).ToList();
                return res;
            }
            else {
                return null;
            }

        }
        public static List<string> GetDeviceIdsBySession(Model.OneDbContext db, Guid SessionId, short BranchId)
        {

            if (SessionId != Guid.Empty)
            {
                string query = $@" SELECT 
		                Distinct(t2.DeviceID)
                FROM Academics.Students t1
                INNER JOIN Academics.StudentSessions t3 ON t1.StudentId = t3.StudentId
	                AND t3.Active = 1
	                AND t3.SessionActive = 1
                INNER JOIN Academics.Families t2 ON t1.FamilyId = t2.FamilyId
                WHERE t3.SessionId = '{SessionId}'
	                AND t3.BranchId = {BranchId}
	                AND t1.Active = 1 AND t2.FamilyMobile is not null AND LEN(t2.FamilyMobile) = 12 AND t2.DeviceID is not null";
                var res = db.Database.SqlQuery<string>(query).ToList();
                return res;
            }
            else {
                return null;
            }

        }
    }
}