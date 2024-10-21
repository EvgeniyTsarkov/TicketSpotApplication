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
           ,1)
GO


-- Add sections

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[Sections]
           ([Name])
     VALUES
           ('A'),
		   ('B'),
		   ('C'),
		   ('D'),
		   ('E')
GO


-- Add seats 

---- First seat

INSERT INTO [dbo].[Seats]
           ([SeatNumber]
           ,[SectionId]
           ,[RowNumber]
           ,[EventId]
           ,[VenueId])
     VALUES
           (5
           ,1
           ,5
           ,1
           ,1)
GO

---- Second seat

INSERT INTO [dbo].[Seats]
           ([SeatNumber]
           ,[SectionId]
           ,[RowNumber]
           ,[EventId]
           ,[VenueId])
     VALUES
           (1
           ,3
           ,7
           ,1
           ,1)
GO

-- Add customer

INSERT INTO [dbo].[Customers]
           ([FirstName]
           ,[LastName]
           ,[Email])
     VALUES
           ('Jack'
           ,'Doe'
           ,'jack.doe@mail.com')
GO

-- Add status

INSERT INTO [dbo].[Status]
           ([Name])
     VALUES
           ('Available'),
		   ('Booked'),
		   ('Purchased')
GO


-- Add PriceOptions

INSERT INTO [dbo].[PriceOptions]
           ([Name]
           ,[Price])
     VALUES
           ('Discounted Price'
           ,25.00),
		   ('Full price' 
		   ,50.00)
GO


-- Add tickets

---- First ticket

INSERT INTO [dbo].[Tickets]
           ([PriceOptionId]
           ,[PurchaseDate]
           ,[EventId]
           ,[SeatId]
           ,[CustomerId]
           ,[StatusId])
     VALUES
           (1
           ,'2024-10-08'
           ,1
           ,1
           ,1
           ,1)
GO

---- Second ticket

INSERT INTO [dbo].[Tickets]
           ([PriceOptionId]
           ,[PurchaseDate]
           ,[EventId]
           ,[SeatId]
           ,[CustomerId]
           ,[StatusId])
     VALUES
           (2
           ,'2024-10-11'
           ,1
           ,2
           ,1
           ,2)
GO





-- Database Update

-- dotnet ef migrations add MigrationName --project DataAccessLayer --startup-project PublicWebAPI
-- dotnet ef database update --project DataAccessLayer --startup-project PublicWebAPI
