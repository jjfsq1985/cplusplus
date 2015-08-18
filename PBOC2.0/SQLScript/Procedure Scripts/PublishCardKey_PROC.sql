USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_PublishCardKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_PublishCardKey

/****** 用户卡制卡，发卡 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_PublishCardKey                      */

/*       创建时间 2015-04-03                                                                   */

/*       记录制卡、发卡过程                                                                            */                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_PublishCardKey(
	@CardId char(16),--卡号
	@UserKeyGuid uniqueidentifier,   --密钥信息的GUID
	@OrgKey char(32),		--初始密钥
	@MasterKey char(32),	--主控密钥
	@ApplicationIndex int,	--应用号
	@AppTendingKey char(32),--应用维护密钥
	@AppConsumerKey char(32), --消费密钥
	@AppLoadKey char(32),	--圈存密钥
	@AppUnLoadKey char(32),--圈提密钥
	@AppUnGrayKeychar(32), --解灰密钥	
	@AppPinUnlockKey char(32), --PIN解锁密钥
	@AppPinResetKey char(32), --PIN重装密钥
	) With Encryption
 AS    
	--如果外部存在事务则不执行存储过程
	if (@@trancount<>0)
		return 1
	set xact_abort on 
	if(len(@CardId)<>16)
		return 2                                        
	--判断卡号在吗
	if not exists(select * from Base_Card where CardNum=@CardId)
		return 3;
begin
		--开始事务
		begin tran maintran
				insert into Base_Card_Key values(@UserKeyGuid,@OrgKey,@MasterKey,
					@ApplicationIndex,@AppTendingKey,@AppConsumerKey,@AppLoadKey,@AppUnLoadKey,
					@AppUnGrayKey,@AppPinUnlockKey,@AppPinResetKey);
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		commit tran miantran
end
	return 0
GO


