USE [FunnettStation]
GO

/******解灰卡记录表******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_GreyCardRecord](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[StationNo] [char](8) NOT NULL,
	[GunNo] [int] NOT NULL,
	[CardNo] [char](16) NOT NULL,
	[TradeDateTime] [datetime] NOT NULL,
	[GrayPrice] [decimal](18, 2) NOT NULL,
	[GrayGas] [decimal](18, 2) NOT NULL,
	[GrayMoney] [decimal](18, 2) NOT NULL,
	[ResidualAmount] [decimal](18, 2) NOT NULL,	
	[Operator] int NOT NULL,
	[OperateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Data_GreyCardRecord] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'灰卡记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'RecordId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'StationNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'枪号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'GunNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'CardNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'TradeDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'灰记录单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'GrayPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'气量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'GrayGas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'GrayMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡内余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'ResidualAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'Operator'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'解灰时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_GreyCardRecord', @level2type=N'COLUMN',@level2name=N'OperateTime'
GO


