﻿namespace Ordering.Domain.Exceptions
{
    internal class DomainException : Exception
    {
        public DomainException(string message) : base($"Domain Exception: \"{message}\" throws from Domain Layer.")
        {
        }
    }
}
