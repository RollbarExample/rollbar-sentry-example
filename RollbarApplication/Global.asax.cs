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
using Sentry.EntityFramework;
using Sentry;
using System.Configuration;

namespace RollbarApplication
{
    

    public class MvcApplication : System.Web.HttpApplication
    {
        private IDisposable _sentrySdk;
        static string host;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SentryDatabaseLogging.UseBreadcrumbs();
            _sentrySdk = SentrySdk.Init(o =>
            {
                // We store the DSN inside Web.config
                o.Dsn = new Sentry.Dsn(ConfigurationManager.AppSettings["SentryDsn"]);
                // Add the EntityFramework integration
                o.AddEntityFramework();
                o.Release = "1fbdda4534d3d7b5a282150dd041756661aba62e";
            });
            const string postServerItemAccessToken = "d903ca7d69e044908479c96e8c4af17a";
            RollbarLocator.RollbarInstance.Configure(new RollbarConfig(postServerItemAccessToken)
            {
                Environment = "production"
            });
            ConfigureRollbarServer();
            ConfigureRollbarTelemetry();
            ConfigurePersonData();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            SentrySdk.CaptureException(exception);
        }

        public override void Dispose()
        {
            _sentrySdk.Dispose();
            base.Dispose();
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

        private static void ConfigurePersonData()
        {
            Person person = new Person("007");
            person.Email = "jbond@mi6.uk";
            person.UserName = "JBOND";
            RollbarLocator.RollbarInstance.Config.Person = person;
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
