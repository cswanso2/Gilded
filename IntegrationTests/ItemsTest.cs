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
                var response = sdk.CreateItem(TestItemOne, AdminApiKey);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Test]
        [TestCase("CreateDuplicateItems")]
        public void CreateItemsDuplicate(string itemName)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var sdk = new ItemsSdk(server);
                sdk.CreateItem(TestItemOne, AdminApiKey);
                var response = sdk.CreateItem(TestItemOne, AdminApiKey);
                Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
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
                var response = sdk.CreateItem(TestItemOne, "aaa");
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Test]
        [TestCase("CreateItemsThree", "nonAdmin@mail.com")]
        public void CreateItemsNonAdminUser(string itemName, string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var itemsSdk = new ItemsSdk(server);
                var userSdk = new UsersSdk(server);
                var registerResponse = userSdk.Register(emailAddress);
                var apiKey = userSdk.GetApiKeyFromRegisterResponse(registerResponse);

                var response = itemsSdk.CreateItem(TestItemOne, apiKey);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
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

                var response = sdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey);
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
            }
        }

        [Test]
        [TestCase("IncreaseInventory", "nonadmin2@mail.com")]
        public void IncreaseInventoryNonAdmin(string itemName, string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;
                var sdk = new ItemsSdk(server);
                sdk.CreateItem(TestItemOne, AdminApiKey);

                var userSdk = new UsersSdk(server);

                var registerResponse = userSdk.Register(emailAddress);
                var apiKey = userSdk.GetApiKeyFromRegisterResponse(registerResponse);

                var response = sdk.IncreaseInventory(TestItemOne.Name, 1, apiKey);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Test]
        [TestCase("get")]
        public void GetItems(string itemNameOne)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemNameOne;
                var sdk = new ItemsSdk(server);
                sdk.CreateItem(TestItemOne, AdminApiKey);

                sdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey);
                var response = sdk.GetItems();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                string data = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                var values = JSserializer.Deserialize<List<Item>>(data);
                var firstItem = values.SingleOrDefault(x => x.Name == itemNameOne);
                Assert.AreEqual(TestItemOne.Name, firstItem.Name);
                Assert.AreEqual(TestItemOne.Description, firstItem.Description);
                Assert.AreEqual(TestItemOne.Price, firstItem.Price);
            }
        }

        [Test]
        [TestCase("purchaseItem", "purchase@mail.com")]
        public void CreateAndPurchaseItem(string itemName, string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;

                var itemSdk = new ItemsSdk(server);
                itemSdk.CreateItem(TestItemOne, AdminApiKey);
                itemSdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey);

                var userSdk = new UsersSdk(server);
                var userResponse = userSdk.Register(emailAddress);
                var apiKey = userSdk.GetApiKeyFromRegisterResponse(userResponse);
                userSdk.IncreaseBalance(apiKey, TestItemOne.Price + 1);

                var response = itemSdk.PurchaseItem(itemName, apiKey);
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
            }
        }

        [Test]
        [TestCase("itemName", "purchase2@mail.com")]
        public void CreateAndPurchaseItemInsufficientFunds(string itemName, string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;

                var itemSdk = new ItemsSdk(server);
                itemSdk.CreateItem(TestItemOne, AdminApiKey);
                itemSdk.IncreaseInventory(TestItemOne.Name, 1, AdminApiKey);

                var userSdk = new UsersSdk(server);
                var userResponse = userSdk.Register(emailAddress);
                var apiKey = userSdk.GetApiKeyFromRegisterResponse(userResponse);
                userSdk.IncreaseBalance(apiKey, TestItemOne.Price - 1);

                var response = itemSdk.PurchaseItem(itemName, apiKey);
                Assert.AreEqual(HttpStatusCode.PaymentRequired, response.StatusCode);
            }
        }

        [Test]
        [TestCase("nopurchase", "purchase3@mail.com")]
        public void CreateAndPurchaseItemNoInventory(string itemName, string emailAddress)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;

                var itemSdk = new ItemsSdk(server);
                itemSdk.CreateItem(TestItemOne, AdminApiKey);

                var userSdk = new UsersSdk(server);
                var userResponse = userSdk.Register(emailAddress);
                var apiKey = userSdk.GetApiKeyFromRegisterResponse(userResponse);
                userSdk.IncreaseBalance(apiKey, TestItemOne.Price - 1);

                var response = itemSdk.PurchaseItem(itemName, apiKey);
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Test]
        [TestCase("noauth")]
        public void CreateAndPurchaseItemNoAuth(string itemName)
        {
            using (var server = new HttpServer(_config))
            {
                TestItemOne.Name = itemName;

                var itemSdk = new ItemsSdk(server);
                itemSdk.CreateItem(TestItemOne, AdminApiKey);

                var response = itemSdk.PurchaseItem(itemName);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}
