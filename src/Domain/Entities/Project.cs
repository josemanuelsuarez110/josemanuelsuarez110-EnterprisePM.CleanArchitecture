using System;
using System.Collections.Generic;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Base;

namespace ProjectManagementERP.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planned;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public ICollection<ResourceAllocation> ResourceAllocations { get; set; } = new List<ResourceAllocation>();

        public void Archive()
        {
            Status = ProjectStatus.Archived;
            Touch();
        }
    }
}
