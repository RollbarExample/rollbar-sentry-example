using RollBar.App_Start;
using System.Web;
using System.Web.Mvc;
using SharpRaven;
using RollbarApplication.App_Start;

namespace RollbarApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RollbarExceptionFilter());
            //GlobalFilters.Filters.Add(new RavenHandleErrorAttribute());
            filters.Add(new RavenHandleErrorAttribute());
        }
    }
}
