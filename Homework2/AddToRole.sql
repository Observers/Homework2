USE [Trainee15]
GO

INSERT INTO [dbo].[Role]
           ([roleName]
           ,[description]
           ,[status]
           ,[createDate]
           ,[createUser]
           ,[modifyDate]
           ,[modifyUser])
     VALUES
           ('GSYSADM'
           ,'Responsible for all region'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('GADM'
           ,'RO Manager'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('GOWNER'
           ,'Systems Manager'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('Contractor'
           ,'Organiser'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('Other'
           ,'Normal user'
           ,'0'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin')
GO


