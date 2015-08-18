USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdateAccount') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdateAccount

/****** 用户账户 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_UpdateAccount    */

/*       创建时间 2015-05-21                              */

/*       记录用户账户密码                                   */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdateAccount(
	@UserId int,--用户ID,增加记录时无效
	@UserName varchar(32),--用户名
	@Password varchar(64),--MD5加密的用户密码
	@Authority int,--权限
	@Status int,--状态（是否登陆）
	@DbState int, --操作类型（0-不操作，1-更新，2-增加，3-删除）	
	@AddUserId int output --增加记录时输出用户ID
	) With Encryption
 AS    
	--如果外部存在事务则不执行存储过程
	if( (@@trancount<>0) or (@DbState = 0))
		return 1
	set xact_abort on  
	if(@DbState <> 2)
		begin
		if not exists(select * from UserDb where UserId=@UserId)
			return 2
		end

	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		update UserDb set Password = @Password,
							Authority=@Authority,
							Status = @Status where UserId = @UserId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 3
		    end	
		commit tran miantran
		return 0
	end -- //更新记录end
	else if(@DbState = 2)--添加记录
		begin
		--开始事务
		begin tran maintran
		insert into UserDb values(@UserName,@Password,@Authority,@Status);
		set @AddUserId = @@IDENTITY
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 3
		    end	
		commit tran miantran
		return 0
		end --添加记录end
	else if(@DbState = 3)	--删除记录	
		begin
		--开始事务
		begin tran maintran
		delete from UserDb where UserId = @UserId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 3
		    end	
		commit tran miantran
		return 0
		end --删除记录end
GO


