USE [FunnettStation]
GO

/****** 系统配置信息表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Config_SysParams](
	[RunningNum] [int] IDENTITY(1,1) NOT NULL,
	[CommandInterval] [int] NOT NULL,
	[CommandWait] [int] NOT NULL,
	[UseKeyID] [int] NOT NULL,
	[UsePsamKeyID] [int] NOT NULL,
	[OrgKeyId] [int] NOT NULL,
	[OrgPsamKeyId] [int] NOT NULL,
	[BaseBlacklistVer] [int] NOT NULL,
	[IncBlacklistVer] [int] NOT NULL,
	[DecBlacklistVer] [int] NOT NULL,
	[WhitelistVer] [int] NOT NULL,
	[UnitPriceVer] [int] NOT NULL,
	[GeneralInfoVer] [int] NOT NULL,
	[CommLogRemain] [int] NOT NULL,
	[OperateLogRemain] [int] NOT NULL,
	[IsPrintTicket] [bit] NOT NULL,
	[WhenToUpLoadData] [datetime] NOT NULL,
	[DownLoadRn] [int] NOT NULL,
	[ShiftMaxTradeRecordRn] [int] NOT NULL,
	[ShiftMaxRechargeRn] [int] NOT NULL,
	[ShiftMaxOilInstoreRn] [int] NOT NULL,
	[ShiftMaxIdentifyingRn] [int] NOT NULL,
	[BackUpTime] [datetime] NOT NULL,
	[BackUpBegin] [datetime] NOT NULL,
	[BackUpEnd] [datetime] NOT NULL,
	[SaveHour] [int] NOT NULL,
	[SaveDay] [int] NOT NULL,
	[BackPath] [varchar](50) NOT NULL,
	[IsOnceDay] [bit] NOT NULL,
	[CurrentShiftDate] [datetime] NOT NULL,
	[CurrentShiftNum] [int] NOT NULL,
 CONSTRAINT [PK_Config_SysParams] PRIMARY KEY CLUSTERED 
(
	[RunningNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU卡制卡密钥ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'UseKeyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM卡制卡密钥ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'UsePsamKeyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU卡原始密钥ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'OrgKeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM卡原始密钥ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'OrgPsamKeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基础黑名单版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'BaseBlacklistVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'黑名单增量版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'IncBlacklistVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'黑名单减量版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'DecBlacklistVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'白名单版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'WhitelistVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'价格版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'UnitPriceVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通用信息版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Config_SysParams', @level2type=N'COLUMN',@level2name=N'GeneralInfoVer'
GO
------------------------------------------------------
--Config_SysParams表只有一行数据，只会修改，不会增加
------------------------------------------------------
declare @curTime datetime --时间
set @curTime = GETDATE()

insert into Config_SysParams values(1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,@curTime,1,1,1,1,1,@curTime,@curTime,@curTime,1,1,N' ',1,@curTime,1);
