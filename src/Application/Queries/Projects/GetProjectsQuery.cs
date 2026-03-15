using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Projects
{
    public class GetProjectsQuery : IRequest<Result<PagedResult<ProjectDto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public ProjectStatus? Status { get; set; }
        public bool UseCache { get; set; } = true;
    }
}
