using System;
using MediatR;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Tasks
{
    public class AssignTaskCommand : IRequest<Result>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
    }
}
