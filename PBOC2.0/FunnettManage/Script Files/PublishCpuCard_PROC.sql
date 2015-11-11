USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_PublishCpuCard') and type in (N'P', N'PC'))
drop procedure PROC_PublishCpuCard                                                                        
GO

/****** 用户卡制卡，发卡 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_PublishCpuCard                      */

/*       创建时间 2015-04-03                                                                   */

/*       记录制卡、发卡过程                                                                            */                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_PublishCpuCard(
	@CardId char(16),--卡号
	@CardType varchar(2),--卡类型
	@ClientId int,--所属单位ID
	@UseValidateDate datetime,--卡生效日期
	@UseInvalidateDate datetime,--卡失效日期
	@Plate nvarchar(16),--车牌
	@SelfId varchar(50),--自编号
	@CertificatesType varchar(2), --证件类型
	@PersonalId varchar(32), --证件ID
	@DriverName nvarchar(50), --持卡人姓名
	@DriverTel varchar(32), --持卡人电话
	@VechileCategory nvarchar(8), --车类别
	@SteelCylinderId varchar(32), --气瓶编号
	@CylinderTestDate datetime, --气瓶有效期
	@Remark nvarchar(50), --备注
	@R_OilTimesADay int, --每天限加油次数	
	@R_OilVolATime decimal(18, 2), --每次限加油量
	@R_OilVolTotal decimal(18, 2), --每天最大油量
	@R_OilEndDate datetime, --加油截止日期
	@R_Plate bit, --限车牌
	@R_Oil varchar(4), --限油品
	@R_RFID bit, --限标签
	@CylinderNum int, --钢瓶数量
	@FactoryNum char(7), --钢瓶生产厂家编号
	@CylinderVolume int, --钢瓶容积
	@BusDistance varchar(10), --公交路数
	@UserKeyGuid uniqueidentifier,   --密钥信息的GUID
	@RelatedMotherCard char(16) --子卡关联的母卡卡号
	) With Encryption
 AS    
	declare @SrcKeyGuid  uniqueidentifier
	
    declare @curTime datetime --时间
    set @curTime = GETDATE()
	--如果外部存在事务则不执行存储过程
	if (@@trancount<>0)
		return 1
	set xact_abort on                                         
	if(len(@CardId)<>16)
		return 2
	--判断卡号在吗
	if exists(select * from Base_Card where CardNum=@CardId)
		begin
		select @SrcKeyGuid = KeyGuid from Base_Card where CardNum=@CardId;
		delete from Base_Card where CardNum=@CardId;
		delete from Base_Card_Key where KeyGuid=@SrcKeyGuid;
		end
begin
		--开始事务
		begin tran maintran
		insert into Base_Card values(@CardId,@CardType,@ClientId,0,@RelatedMotherCard,@UseValidateDate,@UseInvalidateDate,
									@Plate,@SelfId,@CertificatesType,@PersonalId,@DriverName,@DriverTel,
									@VechileCategory,@SteelCylinderId,@CylinderTestDate,@Remark,
									0,0,0,0,0,@R_OilTimesADay,@R_OilVolATime,@R_OilVolTotal,@R_OilEndDate,
									@R_Plate,@R_Oil,@R_RFID,@CylinderNum,@FactoryNum,@CylinderVolume,@BusDistance,'0',@curTime,@UserKeyGuid,0);		
		if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 3
		    end	
		commit tran miantran
end
	return 0
GO


