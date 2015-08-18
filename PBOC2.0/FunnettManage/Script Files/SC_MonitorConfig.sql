USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_MonitorConfig]    Script Date: 08/07/2015 08:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_MonitorConfig](
	[FCommName] [varchar](20) NOT NULL,
	[FBaudRate] [int] NULL,
	[Fparity] [varchar](10) NULL,
	[FByteSize] [varchar](10) NULL,
	[FStopBits] [varchar](10) NULL,
	[FHandshake] [int] NULL,
	[FReceiveBytes] [int] NULL,
	[FRollcallTime] [int] NULL,
	[FGunS] [varchar](254) NOT NULL,
	[FDtrControl] [varchar](10) NULL,
	[FRtsControl] [varchar](10) NULL,
	[FProtocol] [varchar](300) NULL,
 CONSTRAINT [PK_SC_MonitorConfig] PRIMARY KEY CLUSTERED 
(
	[FCommName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

insert into SC_MonitorConfig values('COM1',38400,'Odd','8','1',NULL,NULL,1000,'1','DtrDisable','RtsDisable',NULL);
GO


