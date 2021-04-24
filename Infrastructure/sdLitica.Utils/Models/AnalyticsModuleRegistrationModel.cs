﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Utils.Models
{
    public class AnalyticsModuleRegistrationModel
    {
        public AnalyticsModuleRegistrationModel() { }
        public Guid ModuleGuid { get; set; }
        public IList<AnalyticsOperationModel> Operations { get; set; }
    }
}
