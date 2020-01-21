USE [Trainee15]
GO

/****** Object:  Table [dbo].[Menu]    Script Date: 17/01/2020 9:23:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Menu](
	[menuID] [int] IDENTITY(1,1) NOT NULL,
	[menuNo] [varchar](50) NULL,
	[level] [int] NULL,
	[title] [varchar](50) NULL,
	[linkType] [varchar](50) NULL,
	[linkUrl] [varchar](50) NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[menuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


