using Desafio.NET.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desafio.NET.Domain.Entities;
using Desafio.NET.Repository.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Desafio.NET.Infraestructure.Common.Exceptions;

namespace Desafio.NET.Domain.Services
{
    /// <summary>
    /// Implementations for token service contract
    /// </summary>
    public class TokenDomainService : ITokenDomainService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _tokenSecretKey;

        /// <inheritdoc />
        public TokenDomainService(ITokenRepository tokenRepository,
            IUserRepository userRepository,
            string tokenSecretKey)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _tokenSecretKey = tokenSecretKey;
        }

        /// <inheritdoc />
        public async Task<Token> GenerateTokenForUserAsync(long userId)
        {
            if (userId.Equals(0)) throw new ArgumentException(nameof(userId));

            var user = await this._userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new ValidationException("User not found!");

            var token = new Token()
            {
                UserId = userId,
                IssuedOn = DateTime.Now,
                ExpireOn = DateTime.Now.AddMinutes(30)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._tokenSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var payload = new JwtPayload
            {
               { "userName", user.Name},
               { "userEmail", user.Email},
               { "userId", user.Id},
            };

            var jwt = new JwtSecurityToken(new JwtHeader(credentials), payload);

            token.Code = new JwtSecurityTokenHandler().WriteToken(jwt);

            await this._tokenRepository.DeleteUserTokensAsync(user.Id);

            return await this._tokenRepository.CreateTokenAsync(token);
        }

        /// <inheritdoc />
        public async Task<bool> ValidateTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentException(nameof(accessToken));

            var token = await this._tokenRepository.GetTokenByCodeAsync(accessToken);

            if (token == null) throw new TokenValidationException("Access denied!");
            if (token.ExpireOn <= DateTime.Now) throw new TokenValidationException("Access token expired!");

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> ValidateTokenAsync(string accessToken, long userId)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentException(nameof(accessToken));
            if (userId == 0) throw new ArgumentException(nameof(userId));

            var user = await this._userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new ValidationException("User not found!");

            await this.ValidateTokenAsync(accessToken);

            var token = await this._tokenRepository.GetTokenByCodeAsync(accessToken);

            if (!token.UserId.Equals(user.Id)) throw new TokenValidationException("Access denied!");

            return true;
        }
    }
}
