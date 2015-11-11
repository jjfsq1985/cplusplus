USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_PublishPsamCard') and type in (N'P', N'PC'))
drop procedure PROC_PublishPsamCard
GO

/****** PSAM卡制卡，发卡 ******/
SET ANSI_NULLS ON
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
	@TerminalId varchar(12),--终端机编号
	@ClientId int,--所属单位ID
	@UseValidateDate datetime,--卡生效日期
	@UseInvalidateDate datetime,--卡失效日期
	@CompanyFrom varchar(16), --发行方
	@CompanyTo varchar(16), --接收方
	@Remark nvarchar(50), --备注
	@OrgKey char(32),    --卡片原始密钥
	@PsamMasterKey char(32), --主控密钥
	@AppMADKey char(32) --MAC或加密密钥
	) With Encryption
 AS    
    declare @SrcClientId   int
    declare @SrcOrgKey char(32)
    declare @SrcPsamMasterKey char(32)
    declare @SrcAppMADKey char(32)    
    declare @LogContent nvarchar(1024)
    declare @curTime datetime --时间
    set @curTime = GETDATE()
	--如果外部存在事务则不执行存储过程
	if (@@trancount<>0)
		return 1
	set xact_abort on                                         
	if(len(@PsamCardId)<>16 or len(@TerminalId) <> 12)
		return 2
	--判断卡号在吗
	if exists(select * from Psam_Card where PsamId=@PsamCardId)
		begin
			select @SrcClientId=ClientId,@SrcOrgKey=OrgKey,@SrcPsamMasterKey=PsamMasterKey,@SrcAppMADKey=MacEncryptKey from Psam_Card where PsamId=@PsamCardId;
			--开始事务
			begin tran maintran
			update  Psam_Card set TerminalId = @TerminalId,ClientId=@ClientId,CardState = 1,UseValidateDate = @UseValidateDate, UseInvalidateDate = @UseInvalidateDate,
							IssueCode = @CompanyFrom,RecvCode = @CompanyTo,Remark=@Remark,OperateDateTime=@curTime,OrgKey=@OrgKey,PsamMasterKey=@PsamMasterKey,MacEncryptKey=@AppMADKey;
			if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end		
			--插入修改卡信息的记录
			set @LogContent = '修改卡信息：' + '所属单位' + convert(varchar(10),@SrcClientId) + '->' + convert(varchar(10),@ClientId) + ';';
			set @LogContent	= 	@LogContent + '原始密钥' + @SrcOrgKey + '->' + @OrgKey + ';';
			set @LogContent	= 	@LogContent + '主控密钥' + @SrcPsamMasterKey + '->' + @PsamMasterKey + ';';
			set @LogContent	= 	@LogContent + 'MAC或加密密钥' + @SrcAppMADKey + '->' + @AppMADKey + ';';
			set @LogContent	= 	@LogContent + '终端机编号' + @TerminalId + ';';
			set @LogContent	= 	@LogContent + '发行方' + @CompanyFrom + ';';
			set @LogContent	= 	@LogContent + '接收方' + @CompanyTo + ';';				             
		insert into Log_PublishCard values(@curTime,@LogContent,@ClientId,@PsamCardId);
	commit tran miantran
		end
	else
		begin
			--开始事务
			begin tran maintran
			insert into Psam_Card values(@PsamCardId,@TerminalId,@ClientId,0,@UseValidateDate,@UseInvalidateDate,
									@CompanyFrom,@CompanyTo,@Remark,@curTime,@OrgKey,@PsamMasterKey,@AppMADKey,0);
			if(@@ERROR <> 0)
				begin
			    rollback tran maintran
			    return 4
				end		 
			commit tran miantran
		end
	return 0
GO


