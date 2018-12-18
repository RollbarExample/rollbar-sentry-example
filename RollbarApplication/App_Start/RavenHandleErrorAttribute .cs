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
            var ravenClient = new RavenClient("https://83b1b8aeded04c109de4f0e11a5be07d@sentry.io/1354823");
            ravenClient.Capture(new SentryEvent(filterContext.Exception));
        }
    }
}