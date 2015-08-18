USE [FunnettStation]
GO


if exists (select * from sysobjects where id = object_id(N'Pro_SC_Shift') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure Pro_SC_Shift


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--站端交班明细插入
CREATE PROCEDURE  Pro_SC_Shift( 
   @ReValue int output,
   @FGUID                varchar(36),
   @FSerialNo            int,
   @FShiftType           varchar(10),
   @FTradeDateTime       datetime,
   @FOperatorCardNo      varchar(16),
   @FBAL                 decimal(15, 2),
   @FGunNo               int,
   @FShiftNo             int,
   @FSumGas              decimal(15,2),
   @FFinanceDate         varchar(16),
   @FStoreNo             int,
   @FT_MAC               varchar(10),
   @RawData	varchar(600)
    )With Encryption
AS
-- declare @ReValue int
BEGIN
     set @ReValue=0
	 begin try
	     begin tran 

	       --需要上传的表
	       insert into SC_Shift(
	              FGUID,FSerialNo,FShiftType,FTradeDateTime,FOperatorCardNo,
                  FBAL,FGunNo,FShiftNo,FSumGas,FFinanceDate,FStoreNo,FT_MAC,FUpFlag--,RawData
	          )
	       values(
	              @FGUID,@FSerialNo,@FShiftType,@FTradeDateTime,@FOperatorCardNo,
                  @FBAL,@FGunNo,@FShiftNo,@FSumGas,@FFinanceDate,@FStoreNo,@FT_MAC,'N'--,@RawData
	          ) 
	      insert into SC_SendDetail(
	              FGUID,FGunNo,FSerialNo,RawData
	          )
	       values(
	              @FGUID,@FGunNo,@FSerialNo,@RawData
	          )  
	      set @ReValue=1
	     commit tran    
	 end try
	 begin catch	     
	      --消息 2627:数据重复
          set @ReValue=@@ERROR
	      if @ReValue=2627  
	      begin
	          set @ReValue=1
	      end
	      rollback tran       
	 end catch
	 return @ReValue
END


GO


