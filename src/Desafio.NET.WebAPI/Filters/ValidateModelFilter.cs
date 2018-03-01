using Desafio.NET.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Desafio.NET.WebAPI.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                var result = new NonSuccessValidationResponseModel()
                {
                    StatusCode = (int) HttpStatusCode.BadRequest,
                    Message = "The request is invalid!",
                    Errors = new Dictionary<string, string>()
                };

                foreach (var item in modelState)
                    foreach (var error in item.Value.Errors)
                        result.Errors.Add(item.Key, error.ErrorMessage);
                
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}