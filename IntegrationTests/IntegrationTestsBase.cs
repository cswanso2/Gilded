using Gilded;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace IntegrationTests
{
    public abstract class IntegrationTestsBase
    {
        protected readonly HttpConfiguration _config;

        protected IntegrationTestsBase()
        {
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config);
            //configure web api

        }

    }
}
