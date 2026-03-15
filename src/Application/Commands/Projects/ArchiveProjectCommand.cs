using System;
using MediatR;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.Projects
{
    public class ArchiveProjectCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
