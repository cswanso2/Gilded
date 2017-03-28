using Gilded.Models;
using IntegrationTests.Sdk;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IntegrationTests
{
    public class ItemsTest : IntegrationTestsBase
    {
        private const string Url = "http://localhost:56882/api/items";
        private const string AdminApiKey = "adminapikey";
        private readonly Item TestItemOne = new Item() { 
            Description = "Description",
            Price = 5
        };

        private readonly Item User = new Item()
        {
            Name = "NameTwo",
            Description = "DescriptionTwo",
            Price = 6
        };

        
        [Test]
        [TestCase("CreateItems")]
        public void CreateItems(string itemName)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var sdk = new ItemsSdk(server);
                using (var response = sdk.CreateItem(TestItemOne, AdminApiKey))
                {
                    Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                }
            }
        }

        [Test]
        [TestCase("CreateItemsTwo")]
        public void CreateItemsFakeUser(string itemName)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var sdk = new ItemsSdk(server);
                using (var response = sdk.CreateItem(TestItemOne, "aaa"))
                {
                    Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
                }
            }
        }


        [Test]
        [TestCase("IncreaseInventory")]
        public void IncreaseInventory(string itemName)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var sdk = new ItemsSdk(server);
                sdk.CreateItem(TestItemOne, AdminApiKey);

                using (var response = sdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey))
                {
                    Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                }
            }
        }

        [Test]
        [TestCase("GetItems")]
        public void GetItems(string itemNameOne)
        {
            using (var server = new HttpServer(_config))
            {
                var sdk = new ItemsSdk(server);
                sdk.CreateItem(TestItemOne, AdminApiKey);

                sdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey);
                using (var response = sdk.GetItems())
                {
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    string data = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    var values = JSserializer.Deserialize<List<Item>>(data);
                    Assert.AreEqual(1, values.Count);
                    var firstItem = values[0];
                    Assert.AreEqual(TestItemOne.Name, firstItem.Name);
                    Assert.AreEqual(TestItemOne.Description, firstItem.Description);
                    Assert.AreEqual(TestItemOne.Price, firstItem.Price);
                }
            }
        }
    }
}
