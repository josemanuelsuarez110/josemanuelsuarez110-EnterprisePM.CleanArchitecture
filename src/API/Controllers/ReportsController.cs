using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.DTOs.Reports;
using ProjectManagementERP.Application.Queries.Reports;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("project-time")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<ProjectTimeReportDto>>> GetProjectTime([FromQuery] Guid projectId)
        {
            var result = await _mediator.Send(new GetProjectTimeReportQuery { ProjectId = projectId });
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("user-time")]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Result<UserTimeReportDto>>> GetUserTime([FromQuery] Guid userId)
        {
            var result = await _mediator.Send(new GetUserTimeReportQuery { UserId = userId });
            return Ok(result);
        }
    }
}
