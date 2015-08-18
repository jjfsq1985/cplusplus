USE [FunnettStation]
GO

/****** CPU卡密钥表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Key_CpuCard](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[MasterKey] [char](32) NOT NULL,
	[MasterTendingKey] [char](32) NOT NULL,
	[InternalAuthKey] [char](32) NOT NULL,
	[InfoRemark]  [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Key_CpuCard] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--关联的应用密钥
CREATE TABLE [dbo].[Key_CARD_ADF](
	[ADFKeyId] [int] IDENTITY(1,1) NOT NULL,
	[RelatedKeyId] [int] NOT NULL,    --关联在Key_CpuCard表中的KeyId
	[ApplicationIndex] [int] NOT NULL,   --应用号，1-加气应用；2-积分应用；3-监管应用；...
	[AppMasterKey] [char](32) NOT NULL,
	[AppTendingKey] [char](32) NOT NULL,
	[AppInternalAuthKey] [char](32) NOT NULL,
	[AppPinResetKey] [char](32) NULL,
	[AppPinUnlockKey] [char](32) NULL,
	[AppConsumerKey] [char](32) NULL,
	[AppLoadKey] [char](32) NULL,
	[AppTacKey] [char](32) NULL,
	[AppUnGrayKey] [char](32) NULL,
	[AppUnLoadKey] [char](32) NULL,
	[AppOverdraftKey] [char](32) NULL,

 CONSTRAINT [PK_Key_CARD_ADF] PRIMARY KEY CLUSTERED 
(
	[ADFKeyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CpuCard', @level2type=N'COLUMN',@level2name=N'KeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CpuCard', @level2type=N'COLUMN',@level2name=N'MasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CpuCard', @level2type=N'COLUMN',@level2name=N'MasterTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片内部认证密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CpuCard', @level2type=N'COLUMN',@level2name=N'InternalAuthKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥信息(客户名称等便于查看)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CpuCard', @level2type=N'COLUMN',@level2name=N'InfoRemark'
GO

--------应用密钥表--------------

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用密钥编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'ADFKeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联的密钥编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'RelatedKeyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'ApplicationIndex'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用内部认证密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppAuthKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PIN重装密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppPinResetKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PIN解锁密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppPinUnlockKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppConsumerKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圈存主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppLoadKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TAC主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppTacKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联机解扣主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppUnGrayKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圈提主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppUnLoadKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改透支限额主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppOverdraftKey'
GO

insert into Key_CpuCard values('F21B1234043830D448293E6636883378',
								'12345678901234567890112233445566',
								'F211206C056830D448293E6636883334',
								N'测试密钥');
GO
								
insert into Key_CARD_ADF values(1,1,'F21B1234043830D448293E66368833CC',
									'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA',
									'F211206C056830D448293E66368833BB',
									'99999999999999999999999999999999',
									'88888888888888888888888888888888',
									'11111111111111111111111111111111',
									'66666666666666666666666666666666',
									'77777777777777777777777777777777',
									'BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB',
									'BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB',
									'CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC');
GO