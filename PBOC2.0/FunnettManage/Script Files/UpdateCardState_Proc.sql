USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdateCardState') and type in (N'P', N'PC'))
drop procedure PROC_UpdateCardState                                                                        
GO

/****** 卡挂失和解挂 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure PROC_UpdateCardState    */

/*       创建时间 2015-08-24                              */

/*       更新用户卡状态（挂失、解挂），增删黑名单                                   */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdateCardState(
	@CardId char(16),--卡号
	@CardState int,--状态 0-正常，1-挂失，2-已补卡，3-已退卡
	@BlackCard bit--true新增黑名单，false 新删黑名单
	) With Encryption
 AS    
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on		
	--判断卡号在吗
	if not exists(select * from Base_Card where CardNum=@CardId)
		return 2
begin
	declare @Today varchar(32)
	set @Today = Convert(varchar(32),GetDate(),120)	
		--开始事务
		begin tran maintran			
			update Base_Card set CardState = @CardState, OperateDateTime = GetDate() where CardNum=@CardId;
			if(@BlackCard = 1)
				begin	--新增黑名单					
				if not exists(select * from SC_BlackCard where FUserCardNo=@CardId)
					begin
					insert into SC_BlackAddCard values(@CardId,Left(@Today,10));
					insert into SC_BlackCard values(@CardId,Left(@Today,10));
					end				
				end
			else
				begin --新删黑名单
				if exists(select * from SC_BlackCard where FUserCardNo=@CardId)
					begin
					insert into SC_BlackDelCard values(@CardId,Left(@Today,10));				
					delete from SC_BlackCard where FUserCardNo = @CardId;
					end
				end
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 3
		    end	
		commit tran miantran
		return 0
end
GO


