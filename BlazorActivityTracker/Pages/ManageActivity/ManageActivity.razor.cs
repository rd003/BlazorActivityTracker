using Microsoft.AspNetCore.Components;

namespace BlazorActivityTracker.Pages.ManageActivity;
public partial class ManageActivity
{
    [Inject]
    ILogger<ManageActivity> logger { get; set; }
    DataAccess.Models.Activity activity = new DataAccess.Models.Activity
    {
        ActivityDate = DateTime.UtcNow
    };

    IEnumerable<DataAccess.Models.Activity> activities = new List<DataAccess.Models.Activity>{
        new DataAccess.Models.Activity{
          ActivityDate=DateTime.UtcNow,
          TotalHours=4,
          Description="working on c#"
        }
    };

    void HandleSubmit(DataAccess.Models.Activity activity)
    {
        try
        {
            logger.LogDebug("submitted");
            Console.WriteLine(activity.ActivityDate.ToString());
            Console.WriteLine(activity.TotalHours);
            Console.WriteLine(activity.Description);
            //TODO: Save data to database
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    void HandleReset()
    {
        activity = new DataAccess.Models.Activity
        {
            ActivityDate = DateTime.UtcNow
        };
        StateHasChanged();
    }


    void HandleEdit(
        DataAccess.Models.Activity submitedActivity)
    {
        activity = submitedActivity;
    }


    async Task HandleDelete(
       DataAccess.Models.Activity activityToDelete)
    {
        Console.WriteLine(activityToDelete.Id + " deleted");
    }

}