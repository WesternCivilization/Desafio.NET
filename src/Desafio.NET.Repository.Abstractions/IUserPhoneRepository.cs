using Desafio.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Repository.Abstractions
{
    public interface IUserPhoneRepository
    {
        /// <summary>
        /// Delete all phones of user
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task DeleteUserPhonesAsync(long userId);

        /// <summary>
        /// Save user phone on database
        /// </summary>
        /// <param name="userPhone">Instance of UserPhone</param>
        /// <returns></returns>
        Task<UserPhone> CreateUserPhoneAsync(UserPhone userPhone);

        /// <summary>
        /// Get user phones
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task<UserPhone[]> GetUserPhonesAsync(long userId);
    }
}