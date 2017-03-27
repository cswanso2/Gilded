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
        private const string RegisterUrl = "http://localhost:56882/api/users/register";

        private const string EmailAddress = "mail@chimp.com";
        [Test]
        public void Register()
        {
            using (var server = new HttpServer(_config))
            {

                var client = new HttpClient(server);
                var stringContent = string.Format("{{\"EmailAddress\":\" {0} \" }}",
                    EmailAddress);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(RegisterUrl),
                    Method = HttpMethod.Post,
                    Content = new StringContent(stringContent,
                    Encoding.UTF8, "application/json")
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.SendAsync(request).Result)
                {
                    Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                    var apiKey = response.Content.ReadAsAsync<string>().Result;
                    Assert.IsNotNull(apiKey);
                    Assert.IsNotEmpty(apiKey);
                }
            }

        }

        [Test]
        public void RegisterNoDuplicates()
        {
            using (var server = new HttpServer(_config))
            {

                var client = new HttpClient(server);
                var stringContent = string.Format("{{\"EmailAddress\":\" {0} \" }}",
                    EmailAddress);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(RegisterUrl),
                    Method = HttpMethod.Post,
                    Content = new StringContent(stringContent,
                    Encoding.UTF8, "application/json")
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SendAsync(request);
                using (var response = client.SendAsync(request).Result)
                {
                    Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
                }
            }
        }

        [Test]
        public void RegisterInvalidEmail()
        {
            using (var server = new HttpServer(_config))
            {

                var client = new HttpClient(server);
                var stringContent = string.Format("{{\"EmailAddress\":\" notavalidemail \" }}",
                    EmailAddress);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(RegisterUrl),
                    Method = HttpMethod.Post,
                    Content = new StringContent(stringContent,
                    Encoding.UTF8, "application/json")
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.SendAsync(request).Result)
                {
                    Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
                }
            }
        }
    }
}
