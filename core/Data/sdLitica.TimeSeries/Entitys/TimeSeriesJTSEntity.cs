using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sdLitica.TimeSeries.Services
{

    public class TimeSeriesJTSEntity
    {
        [Required]
        public string DocType { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public HeaderModel Header { get; set; }
        [Required]
        public List<DataModel> Data { get; set; }

        public class HeaderModel
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int RecordCount { get; set; }
            public Dictionary<string, Column> Columns { get; set; }
        }

        public class Column
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string DataType { get; set; }
            public string RenderType { get; set; }
            public string Format { get; set; }
            public string Aggregate { get; set; }
        }

        public class DataModel
        {
            public DateTime Ts { get; set; }
            public Dictionary<string, ValueModel> F { get; set; }
        }

        public class ValueModel
        {
            public object V { get; set; }
            public object Q { get; set; }
            public object A { get; set; }
        }

        public TimeSeriesJTSEntity()
        {
        }

        public TimeSeriesJTSEntity(TimeSeriesJTSEntity timeSeriesJtsEntity)
        {
            DocType = timeSeriesJtsEntity.DocType;
            Version = timeSeriesJtsEntity.Version;
            Header = timeSeriesJtsEntity.Header;
            Data = timeSeriesJtsEntity.Data;
        }
    }
}