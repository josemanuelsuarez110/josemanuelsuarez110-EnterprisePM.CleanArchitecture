using System;
using System.Collections.Generic;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Projects
{
    public class GetProjectTimelineQuery : IRequest<Result<IEnumerable<MilestoneDto>>>
    {
        public Guid ProjectId { get; set; }
    }
}
