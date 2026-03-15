using System;
using ProjectManagementERP.Shared.Base;

namespace ProjectManagementERP.Domain.Entities
{
    public class ResourceAllocation : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
        public decimal AllocationPercentage { get; set; }
    }
}
