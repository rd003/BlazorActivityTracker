using Microsoft.AspNetCore.Components;
using DataAccess.Repositories;
using BlazorActivityTracker.Helpers;
using Microsoft.JSInterop;
using BlazorActivityTracker.ViewModels;


namespace BlazorActivityTracker.Pages.ManageActivity;
public partial class ManageActivity
{
    [Inject]
    ILogger<ManageActivity> logger { get; set; }
    [Inject]
    private IActivityRepository activityRepos { get; set; }

    [Inject]
    private IJSRuntime jSRuntime { get; set; }

    DataAccess.Models.Activity activity = new DataAccess.Models.Activity
    {
        ActivityDate = DateTime.UtcNow
    };

    List<DataAccess.Models.Activity> activities = new List<DataAccess.Models.Activity>();

    int state = (int)States.None;
    string message = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadActivities();
    }


    async void HandleSubmit(DataAccess.Models.Activity activity)
    {
        try
        {
            state = (int)States.Pending;
            bool isActivityDateExists = await activityRepos.IsActivityDateExists(activity.ActivityDate, activity.Id);
            if (isActivityDateExists)
            {
                state = (int)States.Error;
                message = "This activity date already exists,please select different one.";
            }
            else
            {
                if (activity.Id == 0)
                {
                    await activityRepos.AddActivityAsync(activity);
                    activities.Add(activity);
                }
                else
                    await activityRepos.UpdateActivityAsync(activity);
                message = "Saved successfully";
                state = (int)States.Completed;
                HandleReset();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            message = "Error has occured";
            state = (int)States.Error;
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
        try
        {
            bool result = await jSRuntime.InvokeAsync<bool>("confirm", "Are you sure to delete??");
            if (result)
            {
                state = (int)States.Pending;
                message = "deleted successfully";
                await activityRepos.DeleteActivityAsync(activityToDelete.Id);
                activities.Remove(activityToDelete);
                state = (int)States.Completed;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            message = "Could not deleted";
            state = (int)States.Error;
        }

    }


    async Task LoadActivities(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            state = (int)States.Pending;
            if (startDate != null && endDate != null)
            {
                activities = (await activityRepos.GetAllActivitiesAsync(startDate, endDate)).ToList();

            }
            else
            {
                activities = (await activityRepos.GetAllActivitiesAsync()).ToList();
            }
            state = (int)States.Completed;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            message = "Error has occured";
            state = (int)States.Error;
        }
    }



    async Task HandleSelectDate(FilterModel filters)
    {
        var startDate = filters.StartDate;
        var endDate = filters.EndDate;
        if (startDate != null & endDate != null)
        {
            await LoadActivities(filters.StartDate, filters.EndDate);
        }
    }

    async void HandleLoadAll()
    {
        await LoadActivities();
        StateHasChanged();
    }



}