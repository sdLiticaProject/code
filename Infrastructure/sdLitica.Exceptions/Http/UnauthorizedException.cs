using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using sdLitica.Exceptions.Abstractions;

namespace sdLitica.Exceptions.Http
{
    public class UnauthorizedException : BaseExceptionModel
    {
        public UnauthorizedException()
        : base()
        {
            Initialize();
        }

        public UnauthorizedException(string message)
            : base(message)
        {
            Initialize();
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        private void Initialize()
        {
            base.StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
