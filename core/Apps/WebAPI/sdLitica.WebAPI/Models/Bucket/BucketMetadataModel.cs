using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.WebAPI.Entities.Common
{

    public class BucketMetadataModel
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string DateCreated { get; set; }

        public string DateModified { get; set; }

        public string UserId { get; set; }

        public string InfluxId { get; set; }
        
        [Required]
        public int RetentionPeriod { get; set; }

        
        /// <summary>
        /// no-args constructor
        /// </summary>
        public BucketMetadataModel() { }

        /// <summary>
        /// constructs from the object which this model represents
        /// </summary>
        /// <param name="timeSeriesMetadata"></param>
        public BucketMetadataModel(BucketMetadata bucketMetadata)
        {
            Id = bucketMetadata.Id.ToString();
            Name = bucketMetadata.Name;
            Description = bucketMetadata.Description;
            DateCreated = bucketMetadata.DateCreated.ToString();
            DateModified = bucketMetadata.DateModified.ToString();
            UserId = bucketMetadata.UserId.ToString();
            InfluxId = bucketMetadata.InfluxId.ToString();
            RetentionPeriod = bucketMetadata.RetentionPeriod;
        }
    }
}