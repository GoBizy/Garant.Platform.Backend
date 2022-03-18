﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Services.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetFranchisesCitiesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetFranchisesCitiesListAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetFranchisesCitiesListAsync());
            var component = new FranchiseService(null, FranchiseRepository, NotificationsService, UserRepository, NotificationsRepository);
            var result = await component.GetFranchisesCitiesListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
