using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Exceptions
{
    public class NotFoundException : BaseExceptionModel
    {
        public NotFoundException()
          : base()
        {
            Initialize();
        }

        public NotFoundException(string message)
            : base(message)
        {
            Initialize();
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        private void Initialize()
        {
            base.StatusCode = HttpStatusCode.NotFound;
        }
    }
}
