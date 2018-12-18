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
        //RavenClient ravenClient = new RavenClient("https://83b1b8aeded04c109de4f0e11a5be07d@sentry.io/1354823");
        // GET: CreateError
        public ActionResult Index()


        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateError()
        {

            //throw new Exception("This is test exception");
             string teststring = null;
             int a = teststring.Length;
             return null;

        }
        public ActionResult GenerateCoughtError()
        {
            try
            {
                string[] teststring = null;
                int a = teststring.Count();

            }
            catch (Exception )
            {
           
                throw;
            }
            return null;
           // throw new Exception("This is test exception");

        }

    }
}