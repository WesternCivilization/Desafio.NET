using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Repository.Abstractions
{
    /// <summary>
    /// User repository contracts
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Create a new user on database
        /// </summary>
        /// <param name="user">Instance of User</param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Update informations about user
        /// </summary>
        /// <param name="user">Instance of User</param>
        /// <returns></returns>
        Task<User> UpdateUserAsync(User user);

        /// <summary>
        /// Update last login field on database
        /// </summary>
        /// <param name="userId">Id of updated user</param>
        /// <param name="lastLogon">Date of last login</param>
        /// <returns></returns>
        Task UpdateLastLogonAsync(long userId, DateTime lastLogon);

        /// <summary>
        /// Get user using e-mail address
        /// </summary>
        /// <param name="email">E-mail address</param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user using database id
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(long userId);
    }
}