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
