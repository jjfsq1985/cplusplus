USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'CustomReport') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure CustomReport
 
/****** Object:  StoredProcedure [dbo].[CustomReport]    Script Date: 08/18/2015 16:28:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[CustomReport] 
(
 @FType int,                 --类型
 @FStartime Datetime,         --开始时间
 @FEndtime Datetime,          --结束时间
 @FOperator varchar(30),     --操作员
 @FCardNo  varchar(20),       --用户卡号
 @FOperatorCardno varchar(20), -- 操作员卡号
 @FGunno  int,
 @ClientName varchar(40) 
)With Encryption
as
if(@FType = 1)
	begin
	select FID,FUserCardNo,Plate,FTradeDateTime,FPrice,FGas,FMoney,FResidualAmount,FGunNo,ClientName,FOperatorCardNo 
	from SC_ConsumerDetail S Left outer join (select C.ClientName,B.CardNum,B.Plate 
												from Base_Card B left join Base_Client C 
												On B.ClientId = C.ClientId)P ON S.FUserCardNo = P.CardNum
	where ((FGunNo = @FGunno) or (@FGunno = 0)) and ((P.ClientName = @ClientName) or (@ClientName = '')) 
			and ((FOperatorCardNo like '%'+@FOperatorCardno+'%') or (@FOperatorCardno = '')) 
			and (FTradeDateTime between @FStartime and @FEndtime);
	end
else if (@FType = 2)
		begin
		select SUM(FGas) FGas,COUNT(1) FNum, SUM(FMoney) FMoney,FGunNo,FOperatorCardNo,ClientName
		from (Select FGas,FMoney,FGunNo,ClientName,FOperatorCardNo 
					from SC_ConsumerDetail S Left Outer JOIN (select C.ClientName,B.CardNum,B.Plate from Base_Card B
							    left outer join Base_Client C On B.ClientId = C.ClientId) B On S.FUserCardNo = B.CardNum
								where ((S.FGunNo = @FGunno) or (@FGunno = 0)) and ((B.ClientName = @ClientName) or (@ClientName = '')) 
								and ((FOperatorCardNo like '%'+@FOperatorCardno+'%') or (@FOperatorCardno = ''))
								and (FTradeDateTime between @FStartime and @FEndtime))C
		Group by FGunNo,FOperatorCardNo,ClientName;
	end 
else if (@FType = 3)
	begin
	select StationNo,GunNo,CardNo,TradeDateTime,GrayPrice,GrayGas,GrayMoney,ResidualAmount,Operator,OperateTime 
	from Data_GreyCardRecord where ((GunNo = @FGunno) or (@FGunno = 0)) and ((CardNo = @FCardNo) or (@FCardNo = ''))
  		 and (OperateTime between @FStartime and @FEndtime);
	end
else if (@FType = 4)
	begin
	select R.CardNum,isnull(ForwardBalance,0)ForwardBalance,
			isnull(RechargeValue,0)RechargeValue,
			isnull(PreferentialVal,0)PreferentialVal,
			isnull(ReceivedVal,0)ReceivedVal,
			isnull(CurrentBalance,0)CurrentBalance,R.RechargeDateTime,OperatorId from Data_RechargeCardRecord R
			left outer join (select B.CardNum,C.ClientName from Base_Card B inner join Base_Client C ON B.ClientId = C.ClientId )C ON R.CardNum = C.CardNum
			where  ((R.CardNum = @FCardNo) or (@FCardNo = '')) and ((C.ClientName = @ClientName) or (@ClientName = ''))  
  			and (RechargeDateTime between @FStartime and @FEndtime);
	end
else if (@FType = 5)
	begin
	select B.CardNum,B.Plate,C.FTradeDateTime,R.RechargeDateTime,B.AccountBalance,C.FGas,C.FMoney,R.RechargeValue, 
	(R.RechargeValue - C.FMoney-B.AccountBalance)AC
	from Base_Card B 
					left outer join (select FUserCardNo,SUM(FGas)FGas,SUM(FMoney)FMoney,MAX(FTradeDateTime)FTradeDateTime from SC_ConsumerDetail 
					where (FTradeDateTime between  @FStartime and @FEndtime) group by FUserCardNo ) C On B.CardNum = C.FUserCardNo
					left outer join (select CardNum,SUM(RechargeValue)RechargeValue,MAX(RechargeDateTime)RechargeDateTime from Data_RechargeCardRecord 
					where (RechargeDateTime between  @FStartime and @FEndtime)
					group by CardNum) R On B.CardNum = R.CardNum;
	end
	
GO

