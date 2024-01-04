using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.TimeSeries.Services.Dashboard
{

    public interface IDashboardService
    {
        Task<DashboardMetadata> CreateDashboardMetadata(string title, string description, string userId);
        
        List<DashboardMetadata> GetByUserId(string userId);
        
        Task<DashboardMetadata> UpdateDashboardMetadata(string guid, string title, string description);

        Task DeleteDashboardtMetadata(string guid);
        
        DashboardMetadata getDashboardMetadata(string guid);
    }
}