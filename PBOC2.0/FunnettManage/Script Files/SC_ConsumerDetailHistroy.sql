USE [FunnettStation]
GO

/****** Object:  Table [dbo].[SC_ConsumerDetailHistroy]    Script Date: 08/07/2015 08:25:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SC_ConsumerDetailHistroy](
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
	[RawData] [varchar](600) NULL,
 CONSTRAINT [PK_TFuelGasHistory] PRIMARY KEY CLUSTERED 
(
	[FGUID] ASC,
	[FUserCardNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


