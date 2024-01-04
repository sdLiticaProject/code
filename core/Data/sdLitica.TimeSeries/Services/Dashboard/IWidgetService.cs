using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.TimeSeries.Services.Dashboard
{

    public interface IWidgetService
    {
        Task<WidgetMetadata> CreateWidgetMetadata(string title, string description, string userId);
        
        List<WidgetMetadata> GetByDashboardId(string userId);
        
        Task<WidgetMetadata> UpdateDashboardMetadata(string guid, string title, string description);

        Task DeleteWidgetMetadata(string guid);
        
        WidgetMetadata getWidgetMetadata(string guid);
    }
}