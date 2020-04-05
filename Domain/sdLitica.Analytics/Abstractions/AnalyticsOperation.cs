using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics.Abstractions
{
    public class AnalyticsOperation: Entity//: IAnalyticsOperation
    {

        public AnalyticsOperation()
        {
        }
        public void SetId()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string OpName { get; set; } // all have to be protected set
        public string TsId { get; set; }
        //public Dictionary<string,string> Parameters { get; set; }
        //public List<OperationParameter> Parameters { get; set; }

        public int Status { get; set; }

    }
}
