using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Projects
{
    public class UpdateProjectCommand : IRequest<Result<ProjectDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
