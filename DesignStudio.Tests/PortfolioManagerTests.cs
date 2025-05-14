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
using AutoMapper;
using Moq;
using DesignStudio.DAL.Repositories;
using DesignStudio.BLL.Mapping;
using AutoFixture.Kernel;

namespace DesignStudio.Tests
{
    public class PortfolioManagerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly IMapper _mapper;
        private readonly PortfolioManager _manager;
        private readonly Fixture _fixture;

        public PortfolioManagerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var repoMock = new Mock<IGenericRepository<PortfolioItem>>();
            _uowMock.Setup(u => u.Portfolio).Returns(repoMock.Object);

            _manager = new PortfolioManager(_uowMock.Object, _mapper);
        }

        [Fact]
        public async Task GetPortfolioAsync_MapsAllItems()
        {
            var entities = _fixture.CreateMany<PortfolioItem>(2).ToList();
            _uowMock.Setup(u => u.Portfolio.GetWithIncludeAsync(It.IsAny<Expression<Func<PortfolioItem, object>>>()))
                .ReturnsAsync(entities);

            var dtos = await _manager.GetPortfolioAsync();

            Assert.Equal(entities.Count, dtos.Count());
            Assert.Equal(entities.First().Title, dtos.First().Title);
        }
    }
}
