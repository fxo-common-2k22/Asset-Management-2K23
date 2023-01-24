using FAPP.Model;
using System.Linq;
using System.Web.Http;

namespace FAPP.Controllers
{
    public class BaseApiController : ApiController
    {
        public OneDbContext db;
        public static string tokenId = "custom";
        public static string tokenValue = "encryptedvalue";
        public BaseApiController()
        {
            db = new OneDbContext();
        }


        public static bool Authenticated(System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            if (headers.Contains(tokenId))
            {
                if (headers.GetValues("custom").First().Equals(tokenValue))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public static int? GetUserIdFromRequestHeader(System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            if (headers.Contains("User"))
            {
                //if (headers.GetValues("User").First().Equals(tokenValue))
                    return System.Convert.ToInt32(headers.GetValues("User").First());
                //else
                    //return null ;
            }
            return null;
        }
    }
}
