using System;

namespace ProjectManagementERP.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"{name} ({key}) was not found") { }
    }

    public class ValidationFailedException : Exception
    {
        public ValidationFailedException(string message) : base(message) { }
    }
}
