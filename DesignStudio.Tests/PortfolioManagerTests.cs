using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Models;
using NSubstitute;
using Xunit;

namespace DesignStudio.Tests
{
    public class PortfolioManagerTests : TestBase
    {
        private readonly PortfolioManager _manager;

        public PortfolioManagerTests()
        {
            _manager = new PortfolioManager(Uow, Mapper);
        }

        [Theory, SafeAutoData]
        public async Task GetPortfolioAsync_MapsAllItems(
            [Frozen] List<PortfolioItem> entities)
        {
            // Arrange
            Uow.Portfolio.GetWithIncludeAsync(Arg.Any<Expression<Func<PortfolioItem, object>>>())
               .Returns(Task.FromResult((IEnumerable<PortfolioItem>)entities));

            // Act
            var dtos = await _manager.GetPortfolioAsync();

            // Assert
            Assert.Equal(entities.Count, dtos.Count());
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var dto = dtos.ElementAt(i);
                Assert.Equal(entity.PortfolioItemId, dto.Id);
                Assert.Equal(entity.Title, dto.Title);
                Assert.Equal(entity.Description, dto.Description);
            }
        }
    }
}
