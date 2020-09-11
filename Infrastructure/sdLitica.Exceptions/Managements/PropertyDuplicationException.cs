﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using sdLitica.Exceptions.Abstractions;

namespace sdLitica.Exceptions.Managements
{
    public class PropertyDuplicationException : BaseExceptionModel
    {
        public PropertyDuplicationException(string propName, string propValue) 
            : base($"The property '{propName}' value '{propValue}' already exists in the system")
        {
            base.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
