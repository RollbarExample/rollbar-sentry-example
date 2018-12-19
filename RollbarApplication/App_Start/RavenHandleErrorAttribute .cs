using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpRaven;
using SharpRaven.Data;


namespace RollbarApplication.App_Start
{
    public class RavenHandleErrorAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;
            var ravenClient = new RavenClient("https://f3d2512a066842aca3d8783547ba1381@sentry.io/1356616");
            ravenClient.Capture(new SentryEvent(filterContext.Exception));
        }
    }
}