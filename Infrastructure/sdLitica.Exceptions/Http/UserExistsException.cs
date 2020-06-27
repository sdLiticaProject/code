using sdLitica.Exceptions.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace sdLitica.Exceptions.Http
{
    public class UserExistsException : BaseExceptionModel 
    {
        public UserExistsException()
            : base()
        {
            Initialize();
        }

        public UserExistsException(string message)
            : base(message)
        {
            Initialize();
        }

        public UserExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        private void Initialize()
        {
            base.StatusCode = HttpStatusCode.Conflict;
        }
    }
}
