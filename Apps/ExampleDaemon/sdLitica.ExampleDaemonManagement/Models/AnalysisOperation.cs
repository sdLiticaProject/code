using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.ExampleDaemonManagement.Models
{
    public class AnalysisOperation
    {
        public AnalysisOperation()
        {

        }

        public string OpName { get; set; }
        public string TsId { get; set; }
        public List<OperationParameter> Parameters { get; set; }
    }
}
