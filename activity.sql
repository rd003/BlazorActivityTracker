create database ActivityTracker

use ActivityTracker

create table Activity
(
 Id int primary key identity,
 ActivityDate datetime not null,
 TotalHours float not null,
 [Description] nvarchar(200) not null
)

create procedure dbo.GetActivities
@startDate datetime=null,
@endDate datetime= null
as
begin
if(@startDate is null or @endDate is null)
begin
  select top 7 * from Activity order by id desc
end

else
begin
  select * from Activity where ActivityDate between @startDate and @endDate
end
end

/* ** Get activity by date** */

select top 1 Id from Activity where Convert(Date,ActivityDate)= CONVERT(Date,@activityDate) and Id!=@id