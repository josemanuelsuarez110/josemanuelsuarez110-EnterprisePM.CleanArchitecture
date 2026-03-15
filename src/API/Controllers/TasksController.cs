using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Commands.Tasks;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Queries.Tasks;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/projects/{projectId}/tasks")]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result<PagedResult<TaskDto>>>> GetProjectTasks(Guid projectId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetTasksByProjectQuery { ProjectId = projectId, Page = page, PageSize = pageSize });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<TaskDto>>> Create([FromBody] CreateTaskCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPost("{id}/assign")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result>> Assign(Guid id, [FromBody] AssignTaskCommand command)
        {
            command.TaskId = id;
            var result = await _mediator.Send(command);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result>> UpdateStatus(Guid id, [FromBody] UpdateTaskStatusCommand command)
        {
            command.TaskId = id;
            var result = await _mediator.Send(command);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
