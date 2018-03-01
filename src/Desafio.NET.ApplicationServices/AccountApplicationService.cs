using Desafio.NET.ApplicationServices.Abstractions;
using Desafio.NET.Domain.Entities;
using Desafio.NET.Domain.Services.Abstractions;
using Desafio.NET.Infraestructure.Common.Exceptions;
using Desafio.NET.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Desafio.NET.ApplicationServices
{
    /// <summary>
    /// Account appliation service implementation
    /// </summary>
    public class AccountApplicationService : IAccountApplicationService
    {
        private readonly ITokenDomainService _tokenDomainService;
        private readonly IUserDomainService _userDomainService;
        private readonly IUserPhoneRepository _userPhoneRepository;
        private readonly IUserRepository _userRepository;

        /// <inheritdoc />
        public AccountApplicationService(IUserDomainService userDomainService,
            ITokenDomainService tokenDomainService,
            IUserRepository userRepository,
            IUserPhoneRepository userPhoneRepository)
        {
            _userDomainService = userDomainService;
            _tokenDomainService = tokenDomainService;
            _userRepository = userRepository;
            _userPhoneRepository = userPhoneRepository;
        }

        /// <inheritdoc />
        public async Task<User> CreateAccountAsync(User account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            account.CreatedOn = DateTime.Now;
            account.LastLogon = DateTime.Now;

            account = await _userDomainService.CreateUserAsync(account);
            account.AccessToken = await this._tokenDomainService.GenerateTokenForUserAsync(account.Id);

            return account;
        }

        /// <inheritdoc />
        public async Task<User> UpdateAccountAsync(User account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (account.Id == 0) throw new ValidationException("Account id not informed!");

            return await _userDomainService.UpdateUserAsync(account);
        }

        /// <inheritdoc />
        public async Task<User> GetPrivateInformationsByIdAsync(string accessToken, long accountId)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentException(nameof(accessToken));
            if (accountId == 0) throw new ArgumentException(nameof(accountId));

            var tokenValidationResult = await this._tokenDomainService.ValidateTokenAsync(accessToken, accountId);

            if (tokenValidationResult)
            {
                var user = await this._userRepository.GetUserByIdAsync(accountId);

                user.Phones = await this._userPhoneRepository.GetUserPhonesAsync(accountId);

                return user;
            }

            return default(User);
        }
    }
}