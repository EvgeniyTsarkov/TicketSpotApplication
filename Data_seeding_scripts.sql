USE [TicketSpotDb]
GO

BEGIN TRANSACTION;

-- Add Venue manager
print('Add Event managers');

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
print('Add Venues');


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
print('Add Events');

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
print('Add Sections');

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
print('Add Seat 1');

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
print('Add Seat 2');

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
print('Add Customer');

INSERT INTO [dbo].[Customers]
           ([FirstName]
           ,[LastName]
           ,[Email])
     VALUES
           ('Jack'
           ,'Doe'
           ,'jack.doe@mail.com')
GO


-- Add PriceOptions
print('Add Price Option');

INSERT INTO [dbo].[PriceOptions]
           ([Name]
           ,[Price])
     VALUES
           ('Discounted Price'
           ,25.00),
		   ('Full price' 
		   ,50.00)
GO

--Add payment
print('Add payment');

USE [TicketSpotDb]
GO

INSERT INTO [dbo].[Payments]
           ([Status]
           ,[TotalAmount])
     VALUES
           (0
           ,0)
GO


-- Add cart
print('Add cart');

INSERT INTO [dbo].[Carts]
           ([Id]
           ,[CartStatus]
           ,[CustomerId]
           ,[PaymentId])
     VALUES
           ('0a1b428a-9fb0-4ff2-90ef-d3d720304cc0'
           , 1
           , 1
           , 1)
GO



-- Add tickets
print('Add Tickets');

---- First ticket
print('Add Ticket 1');


INSERT INTO [dbo].[Tickets]
           ([PriceOptionId]
           ,[PurchaseDate]
           ,[EventId]
           ,[SeatId]
           ,[CustomerId]
           ,[TicketStatus]
           ,[CartId])
     VALUES
           (1
           ,'2024-10-08'
           ,1
           ,1
           ,1
           ,1
           ,'0a1b428a-9fb0-4ff2-90ef-d3d720304cc0')
GO

---- Second ticket
print('Add Ticket 2');

INSERT INTO [dbo].[Tickets]
           ([PriceOptionId]
           ,[PurchaseDate]
           ,[EventId]
           ,[SeatId]
           ,[CustomerId]
           ,[TicketStatus]
           ,[CartId])
     VALUES
           (2
           ,'2024-10-11'
           ,1
           ,2
           ,1
           ,2
           ,'0a1b428a-9fb0-4ff2-90ef-d3d720304cc0')
GO

COMMIT;


-- Guid 0a1b428a-9fb0-4ff2-90ef-d3d720304cc0


-- Database Update

-- dotnet ef migrations add InitialMigration --project DataAccessLayer --startup-project PublicWebAPI
-- dotnet ef database update --project DataAccessLayer --startup-project PublicWebAPI
