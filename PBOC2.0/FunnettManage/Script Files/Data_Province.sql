USE [FunnettStation]
GO

/****** 加气站所在省代码 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_Province](
	[ProvinceCode] [char](2) NOT NULL,
	[ProvinceName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Data_Province] PRIMARY KEY CLUSTERED 
(
	[ProvinceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_Province', @level2type=N'COLUMN',@level2name=N'ProvinceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_Province', @level2type=N'COLUMN',@level2name=N'ProvinceName'
GO


