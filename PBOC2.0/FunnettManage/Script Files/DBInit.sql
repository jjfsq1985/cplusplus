IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FunnettStation')
	BACKUP DATABASE [FunnettStation] TO DISK = 'C:/\FunnettStation.bak' With NOINIT, NAME = 'FunnettStation-安装包备份', SKIP
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FunnettStation')	
	create database [FunnettStation]
GO

exec sp_dboption N'FunnettStation', N'autoclose', N'false'
GO

exec sp_dboption N'FunnettStation', N'bulkcopy', N'false'
GO

exec sp_dboption N'FunnettStation', N'trunc. log', N'false'
GO

exec sp_dboption N'FunnettStation', N'torn page detection', N'false'
GO

exec sp_dboption N'FunnettStation', N'read only', N'false'
GO

exec sp_dboption N'FunnettStation', N'dbo use', N'false'
GO

exec sp_dboption N'FunnettStation', N'single', N'false'
GO

exec sp_dboption N'FunnettStation', N'autoshrink', N'false'
GO

exec sp_dboption N'FunnettStation', N'ANSI null default', N'false'
GO

exec sp_dboption N'FunnettStation', N'recursive triggers', N'false'
GO

exec sp_dboption N'FunnettStation', N'ANSI nulls', N'false'
GO

exec sp_dboption N'FunnettStation', N'concat null yields null', N'false'
GO

exec sp_dboption N'FunnettStation', N'cursor close on commit', N'false'
GO

exec sp_dboption N'FunnettStation', N'default to local cursor', N'false'
GO

exec sp_dboption N'FunnettStation', N'quoted identifier', N'false'
GO

exec sp_dboption N'FunnettStation', N'ANSI warnings', N'false'
GO

exec sp_dboption N'FunnettStation', N'auto create statistics', N'true'
GO

exec sp_dboption N'FunnettStation', N'auto update statistics', N'true'
GO

if( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) )
   exec sp_dboption N'FunnettStation', N'db chaining', N'false'
GO 