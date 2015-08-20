USE [FunnettStation]
GO

/****** 报表菜单配置 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_MenuItem](
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FIndex] [int] NULL,
	[FName] [varchar](20) NULL,
	[FCaption] [varchar](50) NULL,
	[FSqlType] [int] NULL,
	[FGunno] [int] NULL,
	[FOperatorCard] [int] NULL,
	[FCompanyid] [int] NULL,
	[FDatetime] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单所在位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_MenuItem', @level2type=N'COLUMN',@level2name=N'FIndex'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报表名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_MenuItem', @level2type=N'COLUMN',@level2name=N'FName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报表名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_MenuItem', @level2type=N'COLUMN',@level2name=N'FCaption'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'脚本执行区' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_MenuItem', @level2type=N'COLUMN',@level2name=N'FSqlType'
GO


