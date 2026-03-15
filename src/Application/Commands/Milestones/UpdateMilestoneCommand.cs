using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Milestones
{
    public class UpdateMilestoneCommand : IRequest<Result<MilestoneDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
