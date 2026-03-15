using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Projects
{
    public class GetProjectByIdQuery : IRequest<Result<ProjectDto>>
    {
        public Guid Id { get; set; }
    }
}
