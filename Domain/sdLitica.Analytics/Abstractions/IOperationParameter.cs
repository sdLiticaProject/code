using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public interface IOperationParameter
    {
        string Name { get; }
        string Value { get; }
    }
}
