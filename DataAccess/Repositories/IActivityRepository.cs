using DataAccess.Models;

namespace DataAccess.Repositories;

public interface IActivityRepository
{

    Task<Activity> AddActivityAsync(Activity activity);
    Task DeleteActivityAsync(int id);
    Task<Activity> FindActivityByIdAsync(int id);
    Task<IEnumerable<Activity>> GetAllActivitiesAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task UpdateActivityAsync(Activity activity);
    Task<bool> IsActivityDateExists(DateTime activityDate, int id);
}