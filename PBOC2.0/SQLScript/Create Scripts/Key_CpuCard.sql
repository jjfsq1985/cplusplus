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
	[ApplicatonMasterKey] [char](32) NOT NULL,
	[ApplicationTendingKey] [char](32) NOT NULL,
	[AppInternalAuthKey] [char](32) NOT NULL,
	[PINResetKey] [char](32) NULL,
	[PINUnlockKey] [char](32) NULL,
	[ConsumerMasterKey] [char](32) NULL,
	[LoadMasterKey] [char](32) NULL,
	[TacMasterKey] [char](32) NULL,
	[UnlockUnloadKey] [char](32) NULL,
	[OverdraftKey] [char](32) NULL,
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'ApplicatonMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'ApplicationTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用内部认证密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'AppInternalAuthKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PIN重装密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'PINResetKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PIN解锁密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'PINUnlockKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'ConsumerMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圈存主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'LoadMasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TAC主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'TacMasterKey'
GO
	
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联机解扣、圈提主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'UnlockUnloadKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改透支限额主密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Key_CARD_ADF', @level2type=N'COLUMN',@level2name=N'OverdraftKey'
GO
