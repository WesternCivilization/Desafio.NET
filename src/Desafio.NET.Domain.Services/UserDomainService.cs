using Desafio.NET.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desafio.NET.Domain.Entities;
using Desafio.NET.Repository.Abstractions;
using Desafio.NET.Infraestructure.Common.Exceptions;

namespace Desafio.NET.Domain.Services
{
    /// <summary>
    /// Implementations for user service contract
    /// </summary>
    public class UserDomainService : IUserDomainService
    {
        private readonly IUserPhoneRepository _userPhoneRepository;
        private readonly IUserRepository _userRepository;

        /// <inheritdoc />
        public UserDomainService(IUserRepository userRepository, IUserPhoneRepository userPhoneRepository)
        {
            _userRepository = userRepository;
            _userPhoneRepository = userPhoneRepository;
        }

        /// <inheritdoc />
        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException(nameof(user.Email));

            var currentUser = await _userRepository.GetUserByEmailAsync(user.Email);

            if (currentUser != null) throw new ValidationException(string.Format("This email '{0}' already exists!", user.Email));

            var result = await this._userRepository.CreateUserAsync(user);

            await SaveUserPhonesAsync(user);

            return user;
        }

        /// <inheritdoc />
        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Id == 0) throw new ArgumentException(nameof(user.Id));

            var currentUser = await _userRepository.GetUserByIdAsync(user.Id);

            if (currentUser == null) throw new ValidationException("This user not exists!");

            await SaveUserPhonesAsync(user);

            user.UpdatedOn = DateTime.Now;

            return await this._userRepository.UpdateUserAsync(user);
        }

        /// <inheritdoc />
        public async Task SaveUserPhonesAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (!user.Id.Equals(0))
                await this._userPhoneRepository.DeleteUserPhonesAsync(user.Id);

            if (user.Phones != null && user.Phones.Any())
                foreach (UserPhone item in user.Phones)
                {
                    item.UserId = user.Id;

                    var itemResult = await this._userPhoneRepository.CreateUserPhoneAsync(item);

                    item.Id = itemResult.Id;
                }
        }
    }
}