USE [Trainee15]
GO

INSERT INTO [dbo].[User]
           ([userID]
           ,[username]
           ,[roleID]
           ,[status]
           ,[createDate]
           ,[createUser]
           ,[modifyDate]
           ,[modifyUser])
     VALUES
           ('1'
           ,'admin'
           ,'1'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('2'
           ,'Lyvia'
           ,'1'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'admin'
           ,CURRENT_TIMESTAMP
           ,'admin'),

		   ('3'
           ,'Raco'
           ,'2'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'),

		   ('4'
           ,'Alan'
           ,'3'
           ,'1'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'),

		   ('5'
           ,'Matt'
           ,'4'
           ,'0'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'),

		   ('6'
           ,'Steven'
           ,'5'
           ,'0'
           ,CURRENT_TIMESTAMP
           ,'Lyvia'
           ,CURRENT_TIMESTAMP
           ,'Lyvia')
GO


