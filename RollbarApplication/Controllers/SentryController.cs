using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RollbarApplication.Controllers
{
    public class SentryController : Controller
    {
        // GET: Sentry
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateCoughtErrorForSentry()
        {
            try
            {
                double[] testArray = new double[10];
                double test = testArray[11];
            }
            catch (Exception e)
            {
               e.Data.Add("User Name", "Test User");
               e.Data.Add("User ID", "1");
                SentrySdk.CaptureException(e);
                //throw e;
            }
            return null;
            // throw new Exception("This is test exception");
        }

        [HttpPost]
        public ActionResult GenerateUncaughtErrorForSentry()
        {
            string teststring = null;
            int a = teststring.Length;
            return View();

        }
    }
}