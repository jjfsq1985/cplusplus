USE [FunnettStation]
GO

/****** 操作卡记录表******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OperateCard_Record](
	[Id] [uniqueidentifier] NOT NULL,
	[InvalidCardId] [char](16) NOT NULL,
	[CardType] [varchar](2) NOT NULL,
	[ClientId] int NOT NULL,
	[UseValidateDate] [datetime] NULL,
	[UseInvalidateDate] [datetime] NULL,
	[PersonalId] [varchar](32) NULL,
	[DriverName] [nvarchar](50) NULL,
	[DriverTel] [varchar](32) NULL,
	[CardBalance] [decimal](18, 2) NULL,
	[AccountBalance] [decimal](18, 2) NULL,
	[OperateName] [nvarchar](16) NOT NULL,
	[RePublishCardId] [char](16) NULL,
	[RelatedName] [nvarchar](50) NULL,
	[RelatedPersonalId] [varchar](32) NULL,
	[RelatedTel] [varchar](32) NULL,	
	[OperateDateTime] [datetime] NOT NULL,
	[UpLoadStatus] [int] NOT NULL,
 CONSTRAINT [PK_OperateCard_Record] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失效卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'InvalidCardId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡类型（01-个人用户，02-管理卡，04-员工卡，06-维修卡，11-子卡，21-母卡）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'CardType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'ClientId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡生效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'UseValidateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡失效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'UseInvalidateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'PersonalId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持卡人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'DriverName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持卡人电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'DriverTel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'CardBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户余额（单位母卡余额）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'AccountBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作名称（挂失、解挂、补卡、退卡）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'OperateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'补卡卡号（其他操作为空）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'RePublishCardId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户姓名（挂失者、补卡者）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'RelatedName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'RelatedPersonalId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'RelatedTel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'OperateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已上传（0-否，1-是）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperateCard_Record', @level2type=N'COLUMN',@level2name=N'UpLoadStatus'
GO
