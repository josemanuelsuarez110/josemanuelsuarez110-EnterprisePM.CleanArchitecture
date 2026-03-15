using System;
using System.Collections.Generic;

namespace ProjectManagementERP.Application.DTOs.Reports
{
    public class UserTimeReportDto
    {
        public Guid UserId { get; set; }
        public decimal TotalHours { get; set; }
        public IEnumerable<ProjectHours> Projects { get; set; } = new List<ProjectHours>();
    }

    public class ProjectHours
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public decimal Hours { get; set; }
    }
}
