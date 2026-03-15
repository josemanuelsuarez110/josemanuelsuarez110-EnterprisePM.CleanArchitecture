using System;

namespace ProjectManagementERP.Application.DTOs
{
    public class ResourceAllocationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal AllocationPercentage { get; set; }
    }
}
