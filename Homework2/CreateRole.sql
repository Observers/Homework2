USE [Trainee15]
GO

/****** Object:  Table [dbo].[Role]    Script Date: 17/01/2020 9:23:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Role](
	[roleID] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [varchar](25) NULL,
	[description] [varchar](50) NULL,
	[status] [bit] NULL,
	[createDate] [datetime] NULL,
	[createUser] [varchar](50) NULL,
	[modifyDate] [datetime] NULL,
	[modifyUser] [varchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


