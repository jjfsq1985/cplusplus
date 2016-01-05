USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'V_ConsumerDetail') and type = N'V')
drop view V_ConsumerDetail
GO

/******交易查询视图 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_ConsumerDetail]
AS
SELECT    ROW_NUMBER() over( order by FTradeDateTime) FID  ,D.FFinanceDate,D.FTradeDateTime,D.FStopDateTime,D.FSaveDateTime,D.FGunNo,D.FSerialNo,
D.FGas,D.FMoney,D.FPrice,D.FSumGas,
  D.FUserCardNo,(case D.FCardType when 1 then '用户卡' when 2 then '管理卡' when 4 then '员工卡' when 6 then '维修卡' else '无卡'end) as FCardType,
  C.VechileCategory FCarType,C.Plate FCarNo,D.FBusNo,D.FShiftNo,D.FOperatorCardNo,D.FStartWay,D.FResidualAmount 
 from SC_ConsumerDetail D  left outer join Base_Card C on D.FUserCardNo = C.CardNum  
 

GO
