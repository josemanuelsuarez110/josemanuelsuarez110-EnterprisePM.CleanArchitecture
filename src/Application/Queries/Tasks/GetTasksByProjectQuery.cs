using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Tasks
{
    public class GetTasksByProjectQuery : IRequest<Result<PagedResult<TaskDto>>>
    {
        public Guid ProjectId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
