using System;
using System.Runtime.Serialization;

namespace User.Backend.Api.Core.Exceptions
{

    [Serializable]
    public class UserException : Exception
    {
        public UserException(string message)
        : base(message)
        { }

        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class UserNotFoundException : Exception
    {

        public UserNotFoundException(string message)
        : base(message)
        { }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
