using System;

namespace ProjectManagementERP.Application.DTOs
{
    public class TimeEntryDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public decimal Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
