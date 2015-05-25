USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_GetOrgKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_GetOrgKey

SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_GetOrgKey                      */

/*       创建时间 2015-05-14                                           */

/*       获取当前使用的厂商初始卡密钥                       */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_GetOrgKey(
	@OrgKeyType int
	)With Encryption
 AS
    ---@OrgKeyType 0-CPU卡，1-PSAM卡, 2-通用
    declare @OrgId int  --原始密钥编号
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on 
	--开始事务
	begin tran maintran
    if(@OrgKeyType <> 1)
		select @OrgId = OrgKeyId from Config_SysParams;
	else if(@OrgKeyType <> 0)
		select @OrgId = OrgPsamKeyId from Config_SysParams;
	select OrgKey from Key_OrgRoot where KeyId=@OrgId and (KeyType = @OrgKeyType or KeyType = 2);
	if(@@ERROR <> 0)
		begin
		rollback tran maintran
		return 1
		end	
	commit tran miantran
	return 0
GO


