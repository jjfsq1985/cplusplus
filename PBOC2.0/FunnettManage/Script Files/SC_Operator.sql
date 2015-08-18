USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_Operator]    Script Date: 08/07/2015 08:30:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_Operator](
	[FGUID] [varchar](32) NULL,
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FAnName] [varchar](32) NOT NULL,
	[FLogonName] [varchar](32) NOT NULL,
	[FLogonNameMD5] [varchar](32) NULL,
	[FPasswordMD5] [varchar](32) NULL,
	[FSysFlg] [int] NULL,
	[FValidState] [varchar](5) NULL,
	[FCreateDate] [datetime] NULL,
	[FDescription] [varchar](250) NULL,
 CONSTRAINT [PK_SC_Operator] PRIMARY KEY CLUSTERED 
(
	[FLogonName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

insert into SC_Operator values('FAF9972B9EF6465F066D5F4','π‹¿Ì‘±','admin',NULL,'BB98C1DC69203F11',999,'Y',GETDATE(),NULL);
GO



