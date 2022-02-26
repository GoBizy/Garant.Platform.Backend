﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Services.Service.MainPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Category
{
    [TestClass]
    public class GetCategoriesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetCategoriesListAsyncTest()
        {
            var mock = new Mock<IMainPageService>();
            mock.Setup(a => a.GetCategoriesListAsync());
            var component = new MainPageService();
            var result = await component.GetCategoriesListAsync();

            Assert.IsTrue(result.ResultCol1.Any());
        }
    }
}
