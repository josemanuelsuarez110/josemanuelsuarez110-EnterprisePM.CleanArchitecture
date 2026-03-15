using System;
using System.Collections.Generic;

namespace ProjectManagementERP.Application.DTOs.Reports
{
    public class ProjectTimeReportDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public decimal TotalHours { get; set; }
        public IEnumerable<TaskTimeBreakdown> Tasks { get; set; } = new List<TaskTimeBreakdown>();
    }

    public class TaskTimeBreakdown
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public decimal Hours { get; set; }
    }
}
