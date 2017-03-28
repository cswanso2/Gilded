using IntegrationTests.Sdk;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace IntegrationTests
{
    public class UserTests : IntegrationTestsBase
    {
        [Test]
        [TestCase("mail@mire.com")]
        public void Register(string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                using (var response = sdk.Register(emailAddress))
                {
                    Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                    var key = sdk.GetApiKeyFromRegisterResponse(response);
                    Assert.IsNotEmpty(key);
                }
            }
        }

        [Test]
        [TestCase("mail2@mire.com")]
        public void RegisterDuplicates(string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                sdk.Register(emailAddress);
                using (var response = sdk.Register(emailAddress))
                {
                    Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
                }
            }
        }

        [Test]
        [TestCase("mail4@mire.com")]
        public void DuplicateCaseEmail(string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                sdk.Register(emailAddress);
                emailAddress = emailAddress.ToUpper(); //wont work if test case is all caps
                var response = sdk.Register(emailAddress);
                Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
            }
        }

        [Test]
        [TestCase("mail3mire.com")]
        public void InvalidEmailAddress(string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                sdk.Register(emailAddress);
                var response = sdk.Register(emailAddress);
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Test]
        [TestCase("email@mail.com", 1)]
        public void IncreaseBalance(string email, int balanceAmount)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                var registerResponse = sdk.Register(email);
                var key = sdk.GetApiKeyFromRegisterResponse(registerResponse);
                var response = sdk.IncreaseBalance(key, balanceAmount);
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
            }
        }

        [Test]
        [TestCase("notakey", 1)]
        public void IncreaseBalanceBadAuth(string apiKey, int balanceAmount)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new UsersSdk(server);
                var response = sdk.IncreaseBalance(apiKey, balanceAmount);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}
