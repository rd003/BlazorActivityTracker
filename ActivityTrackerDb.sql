create database [ActivityTracker]

USE [ActivityTracker]


CREATE TABLE [dbo].[Activity]
(
	[Id] [int] primary key IDENTITY(1,1),
	[ActivityDate] [datetime] NOT NULL,
	[TotalHours] [float] NOT NULL,
	[Description] [nvarchar](200) NOT NULL
)
