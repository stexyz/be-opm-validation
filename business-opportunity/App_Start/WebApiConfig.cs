using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using business_opportunity.Persistence;
using business_opportunity.Services;
using Common.Logging;
using Microsoft.Practices.Unity;

namespace business_opportunity
{
    public static class WebApiConfig
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WebApiConfig));

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            #region IoC

            _log.Info("Initializing dependencies.");
            var container = new UnityContainer();

            container.RegisterType<IOpportunityRepository, OpportunityInMemoryRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            _log.Info("Init finished successfully.");
            #endregion IoC
        }
    }
}
