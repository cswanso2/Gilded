using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IntegrationTests.Sdk
{
    public class UsersSdk
    {
        private readonly HttpServer _server;
        public UsersSdk(HttpServer server)
        {
            _server = server;
        }
        private const string Url = "http://localhost:56882/api/users";
        public HttpResponseMessage Register(string emailAddress)
        {
            var client = new HttpClient(_server);
            var stringContent = string.Format("{{\"EmailAddress\":\"{0}\"}}",
                    emailAddress);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(String.Format("{0}", Url, emailAddress)),
                Method = HttpMethod.Post,
                Content = new StringContent(stringContent,
                Encoding.UTF8, "application/json")
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client.SendAsync(request).Result;
        }

        public string GetApiKeyFromRegisterResponse(HttpResponseMessage response)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            var value = JSserializer.Deserialize<dynamic>(data);
            return  value["ApiKey"];
        }

        public HttpResponseMessage IncreaseBalance(string apiKey, int balance)
        {
            var client = new HttpClient(_server);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(String.Format("{0}/balances/{1}", Url, balance)),
                Method = HttpMethod.Put
            };
            request.Headers.Add("Authorization", apiKey);

            return client.SendAsync(request).Result;
        }

    }
}
