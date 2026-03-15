using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Commands.Projects;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Queries.Projects;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result<PagedResult<ProjectDto>>>> GetProjects([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] ProjectStatus? status = null)
        {
            var result = await _mediator.Send(new GetProjectsQuery { Page = page, PageSize = pageSize, Search = search, Status = status });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result<ProjectDto>>> GetProject(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery { Id = id });
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<ProjectDto>>> Create([FromBody] CreateProjectCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProject), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<ProjectDto>>> Update(Guid id, [FromBody] UpdateProjectCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult<Result>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand { Id = id });
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("{id}/archive")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result>> Archive(Guid id)
        {
            var result = await _mediator.Send(new ArchiveProjectCommand { Id = id });
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{id}/timeline")]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result>> GetTimeline(Guid id)
        {
            var result = await _mediator.Send(new GetProjectTimelineQuery { ProjectId = id });
            return Ok(result);
        }
    }
}
