using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Desafio.NET.Repository.Abstractions;
using Moq;
using Desafio.NET.Domain.Services.Abstractions;
using System.Threading.Tasks;
using Desafio.NET.Infraestructure.Common.Exceptions;
using System.Linq.Expressions;
using Desafio.NET.Domain.Entities;

namespace Desafio.NET.ApplicationServices.Tests
{
    [TestClass]
    public class LoginApplicationServiceTests
    {
        private readonly Mock<ITokenDomainService> _mockOfTokenDomainService;
        private readonly Mock<IUserRepository> _mockOfUserRepository;
        private readonly LoginApplicationService _service;

        public LoginApplicationServiceTests()
        {
            _mockOfUserRepository = new Mock<IUserRepository>();
            _mockOfTokenDomainService = new Mock<ITokenDomainService>();
            _service = new LoginApplicationService(_mockOfUserRepository.Object, _mockOfTokenDomainService.Object);
        }

        #region AuthenticateUserAsync tests

        [TestMethod]
        public async Task AuthenticateUserAsync_CheckEmailIsNullOrEmpty_ArgumentException()
        {
            try
            {
                await _service.AuthenticateUserAsync(string.Empty, "123");
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task AuthenticateUserAsync_CheckPasswordIsNullOrEmpty_ArgumentException()
        {
            try
            {
                await _service.AuthenticateUserAsync("123", string.Empty);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task AuthenticateUserAsync_ValidateIfIdExists_ValidationException()
        {
            try
            {
                var user = new User()
                {
                    Id = 10,
                    Email = "account@domain.com"
                };

                Expression<Func<IUserRepository, Task<User>>> getUserByEmailAsync = x => x.GetUserByEmailAsync(It.IsAny<string>());

                this._mockOfUserRepository.Setup(getUserByEmailAsync).ReturnsAsync(default(User)).Verifiable();

                await _service.AuthenticateUserAsync("account@domain.com", "123");

                _mockOfUserRepository.Verify(getUserByEmailAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        [TestMethod]
        public async Task AuthenticateUserAsync_ComparePassword_ValidationException()
        {
            try
            {
                var user = new User()
                {
                    Id = 10,
                    Email = "account@domain.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("pass13")
                };

                Expression<Func<IUserRepository, Task<User>>> getUserByEmailAsync = x => x.GetUserByEmailAsync(It.IsAny<string>());

                this._mockOfUserRepository.Setup(getUserByEmailAsync).ReturnsAsync(user).Verifiable();

                await _service.AuthenticateUserAsync("account@domain.com", "123");

                _mockOfUserRepository.Verify(getUserByEmailAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        #endregion

        #region ValidateTokenAsync tests

        [TestMethod]
        public async Task ValidateTokenAsync_CheckTokenIsNullOrEmpty_ArgumentException()
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

        #endregion
    }
}
