using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.TimeSeries;
using sdLitica.TimeSeries.Services;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Models.TimeSeries;

namespace sdLitica.WebAPI.Controllers.v1
{

    [Route("api/v1/bucket")]
    [Authorize]
    public class BucketController : BaseApiController
    {
        private readonly IBucketService _bucketService;
        private readonly IBucketMetadataService _bucketMetadataService;

        public BucketController(IBucketService bucketService, IBucketMetadataService bucketMetadataService)
        {
            _bucketService = bucketService;
            _bucketMetadataService = bucketMetadataService;
        }
        
        [HttpPost]
        public IActionResult CreateBucket([FromBody] BucketMetadataModel bucketModel)
        {
            var bucketInfluxId = _bucketService.CreateBucket(Guid.NewGuid().ToString(), bucketModel.RetentionPeriod);
            Task<BucketMetadata> t = _bucketMetadataService.AddBucketMetadata(bucketModel.Name, bucketModel.Description, UserId, bucketModel.RetentionPeriod, bucketInfluxId.Result);
            return Ok(new BucketMetadataModel(t.Result));
        }
        
        [HttpPost]
        [Route("{bucketMetadataId}")]
        public IActionResult UpdateBucket([FromRoute] string bucketMetadataId, [FromBody] BucketMetadataModel bucketModel)
        {
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(bucketMetadataId);
            if (bucketMetadata == null || !bucketMetadata.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this bucket");
            }

            _bucketService.UpdateBucketByName(bucketMetadata.InfluxId, bucketModel.RetentionPeriod);
            _bucketMetadataService.UpdateBucketMetadata(bucketMetadataId, bucketModel.Name, bucketModel.Description, bucketModel.RetentionPeriod).Wait();
            return Ok("ok");
        }
        
        [HttpDelete]
        [Route("{bucketMetadataId}")]
        public IActionResult DeleteBucket([FromRoute] string bucketMetadataId)
        {
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(bucketMetadataId);
            if (bucketMetadata == null || !bucketMetadata.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this bucket");
            }

            _bucketService.DeleteBucketByName(bucketMetadata.InfluxId);
            _bucketMetadataService.DeleteBucketMetadata(bucketMetadataId).Wait();
            return Ok("ok");
        }
        
        [HttpGet]
        public IActionResult GetAllBucket()
        {
            List<BucketMetadata> t = _bucketMetadataService.GetByUserId(UserId);
            List<BucketMetadataModel> list = new List<BucketMetadataModel>();
            t.ForEach(e => list.Add(new BucketMetadataModel(e)));
            return Ok(list);
        }
        
        [HttpGet]
        [Route("{bucketMetadataId}")]
        public IActionResult GetBucket([FromRoute] string bucketMetadataId)
        {
            var t = _bucketMetadataService.GetBucketMetadata(bucketMetadataId);
            if (t == null || !t.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this bucket");
            }

            return Ok(new BucketMetadataModel(t));
        }
    }
}