USE [FunnettStation]
GO

/****** 白名单 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_BaseWhiteList](
	[CardNum] [char](20) NOT NULL,
	[MakeDate] [datetime] NULL,
 CONSTRAINT [PK_BaseWhiteList] PRIMARY KEY CLUSTERED 
(
	[CardNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_BaseWhiteList', @level2type=N'COLUMN',@level2name=N'MakeDate'
GO


