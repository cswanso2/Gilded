using Gilded.Models;
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
using System.Xml.Linq;

namespace IntegrationTests
{
    public class ItemsTest : IntegrationTestsBase
    {
        private const string Url = "http://localhost:56882/api/items";

        private readonly Item TestItemOne = new Item() { Name = "Name",
            Description = "Description",
            Price = 5
        };

        private readonly Item TestItemTwo = new Item()
        {
            Name = "NameTwo",
            Description = "DescriptionTwo",
            Price = 6
        };

        private readonly Item User = new Item()
        {
            Name = "NameTwo",
            Description = "DescriptionTwo",
            Price = 6
        };

        /*
         * Lack of a database makes this harder to test. Using get endpoint to test post endpoint. Would normally query database.
         */
        [Test]
        public void PostItems()
        {
            using (var server = new HttpServer(_config))
            {

                var client = new HttpClient(server);
                var content = new { name = "bla",
                    description = "aaa",
                price = 5 };
                var stringContent = string.Format("{{\"Name\":\" {0} \", \"Description\":\"{1}\", \"Price\":{2}}}",
                    TestItemOne.Name,
                    TestItemOne.Description,
                    TestItemOne.Price);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(Url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(stringContent,
                    Encoding.UTF8, "application/json")
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.SendAsync(request).Result)
                {
                    Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                }

                var getRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(Url),
                    Method = HttpMethod.Get
                };

                getRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.SendAsync(getRequest).Result)
                {
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    var values = response.Content.ReadAsAsync<List<Item>>().Result;
                    Assert.AreEqual(1, values.Count);
                    var firstItem = values[0];
                    Assert.AreEqual(TestItemOne.Name, firstItem.Name);
                    Assert.AreEqual(TestItemOne.Description, firstItem.Description);
                    Assert.AreEqual(TestItemOne.Price, firstItem.Price);
                }
            }
        }

        [Test]
        public void GetItems()
        {
            using (var server = new HttpServer(_config))
            {

                var client = new HttpClient(server);
                var content = new
                {
                    name = "bla",
                    description = "aaa",
                    price = 5
                };
                var stringContent = string.Format("{{\"Name\":\" {0} \", \"Description\":\"{1}\", \"Price\":{2}}}",
                    TestItemOne.Name,
                    TestItemOne.Description,
                    TestItemOne.Price);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(Url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(stringContent,
                    Encoding.UTF8, "application/json")
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var postResult = client.SendAsync(request).Result;

                var stringContentTwo = string.Format("{{\"Name\":\" {0} \", \"Description\":\"{1}\", \"Price\":{2}}}",
                    TestItemTwo.Name,
                    TestItemTwo.Description,
                    TestItemTwo.Price);

                request.Content = new StringContent(stringContentTwo, Encoding.UTF8, "application/json");

                var getRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(Url),
                };

                getRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.SendAsync(getRequest).Result)
                {
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    var values = response.Content.ReadAsAsync<List<Item>>().Result;
                    Assert.AreEqual(2, values.Count);
                    var firstItem = values[0];
                    Assert.AreEqual(TestItemOne.Name, firstItem.Name);
                    Assert.AreEqual(TestItemOne.Description, firstItem.Description);
                    Assert.AreEqual(TestItemOne.Price, firstItem.Price);
                }
            }
        }
    }
}
