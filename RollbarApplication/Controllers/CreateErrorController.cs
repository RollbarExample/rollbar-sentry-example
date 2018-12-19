using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rollbar;
using SharpRaven;
using SharpRaven.Data;
using RollbarApplication.App_Start;


namespace RollbarDotnetExample.Controllers
{
    public class CreateErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateUncaughtErrorForRollbar()
        {
             string teststring = null;
             int a = teststring.Length;
             return null;
        }
        [HttpPost]
        public ActionResult GenerateCoughtErrorForRollbar()
        {
            try
            {
                double[] testArray = new double[10];
                double test = testArray[11]; 
            }
            catch (Exception e)
            {
                RollbarLocator.RollbarInstance.Error(e);
            }
            return null;
        }

    }
}