USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'T_INSERT_Sale') and type in (N'TA', N'TR'))
drop trigger T_INSERT_Sale                                                                        
GO
/****** Object:  交易扣款触发器Trigger ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[T_INSERT_Sale] 
ON dbo.SC_ConsumerDetail With Encryption
after INSERT 
AS 
--提交事务处理 
begin try  
BEGIN TRANSACTION 
   IF Exists(select 1 from INSERTED where FCardType <> '11')
   	BEGIN
		--更新卡上余额	
	    UPDATE Base_Card  SET CardBalance = FResidualAmount,OperateDateTime = INSERTED.FTradeDateTime
			FROM INSERTED WHERE CardNum = FUserCardNo  and INSERTED.FTradeDateTime > OperateDateTime
   	END 
   ELSE
   	BEGIN
	  --更新主卡余额
      UPDATE Base_Card set AccountBalance = isnull(AccountBalance,0) - FMoney,  
	    											 OperateDateTime = I.FTradeDateTime  
	    											 from inserted I, Base_Card C where C.CardNum = I.FUserCardNo and CardNum = C.RelatedMotherCard 
	 --更新子卡数据
	    UPDATE Base_Card  SET CardBalance = INSERTED.FResidualAmount,
														OperateDateTime = INSERTED.FTradeDateTime
														FROM INSERTED   WHERE CardNum = INSERTED.FUserCardNo 
																						AND  INSERTED.FTradeDateTime > OperateDateTime;
      INSERT INTO SC_SalesMainCard(FGunNo,FCardNo,FMotherCardNo,FTradeDateTime,FSaveDateTime,FAmount,FResidualAmount,FBeforeAmount,FStatus)
        select I.FGunNo,I.FUserCardNo,C.RelatedMotherCard,I.FTradeDateTime,I.FSaveDateTime,I.FMoney,I.FResidualAmount,I.FBeforeAmount,0
        FROM inserted I,Base_Card C WHERE I.FUserCardNo = C.CardNum 

   	END
COMMIT TRANSACTION 
end try 
begin catch 
rollback transaction 
end catch
