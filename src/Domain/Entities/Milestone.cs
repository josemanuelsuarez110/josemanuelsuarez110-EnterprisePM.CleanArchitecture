using System;
using ProjectManagementERP.Shared.Base;

namespace ProjectManagementERP.Domain.Entities
{
    public class Milestone : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }
}
