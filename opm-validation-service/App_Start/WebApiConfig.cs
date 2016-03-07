using System;
using System.Web;
using System.Web.Http;
using Common.Logging;
using Microsoft.Practices.Unity;
using opm_validation_service.Persistence;
using opm_validation_service.Services;

namespace opm_validation_service {
    public static class WebApiConfig {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WebApiConfig));
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            bool recreateDatabase = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["RecreateDatabase"]);

            if (recreateDatabase)
            {
                _log.Info("DB recreate started.");
                DbRepositoryUtil.RecreateDatabase();
                //TODO SP: move sample data to AppData
                String pathToSampleData = HttpContext.Current.Server.MapPath("~/Persistence/OpmRepoSampleData.csv");
                DbRepositoryUtil.FillSampleOpm(pathToSampleData);
                _log.Info("DB recreate finished.");
            }

            #region IoC

            _log.Info("Initializing dependencies.");
            var container = new UnityContainer();
            container.RegisterType<IOpmVerificator, OpmVerificator>(new HierarchicalLifetimeManager());

            string idmUrl = System.Configuration.ConfigurationManager.AppSettings["IdmUrl"];
            IIdentityManagement idm = new IdentityManagement(idmUrl);
            container.RegisterInstance(idm);
            _log.Info("IDM found on " + idmUrl + ".");
            
            string eanEicCheckerUrl = System.Configuration.ConfigurationManager.AppSettings["EanEicCheckerUrl"];
            IEanEicCheckerHttpClient eanEicCheckerHttpClient = new EanEicCheckerHttpClient(eanEicCheckerUrl);
            container.RegisterInstance(eanEicCheckerHttpClient);
            _log.Info("EAN/EIC checker found on " + eanEicCheckerUrl + ".");

            IOpmRepository opmRepository = new OpmDbRepository();
            container.RegisterInstance(opmRepository);

            IUserAccessRepository userAccessRepository = new UserAccessDbRepository();

            int maxUserLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxUserLimit"]);
            int userLimitTimeWindownInSeconds = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserLimitTimeWindownInSeconds"]);
            IUserAccessService userAccessService = new UserAccessService(userAccessRepository, new TimeSpan(0, 0, 0, userLimitTimeWindownInSeconds), maxUserLimit);
            container.RegisterInstance(userAccessService);
            _log.Info("User access limitation initialized with user limit [" + maxUserLimit + "], time window for access limitation ["+ userLimitTimeWindownInSeconds + " s].");
            
            config.DependencyResolver = new UnityResolver(container);
            _log.Info("Init finished successfully.");

#endregion IoC
        }
    }
}
