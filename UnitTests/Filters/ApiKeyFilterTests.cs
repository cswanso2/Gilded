using Gilded.Filters;
using Gilded.Models;
using Gilded.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace UnitTests
{
    public class ApiKeyFilterTests
    {
        private ApiKeyFilter _filter;
        private Mock<IUserRepository> _mockUserRepository;

        private const string ApiKey = "abc";

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _filter = new ApiKeyFilter();
        }

        [Test]
        public void UserFound()
        {
            _mockUserRepository.Setup(x => x.GetUser(ApiKey)).Returns(new User() { EmailAddress = "test@test.com"});
            var context = new HttpActionContext();
            context.Request.Headers.Add("Authorization", ApiKey);
            _filter.OnAuthorization(context);
            Assert.AreEqual((context.Request.Properties["user"] as User).EmailAddress, "test@test.com");
        }
    }
}
