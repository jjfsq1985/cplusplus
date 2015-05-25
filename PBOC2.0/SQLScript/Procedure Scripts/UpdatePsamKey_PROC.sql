USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdatePsamKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdatePsamKey

/****** PSAM卡密钥记录 ******/
SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_UpdatePsamKey    */

/*       创建时间 2015-04-03                              */

/*       记录PSAM卡密钥                                   */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdatePsamKey(
	@KeyId int,--密钥ID,增加记录时无效
	@MasterKey char(32),--主控密钥
	@MasterTendingKey char(32),--卡片维护密钥
	@AppMasterKey char(32),--应用主控密钥
	@AppTendingKey char(32),--应用维护密钥
	@ConsumerMasterKey char(32),--消费主密钥
	@GrayCardKey char(32),--灰锁密钥
	@MacEncryptKey char(32),--MAC加密密钥        
	@KeyDetail nvarchar(50), --密钥描述
	@KeyState bit,--密钥状态 （0-不使用，1-使用）
	@DbState int, --操作类型（0-不操作，1-更新，2-增加，3-删除）
	@AddKeyId int output --增加记录时输出密钥ID
	) With Encryption
 AS    
    declare @PsamKeyId int  --psam卡密钥编号
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
		if(len(@AppMasterKey)<>32)
			return 2
		if(len(@AppTendingKey)<>32)
			return 2
		if(len(@ConsumerMasterKey)<>32)
			return 2
		if(len(@GrayCardKey)<>32)
			return 2
		if(len(@MacEncryptKey)<>32)
			return 2
		end
	if(@DbState <> 2)
		begin
		if not exists(select * from Key_PsamCard where KeyId=@KeyId)
			return 3
		end

	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		update Key_PsamCard set MasterKey = @MasterKey,
							MasterTendingKey=@MasterTendingKey,
							ApplicatonMasterKey = @AppMasterKey,
							ApplicationTendingKey = @AppTendingKey,
							ConsumerMasterKey = @ConsumerMasterKey,
							GrayCardKey = @GrayCardKey,
							MacEncryptKey = @MacEncryptKey,
							InfoRemark = @KeyDetail where KeyId = @KeyId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @PsamKeyId = UsePsamKeyID from Config_SysParams;
		if(@KeyState = 1 and @PsamKeyId <> @KeyId)
			update Config_SysParams set UsePsamKeyID = @KeyId;			
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
		insert into Key_PsamCard values(@MasterKey,@MasterTendingKey,@AppMasterKey,@AppTendingKey,@ConsumerMasterKey,@GrayCardKey,@MacEncryptKey,@KeyDetail);
		set @AddKeyId = @@IDENTITY
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @PsamKeyId = UsePsamKeyID from Config_SysParams;
		if(@KeyState = 1 and @PsamKeyId <> @@IDENTITY)
			update Config_SysParams set UsePsamKeyID = @@IDENTITY;			
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
		delete from Key_PsamCard where KeyId = @KeyId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @PsamKeyId = UsePsamKeyID from Config_SysParams;
		if(@PsamKeyId = @KeyId)
			begin
			select @KeyIdDel = ISNULL(MAX(KeyId),1) from Key_PsamCard;
			update Config_SysParams set UsePsamKeyID = @KeyIdDel;
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


