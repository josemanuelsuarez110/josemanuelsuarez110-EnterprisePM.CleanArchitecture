using System;

namespace ProjectManagementERP.Application.DTOs
{
    public class MilestoneDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
