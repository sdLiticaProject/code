using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public interface IAnalyticsOperation
    {
        string OpName { get; set; }
        string TsId { get; set; }
        List<OperationParameter> Parameters { get; set; } // IOperationParameter
        

    }
}
