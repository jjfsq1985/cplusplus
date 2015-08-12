USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_StationInfo]    Script Date: 08/07/2015 08:45:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_StationInfo](
	[FProv] [varchar](20) NULL,
	[FCity] [varchar](50) NULL,
	[FSuperior] [varchar](50) NULL,
	[FSID] [varchar](50) NULL,
	[FPOS_P] [varchar](20) NULL,
	[FPwdVER] [varchar](20) NULL,
	[FTmacKey] [varchar](20) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


