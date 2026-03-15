using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProjectManagementERP.API;
using ProjectManagementERP.Application.DTOs;
using Xunit;

namespace ProjectManagementERP.IntegrationTests
{
    public class ProjectsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProjectsEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact(Skip = "Requires database/redis setup")]
        public async Task GetProjects_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/projects");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
