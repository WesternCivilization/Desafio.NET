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

namespace Desafio.NET.ApplicationServices
{
    /// <summary>
    /// Login application service implementation
    /// </summary>
    public class LoginApplicationService : ILoginApplicationService
    {
        private readonly ITokenDomainService _tokenDomainService;
        private readonly IUserRepository _userRepository;

        /// <inheritdoc />
        public LoginApplicationService(IUserRepository userRepository,
            ITokenDomainService tokenDomainService)
        {
            _userRepository = userRepository;
            _tokenDomainService = tokenDomainService;
        }

        /// <inheritdoc />
        public async Task<Token> AuthenticateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(password));

            var user = await this._userRepository.GetUserByEmailAsync(email);

            if (user == null) throw new ValidationException("Invalid credentials!");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) throw new ValidationException("Invalid credentials!");

            await this._userRepository.UpdateLastLogonAsync(user.Id, DateTime.Now);

            return await this._tokenDomainService.GenerateTokenForUserAsync(user.Id);
        }

        /// <inheritdoc />
        public async Task<bool> ValidateTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentException(nameof(accessToken));

            return await this._tokenDomainService.ValidateTokenAsync(accessToken);
        }
    }
}