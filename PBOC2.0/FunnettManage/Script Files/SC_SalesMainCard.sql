USE [FunnettStation]
GO

/****** 母卡扣款记录 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
--Drop table SC_SalesMainCard
CREATE TABLE [dbo].[SC_SalesMainCard](
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FGunNo] [int] NOT NULL,
	[FCardNo] [varchar](50) NOT NULL,
	[FMotherCardNo] [varchar](50) NULL,
	[FTradeDateTime] [datetime] NOT NULL,
	[FSaveDateTime] [datetime] NULL,
	[FAmount] [float] NULL,
	[FResidualAmount] [float] NULL,
	[FBeforeAmount] [float] NULL,
	[FStatus] [int] NULL,
 CONSTRAINT [PK_SC_SalesMainCard] PRIMARY KEY CLUSTERED 
(
	[FGunNo] ASC,
	[FCardNo] ASC,
	[FTradeDateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增长编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'枪号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FGunNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子卡卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FCardNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母卡卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FMotherCardNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FTradeDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款时间（保存时间）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FSaveDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款前余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FResidualAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款后余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FBeforeAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（0-正常，1-异常）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_SalesMainCard', @level2type=N'COLUMN',@level2name=N'FStatus'
GO

ALTER TABLE [dbo].[SC_SalesMainCard] ADD  CONSTRAINT [DF_SC_SalesMainCard_FStatus]  DEFAULT ((0)) FOR [FStatus]
GO


