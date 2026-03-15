using System;
using MediatR;
using ProjectManagementERP.Application.DTOs.Reports;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Queries.Reports
{
    public class GetProjectTimeReportQuery : IRequest<Result<ProjectTimeReportDto>>
    {
        public Guid ProjectId { get; set; }
    }
}
