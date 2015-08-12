USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_Shift]    Script Date: 08/07/2015 08:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_Shift](
	[FGUID] [varchar](36) NOT NULL,
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FTradeDateTime] [datetime] NOT NULL,
	[FFinanceDate] [date] NOT NULL,
	[FGunNo] [int] NULL,
	[FShiftNo] [int] NULL,
	[FFilledTotalCount] [int] NULL,
	[FSumGas] [decimal](15, 2) NULL,
	[FSumMoney] [decimal](15, 2) NULL,
	[FShiftSumGas] [decimal](15, 2) NULL,
	[FShiftSumMoney] [decimal](15, 2) NULL,
	[FStoreNo] [int] NULL,
	[FOperatorCardNo] [varchar](16) NULL,
	[FShiftType] [varchar](10) NULL,
	[FSerialNo] [int] NULL,
	[FBAL] [decimal](15, 2) NULL,
	[FT_MAC] [varchar](10) NULL,
	[FUpFlag] [char](1) NULL,
	[RawData] [varchar](600) NULL,
 CONSTRAINT [PK_SC_SHIFT] PRIMARY KEY CLUSTERED 
(
	[FGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SC_Shift]  WITH CHECK ADD  CONSTRAINT [CKC_FUPFLAG_SC_SHIFT] CHECK  (([FUpFlag]='N' OR [FUpFlag]='Y'))
GO

ALTER TABLE [dbo].[SC_Shift] CHECK CONSTRAINT [CKC_FUPFLAG_SC_SHIFT]
GO


