USE [Trainee15]
GO

INSERT INTO [dbo].[Menu]
           ([menuNo]
           ,[level]
           ,[title]
           ,[linkType]
           ,[linkUrl]
           ,[status])
     VALUES
           ('00'
           ,'0'
           ,'Home'
           ,'Menu'
           ,''
		   ,'1'),

		   ('001'
           ,'1'
           ,'System Setup'
           ,'Menu'
           ,''
		   ,'1'),

		   ('0011'
           ,'2'
           ,'User Maintenance'
           ,'Program'
           ,'/SystemSetup/UserMaintenance'
		   ,'1'),

		   ('0012'
           ,'2'
           ,'Role Maintenance'
           ,'Program'
           ,'/SystemSetup/RoleMaintenance'
		   ,'1'),

		   ('0013'
           ,'2'
           ,'Menu Maintenance'
           ,'Program'
           ,'/SystemSetup'
		   ,'1'),

		   ('002'
           ,'1'
           ,'System Operation'
           ,'Menu'
           ,''
		   ,'1'),

		   ('0021'
           ,'2'
           ,'Test'
           ,'Program'
           ,'/SystemSetup/Test'
		   ,'0'),

		   ('0022'
           ,'2'
           ,'Test2'
           ,'Program'
           ,'/SystemSetup/Test2'
		   ,'0'),

		   ('003'
           ,'1'
           ,'Profile'
           ,'Menu'
           ,''
		   ,'1'),

		   ('0031'
           ,'2'
           ,'Change Password'
           ,'Program'
           ,'/Profile/ChangePassword'
		   ,'1'),

		   ('0032'
           ,'2'
           ,'Logout'
           ,'Program'
           ,'/Profile/Logout'
		   ,'1')
GO


