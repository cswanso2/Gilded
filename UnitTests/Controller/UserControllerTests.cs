using Gilded.Controllers;
using Gilded.Exceptions;
using Gilded.Models;
using Gilded.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controller
{
    public class UserControllerTests
    {
        private UsersController _userController;

        private Mock<IUserRepository> _mockRepository;

        private readonly User _user = new User()
        {
            EmailAddress = "abc.com"
        };

        private const string ApiKey = "abc";

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IUserRepository>();
            _userController = new UsersController(_mockRepository.Object);
        }

        [Test]
        [TestCase("mike@abc.com")]
        [TestCase("mike@email.com")]
        public void Register(string emailAddress)
        {
            _mockRepository.Setup(x => x.CreateUser(emailAddress)).Returns(ApiKey);
            var response = _userController.Register(emailAddress);
            Assert.AreEqual(ApiKey, response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            _mockRepository.Verify(x => x.CreateUser(emailAddress));
        }

        [Test]
        [TestCase("mikeabc.com")]
        public void RegisterBadEmail(string emailAddress)
        {
            _mockRepository.Setup(x => x.CreateUser(emailAddress)).Throws<InvalidEmailException>();
            var response = _userController.Register(emailAddress);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("mike@abc.com")]
        public void DuplicateUser(string emailAddress)
        {
            _mockRepository.Setup(x => x.CreateUser(emailAddress)).Throws<DuplicateUserException>();
            var response = _userController.Register(emailAddress);
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Test]
        [TestCase(-5)]
        [TestCase(5)]
        public void AddBalance(int balance)
        {
            _userController.Request = new HttpRequestMessage();
            _userController.ActionContext.Request.Properties["user"] = _user;
            var response = _userController.AddBalance(balance);
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
            _mockRepository.Verify(x => x.AddBalance(_user.EmailAddress, balance));
        }
    }
}
