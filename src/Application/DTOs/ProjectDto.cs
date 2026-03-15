using System;
using System.Collections.Generic;
using ProjectManagementERP.Domain.Enums;

namespace ProjectManagementERP.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
        public IEnumerable<MilestoneDto> Milestones { get; set; } = new List<MilestoneDto>();
        public IEnumerable<ResourceAllocationDto> ResourceAllocations { get; set; } = new List<ResourceAllocationDto>();
    }
}
