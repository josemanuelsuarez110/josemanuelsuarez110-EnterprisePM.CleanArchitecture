using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Commands.Milestones;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MilestonesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MilestonesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<MilestoneDto>>> Create([FromBody] CreateMilestoneCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<MilestoneDto>>> Update(Guid id, [FromBody] UpdateMilestoneCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
