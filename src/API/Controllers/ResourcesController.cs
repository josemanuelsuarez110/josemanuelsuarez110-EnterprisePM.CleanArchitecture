using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Commands.Resources;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<ResourceAllocationDto>>> Allocate([FromBody] AllocateResourceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<ResourceAllocationDto>>> Update(Guid id, [FromBody] UpdateResourceAllocationCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
