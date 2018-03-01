using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Repository.Abstractions
{
    /// <summary>
    /// Token repository contracts
    /// </summary>
    /// <typeparam name="Token">Type of token object</typeparam>
    public interface ITokenRepository
    {
        /// <summary>
        /// Create a token on database
        /// </summary>
        /// <param name="token">Instance of token</param>
        /// <returns></returns>
        Task<Token> CreateTokenAsync(Token token);

        /// <summary>
        /// Get token using code
        /// </summary>
        /// <param name="code">Code of token</param>
        /// <returns></returns>
        Task<Token> GetTokenByCodeAsync(string code);

        /// <summary>
        /// Delete all tokens of user
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task DeleteUserTokensAsync(long userId);
    }
}