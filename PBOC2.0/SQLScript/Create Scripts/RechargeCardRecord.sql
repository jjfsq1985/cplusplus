USE [FunnettStation]
GO

/****** 充值记录表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_RechargeCardRecord](
	[RunningNum] [int] IDENTITY(1,1) NOT NULL,
	[CardNum] [char](16) NULL,
	[OperateType] [nvarchar](2) NULL,
	[ForwardBalance] [decimal](18, 2) NULL,
	[RechargeValue] [decimal](18, 2) NULL,
	[PreferentialVal] [decimal](18, 2) NULL,
	[ReceivedVal] [decimal](18, 2) NULL,
	[CurrentBalance] [decimal](18, 2) NULL,
	[RechargeDateTime] [datetime] NULL,
	[OperatorId] [int] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[ShiftNum] [varchar](10) NULL,
	[UpLoadStatus] [int] NOT NULL,
 CONSTRAINT [PK_RechargeCardRecord] PRIMARY KEY CLUSTERED 
(
	[RunningNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'RunningNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'CardNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作类型：充值、转账' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'OperateType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值前余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'ForwardBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'RechargeValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'PreferentialVal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实充金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'ReceivedVal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值后余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'CurrentBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'RechargeDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款方式(现金、银行卡)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'PaymentMethod'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'yyyyMMddnn(nn为班次)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'ShiftNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经上传' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_RechargeCardRecord', @level2type=N'COLUMN',@level2name=N'UpLoadStatus'
GO

