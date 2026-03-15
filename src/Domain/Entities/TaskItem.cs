using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Base;

namespace ProjectManagementERP.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public decimal EstimatedHours { get; set; }
        public Guid? AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        [NotMapped]
        public decimal ActualHours => TimeEntries?.Sum(t => t.Hours) ?? 0m;
    }
}
