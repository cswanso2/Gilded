using Gilded.Exceptions;
using Gilded.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repository
{
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;
        
        /*
         * I would much prefer not to call multiple functions in one unit test, 
         * I think of these more like service tests for the database
         */

        [SetUp]
        public void Setup()
        {
            UserRepository.Clear();
            _userRepository = UserRepository.Get();
        }

        [Test]
        public void GetAdminUser()
        {
            var user = _userRepository.GetUser("adminapikey");
            Assert.AreEqual("adminapikey", user.ApiKey);
        }

        [Test]
        [TestCase("mike@mail.com")]
        public void CreateGetUser(string emailAddress)
        {
            var apiKey = _userRepository.CreateUser(emailAddress);
            var user = _userRepository.GetUser(apiKey);
            Assert.AreEqual(emailAddress, user.EmailAddress);
            Assert.AreEqual("user", user.Role);
            Assert.AreEqual(0, user.Balance);
        }

        [Test]
        [TestCase("mike@mail21.com")]
        public void DuplicateUser(string emailAddress)
        {
            var apiKey = _userRepository.CreateUser(emailAddress);
            Assert.Throws<DuplicateUserException>(() => _userRepository.CreateUser(emailAddress));
        }

        [Test]
        [TestCase("mikemail.com")]
        public void InvalidEmailAddress(string emailAddress)
        {
            Assert.Throws<FormatException>(() => _userRepository.CreateUser(emailAddress));
        }

        [Test]
        [TestCase("mike@mail2.com")]
        public void Addbalance(string emailAddress)
        {
            var apiKey = _userRepository.CreateUser(emailAddress);
            _userRepository.AddBalance(apiKey, 5);
            var user = _userRepository.GetUser(apiKey);
            Assert.AreEqual(5, user.Balance);
        }
    }
}
