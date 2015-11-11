USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'PROC_RewriteCpuCard') and type in (N'P', N'PC'))
drop procedure PROC_RewriteCpuCard                                                                        
GO

/****** 用户卡制卡，发卡 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

/*************************************************************************************/

/*       Object:  Stored Procedure dbo.PROC_RewriteCpuCard                      */

/*       创建时间 2015-04-03                                                                   */

/*       记录制卡、发卡过程                                                                            */                                                                                                                 
/*************************************************************************************/

CREATE PROCEDURE PROC_RewriteCpuCard(
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
	@RelatedMotherCard char(16) --子卡关联的母卡卡号
	) With Encryption
 AS    	
    declare @curTime datetime --时间
    declare @SrcClientId   int
    declare @SrcRelatedMotherCard char(16)
    declare @SrcPersonalId  varchar(32)
    declare @SrcDriverName nvarchar(50)
    declare @SrcDriverTel varchar(32)
    declare @SrcSteelCylinderId varchar(32)
    declare @SrcFactoryNum char(7)
    declare @LogContent nvarchar(1024)
    set @curTime = GETDATE()
	--如果外部存在事务则不执行存储过程
	if (@@trancount<>0)
		return 1
	set xact_abort on                                         
	if(len(@CardId)<>16)
		return 2
	--判断卡号在吗
	if not exists(select * from Base_Card where CardNum=@CardId)
		return 3
begin
		select @SrcClientId=ClientId,@SrcRelatedMotherCard = RelatedMotherCard,@SrcPersonalId=PersonalId,@SrcDriverName=DriverName,@SrcDriverTel=DriverTel,@SrcSteelCylinderId= SteelCylinderId,@SrcFactoryNum=FactoryNum from Base_Card where CardNum=@CardId;
		--开始事务
		begin tran maintran
		update  Base_Card set ClientId = @ClientId,RelatedMotherCard=@RelatedMotherCard,UseValidateDate = @UseValidateDate, UseInvalidateDate = @UseInvalidateDate,
							Plate = @Plate,SelfId = @SelfId,PersonalId=@PersonalId,DriverName=@DriverName,DriverTel=@DriverTel,
							VechileCategory=@VechileCategory,SteelCylinderId=@SteelCylinderId,CylinderTestDate=@CylinderTestDate,Remark=@Remark,
							R_OilTimesADay=@R_OilTimesADay,R_OilVolATime=@R_OilVolATime,R_OilVolTotal=@R_OilVolTotal,R_OilEndDate=@R_OilEndDate,
							R_Plate=@R_Plate,R_Oil=@R_Oil,R_RFID=@R_RFID,CylinderNum=@CylinderNum,FactoryNum=@FactoryNum,CylinderVolume=@CylinderVolume,
							BusDistance=@BusDistance,OperateDateTime=@curTime where CardNum=@CardId;								
			if(@@ERROR <> 0)
			begin
		    rollback tran maintran
		    return 4
		    end		
		--插入修改卡信息的记录
		set @LogContent = '修改卡信息：' + '所属单位' + convert(varchar(10),@SrcClientId) + '->' + convert(varchar(10),@ClientId) + ';';
		set @LogContent	= 	@LogContent + '关联母卡' + @SrcRelatedMotherCard + '->' + @RelatedMotherCard + ';';
		set @LogContent	= 	@LogContent + '证件号' + @SrcPersonalId + '->' + @PersonalId + ';';
		set @LogContent	= 	@LogContent + '持卡人姓名' + @SrcDriverName + '->' + @DriverName + ';';
		set @LogContent	= 	@LogContent + '持卡人电话' + @SrcDriverTel + '->' + @DriverTel + ';';
		set @LogContent	= 	@LogContent + '气瓶编号' + @SrcSteelCylinderId + '->' + @SteelCylinderId + ';';
		set @LogContent	= 	@LogContent + '钢瓶生产厂家编号' + @SrcFactoryNum + '->' + @FactoryNum + ';';				             
		insert into Log_PublishCard values(@curTime,@LogContent,@ClientId,@CardId);
	commit tran miantran
	return 0
end
GO


