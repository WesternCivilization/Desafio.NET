using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Desafio.NET.Repository.Abstractions;
using System.Threading.Tasks;
using Desafio.NET.Infraestructure.Common.Exceptions;
using System.Linq.Expressions;
using Desafio.NET.Domain.Entities;

namespace Desafio.NET.Domain.Services.Tests
{
    [TestClass]
    public class TokenDomainServiceTests
    {
        private readonly Mock<ITokenRepository> _mockOfTokenRepository;
        private readonly Mock<IUserRepository> _mockOfUserRepository;
        private readonly TokenDomainService _service;

        public TokenDomainServiceTests()
        {
            _mockOfTokenRepository = new Mock<ITokenRepository>();
            _mockOfUserRepository = new Mock<IUserRepository>();

            _service = new TokenDomainService(_mockOfTokenRepository.Object, _mockOfUserRepository.Object, "sdfhjsdfsdflsdfljsdkfhosjodf48748748487sdf7s7d8faddfsdf45sdf54df4");
        }

        #region GenerateTokenForUserAsync tests

        [TestMethod]
        public async Task GenerateTokenForUserAsync_CheckUserIdIsNotZero_ArgumentException()
        {
            try
            {
                await _service.GenerateTokenForUserAsync(0);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task GenerateTokenForUserAsync_ValidateIfUserIdExists_ArgumentException()
        {
            try
            {
                Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());

                this._mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(default(User)).Verifiable();

                await _service.GenerateTokenForUserAsync(7);

                _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        [TestMethod]
        public async Task GenerateTokenForUserAsync_CheckTokenHasBeenSaved_IdNotEqualToZero()
        {
            var token = new Token()
            {
                Code = Guid.NewGuid().ToString()
            };

            Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());

            this._mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(new User() { Id = 71 }).Verifiable();

            Expression<Func<ITokenRepository, Task<Token>>> createTokenAsync = x => x.CreateTokenAsync(It.IsAny<Token>());

            this._mockOfTokenRepository.Setup(createTokenAsync).Callback((Token t) => { token.Id = 17; }).ReturnsAsync(token).Verifiable();

            var result = await _service.GenerateTokenForUserAsync(7);

            Assert.IsTrue(result.Id.Equals(17));

            _mockOfTokenRepository.Verify(createTokenAsync, Times.Once);
            _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
        }

        #endregion

        #region ValidateTokenAsync tests

        [TestMethod]
        public async Task ValidateTokenAsync_CheckTokenIsNotNullOrEmpty_ArgumentException()
        {
            try
            {
                await _service.ValidateTokenAsync(string.Empty);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckUserTokenIsNotNullOrEmpty_ArgumentException()
        {
            try
            {
                await _service.ValidateTokenAsync(string.Empty, 17);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckUserIdIsNotZero_ArgumentException()
        {
            try
            {
                await _service.ValidateTokenAsync("123", 0);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckTokenExists_TokenValidationException()
        {
            try
            {
                Expression<Func<ITokenRepository, Task<Token>>> getTokenByCodeAsync = x => x.GetTokenByCodeAsync(It.IsAny<string>());
                this._mockOfTokenRepository.Setup(getTokenByCodeAsync).ReturnsAsync(default(Token)).Verifiable();

                await _service.ValidateTokenAsync("123");

                _mockOfTokenRepository.Verify(getTokenByCodeAsync, Times.Once);
            }
            catch (TokenValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TokenValidationException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckTokenIsExpired_TokenValidationException()
        {
            try
            {
                var token = new Token()
                {
                    Code = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                    ExpireOn = DateTime.Now.Subtract(TimeSpan.FromMinutes(30)),
                };

                Expression<Func<ITokenRepository, Task<Token>>> getTokenByCodeAsync = x => x.GetTokenByCodeAsync(It.IsAny<string>());
                this._mockOfTokenRepository.Setup(getTokenByCodeAsync).ReturnsAsync(token).Verifiable();

                await _service.ValidateTokenAsync("123");

                _mockOfTokenRepository.Verify(getTokenByCodeAsync, Times.Once);
            }
            catch (TokenValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TokenValidationException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckUserIdExists_TokenValidationException()
        {
            try
            {
                Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());
                _mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(default(User)).Verifiable();

                await _service.ValidateTokenAsync("123", 71);

                _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        [TestMethod]
        public async Task ValidateTokenAsync_CheckUserIsOwnerOfToken_TokenValidationException()
        {
            try
            {
                var user = new User()
                {
                    Id = 17
                };

                var token = new Token()
                {
                    UserId = 10,
                    Code = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpireOn = DateTime.Now.AddMinutes(30)
                };

                Expression<Func<ITokenRepository, Task<Token>>> getTokenByCodeAsync = x => x.GetTokenByCodeAsync(It.IsAny<string>());
                this._mockOfTokenRepository.Setup(getTokenByCodeAsync).ReturnsAsync(token).Verifiable();

                Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());
                _mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(user).Verifiable();

                var result = await _service.ValidateTokenAsync("123", 17);

                Assert.IsFalse(result);

                _mockOfTokenRepository.Verify(getTokenByCodeAsync, Times.Exactly(2));
                _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
            }
            catch (TokenValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TokenValidationException));
            }
        }

        #endregion
    }
}