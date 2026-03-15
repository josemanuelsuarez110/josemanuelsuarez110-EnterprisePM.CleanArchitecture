using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Resources
{
    public class UpdateResourceAllocationCommand : IRequest<Result<ResourceAllocationDto>>
    {
        public Guid Id { get; set; }
        public decimal AllocationPercentage { get; set; }
    }
}
