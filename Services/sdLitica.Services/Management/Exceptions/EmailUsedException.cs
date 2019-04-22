using Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace sdLitica.Services.Management.Exceptions
{
    public class EmailUsedException : BaseExceptionModel
    {
        public EmailUsedException(string email) 
            : base($"The Email '{email}' is in use")
        {
            base.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
