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

--使用安装包升级软件时，更新软件升级时间； 数据库升级时间只在安装数据库或手动设置时更新
declare @curTime datetime
set @curTime = GETDATE()
insert into Funnett_Version values('1.07.10.26','2015-10-28 08:15:10', '1.0.0.1', '2015-10-28 08:15:10', '数据库第一版');
insert into Funnett_Version values('1.07.10.28','2015-10-29 14:15:10', '1.0.0.2', '2015-10-29 14:15:10', '增加funnett_version,SC_MonitorConfig增加FGasVariety');
insert into Funnett_Version values('1.07.10.30','2015-10-30 15:25:30', '1.0.0.3','2015-10-30 15:25:30', '存储过程Pro_SC_ConsumerDetail修改');
insert into Funnett_Version values('1.07.10.30',@curTime, '1.0.0.4', @curTime, '存储过程PROC_PublishPsamCard修改,表Psam_Card结构修改');
GO