using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace backend.Error
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException()
        {
        }

        public StoreNotFoundException(string? message) : base(message)
        {
        }

        public StoreNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StoreNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}