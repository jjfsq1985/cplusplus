USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_GetPublishedCard') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_GetPublishedCard

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_GetPublishedCard    */

/*       创建时间 2015-05-14                              */

/*       获取已经发卡的CPU卡信息  */
                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_GetPublishedCard(
	@CardNum char(16),    --卡号
	@ApplicationIndex int
	) With Encryption
 AS    
    declare @KeyGuid uniqueidentifier  --发卡时使用密钥的唯一编号
	--如果外部存在事务则不执行存储过程
	if(@@trancount<>0)
		return 1
	set xact_abort on 
	--开始事务
	begin tran maintran
	select @KeyGuid = KeyGuid from Base_Card where CardNum = @CardNum;	
	select * from Base_Card inner join Base_Card_Key on Base_Card.KeyGuid = Base_Card_Key.KeyGuid where Base_Card.KeyGuid = @KeyGuid and Base_Card_Key.ApplicationIndex = @ApplicationIndex;
	if(@@ERROR <> 0)
		begin
		rollback tran maintran
		return 1
		end	
	commit tran miantran
	return 0
GO
