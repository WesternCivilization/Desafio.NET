using AutoMapper;
using Desafio.NET.ApplicationServices.Abstractions;
using Desafio.NET.WebAPI.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Desafio.NET.WebAPI.Controllers
{
    /// <summary>
    /// Login endpoints
    /// </summary>
    public class LoginController : ApiController
    {
        private readonly ILoginApplicationService _loginApplicationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginApplicationService"></param>
        public LoginController(ILoginApplicationService loginApplicationService)
        {
            _loginApplicationService = loginApplicationService;
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="credentials">Credentials</param>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, Type = typeof(NonSuccessValidationResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.OK, Type = typeof(LoginResultModel))]
        public async Task<HttpResponseMessage> PostAsync([FromBody]LoginModel credentials)
        {
            var result = await this._loginApplicationService.AuthenticateUserAsync(credentials.Email, credentials.Password);

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<LoginResultModel>(result));
        }
    }
}