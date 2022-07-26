﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Services.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetMainPopularFranchisesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetMainPopularFranchisesAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetMainPopularFranchises());
            var component = new FranchiseService(null, FranchiseRepository, NotificationsService, UserRepository, NotificationsRepository);
            var result = await component.GetMainPopularFranchises();

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task GetFranchisesListAsyncAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetFranchisesListAsync());
            var component = new FranchiseService(null, FranchiseRepository, NotificationsService, UserRepository, NotificationsRepository);
            var result = await component.GetFranchisesListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
