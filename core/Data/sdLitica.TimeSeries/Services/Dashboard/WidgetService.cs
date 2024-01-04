using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.TimeSeries.Services.Dashboard;

public class WidgetService : IWidgetService
{
    public Task<WidgetMetadata> CreateWidgetMetadata(string title, string description, string userId)
    {
        throw new System.NotImplementedException();
    }

    public List<WidgetMetadata> GetByDashboardId(string userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<WidgetMetadata> UpdateDashboardMetadata(string guid, string title, string description)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteWidgetMetadata(string guid)
    {
        throw new System.NotImplementedException();
    }

    public WidgetMetadata getWidgetMetadata(string guid)
    {
        throw new System.NotImplementedException();
    }
}