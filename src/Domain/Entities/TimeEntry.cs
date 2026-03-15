using System;
using ProjectManagementERP.Shared.Base;

namespace ProjectManagementERP.Domain.Entities
{
    public class TimeEntry : BaseEntity
    {
        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = default!;
        public Guid UserId { get; set; }
        public decimal Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
