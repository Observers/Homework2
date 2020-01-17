USE [Trainee15]
GO

/****** Object:  Table [dbo].[User]    Script Date: 17/01/2020 9:24:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[userID] [int] NOT NULL,
	[username] [varchar](50) NOT NULL,
	[roleID] [int] NOT NULL,
	[status] [bit] NULL,
	[createDate] [datetime] NULL,
	[createUser] [varchar](50) NULL,
	[modifyDate] [datetime] NULL,
	[modifyUser] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Account] FOREIGN KEY([userID])
REFERENCES [dbo].[Account] ([userID])
GO

ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Account]
GO

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([roleID])
GO

ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO


