USE [FunnettStation]
GO

/****** ±®±Ì…Ë÷√ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_CustomReportInfo](
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FReportID] [int] NULL,
	[FField] [varchar](20) NULL,
	[FieldName] [varchar](30) NULL,
	[Fieldwidth] [int] NULL,
	[Fieldsummary] [varchar](20) NULL,
	[FieldsummaryFormat] [varchar](20) NULL,
	[FieldPrintwidth] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


