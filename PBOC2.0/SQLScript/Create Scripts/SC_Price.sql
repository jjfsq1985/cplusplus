USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_Price]    Script Date: 08/07/2015 08:38:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_Price](
	[FVER] [int] NOT NULL,
	[FVDT] [datetime] NULL,
	[FGasType] [varchar](4) NULL,
	[FPrice1] [decimal](18, 2) NULL,
	[FPrice2] [decimal](18, 2) NULL,
	[FPrice3] [decimal](18, 2) NULL,
	[FPrice4] [decimal](18, 2) NULL,
	[FPrice5] [decimal](18, 2) NULL,
	[FPrice6] [decimal](18, 2) NULL,
	[FPrice7] [decimal](18, 2) NULL,
	[FPrice8] [decimal](18, 2) NULL,
	[FPrice9] [decimal](18, 2) NULL,
	[FPrice10] [decimal](18, 2) NULL,
	[FPrice11] [decimal](18, 2) NULL,
	[FPrice12] [decimal](18, 2) NULL,
	[FPrice13] [decimal](18, 2) NULL,
	[FPrice14] [decimal](18, 2) NULL,
	[FPrice15] [decimal](18, 2) NULL,
	[FPrice16] [decimal](18, 2) NULL,
	[FPrice17] [decimal](18, 2) NULL,
	[FPrice18] [decimal](18, 2) NULL,
	[FPrice19] [decimal](18, 2) NULL,
	[FPrice20] [decimal](18, 2) NULL,
 CONSTRAINT [PK_SC_PriceENN] PRIMARY KEY CLUSTERED 
(
	[FVER] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


insert into SC_Price values(0,GETDATE(),1,5.21,5.22,5.23,5.24,5.25,5.26,5.27,5.28,5.29,5.30,5.31,5.32,5.33,5.34,5.35,5.36,5.37,5.38,5.39,5.40);
GO