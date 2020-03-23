﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public class AnalyticsOperation: IAnalyticsOperation
    {
        public string OpName { get; set; }
        public string TsId { get; set; }
        public IOperationParameter[] Parameters { get; set; }

    }
}
