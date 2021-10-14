﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.Blog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class GetTopNewsMainPageTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetTopNewsMainPageAsyncTest()
        {
            var mock = new Mock<IBlogService>();
            mock.Setup(a => a.GetTopNewsMainPageAsync());
            var component = new BlogService(PostgreDbContext);
            var result = await component.GetTopNewsMainPageAsync();

            Assert.IsTrue(result.Any());
        }
    }
}