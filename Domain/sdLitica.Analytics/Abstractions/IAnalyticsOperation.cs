using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public interface IAnalyticsOperation
    {
        string OpName { get; }
        string TsId { get; }
        IOperationParameter[] Parameters { get; }
        

    }
}
