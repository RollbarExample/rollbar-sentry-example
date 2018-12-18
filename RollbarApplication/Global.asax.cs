using Rollbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SharpRaven;
using RollBar.App_Start;
using RollbarApplication.App_Start;
using Rollbar.Telemetry;
using Rollbar.DTOs;

namespace RollbarApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            const string postServerItemAccessToken = "d903ca7d69e044908479c96e8c4af17a";
            RollbarLocator.RollbarInstance.Configure(new RollbarConfig(postServerItemAccessToken)
            {
                Environment = "production"
            });
            ConfigureRollbarTelemetry();
            ConfigureRollbarServer();
            //var ravenClient = new RavenClient("https://83b1b8aeded04c109de4f0e11a5be07d@sentry.io/1354823");
           
            //GlobalFilters.Filters.Add(new HandleErrorAttribute());
            //GlobalFilters.Filters.Add(new RavenHandleErrorAttribute());
        }

        private static void ConfigureRollbarTelemetry()
        {
            TelemetryConfig telemetryConfig = new TelemetryConfig(
              telemetryEnabled: true,
              telemetryQueueDepth: 3
            );
            TelemetryCollector.Instance.Config.Reconfigure(telemetryConfig);
        }

        private static void ConfigureRollbarServer()
        {
            Server server = new Server();
           // server.CodeVersion = "00e9466";
            server.Root = "/WebApplication2";
            server.Branch = "master";
            RollbarLocator.RollbarInstance.Config.Server = server;
        }
    }
}
