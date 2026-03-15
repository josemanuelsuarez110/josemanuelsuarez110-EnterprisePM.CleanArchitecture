using System;

namespace ProjectManagementERP.Domain.ValueObjects
{
    public record DateRange(DateTime Start, DateTime? End)
    {
        public bool IsValid() => !End.HasValue || End.Value >= Start;
    }
}
