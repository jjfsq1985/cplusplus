USE [FunnettStation]
GO

/****** 交易记录表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_TradingRecord](
	[RunningNum] [int] IDENTITY(1,1) NOT NULL,
	[POS_P] [int] NOT NULL,
	[POS_TTC] [char](10) NOT NULL,
	[TradeType] [char](2) NOT NULL,
	[StartGasTime] [datetime] NOT NULL,
	[CardNum] [char](20) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[GasMoney] [decimal](18, 2) NOT NULL,
	[CTC] [char](4) NOT NULL,
	[TAC] [char](8) NOT NULL,
	[GMAC] [char](8) NOT NULL,
	[PSAM_TAC] [char](8) NOT NULL,
	[PSAM_ASN] [char](20) NOT NULL,
	[PSAM_TID] [char](12) NOT NULL,
	[PSAM_TTC] [char](8) NOT NULL,
	[DS] [int] NOT NULL,
	[UNIT] [int] NOT NULL,
	[C_TYPE] [int] NOT NULL,
	[VER] [int] NOT NULL,
	[NZN] [int] NOT NULL,	
	[GasVol] [decimal](18, 2) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[EMP] [int] NOT NULL,
	[SumTotal] [decimal](18, 2) NOT NULL,
	[RFU] [char](22) NOT NULL,
	[T_MAC] [char](8) NOT NULL,
	[TickFlag] [bit] NOT NULL,
	[Credits] [int] NOT NULL,
	[UpLoadStatus] [int] NOT NULL,
 CONSTRAINT [PK_Data_TradingRecord] PRIMARY KEY CLUSTERED 
(
	[RunningNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_TradingRecord', @level2type=N'COLUMN',@level2name=N'RunningNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'枪号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_TradingRecord', @level2type=N'COLUMN',@level2name=N'NZN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经上传' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_TradingRecord', @level2type=N'COLUMN',@level2name=N'UpLoadStatus'
GO



