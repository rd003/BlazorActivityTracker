using System.Data;
using Dapper;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly IConfiguration _config;
    private readonly string connectionString;
    public ActivityRepository(IConfiguration config)
    {
        _config = config;
        connectionString = config.GetConnectionString("default");
    }

    public async Task<Activity> AddActivityAsync(Activity activity)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "insert into Activity (ActivityDate,TotalHours,Description) values(@ActivityDate,@TotalHours,@Description); select SCOPE_IDENTITY()";
        int createdId = await connection.ExecuteScalarAsync<int>(sql, new
        {
            activity.ActivityDate,
            activity.TotalHours,
            activity.Description
        });
        activity.Id = createdId;
        return activity;
    }

    public async Task UpdateActivityAsync(Activity activity)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "update Activity set ActivityDate=@ActivityDate,TotalHours=@TotalHours,Description=@Description where Id=@Id";
        await connection.ExecuteAsync(sql, activity);
    }

    public async Task DeleteActivityAsync(int id)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "delete from Activity where Id=@id";
        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task<Activity> FindActivityByIdAsync(int id)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "select * from Activity where Id=@id";
        var activity = await connection.QueryFirstOrDefaultAsync<Activity>(sql, new { id });
        return activity;
    }

    public async Task<IEnumerable<Activity>> GetAllActivitiesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        // string sql = "select * from Activity";
        var activities = await connection.QueryAsync<Activity>("getActivities @startDate,@endDate", new { startDate, endDate });
        return activities;
    }



}