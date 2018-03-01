using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.ApplicationServices.Abstractions
{
    /// <summary>
    /// Contracts for login application service
    /// </summary>
    public interface ILoginApplicationService
    {
        /// <summary>
        /// Authenticate user using credentials
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// /// <param name="password">User password</param>
        /// <returns></returns>
        Task<Token> AuthenticateUserAsync(string email, string password);

        /// <summary>
        /// Validate access token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        Task<bool> ValidateTokenAsync(string accessToken);
    }
}