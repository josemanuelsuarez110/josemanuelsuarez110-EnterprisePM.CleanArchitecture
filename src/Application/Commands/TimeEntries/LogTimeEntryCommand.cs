using System;
using MediatR;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Commands.TimeEntries
{
    public class LogTimeEntryCommand : IRequest<Result<TimeEntryDto>>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public decimal Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
