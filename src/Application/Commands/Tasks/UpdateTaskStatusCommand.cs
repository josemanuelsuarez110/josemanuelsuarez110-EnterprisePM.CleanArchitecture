using System;
using MediatR;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Tasks
{
    public class UpdateTaskStatusCommand : IRequest<Result>
    {
        public Guid TaskId { get; set; }
        public TaskStatus Status { get; set; }
    }
}
