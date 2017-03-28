﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

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
                RequestUri = new Uri(String.Format("{0}/register", Url)),
                Method = HttpMethod.Post,
                Content = new StringContent(stringContent,
                Encoding.UTF8, "application/json"),
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.SendAsync(request).Result;
        }

        public HttpResponseMessage IncreaseBalance(string apiKey, int balance)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(String.Format("{0}/balances/{1}", Url, balance)),
                Method = HttpMethod.Put
            };
            request.Headers.Add("Authorization", apiKey);

            var client = new HttpClient(_server);
            return client.SendAsync(request).Result;
        }

    }
}
