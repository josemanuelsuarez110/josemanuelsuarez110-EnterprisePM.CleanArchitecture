using System;
using ProjectManagementERP.Domain.Enums;

namespace ProjectManagementERP.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public decimal EstimatedHours { get; set; }
        public decimal ActualHours { get; set; }
        public Guid? AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
