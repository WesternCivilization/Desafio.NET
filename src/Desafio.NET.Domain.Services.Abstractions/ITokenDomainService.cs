using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Domain.Services.Abstractions
{
    /// <summary>
    /// Contracts for token management service
    /// </summary>
    public interface ITokenDomainService
    {
        /// <summary>
        /// Generate a new token for user
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task<Token> GenerateTokenForUserAsync(long userId);

        /// <summary>
        /// Validate user token
        /// </summary>
        /// <param name="accessToken">Code of token</param>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task<bool> ValidateTokenAsync(string accessToken, long userId);

        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns></returns>
        Task<bool> ValidateTokenAsync(string accessToken);
    }
}