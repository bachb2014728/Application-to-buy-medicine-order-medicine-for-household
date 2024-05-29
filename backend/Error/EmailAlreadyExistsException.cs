using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace backend.Error
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
        {
        }

        public EmailAlreadyExistsException(string? email) : base($"Email {email} đã tồn tại.")
        {
        }

        public EmailAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}