USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_Config]    Script Date: 08/06/2015 14:23:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_Config](
	[FCode] [varchar](32) NOT NULL,
	[FPARENTCODE] [varchar](32) NULL,
	[FName] [nvarchar](50) NULL,
	[FValueS] [nvarchar](50) NULL,
	[FValueI] [int] NULL,
	[FValueF] [decimal](18, 2) NULL,
	[FDefault] [nvarchar](50) NULL,
	[FSYS] [varchar](1) NULL,
	[FType] [varchar](1) NULL,
	[FResCode] [int] NULL,
 CONSTRAINT [PK_SC_CONFIG] PRIMARY KEY CLUSTERED 
(
	[FCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


