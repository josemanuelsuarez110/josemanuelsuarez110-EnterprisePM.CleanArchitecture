using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectManagementERP.Application.Commands.Projects;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Handlers;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Application.Interfaces.Services;
using ProjectManagementERP.Application.Mapping;
using ProjectManagementERP.Shared.Utilities;
using Xunit;

namespace ProjectManagementERP.UnitTests
{
    public class ProjectHandlersTests
    {
        private readonly IMapper _mapper;

        public ProjectHandlersTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateProject_ShouldReturnProjectDto()
        {
            var projectRepo = new Mock<IProjectRepository>();
            var uow = new Mock<IUnitOfWork>();
            var cache = new Mock<ICacheService>();

            projectRepo.Setup(r => r.AddAsync(It.IsAny<ProjectManagementERP.Domain.Entities.Project>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateProjectCommandHandler(projectRepo.Object, uow.Object, _mapper, cache.Object);
            var command = new CreateProjectCommand { Name = "Test", StartDate = System.DateTime.UtcNow };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeOfType<ProjectDto>();
        }
    }
}
