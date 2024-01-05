using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;
using sdLitica.Entities.Management;

namespace sdLitica.Entities.TimeSeries
{
    [Table("BUCKET_METADATA")]
    public class BucketMetadata: Entity
    {
        /// <summary>
        /// Creates a bucket with no parameters
        /// </summary>
        protected BucketMetadata()
        {
        }

        /// <summary>
        /// User-defined name of bucket
        /// </summary>
        [Column("NAME")]
        public string Name { get; protected set; }
        /// <summary>
        /// User-defined description of bucket
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; protected set; }
        /// <summary>
        /// Name of bucket in InfluxDB
        /// </summary>
        [Column("INFLUX_ID")]
        public string InfluxId { get; protected set; }
        /// <summary>
        /// Date-time of creation
        /// </summary>
        [Column("DATE_CREATED")]
        public DateTime DateCreated { get; protected set; }
        
        [Column("DATE_MODIFIED")]
        public DateTime DateModified { get; protected set; }

        [Column("RETENTION_PERIOD")] 
        public int RetentionPeriod { get; protected set; }
        
        [Column("USER_ID")]
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Navigation property for user owning the bucket
        /// </summary>
        public User User { get; protected set; }

        public void Modify(string name, string description)
        {
            Name = name;
            Description = description;
            DateModified = DateTime.Now;
        }
        
        public void Modify(string name, string description, int retentionPeriod)
        {
            Name = name;
            Description = description;
            DateModified = DateTime.Now;
            RetentionPeriod = retentionPeriod;
        }

        /// <summary>
        /// Create new bucket metadata entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static BucketMetadata Create(string name, User owner, int retentionPeriod, string influxId, string description = "")
        {
            BucketMetadata bucketMetadata = new BucketMetadata()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                User = owner,
                InfluxId = influxId,
                RetentionPeriod = retentionPeriod
            };
            return bucketMetadata;
        }
    }
}