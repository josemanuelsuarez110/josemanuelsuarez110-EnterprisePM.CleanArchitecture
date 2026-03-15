using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Commands.TimeEntries;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TimeEntriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireTeamMember")]
        public async Task<ActionResult<Result<TimeEntryDto>>> Log([FromBody] LogTimeEntryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
