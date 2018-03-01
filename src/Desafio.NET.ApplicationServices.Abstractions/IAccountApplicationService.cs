using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.ApplicationServices.Abstractions
{
    /// <summary>
    /// Contracts for account application service
    /// </summary>
    public interface IAccountApplicationService
    {
        /// <summary>
        /// Create a new user account
        /// </summary>
        /// <param name="account">Account information</param>
        /// <returns></returns>
        Task<User> CreateAccountAsync(User account);

        /// <summary>
        /// Update user account
        /// </summary>
        /// <param name="account">Account information</param>
        /// <returns></returns>
        Task<User> UpdateAccountAsync(User account);

        /// <summary>
        /// Return private informations about account using id
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="accountId">Id of account</param>
        /// <returns></returns>
        Task<User> GetPrivateInformationsByIdAsync(string accessToken, long accountId);
    }
}