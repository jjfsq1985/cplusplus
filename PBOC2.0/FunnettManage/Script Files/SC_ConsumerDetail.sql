USE [FunnettStation]
GO

/****** 消费记录 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_ConsumerDetail](
	[FGUID] [varchar](36) NOT NULL,
	[FID] [int] IDENTITY(1,1) NOT NULL,
	[FFinanceDate] [date] NOT NULL,
	[FTradeDateTime] [datetime] NOT NULL,
	[FStopDateTime] [datetime] NOT NULL,
	[FSaveDateTime] [datetime] NULL,
	[FTagNo] [varchar](36) NULL,
	[FGunNo] [int] NULL,
	[FSerialNo] [int] NULL,
	[FStoredNo] [int] NULL,
	[FGas] [decimal](15, 2) NULL,
	[FPrice] [decimal](6, 2) NULL,
	[FMoney] [decimal](15, 2) NULL,
	[FUserCardNo] [varchar](16) NOT NULL,
	[FCardType] [varchar](6) NULL,
	[FPayWay] [varchar](8) NULL,
	[FCarType] [varchar](6) NULL,
	[FCarNo] [varchar](28) NULL,
	[FBusNo] [varchar](12) NULL,
	[FCarTypeInput] [varchar](6) NULL,
	[FShiftNo] [varchar](2) NULL,
	[FOperatorCardNo] [varchar](16) NULL,
	[FRecordType] [varchar](4) NULL,
	[FStartWay] [varchar](4) NULL,
	[FStopReason] [varchar](50) NULL,
	[FPreferential] [varchar](4) NULL,
	[FSumGas] [decimal](18, 2) NULL,
	[FSumMoney] [decimal](18, 2) NULL,
	[FStartPress] [decimal](15, 2) NULL,
	[FStopPress] [decimal](15, 2) NULL,
	[FStopGas] [decimal](15, 2) NULL,
	[FDensity] [decimal](15, 2) NULL,
	[FEquivalent] [decimal](15, 2) NULL,
	[FFuelType] [varchar](4) NULL,
	[FUseAmount] [decimal](18, 2) NULL,
	[FUseDiscount] [decimal](18, 2) NULL,
	[FResidualAmount] [decimal](18, 2) NULL,
	[FResidualBenefits] [decimal](18, 2) NULL,
	[FBeforeAmount] [decimal](18, 2) NULL,
	[FTimeLong] [decimal](18, 2) NULL,
	[FAreaNo] [varchar](16) NULL,
	[FComID] [varchar](16) NULL,
	[FRFID] [varchar](16) NULL,
	[FRFIDCylinderDate] [date] NULL,
	[FRFIDCylinderNo] [int] NULL,
	[FRFIDGasV] [int] NULL,
	[FRFIDScanTime] [int] NULL,
	[FRFIDOperator] [int] NULL,
	[FRFIDCarNo] [varchar](28) NULL,
	[FCoefficient] [varchar](16) NULL,
	[FInletQuality] [decimal](18, 2) NULL,
	[FBackAirQuality] [decimal](18, 2) NULL,
	[FMediumTemperature] [decimal](18, 2) NULL,
	[FMediumDensity] [decimal](18, 2) NULL,
	[FIntegral] [int] NULL,
	[FDiscountAmount] [decimal](18, 2) NULL,
	[FDiscountGas] [decimal](18, 2) NULL,
	[FUpFlag] [char](3) NULL,
	[FOperatorName] [varchar](30) NULL,
	[FChargeGas] [decimal](18, 2) NULL,
	[FOfferGas] [decimal](18, 2) NULL,
	[FStationNO] [varchar](10) NULL,
	[FCompanyNO] [varchar](10) NULL,
	[FCarVin] [varchar](10) NULL,
	[FCylinderNO] [varchar](10) NULL,
	[FUserCardUseTimes] [varchar](10) NULL,
	[FRFIDType] [varchar](50) NULL,
	[FAddGasTimes] [varchar](10) NULL,
	[FT_BAL] [decimal](18, 2) NULL,
	[FCTC] [int] NULL,
	[FTAC] [varchar](10) NULL,
	[FGMAC] [varchar](8) NULL,
	[FPSAM_TAC] [varchar](8) NULL,
	[FPSAM_ASN] [varchar](20) NULL,
	[FPSAM_TID] [varchar](12) NULL,
	[FPSAM_TTC] [varchar](14) NULL,
	[FDS] [int] NULL,
	[FVER] [varchar](2) NULL,
	[FDCT] [varchar](4) NULL,
	[FRecPrice] [decimal](6, 2) NULL,
	[FRecMonry] [decimal](15, 2) NULL,
	[FPreMoney] [decimal](15, 2) NULL,
	[FT_MAC] [varchar](10) NULL,
	[FPriceVer] [varchar](3) NULL,
	[FReGas] [decimal](18, 2) NULL,
 CONSTRAINT [PK_TFuelGas] PRIMARY KEY CLUSTERED 
(
	[FGUID] ASC,
	[FUserCardNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FGUID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'财务日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FFinanceDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气启动时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FTradeDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气停止时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStopDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气机数据保存时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FSaveDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'枪号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FGunNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气机存储序列号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FSerialNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存储号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStoredNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气气量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FGas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'挂牌价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FUserCardNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FCardType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPayWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车辆类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FCarType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车牌号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FCarNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公交路数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FBusNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班组号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FShiftNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FOperatorCardNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FRecordType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气机启动方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStartWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气机停止原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStopReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优先选择' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPreferential'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加气机总加气量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FSumGas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总加气金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FSumMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启动压力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStartPress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'停止压力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStopPress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FDensity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'质量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FEquivalent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'介质类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FFuelType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FUseDiscount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款后剩余金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FResidualAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'剩余优惠金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FResidualBenefits'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款前余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FBeforeAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FAreaNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供气量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FOfferGas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FStationNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FCompanyNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可透支余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FT_BAL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡交易序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FCTC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子签名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FTAC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'解灰认证码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FGMAC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM应用号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPSAM_TAC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPSAM_ASN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPSAM_TID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PSAM_TTC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPSAM_TTC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FDS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FVER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FDCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'无用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FRecPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'无用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FRecMonry'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单价版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FPriceVer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回气量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SC_ConsumerDetail', @level2type=N'COLUMN',@level2name=N'FReGas'
GO

ALTER TABLE [dbo].[SC_ConsumerDetail]  WITH CHECK ADD  CONSTRAINT [CKC_FUPFLAG_SC_CONSU] CHECK  (([FUpFlag] IS NULL OR ([FUpFlag]='N' OR [FUpFlag]='Y')))
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] CHECK CONSTRAINT [CKC_FUPFLAG_SC_CONSU]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FTrad__25918339]  DEFAULT ('0') FOR [FTradeDateTime]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStop__2685A772]  DEFAULT ('0') FOR [FStopDateTime]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FSave__2779CBAB]  DEFAULT ('0') FOR [FSaveDateTime]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FTagN__286DEFE4]  DEFAULT ('0') FOR [FTagNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FGunN__2962141D]  DEFAULT ((0)) FOR [FGunNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FSeri__2A563856]  DEFAULT ((0)) FOR [FSerialNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStor__2B4A5C8F]  DEFAULT ((0)) FOR [FStoredNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consume__FGas__2C3E80C8]  DEFAULT ((0)) FOR [FGas]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FPric__2D32A501]  DEFAULT ((0)) FOR [FPrice]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FMone__2E26C93A]  DEFAULT ((0)) FOR [FMoney]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FUser__2F1AED73]  DEFAULT ('0') FOR [FUserCardNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FCard__300F11AC]  DEFAULT ('0') FOR [FCardType]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FPayW__310335E5]  DEFAULT ('0') FOR [FPayWay]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FCarT__31F75A1E]  DEFAULT ('0') FOR [FCarType]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FCarN__32EB7E57]  DEFAULT ('0') FOR [FCarNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FBusN__33DFA290]  DEFAULT ('0') FOR [FBusNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FCarT__34D3C6C9]  DEFAULT ('0') FOR [FCarTypeInput]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FShif__35C7EB02]  DEFAULT ('0') FOR [FShiftNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FOper__36BC0F3B]  DEFAULT ('0') FOR [FOperatorCardNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FReco__37B03374]  DEFAULT ('0') FOR [FRecordType]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStar__38A457AD]  DEFAULT ('0') FOR [FStartWay]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStop__39987BE6]  DEFAULT ('0') FOR [FStopReason]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FPref__3A8CA01F]  DEFAULT ('0') FOR [FPreferential]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FSumG__3B80C458]  DEFAULT ((0)) FOR [FSumGas]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FSumM__3C74E891]  DEFAULT ((0)) FOR [FSumMoney]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStar__3D690CCA]  DEFAULT ((0)) FOR [FStartPress]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStop__3E5D3103]  DEFAULT ((0)) FOR [FStopPress]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FStop__3F51553C]  DEFAULT ((0)) FOR [FStopGas]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FDens__40457975]  DEFAULT ((0)) FOR [FDensity]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FEqui__41399DAE]  DEFAULT ((0)) FOR [FEquivalent]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FFuel__422DC1E7]  DEFAULT ('0') FOR [FFuelType]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FUseA__4321E620]  DEFAULT ((0)) FOR [FUseAmount]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FUseD__44160A59]  DEFAULT ((0)) FOR [FUseDiscount]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FResi__450A2E92]  DEFAULT ((0)) FOR [FResidualAmount]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FResi__45FE52CB]  DEFAULT ((0)) FOR [FResidualBenefits]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FBefo__46F27704]  DEFAULT ((0)) FOR [FBeforeAmount]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FTime__47E69B3D]  DEFAULT ((0)) FOR [FTimeLong]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FArea__48DABF76]  DEFAULT ('0') FOR [FAreaNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FComI__49CEE3AF]  DEFAULT ('0') FOR [FComID]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FRFID__4AC307E8]  DEFAULT ('00000') FOR [FRFID]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FRFID__4CAB505A]  DEFAULT ('车牌号') FOR [FRFIDCarNo]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FCoef__4D9F7493]  DEFAULT ('0') FOR [FCoefficient]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FInle__4E9398CC]  DEFAULT ((0)) FOR [FInletQuality]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FBack__4F87BD05]  DEFAULT ((0)) FOR [FBackAirQuality]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FMedi__507BE13E]  DEFAULT ((0)) FOR [FMediumTemperature]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FMedi__51700577]  DEFAULT ((0)) FOR [FMediumDensity]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FInte__526429B0]  DEFAULT ((0)) FOR [FIntegral]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FDisc__53584DE9]  DEFAULT ((0)) FOR [FDiscountAmount]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FDisc__544C7222]  DEFAULT ((0)) FOR [FDiscountGas]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FUpFl__5540965B]  DEFAULT ('N') FOR [FUpFlag]
GO

ALTER TABLE [dbo].[SC_ConsumerDetail] ADD  CONSTRAINT [DF__SC_Consum__FOper__5728DECD]  DEFAULT ('tester') FOR [FOperatorName]
GO


