using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Tasks
{
    public class CreateTaskCommand : IRequest<Result<TaskDto>>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public decimal EstimatedHours { get; set; }
        public Guid? AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
