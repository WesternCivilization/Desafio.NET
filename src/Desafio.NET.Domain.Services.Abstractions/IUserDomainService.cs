using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Domain.Services.Abstractions
{
    public interface IUserDomainService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">Instance of user</param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Update a user data
        /// </summary>
        /// <param name="user">Instance of user</param>
        /// <returns></returns>
        Task<User> UpdateUserAsync(User user);

        /// <summary>
        /// Save user phones
        /// </summary>
        /// <param name="user">User phones</param>
        /// <returns></returns>
        Task SaveUserPhonesAsync(User user);
    }
}