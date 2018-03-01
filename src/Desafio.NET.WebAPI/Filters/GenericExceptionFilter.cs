using Desafio.NET.Infraestructure.Common.Exceptions;
using Desafio.NET.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Threading;
using System.Web.Http.Results;
using System.Web.Http.Filters;

namespace Desafio.NET.WebAPI.Filters
{
    public class GenericExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = this.HandleException(context);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            context.Response = this.HandleException(context);

            return Task.CompletedTask;
        }

        private HttpResponseMessage HandleException(HttpActionExecutedContext context)
        {
            if (this.CheckExceptionType(context.Exception, typeof(TokenValidationException)))
                return this.GetUnauthorizedResponse(context.Request, (TokenValidationException) this.GetException(context.Exception, typeof(TokenValidationException)));

            if (this.CheckExceptionType(context.Exception, typeof(ValidationException)))
                return this.GetBadRequestResponse(context.Request, (ValidationException) this.GetException(context.Exception, typeof(ValidationException)));

            return this.GetErrorResponse(context.Request, this.GetException(context.Exception, typeof(Exception)));
        }

        private bool CheckExceptionType(Exception exception, Type typeOfException)
        {
            return exception.GetType().Equals(typeOfException) || (exception.InnerException != null && exception.InnerException.GetType().Equals(typeOfException));
        }

        private Exception GetException(Exception exception, Type typeOfException)
        {
            if (exception.GetType().Equals(typeOfException))
                return exception;
            else if (exception.InnerException != null && exception.InnerException.GetType().Equals(typeOfException))
                return exception.InnerException;

            return exception;
        }

        private HttpResponseMessage GetErrorResponse(HttpRequestMessage request, Exception exception)
        {
            return request.CreateResponse(HttpStatusCode.InternalServerError, new NonSuccessResponseModel()
            {
                StatusCode = (int) HttpStatusCode.InternalServerError,
                Message = exception.Message
            });
        }

        private HttpResponseMessage GetUnauthorizedResponse(HttpRequestMessage request, TokenValidationException exception)
        {
            return request.CreateResponse(HttpStatusCode.Unauthorized, new NonSuccessResponseModel()
            {
                StatusCode = (int) HttpStatusCode.Unauthorized,
                Message = exception.Message
            });
        }

        private HttpResponseMessage GetBadRequestResponse(HttpRequestMessage request, ValidationException exception)
        {
            return request.CreateResponse(HttpStatusCode.BadRequest, new NonSuccessResponseModel()
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Message = exception.Message
            });
        }
    }
}