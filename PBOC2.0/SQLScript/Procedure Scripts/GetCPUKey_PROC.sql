USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_GetCpuKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_GetCpuKey

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_GetCpuKey    */

/*       创建时间 2015-05-14                              */

/*       获取当前使用的CPU卡密钥,涉及Key_CpuCard卡密钥表和Key_CARD_ADF应用密钥表  */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_GetCpuKey(
	@ApplicationIndex int    --应用索引
	) With Encryption
 AS    
    declare @CpuKeyId int  --cpu卡密钥编号
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on 
	--开始事务
	begin tran maintran
	select @CpuKeyId = UseKeyID from Config_SysParams;
	select * from Key_CpuCard inner join Key_CARD_ADF on Key_CpuCard.KeyId = Key_CARD_ADF.RelatedKeyId and Key_CpuCard.KeyId=@CpuKeyId and Key_CARD_ADF.ApplicationIndex = @ApplicationIndex;
	if(@@ERROR <> 0)
		begin
		rollback tran maintran
		return 1
		end	
	commit tran miantran
	return 0
GO
