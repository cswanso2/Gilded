using Gilded.Filters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ApiKeyFilterTests
    {
        private ApiKeyFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new ApiKeyFilter();
        }
    }
}
