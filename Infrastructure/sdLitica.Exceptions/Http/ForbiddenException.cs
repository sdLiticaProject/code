using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using sdLitica.Exceptions.Abstractions;

namespace sdLitica.Exceptions.Http
{
    public class ForbiddenException : BaseExceptionModel 
    {
        public ForbiddenException()
            : base()
        {
            Initialize();
        }

        public ForbiddenException(string message)
            : base(message)
        {
            Initialize();
        }

        public ForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        private void Initialize()
        {
            base.StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
