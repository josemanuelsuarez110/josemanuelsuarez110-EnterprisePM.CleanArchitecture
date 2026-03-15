using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Resources
{
    public class AllocateResourceCommand : IRequest<Result<ResourceAllocationDto>>
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal AllocationPercentage { get; set; }
    }
}
