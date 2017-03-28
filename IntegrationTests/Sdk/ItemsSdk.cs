using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gilded.Models;
using System.Web.Http;
using System.Net.Http.Headers;

namespace IntegrationTests.Sdk
{
    public class ItemsSdk
    {
        private readonly HttpServer _server;
        public ItemsSdk(HttpServer server)
        {
            _server = server;
        }
        private const string Url = "http://localhost:56882/api/items";
        public HttpResponseMessage CreateItem(Item item, string apiKey)
        {
            var client = new HttpClient(_server);

            var stringContent = string.Format("{{\"Name\":\"{0}\", \"Description\":\"{1}\", \"Price\":{2}}}",
                    item.Name,
                    item.Description,
                    item.Price);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(Url),
                Method = HttpMethod.Put,
                Content = new StringContent(stringContent,
                Encoding.UTF8, "application/json"),
            };
            request.Headers.Add("Authorization", apiKey);


            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client.SendAsync(request).Result;
        }

        public HttpResponseMessage IncreaseInventory(string name, int amount, string apiKey)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(String.Format("{0}/{1}/inventory/{2}", Url, name, amount)),
                Method = HttpMethod.Put
            };

            request.Headers.Add("Authorization", apiKey);

            var client = new HttpClient(_server);

            return client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetItems()
        {
            var getRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(Url),
                Method = HttpMethod.Get
            };

            getRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var client = new HttpClient(_server);
            return client.SendAsync(getRequest).Result;
        }
    }
}
