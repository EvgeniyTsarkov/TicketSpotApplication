USE [TicketSpotDb]
GO
SET IDENTITY_INSERT [dbo].[EventManagers] ON 
GO
INSERT [dbo].[EventManagers] ([Id], [FirstName], [LastName], [Email]) VALUES (1, N'Elon', N'Musk', N'e.musk@spacex.com')
GO
SET IDENTITY_INSERT [dbo].[EventManagers] OFF
GO
SET IDENTITY_INSERT [dbo].[Venues] ON 
GO
INSERT [dbo].[Venues] ([Id], [Name], [Address], [Description], [EventManagerId]) VALUES (1, N'Dome', N'Las Vegas', N'', 1)
GO
SET IDENTITY_INSERT [dbo].[Venues] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
GO
INSERT INTO [TicketSpotDb].[dbo].[Customers] ([Id], [FirstName], [LastName], [Email]) VALUES (1, 'Chris', 'Nolan', 'ch.nolan@hollywood.us')
GO
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON
GO
INSERT INTO [TicketSpotDb].[dbo].[Events] ([Id], [Name], [Date], [Description], [EventManagerId]) VALUES (1, 'Rolling Stones Concert', '2024-10-05 14:30:00', 'Cool show', 1)
GO
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[Seats] ON
GO
INSERT INTO [TicketSpotDb].[dbo].[Seats] ([Id], [Section], [RowNumber], [SeatNumber], [VenueId]) VALUES (1, 'A', 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Seats] OFF
GO
