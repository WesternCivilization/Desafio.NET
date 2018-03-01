using AutoMapper;
using Desafio.NET.ApplicationServices.Abstractions;
using Desafio.NET.Domain.Entities;
using Desafio.NET.WebAPI.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Desafio.NET.WebAPI.Controllers
{
    /// <summary>
    /// Account endpoins
    /// </summary>
    public class AccountController : ApiController
    {
        private readonly IAccountApplicationService _accountApplicationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountApplicationService"></param>
        public AccountController(IAccountApplicationService accountApplicationService)
        {
            _accountApplicationService = accountApplicationService;
        }

        /// <summary>
        /// Get my user profile
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <returns></returns>
        [Route("account/{id}/me")]
        [HttpGet]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.OK, Type = typeof(AccountModel))]
        public async Task<HttpResponseMessage> GetAsync(long id)
        {
            var result = await _accountApplicationService.GetPrivateInformationsByIdAsync(Request.Headers.Authorization.Parameter, id);

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<AccountModel>(result));
        }

        /// <summary>
        /// Create a new user account
        /// </summary>
        /// <param name="account">Account informations</param>
        [Route("account")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.Created, Type = typeof(NewAccountResultModel))]
        public async Task<HttpResponseMessage> PostAsync([FromBody]NewAccountModel account)
        {
            var result = await _accountApplicationService.CreateAccountAsync(Mapper.Map<User>(account));

            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<NewAccountResultModel>(result));
        }

        /// <summary>
        /// Update user account
        /// </summary>
        /// <param name="account">Account informations</param>
        /// <returns></returns>
        [Route("account")]
        [HttpPut]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(NonSuccessResponseModel))]
        [SwaggerResponse((int) HttpStatusCode.OK, Type = typeof(AccountModel))]
        public async Task<HttpResponseMessage> PutAsync([FromBody]NewAccountModel account)
        {
            var result = await _accountApplicationService.UpdateAccountAsync(Mapper.Map<User>(account));

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<AccountModel>(result));
        }
    }
}