﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Services.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetFranchisesCategoriesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetFranchisesCategoriesListAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetFranchisesCategoriesListAsync());
            var component = new FranchiseService(null, FranchiseRepository);
            var result = await component.GetFranchisesCategoriesListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
