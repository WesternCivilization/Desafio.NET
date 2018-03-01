using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Desafio.NET.Domain.Services.Abstractions;
using Desafio.NET.Repository.Abstractions;
using System.Threading.Tasks;
using Desafio.NET.Domain.Entities;
using System.Linq.Expressions;
using Desafio.NET.Infraestructure.Common.Exceptions;

namespace Desafio.NET.ApplicationServices.Tests
{
    [TestClass]
    public class AccountApplicationServiceTests
    {
        private readonly Mock<ITokenDomainService> _mockOfTokenDomainService;
        private readonly Mock<IUserDomainService> _mockOfUserDomainService;
        private readonly Mock<IUserPhoneRepository> _mockOfUserPhoneRepository;
        private readonly Mock<IUserRepository> _mockOfUserRepository;
        private readonly AccountApplicationService _service;

        public AccountApplicationServiceTests()
        {
            _mockOfUserDomainService = new Mock<IUserDomainService>();
            _mockOfUserRepository = new Mock<IUserRepository>();
            _mockOfTokenDomainService = new Mock<ITokenDomainService>();
            _mockOfUserPhoneRepository = new Mock<IUserPhoneRepository>();

            _service = new AccountApplicationService(_mockOfUserDomainService.Object, _mockOfTokenDomainService.Object, _mockOfUserRepository.Object, _mockOfUserPhoneRepository.Object);
        }

        #region CreateAccountAsync tests

        [TestMethod]
        public async Task CreateAccountAsync_CheckAccountIsNull_ArgumentNullException()
        {
            try
            {
                await _service.CreateAccountAsync(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        #endregion

        #region UpdateAccountAsync tests

        [TestMethod]
        public async Task UpdateAccountAsync_CheckAccountIsNull_ArgumentNullException()
        {
            try
            {
                await _service.UpdateAccountAsync(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public async Task UpdateAccountAsync_CheckAccountIdIsEqualToZero_ArgumentException()
        {
            try
            {
                await _service.UpdateAccountAsync(new User());
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        #endregion

        #region GetPrivateInformationsByIdAsync tests

        [TestMethod]
        public async Task GetPrivateInformationsByIdAsync_CheckAccessTokenIsNullOrEmpty_ArgumentException()
        {
            try
            {
                await _service.GetPrivateInformationsByIdAsync(string.Empty, 71);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task GetPrivateInformationsByIdAsync_CheckAccountIdIsEqualToZero_ArgumentException()
        {
            try
            {
                await _service.GetPrivateInformationsByIdAsync(Guid.NewGuid().ToString(), 0);
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task GetPrivateInformationsByIdAsync_CheckInvalidToken_ArgumentException()
        {
            Expression<Func<ITokenDomainService, Task<bool>>> validateTokenAsync = x => x.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<long>());

            this._mockOfTokenDomainService.Setup(validateTokenAsync).ReturnsAsync(false).Verifiable();

            var result = await _service.GetPrivateInformationsByIdAsync(Guid.NewGuid().ToString(), 71);

            Assert.IsNull(result);

            _mockOfTokenDomainService.Verify(validateTokenAsync, Times.Once);
        }

        [TestMethod]
        public async Task GetPrivateInformationsByIdAsync_CheckValidToken_ArgumentException()
        {
            Expression<Func<ITokenDomainService, Task<bool>>> validateTokenAsync = x => x.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<long>());

            this._mockOfTokenDomainService.Setup(validateTokenAsync).ReturnsAsync(true).Verifiable();

            Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());

            this._mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(new User() { Id = 17 }).Verifiable();

            var result = await _service.GetPrivateInformationsByIdAsync(Guid.NewGuid().ToString(), 71);

            Assert.IsNotNull(result);

            _mockOfTokenDomainService.Verify(validateTokenAsync, Times.Once);
            _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
        }

        #endregion
    }
}