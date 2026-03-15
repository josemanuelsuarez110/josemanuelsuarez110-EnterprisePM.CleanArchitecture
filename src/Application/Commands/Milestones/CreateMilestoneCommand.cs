using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Milestones
{
    public class CreateMilestoneCommand : IRequest<Result<MilestoneDto>>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
