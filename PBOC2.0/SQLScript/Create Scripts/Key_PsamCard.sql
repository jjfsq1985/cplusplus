USE [FunnettStation]
GO

/****** PSAM卡密钥表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Key_PsamCard](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[MasterKey] [char](32) NOT NULL,
	[MasterTendingKey] [char](32) NOT NULL,
	[ApplicatonMasterKey] [char](32) NOT NULL,
	[ApplicationTendingKey] [char](32) NOT NULL,
	[ConsumerMasterKey] [char](32) NOT NULL,
	[GrayCardKey] [char](32) NOT NULL,
	[MacEncryptKey] [char](32) NOT NULL,
	[InfoRemark]  [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Key_PsamCard] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'KeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'MasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'MasterTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'ApplicatonMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'ApplicationTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全国消费主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'ConsumerMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'灰锁密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'GrayCardKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MAC加密密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'MacEncryptKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥信息(客户名称等便于查看)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_PsamCard', @level2type=N'COLUMN',@level2name=N'InfoRemark'
GO

insert into Key_PsamCard values('000102030405060708090A0B0C0D0E0F',
								'000102030405060708090A0B0C0D0E0F',
								'000102030405060708090A0B0C0D0E0F',
								'000102030405060708090A0B0C0D0E0F',
								'11111111111111111111111111111111',
								'11111111111111111111111111111111',
								'000102030405060708090A0B0C0D0E0F',
								N'测试密钥');
GO
