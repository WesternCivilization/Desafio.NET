using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using Desafio.NET.DependencyInjection;
using Desafio.NET.WebAPI.Filters;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using Desafio.NET.ApplicationServices.Abstractions;

namespace Desafio.NET.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            //config.SuppressDefaultHostAuthentication();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<ApplicationServiceMappings>();
            builder.RegisterModule<DomainServiceMappgins>();
            builder.RegisterModule<RepositoryMappings>();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}