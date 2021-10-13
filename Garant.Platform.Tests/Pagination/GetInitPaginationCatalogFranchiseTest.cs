﻿using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Pagination
{
    [TestClass]
    public class GetInitPaginationCatalogFranchiseTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetInitPaginationCatalogFranchiseAsync()
        {
            var mock = new Mock<IPaginationService>();
            mock.Setup(a => a.GetInitPaginationCatalogFranchiseAsync(1));
            var component = new PaginationService(PostgreDbContext);
            var result = await component.GetInitPaginationCatalogFranchiseAsync(1);

            Assert.IsTrue(result.Results != null);
        }
    }
}
