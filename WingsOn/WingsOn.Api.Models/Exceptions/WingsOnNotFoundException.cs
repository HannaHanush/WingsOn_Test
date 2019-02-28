using System;

namespace WingsOn.Api.Models.Exceptions
{
    public class WingsOnNotFoundException : Exception
    {
        public WingsOnNotFoundException()
        {
        }

        public WingsOnNotFoundException(string message)
            : base(message)
        {
        }

        public WingsOnNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
