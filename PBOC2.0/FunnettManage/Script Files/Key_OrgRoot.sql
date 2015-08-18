USE [FunnettStation]
GO

/****** 初始根密钥表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Key_OrgRoot](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[OrgKey] [char](32) NOT NULL,
	[KeyType] [int] NOT NULL,
	[InfoRemark]  [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Key_OrgRoot] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_OrgRoot', @level2type=N'COLUMN',@level2name=N'KeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初始根密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_OrgRoot', @level2type=N'COLUMN',@level2name=N'OrgKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型(0-CPU卡，1-PSAM卡, 2-通用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_OrgRoot', @level2type=N'COLUMN',@level2name=N'KeyType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥信息(客户名称等便于查看)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_OrgRoot', @level2type=N'COLUMN',@level2name=N'InfoRemark'
GO

insert into Key_OrgRoot values('404142434445464748494A4B4C4D4E4F',2,N'测试密钥');
GO