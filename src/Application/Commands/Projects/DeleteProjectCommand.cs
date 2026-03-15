using System;
using MediatR;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Projects
{
    public class DeleteProjectCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
