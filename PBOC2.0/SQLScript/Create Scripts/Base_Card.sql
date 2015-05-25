USE [FunnettStation]
GO

/****** CPU卡信息表******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Base_Card](
	[CardNum] [char](16) NOT NULL,
	[CardType] [varchar](2) NOT NULL,
	[ClientId] int NOT NULL,
	[CardState] [int] NOT NULL,
	[UseValidateDate] [datetime] NULL,
	[UseInvalidateDate] [datetime] NULL,
	[Plate] [nvarchar](16) NULL,
	[SelfId] [varchar](50) NULL,
	[CertificatesType] [varchar](2) NULL,
	[PersonalId] [varchar](32) NULL,
	[DriverName] [nvarchar](50) NULL,
	[DriverTel] [varchar](32) NULL,
	[VechileCategory] [nvarchar](8) NULL,
	[SteelCylinderId] [varchar](32) NULL,
	[CylinderTestDate] [datetime] NULL,
	[Remark] [nvarchar](50) NULL,
	[RechargeTotal] [decimal](18, 2) NULL,
	[ConsumeTotal] [decimal](18, 2) NULL,
	[CardBalance] [decimal](18, 2) NULL,
	[AccountBalance] [decimal](18, 2) NULL,
	[CreditsTotal] [int] NULL,
	[R_OilTimesADay] [int] NULL,
	[R_OilVolATime] [decimal](18, 2) NULL,
	[R_OilVolTotal] [decimal](18, 2) NULL,
	[R_OilEndDate] [datetime] NULL,
	[R_Plate] [bit] NULL,
	[R_Oil] [varchar](4) NULL,
	[R_RFID] [bit] NULL,
	[CylinderNum] [int] NULL,
	[FactoryNum] [char](7) NULL,
	[CylinderVolume] [int] NULL,
	[BusDistance] [varchar](10) NULL,
	[FixedGroupID] [varchar](2) NULL,
	[OperateDateTime] [datetime] NULL,
	[KeyGuid] uniqueidentifier NOT NULL,
	[UpLoadStatus] [int] NOT NULL,
 CONSTRAINT [PK_Base_Card] PRIMARY KEY CLUSTERED 
(
	[CardNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Base_Card_Key](
	[KeyGuid] uniqueidentifier NOT NULL,
	[OrgKey] [char](32) NOT NULL,
	[MasterKey] [char](32) NOT NULL,
	[ApplicationIndex] [int] NOT NULL,
	[AppTendingKey] [char](32) NULL,
	[AppLoadKey] [char] (32) NULL,
	[AppUnlockKey] [char](32) NULL,
	[AppPinUnlockKey] [char](32) NULL,
	[AppPinResetKey] [char](32) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CardNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡类型（01-个人用户，02-管理卡，04-员工卡，06-维修卡，11-子卡，21-母卡）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CardType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'ClientId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡状态（0-正常，1-挂失，2-已办理补卡，3-已退卡）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CardState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡生效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'UseValidateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡失效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'UseInvalidateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车牌号（卡为员工卡时，存储员工号）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'Plate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'SelfId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CertificatesType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'PersonalId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持卡人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'DriverName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持卡人电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'DriverTel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'VechileCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'气瓶编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'SteelCylinderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'气瓶有效期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CylinderTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'RechargeTotal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'ConsumeTotal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CardBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户余额（未圈存的子卡余额）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'AccountBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CreditsTotal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每天限加油次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_OilTimesADay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每次限加油量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_OilVolATime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每天最大油量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_OilVolTotal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加油截止日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_OilEndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限车牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_Plate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限油品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_Oil'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'R_RFID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'钢瓶数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CylinderNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'钢瓶生产厂家编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'FactoryNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'钢瓶容积' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'CylinderVolume'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公交路数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'BusDistance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'定点单位编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'FixedGroupID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'OperateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片密钥唯一识别码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'KeyGuid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已上传（0-否，1-是）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card', @level2type=N'COLUMN',@level2name=N'UpLoadStatus'
GO

----------------------------------通过KeyGuid（卡片密钥唯一识别码）找到对应的密钥-----------------------------------------------------------------------

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片密钥唯一识别码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'KeyGuid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡片原始密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'OrgKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主控密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'MasterKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'ApplicationIndex'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用维护密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'AppTendingKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用圈存密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'AppLoadKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用联机解扣密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'AppUnlockKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用PIN解锁密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'AppPinUnlockKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用PIN重装密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Card_Key', @level2type=N'COLUMN',@level2name=N'AppPinResetKey'
GO
