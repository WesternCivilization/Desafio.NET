using Autofac.Integration.WebApi;
using Desafio.NET.ApplicationServices.Abstractions;
using Desafio.NET.Infraestructure.Common.Exceptions;
using Desafio.NET.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Desafio.NET.WebAPI.Filters
{
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute, IAutofacAuthorizationFilter
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var allowAnonymous = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (!allowAnonymous)
            {
                var service = actionContext.Request.GetDependencyScope().GetService(typeof(ILoginApplicationService)) as ILoginApplicationService;

                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        var validationResult = await service.ValidateTokenAsync(actionContext.Request.Headers.Authorization.Parameter);

                        if (!validationResult)
                            actionContext.Response = this.GetResponse(actionContext.Request);
                    }
                    catch (TokenValidationException ex)
                    {
                        this.GetResponse(actionContext.Request, ex.Message);
                    }
                });
            }
        }

        private HttpResponseMessage GetResponse(HttpRequestMessage request, string message = "Invalid token!")
        {
            return request.CreateResponse(HttpStatusCode.Unauthorized, new NonSuccessResponseModel()
            {
                StatusCode = (int) HttpStatusCode.Unauthorized,
                Message = message
            });
        }
    }
}