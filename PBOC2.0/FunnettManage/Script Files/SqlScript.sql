IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FunnettStation')
	BACKUP DATABASE [FunnettStation] TO DISK = 'C:/\FunnettStation.bak' With NOINIT, NAME = 'FunnettStation-安装包备份', SKIP
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FunnettStation')	
	BEGIN   
	use [FunnettStation]
	DECLARE @curTime datetime
	set @curTime = GETDATE()
	IF EXISTS (SELECT * FROM sysobjects WHERE ID = object_id(N'Funnett_Version') AND OBJECTPROPERTY(ID, N'IsTable') = 1)
		INSERT INTO Funnett_Version VALUES('SoftwareVersion',@curTime,'1.0.0.5',@curTime,'数据库不作修改,只安装软件')	
	END
GO