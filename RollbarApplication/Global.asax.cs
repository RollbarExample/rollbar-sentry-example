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
        static string host;
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
            
            ConfigureRollbarServer();
          //  ConfigureRollbarTelemetry();
        }
        
        private static void ConfigureRollbarServer()
        {
            Server server = new Server();
            // server.CodeVersion = "00e9466";
            server.Root = "F:\\RollbarDotNet\\Rollbar-Sentry\\rollbar-sentry-example\\";
            server.Branch = "master";
            RollbarLocator.RollbarInstance.Config.Server = server;
        }
        private static void ConfigureRollbarTelemetry()
        {
           TelemetryConfig telemetryConfig = new TelemetryConfig(
              telemetryEnabled: true,
              telemetryQueueDepth: 100
            );
            
            TelemetryCollector.Instance.Config.Reconfigure(telemetryConfig);
           /* TelemetryCollector.Instance.Capture(
                new Telemetry(
                    TelemetrySource.Server,
                    TelemetryLevel.Info,
                    new NetworkTelemetry("GET",host , DateTime.Now,null, 200,null))
                );*/
        }
        
        void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            string[] array = FirstRequestInitialisation.Initialise(context);
           // Console.WriteLine("base url : " + host);
            TelemetryCollector.Instance.Capture(
                new Telemetry(
                    TelemetrySource.Server,
                    TelemetryLevel.Info,
                    new NetworkTelemetry(array[1],array[0], DateTime.Now, null, 200, null))
                );
        }

        class FirstRequestInitialisation
        {
            private static string host = null;

            private static Object s_lock = new Object();

            // Initialise only on the first request
            public static string[] Initialise(HttpContext context)
            {
                string[] array = new string[2];
                Uri uri = HttpContext.Current.Request.Url;
                string method= HttpContext.Current.Request.HttpMethod;
                host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                array[0] = host;
                array[1] = method;
                return array;
            }
        }

    }
}
