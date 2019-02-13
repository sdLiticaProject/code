﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Exceptions
{
    public class InvalidRequestException : BaseExceptionModel
    {
        public InvalidRequestException()
            : base()
        {
            Initialize();
        }

        public InvalidRequestException(string message)
            : base(message)
        {
            Initialize();
        }

        public InvalidRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        private void Initialize()
        {
            base.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
