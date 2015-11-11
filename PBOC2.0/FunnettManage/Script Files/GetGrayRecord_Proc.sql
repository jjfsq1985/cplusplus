USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_GetGrayRecord') and type in (N'P', N'PC'))
drop procedure PROC_GetGrayRecord                                                                        
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_GetGrayRecord    */

/*       创建时间 2015-08-13                              */

/*       获取灰卡的交易记录  */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_GetGrayRecord(
	@CardId char(16),    --卡号
	@PSAM_TID char(12),  --终端机编号
	@GTAC    char(8),   --灰锁的GTAC
	@StationNo char(8) output, --站点编号
	@GunNo int output,      --枪号
	@ConsumerTime datetime output, --交易时间
	@Price  decimal(18,2) output, --单价
	@Gas    decimal(18,2) output, --气量
	@Money  decimal(18,2) output, --金额
	@ResidualAmount decimal(18,2) output --卡内余额
	) With Encryption
 AS 
	declare @RecordCardId char(16)
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on 
	--开始事务
	begin tran maintran	
	select @RecordCardId=FUserCardNo,@StationNo=FStationNO,@GunNo=FGunNo,@ConsumerTime=FTradeDateTime,
			@Price=FPrice,@Gas=FGas,@Money=FMoney,@ResidualAmount=FResidualAmount from SC_ConsumerDetail where FUserCardNo = @CardId and FPSAM_TID = @PSAM_TID and FTAC=@GTAC and FRecordType = '1';
	if(@@ERROR <> 0)
		begin
		rollback tran maintran
		return 1
		end	
	commit tran miantran
	if(len(@RecordCardId) <> 16)
	 return 1
	else
	 return 0
GO
