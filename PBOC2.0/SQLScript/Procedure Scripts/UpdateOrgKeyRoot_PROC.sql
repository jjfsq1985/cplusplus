USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdateOrgKeyRoot') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdateOrgKeyRoot

/****** 厂商初始卡密钥记录 ******/
SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_UpdateOrgKeyRoot                      */

/*       创建时间 2015-04-03                                            */

/*       记录厂商初始卡密钥                                             */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdateOrgKeyRoot(
	@KeyId int,--密钥ID,增加记录时无效
	@OrgKey char(32),--密钥值
	@KeyType int,--密钥类型   （0-cpu, 1-psam）
	@KeyDetail nvarchar(50), --密钥描述
	@KeyState bit,--密钥状态 （0-不使用，1-使用）
	@DbState int, --操作类型（0-不操作，1-更新，2-增加，3-删除）
	@AddKeyId int output --增加记录时输出密钥ID
	) With Encryption
 AS
    declare @OrgKeyId int  --cpu卡原始密钥编号
    declare @OrgPsamKeyId int  --psam卡原始密钥编号
    declare @KeyIdDel int    --删除记录时更新Config_SysParams
	--如果外部存在事务则不执行存储过程
	if( (@@trancount<>0) or (@DbState = 0))
		return 1
	set xact_abort on                                         
	if(@DbState<>3 and len(@OrgKey)<>32)
		return 2
	if(@DbState <> 2)
		begin
		if not exists(select * from Key_OrgRoot where KeyId=@KeyId)
			return 3
		end

	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		update Key_OrgRoot set OrgKey = @OrgKey,KeyType=@KeyType,InfoRemark=@KeyDetail where KeyId = @KeyId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @OrgKeyId = OrgKeyId, @OrgPsamKeyId = OrgPsamKeyId from Config_SysParams;
			if(@KeyState = 1)
				begin
				if(@KeyType = 0 and @OrgKeyId <> @KeyId)
					update Config_SysParams set OrgKeyId = @KeyId;
				else if(@KeyType = 1 and @OrgPsamKeyId <> @KeyId)
					update Config_SysParams set OrgPsamKeyId = @KeyId;
				end
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
		insert into Key_OrgRoot values(@OrgKey,@KeyType,@KeyDetail);
		set @AddKeyId = @@IDENTITY
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @OrgKeyId = OrgKeyId, @OrgPsamKeyId = OrgPsamKeyId from Config_SysParams;
		if(@KeyState = 1)
			begin
			if(@KeyType = 0 and @OrgKeyId <> @@IDENTITY)
				update Config_SysParams set OrgKeyId = @@IDENTITY;
			else if(@KeyType = 1 and @OrgPsamKeyId <> @@IDENTITY)
				update Config_SysParams set OrgPsamKeyId = @@IDENTITY;
			end
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
		delete from Key_OrgRoot where KeyId = @KeyId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		select @OrgKeyId = OrgKeyId, @OrgPsamKeyId = OrgPsamKeyId from Config_SysParams;
		select @KeyIdDel = ISNULL(MAX(KeyId),1) from Key_OrgRoot;
		if(@KeyType = 0 and @OrgKeyId = @KeyId)
				update Config_SysParams set OrgKeyId = @KeyIdDel;
		else if(@KeyType = 1 and @OrgPsamKeyId = @KeyId)
				update Config_SysParams set OrgPsamKeyId = @KeyIdDel;
		if(@@error<>0) 
			 begin
			 rollback tran maintran
			 return 5
			 end			      
		commit tran miantran
		return 0
		end --删除记录end
GO


