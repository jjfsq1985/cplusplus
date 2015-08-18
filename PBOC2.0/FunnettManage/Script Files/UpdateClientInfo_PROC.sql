USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_UpdateClientInfo') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_UpdateClientInfo

/****** 单位信息记录 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_UpdateClientInfo    */

/*       创建时间 2015-04-03                              */

/*       更新单位相关信息                    */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_UpdateClientInfo(
	@ClientId int,--单位ID
	@ClientName nvarchar(50),--单位名称
	@ParentID int,--上级单位ID
	@ParentName nvarchar(50),  --上级单位名称
	@Linkman nvarchar(12),  --联系人
	@Telephone varchar(15), --联系电话
	@FaxNum varchar(50), --传真号码
	@Email varchar(50), --电子邮箱
	@ZipCode varchar(10), --邮编
	@Address nvarchar(50), --地址
	@Bank nvarchar(50),  --开户银行
	@BankAccountNum varchar(25), --银行账户
	@Remark nvarchar(50), --备注
	@DbState int --操作类型（0-不操作，1-更新，2-增加，3-删除）	
	) With Encryption
 AS 
    --如果外部存在事务则不执行存储过程
	if( (@@trancount<>0) or (@DbState = 0))
		return 1
	set xact_abort on   
	if(@ClientId <= 0 or len(@ClientName) <= 0)
		return 2		
	if(@DbState <> 2)
		begin
		if not exists(select * from Base_Client where ClientId=@ClientId)
			return 3
		end
	else
		begin
		if exists(select * from Base_Client where ClientId=@ClientId)--增加记录的@ClientId不能已经存在
			return 3
		end

	if(@DbState = 1) --更新记录		
		begin
		--开始事务
		begin tran maintran
		update Base_Client set ClientName = @ClientName,							
							Linkman=@Linkman,
							Telephone=@Telephone,
							FaxNum=@FaxNum,
							Email=@Email,
							Zipcode=@ZipCode,
							Address=@Address,
							Bank=@Bank,
							BankAccountNum=@BankAccountNum,
							Remark=@Remark where ClientId = @ClientId;
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
  		update Base_Client set ParentName=@ClientName where ParentID = @ClientId;
		if(@@ERROR <> 0)
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
		insert into Base_Client values(@ClientId,@ClientName,@ParentID,@ParentName,@Linkman,@Telephone,@FaxNum,@Email,@ZipCode,@Address,@Bank,@BankAccountNum,@Remark,0,0);		
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
		delete from Base_Client where ClientId = @ClientId;		
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end	
		delete from Base_Client where ParentID = @ClientId;
		if(@@error<>0) 
			 begin
			 rollback tran maintran
			 return 5
			 end			      
		commit tran miantran
		return 0
		end --删除记录end
GO


