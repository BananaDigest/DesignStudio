using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Models;
using NSubstitute;
using Xunit;

namespace DesignStudio.Tests
{
    public class ServiceManagerTests : TestBase
    {
        private readonly ServiceManager _serviceManager;
        private readonly IFixture _fixture;

        public ServiceManagerTests()
        {
            _serviceManager = new ServiceManager(Uow, Mapper);
            _fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            // Remove recursion behavior
            var toRemove = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            foreach (var b in toRemove)
                _fixture.Behaviors.Remove(b);
            // Add omit recursion
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task AddServiceAsync_AddsEntityAndCommits()
        {
            var dto = _fixture.Create<DesignServiceDto>();

            await _serviceManager.AddServiceAsync(dto);

            await Uow.Services.Received(1).AddAsync(Arg.Any<DesignService>());
            await Uow.Received(1).CommitAsync();
        }

        [Fact]
        public async Task GetServicesAsync_ReturnsMappedDtos()
        {
            var entities = _fixture.Create<List<DesignService>>();
            Uow.Services.GetAllAsync()
                .Returns(Task.FromResult((IEnumerable<DesignService>)entities));

            var dtos = await _serviceManager.GetServicesAsync();

            Assert.Equal(entities.Count, dtos.Count());
            for (int i = 0; i < entities.Count; i++)
            {
                Assert.Equal(entities[i].DesignServiceId, dtos.ElementAt(i).Id);
                Assert.Equal(entities[i].Name, dtos.ElementAt(i).Name);
            }
        }

        [Fact]
        public async Task UpdateServiceAsync_ExistingService_MapsAndCommits()
        {
            var dto = _fixture.Create<DesignServiceDto>();
            var entity = _fixture.Create<DesignService>();
            entity.DesignServiceId = dto.Id;
            Uow.Services.GetByIdAsync(dto.Id)
                .Returns(Task.FromResult(entity));

            await _serviceManager.UpdateServiceAsync(dto);

            Assert.Equal(dto.Name, entity.Name);
            Assert.Equal(dto.Description, entity.Description);
            Assert.Equal(dto.Price, entity.Price);
            await Uow.Received(1).CommitAsync();
        }

        [Fact]
        public async Task DeleteServiceAsync_ExistingService_RemovesAndCommits()
        {
            var id = _fixture.Create<int>();
            var entity = new DesignService { DesignServiceId = id };
            Uow.Services.GetByIdAsync(id)
                .Returns(Task.FromResult(entity));

            await _serviceManager.DeleteServiceAsync(id);

            Uow.Services.Received(1).Remove(entity);
            await Uow.Received(1).CommitAsync();
        }
    }
}