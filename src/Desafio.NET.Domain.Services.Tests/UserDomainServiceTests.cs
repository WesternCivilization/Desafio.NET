using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using Desafio.NET.Repository.Abstractions;
using Desafio.NET.Infraestructure.Common.Exceptions;
using Desafio.NET.Domain.Entities;
using System.Linq.Expressions;

namespace Desafio.NET.Domain.Services.Tests
{
    [TestClass]
    public class UserDomainServiceTests
    {
        private readonly Mock<IUserRepository> _mockOfUserRepository;
        private readonly Mock<IUserPhoneRepository> _mockOfUserPhoneRepository;
        private readonly UserDomainService _service;

        public UserDomainServiceTests()
        {
            _mockOfUserRepository = new Mock<IUserRepository>();
            _mockOfUserPhoneRepository = new Mock<IUserPhoneRepository>();

            _service = new UserDomainService(_mockOfUserRepository.Object, _mockOfUserPhoneRepository.Object);
        }

        #region CreateUserAsync tests

        [TestMethod]
        public async Task CreateUserAsync_CheckUserIsNull_ArgumentNullException()
        {
            try
            {
                await new UserDomainService(null, null).CreateUserAsync(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public async Task CreateUserAsync_CheckUserEmailIsNull_ArgumentException()
        {
            try
            {
                await new UserDomainService(null, null).CreateUserAsync(new User() { });
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task CreateUserAsync_ValidateDuplicatedEmail_ValidationException()
        {
            try
            {
                var user = new User()
                {
                    Email = "account@domain.com"
                };

                Expression<Func<IUserRepository, Task<User>>> getUserByEmailAsync = x => x.GetUserByEmailAsync(It.IsAny<string>());

                this._mockOfUserRepository.Setup(getUserByEmailAsync).ReturnsAsync(user).Verifiable();

                await _service.CreateUserAsync(user);

                _mockOfUserRepository.Verify(getUserByEmailAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        [TestMethod]
        public async Task CreateUserAsync_CheckUserHasBeenSaved_IdNotEqualToZero()
        {
            var user = new User()
            {
                Name = "User Name",
                Email = "account@domain.com",
            };

            Expression<Func<IUserRepository, Task<User>>> createUserAsync = x => x.CreateUserAsync(It.IsAny<User>());

            this._mockOfUserRepository.Setup(createUserAsync).Callback((User u) => { user.Id = 10; }).ReturnsAsync(user).Verifiable();

            var result = await _service.CreateUserAsync(user);

            Assert.IsTrue(result.Id.Equals(10));

            _mockOfUserRepository.Verify(createUserAsync, Times.Once);
        }

        #endregion

        #region UpdateUserAsync tests

        [TestMethod]
        public async Task UpdateUserAsync_CheckUserIsNull_ArgumentNullException()
        {
            try
            {
                await new UserDomainService(null, null).CreateUserAsync(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public async Task UpdateUserAsync_CheckUserIdIsNotZero_ArgumentException()
        {
            try
            {
                await new UserDomainService(null, null).CreateUserAsync(new User() { });
            }
            catch (ArgumentException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public async Task UpdateUserAsync_ValidateIfIdExists_ValidationException()
        {
            try
            {
                var user = new User()
                {
                    Id = 10,
                    Email = "account@domain.com"
                };

                Expression<Func<IUserRepository, Task<User>>> getUserByIdAsync = x => x.GetUserByIdAsync(It.IsAny<long>());

                this._mockOfUserRepository.Setup(getUserByIdAsync).ReturnsAsync(default(User)).Verifiable();

                await _service.UpdateUserAsync(user);

                _mockOfUserRepository.Verify(getUserByIdAsync, Times.Once);
            }
            catch (ValidationException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException));
            }
        }

        #endregion

        #region SaveUserPhonesAsync tests

        [TestMethod]
        public async Task SaveUserPhonesAsync_CheckUserIsNull_ArgumentNullException()
        {
            try
            {
                await new UserDomainService(null, null).SaveUserPhonesAsync(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public async Task SaveUserPhonesAsync_CheckUserPhoneHasBeenSaved_IdNotEqualToZero()
        {
            var user = new User()
            {
                Id = 10,
                Name = "User Name",
                Email = "account@domain.com",
                Phones = new UserPhone[]
                {
                    new UserPhone() { AreaCode = "21", PhoneNumber = "99999-3339" },
                    new UserPhone() { AreaCode = "31", PhoneNumber = "99999-3337" }
                }
            };

            Expression<Func<IUserPhoneRepository, Task<UserPhone>>> createUserPhoneAsyncOne = x => x.CreateUserPhoneAsync(user.Phones[0]);

            this._mockOfUserPhoneRepository.Setup(createUserPhoneAsyncOne).Callback((UserPhone u) => { u.Id = 1; }).ReturnsAsync(user.Phones[0]).Verifiable();

            Expression<Func<IUserPhoneRepository, Task<UserPhone>>> createUserPhoneAsyncTwo = x => x.CreateUserPhoneAsync(user.Phones[1]);

            this._mockOfUserPhoneRepository.Setup(createUserPhoneAsyncTwo).Callback((UserPhone u) => { u.Id = 7; }).ReturnsAsync(user.Phones[1]).Verifiable();

            await this._service.SaveUserPhonesAsync(user);

            Assert.IsTrue(user.Phones[0].Id.Equals(1));
            Assert.IsTrue(user.Phones[1].Id.Equals(7));

            _mockOfUserPhoneRepository.Verify(createUserPhoneAsyncOne, Times.Once);
            _mockOfUserPhoneRepository.Verify(createUserPhoneAsyncTwo, Times.Once);
        }

        #endregion
    }
}