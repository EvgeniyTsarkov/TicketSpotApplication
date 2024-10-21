-- Add Venue manager

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[EventManagers]
           ([FirstName]
           ,[LastName]
           ,[Email])
     VALUES
           ('John'
           ,'Smith'
           ,'john.smith@mail.com')
GO


-- Add Venue

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[Venues]
           ([Name]
           ,[Address]
           ,[Description]
           ,[EventManagerId])
     VALUES
           ('Staples Center'
           ,'800 W Olympic Blvd #343, Los Angeles, CA 90015'
           ,'A renowned entertainment venue situated in LA'
           ,1)
GO

-- Add event

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[Events]
           ([Name]
           ,[Date]
           ,[Description]
           ,[EventManagerId]
           ,[VenueId])
     VALUES
           ('Lakers vs Celtics'
           ,'2024-12-25 19:00:00'
           ,'A regular season basketball game'
           ,1
           ,2)
GO


-- Add seats 

USE [TicketSpotDb]
GO

---- First seat

INSERT INTO [dbo].[Events]
           ([Name]
           ,[Date]
           ,[Description]
           ,[EventManagerId]
           ,[VenueId])
     VALUES
           ('Lakers vs Celtics'
           ,'2024-12-25 19:00:00'
           ,'A regular season basketball game'
           ,1
           ,2)
GO

---- Second seat

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[Seats]
           ([SeatNumber]
           ,[Section]
           ,[RowNumber]
           ,[EventId]
           ,[VenueId])
     VALUES
           (1
           ,'C'
           ,7
           ,2
           ,2)
GO


