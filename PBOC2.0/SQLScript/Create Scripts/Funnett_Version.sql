USE [FunnettStation]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE ID = object_id(N'Funnett_Version') AND type in (N'U'))
	DROP TABLE Funnett_Version 
GO

USE [FunnettStation]
GO

/****** 数据库版本 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Funnett_Version](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SoftwareVersion] [varchar](32) NOT NULL,
	[UpgradeTime]   [datetime] NOT NULL,
	[DbVersion] [varchar](32) NOT NULL,
	[DbUpgradeTime]   [datetime] NOT NULL,
	[Info] [nvarchar](256) NULL,	
CONSTRAINT [PK_Funnett_Version] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'软件版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Funnett_Version', @level2type=N'COLUMN',@level2name=N'SoftwareVersion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'软件升级时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Funnett_Version', @level2type=N'COLUMN',@level2name=N'UpgradeTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据库版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Funnett_Version', @level2type=N'COLUMN',@level2name=N'DbVersion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据库升级时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Funnett_Version', @level2type=N'COLUMN',@level2name=N'DbUpgradeTime'
GO

--参照该记录表手动升级数据库
DECLARE @curTime datetime
SET @curTime = GETDATE()
insert into Funnett_Version values('1.07.10.26','2015-10-28 08:15:10', '1.0.0.1', '2015-10-28 08:15:10', '数据库第一版');
insert into Funnett_Version values('1.07.10.28','2015-10-29 14:15:10', '1.0.0.2', '2015-10-29 14:15:10', '增加funnett_version,SC_MonitorConfig增加FGasVariety');
insert into Funnett_Version values('1.07.10.30','2015-10-30 15:25:30', '1.0.0.3','2015-10-30 15:25:30', '存储过程Pro_SC_ConsumerDetail修改');
insert into Funnett_Version values('1.07.10.30','2015-11-03 15:00:00', '1.0.0.4', '2015-11-03 15:00:00', '存储过程PROC_PublishPsamCard修改,表Psam_Card结构修改');
insert into Funnett_Version values('1.07.10.30','2015-11-04 11:05:10', '1.0.0.5','2015-11-04 11:05:10', '表Data_RechargeCardRecord增加TerminalID,存储过程PROC_UpdatePsamKey修改,表Key_PsamCard结构修改');
insert into Funnett_Version values('1.07.11.10','2015-11-10 16:26:50', '1.0.0.6','2015-11-11 10:26:50', '存储过程PROC_PublishCardKey修改');
insert into Funnett_Version values('1.08.12.02','2015-12-02 10:06:50', '1.0.0.7','2015-12-02 10:06:50', '存储过程PROC_PublishPsamCard修改,否则重制PSAM卡会覆盖所有PSAM卡的参数');
insert into Funnett_Version values('1.08.12.02','2015-12-02 10:06:50', '1.0.0.8','2015-12-03 13:53:50', '站控SC_ConsumerDetail表,主键修改为FTradeDateTime和FGunNo');
insert into Funnett_Version values('1.08.12.18','2015-12-18 09:15:00', '1.0.0.9','2015-12-18 09:16:50', '站控增加SC_SalesMainCard表和T_INSERT_Sale触发器');
GO