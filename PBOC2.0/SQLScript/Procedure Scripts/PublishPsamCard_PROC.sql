USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_PublishPsamCard') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure PROC_PublishPsamCard

/****** PSAM卡制卡，发卡 ******/
SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_PublishPsamCard                      */

/*       创建时间 2015-04-16                                                                   */

/*       记录制卡、发卡过程                                                                            */                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_PublishPsamCard(
	@PsamCardId char(16),--PSAM卡号
	@TerminalId varchar(6),--终端机编号
	@ClientId int,--所属单位ID
	@UseValidateDate datetime,--卡生效日期
	@UseInvalidateDate datetime,--卡失效日期
	@CompanyFrom varchar(8), --发行方
	@CompanyTo varchar(8), --接收方
	@Remark nvarchar(50), --备注
	@OrgKey char(32),    --卡片原始密钥
	@PsamMasterKey char(32) --主控密钥
	) With Encryption
 AS    
    declare @curTime datetime --时间
    set @curTime = GETDATE()
	--如果外部存在事务则不执行存储过程
	if (@@trancount<>0)
		return 1
	set xact_abort on                                         
	if(len(@PsamCardId)<>16 or @TerminalId <> 12 or @ClientId <=0)
		return 2
	--判断卡号在吗
	if exists(select * from Psam_Card where PsamId=@PsamCardId)
	return 3
begin
		--开始事务
		begin tran maintran
		insert into Psam_Card values(@PsamCardId,@TerminalId,@ClientId,0,@UseValidateDate,@UseInvalidateDate,
									@CompanyFrom,@CompanyTo,@Remark,@curTime,@OrgKey,@PsamMasterKey,0);
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end		 
	commit tran miantran
	return 0
end
GO


