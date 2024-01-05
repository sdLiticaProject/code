using System.ComponentModel.DataAnnotations;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.Models.Dashboard
{

    public class DashboardMetadataModel
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }

        [Required] public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }


        /// <summary>
        /// no-args constructor
        /// </summary>
        public DashboardMetadataModel()
        {
        }

        /// <summary>
        /// constructs from the object which this model represents
        /// </summary>
        /// <param name="timeSeriesMetadata"></param>
        public DashboardMetadataModel(DashboardMetadata dashboardMetadata)
        {
            Id = dashboardMetadata.Id.ToString();
            Title = dashboardMetadata.Title;
            Description = dashboardMetadata.Description;
            UserId = dashboardMetadata.UserId.ToString();
        }
    }
}