USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_GetPsamKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_GetPsamKey

SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_GetPsamKey    */

/*       创建时间 2015-05-14                              */

/*       获取当前使用的Psam卡密钥                */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_GetPsamKey With Encryption
 AS    
    declare @PsamKeyId int  --PSAM卡密钥编号
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on 
	--开始事务
	begin tran maintran
	select @PsamKeyId = UsePsamKeyID from Config_SysParams;
	select * from Key_PsamCard where PsamId=@PsamKeyId;
	if(@@ERROR <> 0)
		begin
		rollback tran maintran
		return 1
		end	
	commit tran miantran
	return 0
GO


