using System;
using MediatR;
using ProjectManagementERP.Application.DTOs.Reports;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Reports
{
    public class GetUserTimeReportQuery : IRequest<Result<UserTimeReportDto>>
    {
        public Guid UserId { get; set; }
    }
}
