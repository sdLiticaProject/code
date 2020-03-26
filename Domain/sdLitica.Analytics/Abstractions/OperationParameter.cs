using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public class OperationParameter: IOperationParameter
    {
        public OperationParameter()
        {

        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
