USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdateCpuKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdateCpuKey


if exists (select * from sysobjects where id = object_id(N'PROC_UpdateCpuAppKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdateCpuAppKey

/****** CPU卡密钥记录 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_UpdateCpuKey    */

/*       创建时间 2015-04-03                              */

/*       记录CPU卡密钥,涉及Key_CpuCard卡密钥表和Key_CARD_ADF应用密钥表                    */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdateCpuKey(
	@KeyId int,--密钥ID,增加记录时无效
	@MasterKey char(32),--主控密钥
	@MasterTendingKey char(32),--卡片维护密钥
	@InternalAuthKey char(32),  --卡片内部认证密钥
	@KeyDetail nvarchar(50),  --密钥描述
	@KeyState bit,--密钥状态 （0-不使用，1-使用）
	@DbState int, --操作类型（0-不操作，1-更新，2-增加，3-删除）
	@AddKeyId int output --增加记录时输出密钥ID
	) With Encryption
 AS    
    declare @CpuKeyId int  --cpu卡密钥编号
    declare @KeyIdDel int    --删除记录时更新Config_SysParams
	--如果外部存在事务则不执行存储过程
	if( (@@trancount<>0) or (@DbState = 0))
		return 1
	set xact_abort on   
	if(@DbState <> 3)                                      
		begin
		if(len(@MasterKey)<>32)
			return 2
		if(len(@MasterTendingKey)<>32)
			return 2
		if(len(@InternalAuthKey)<>32)
			return 2
		end
	if(@DbState <> 2)
		begin
		if not exists(select * from Key_CpuCard where KeyId=@KeyId)
			return 3
		end

	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		update Key_CpuCard set MasterKey = @MasterKey,
							MasterTendingKey=@MasterTendingKey,
							InternalAuthKey = @InternalAuthKey,
							InfoRemark = @KeyDetail where KeyId = @KeyId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @CpuKeyId = UseKeyID from Config_SysParams;
		if(@KeyState = 1 and @CpuKeyId <> @KeyId)
			update Config_SysParams set UseKeyID = @KeyId;			
		if(@@error<>0) 
			begin
			rollback tran maintran
			return 5
			end			      
		commit tran miantran
		return 0
	end -- //更新记录end
	else if(@DbState = 2)--添加记录
		begin
		--开始事务
		begin tran maintran
		insert into Key_CpuCard values(@MasterKey,@MasterTendingKey,@InternalAuthKey,@KeyDetail);
		set @AddKeyId = @@IDENTITY
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @CpuKeyId = UseKeyID from Config_SysParams;
		if(@KeyState = 1 and @CpuKeyId <> @@IDENTITY)
			update Config_SysParams set UseKeyID = @@IDENTITY;			
		if(@@error<>0) 
			 begin
			 rollback tran maintran
			 return 5
			 end			      
		commit tran miantran
		return 0
		end --添加记录end
	else if(@DbState = 3)	--删除记录	
		begin
		--开始事务
		begin tran maintran
		delete from Key_CpuCard where KeyId = @KeyId;		
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @CpuKeyId = UseKeyID from Config_SysParams;
		if(@CpuKeyId = @KeyId)
			begin
			select @KeyIdDel = ISNULL(MAX(KeyId),1) from Key_CpuCard;			
			update Config_SysParams set UseKeyID = @KeyIdDel;
			end		
		if(@@error<>0) 
			 begin
			 rollback tran maintran
			 return 5
			 end			      
		commit tran miantran
		return 0
		end --删除记录end
GO


CREATE PROCEDURE PROC_UpdateCpuAppKey(
	@RelatedKeyId int,--卡片密钥表对应的密钥ID
	@AppIndex int,          --应用序号
	@AppMasterKey char(32),--应用主控密钥
	@AppTendingKey char(32),--应用维护密钥
	@AppInternalAuthKey char(32), --应用内部认证密钥
	@PinResetKey char(32), --PIN密码重装密钥
	@PinUnlockKey char(32), --PIN解锁密钥
	@ConsumerMasterKey char(32),--消费主密钥
	@LoadKey char(32), --圈存密钥
	@TacMasterKey	char(32), --TAC密钥
	@UnGrayKey char(32),  --联机解扣密钥
	@UnLoadKey char(32), --圈提密钥
	@OvertraftKey char(32),--修改透支限额密钥
	@DbState int --操作类型（0-不操作，1-更新，2-增加，3-删除）	
	) With Encryption
 AS
	declare @CpuKeyId int  --cpu卡密钥编号
	declare @AppCount int  --应用数
	--如果外部存在事务则不执行存储过程
	if( (@@trancount<>0) or (@DbState = 0))
		return 1
	set xact_abort on
	if(@DbState <> 3)                                         
		begin
		if(len(@AppMasterKey)<>32)
			return 2
		if(len(@AppTendingKey)<>32)
			return 2
		if(len(@AppInternalAuthKey)<>32)
			return 2
		if(len(@PinResetKey)<>32)
			return 2
		if(len(@PinUnlockKey)<>32)
			return 2
		if(len(@ConsumerMasterKey)<>32)	
			return 2
		if(len(@LoadKey)<>32)
			return 2
		if(len(@TacMasterKey)<>32)
			return 2
		if(len(@UnGrayKey)<>32)
			return 2	
		if(len(@UnLoadKey)<>32)
			return 2
		if(len(@OvertraftKey)<>32)
			return 2
		end
	if(@DbState <> 2)
		begin
		if not exists(select * from Key_CpuCard where KeyId=@RelatedKeyId)
			return 3
		end
	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		select @AppCount=COUNT(ADFKeyId) from Key_CARD_ADF where RelatedKeyId = @RelatedKeyId and ApplicationIndex = @AppIndex;
		if(@AppCount <> 1)
			return 3;
		update Key_CARD_ADF set ApplicatonMasterKey = @AppMasterKey,
								ApplicationTendingKey=@AppTendingKey,
								AppInternalAuthKey = @AppInternalAuthKey,
								PINResetKey = @PinResetKey,
								PINUnlockKey=@PinUnlockKey,
								ConsumerMasterKey=@ConsumerMasterKey,
								LoadKey=@LoadKey,
								TacMasterKey=@TacMasterKey,
								UnGrayKey=@UnGrayKey,
								UnLoadKey=@UnLoadKey,
								OverdraftKey=@OvertraftKey where RelatedKeyId = @RelatedKeyId and ApplicationIndex = @AppIndex;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end		
		commit tran miantran
		return 0
	end -- //更新记录end
	else if(@DbState = 2)--添加记录
		begin
		--开始事务
		begin tran maintran
		select @AppCount=COUNT(ADFKeyId) from Key_CARD_ADF where RelatedKeyId = @RelatedKeyId and ApplicationIndex = @AppIndex;
		if(@AppCount <> 0)
			return 3;
		insert into Key_CARD_ADF values(@RelatedKeyId,@AppIndex,@AppMasterKey,@AppTendingKey,@AppInternalAuthKey,
										@PinResetKey,@PinUnlockKey,@ConsumerMasterKey,@LoadKey,@TacMasterKey,@UnGrayKey,@UnLoadKey,@OvertraftKey);
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		commit tran miantran
		return 0
		end --添加记录end
	else if(@DbState = 3)	--删除记录	
		begin
		--开始事务
		begin tran maintran
		select @AppCount= COUNT(ADFKeyId) from Key_CARD_ADF where RelatedKeyId = @RelatedKeyId and ApplicationIndex = @AppIndex;
		if(@AppCount <> 1)
			return 3;
		delete from Key_CARD_ADF where RelatedKeyId = @RelatedKeyId and ApplicationIndex = @AppIndex;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		commit tran miantran
		return 0
		end --删除记录end 
 GO
