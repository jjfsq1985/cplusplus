USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'Pro_SC_ConsumerDetail') and type in (N'P', N'PC'))
drop procedure Pro_SC_ConsumerDetail                                                                        
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--站端售气明细插入  
CREATE PROCEDURE  Pro_SC_ConsumerDetail(   
  @ReValue int output,
  @FGUID	varchar(36),
  @FSerialNo	int,
  @FRecordType	varchar(4),
  @FTradeDateTime	datetime,
  @FUserCardNo	varchar(16),
  @FCardType    varchar(6),
  @FResidualAmount	decimal(18, 2),
  @FT_BAL	decimal(18, 2),
  @FMoney	decimal(15, 2),
  @FCTC	int,
  @FTAC	varchar(10),
  @FPSAM_TAC	varchar(8),
  @FGMAC	varchar(8),
  @FPSAM_ASN	varchar(20),
  @FPSAM_TID	varchar(12),
  @FPSAM_TTC	varchar(14),
  @FDS	int	,
  @FVER	varchar(2),
  @FGunNo	int	,
  @FFuelType	varchar(4),
  @FGas	decimal(15, 2),
  @FPrice	decimal(6, 2),
  @FShiftNo	varchar(2),
  @FSumGas	decimal(18, 2),
  @FDCT	varchar(4),
  @FStopDateTime	datetime,
  @FStartWay	varchar(4),
  @FStopReason	varchar(50),
  @FStartPress	decimal(15, 2),
  @FStopPress	decimal(15, 2),
  @FMediumTemperature	decimal(18, 2),
  @FMediumDensity	decimal(18, 2),
  @FDensity	decimal(15, 2),
  @FEquivalent	decimal(15, 2),
  @FFinanceDate	varchar(16),
  @FStoredNo	int,
  @FRecPrice	decimal(6, 2),
  @FOperatorCardNo	varchar(16),
  @FRecMonry	decimal(15, 2),
  @FPreMoney	decimal(15, 2),
  @FT_MAC	varchar(10),
  @FSaveDateTime	datetime,
  @FReGas      decimal(8,2),
  @FPriceVer   varchar(4),
  @RawData	varchar(600)
    ) With Encryption
AS  
 --declare @ReValue int  
BEGIN  
     SET NOCOUNT ON --不返回影响的行数  
     set @ReValue=0 --不成功  
  begin try  
      begin tran              
        --需要上传的表  
        insert into SC_ConsumerDetail(  
            FGUID,FSerialNo,FRecordType,FTradeDateTime,FUserCardNo,FCardType,FResidualAmount,FT_BAL,FMoney,FCTC,FTAC,FPSAM_TAC,
            FGMAC,FPSAM_ASN,FPSAM_TID,FPSAM_TTC,FDS,FVER,FGunNo,FFuelType,FGas,FPrice,FShiftNo,FSumGas,FDCT,FStopDateTime,
            FStartWay,FStopReason,FStartPress,FStopPress,FMediumTemperature,FMediumDensity,FDensity,FEquivalent,
            FFinanceDate,FStoredNo,FPreMoney,FOperatorCardNo,FRecMonry,FRecPrice,FT_MAC,FSaveDateTime,FUpFlag,FReGas,FPriceVer--,RawData
           )  
        values(  
            @FGUID,@FSerialNo,@FRecordType,@FTradeDateTime,@FUserCardNo,@FCardType,@FResidualAmount,@FT_BAL,@FMoney,@FCTC,@FTAC,@FPSAM_TAC,
            @FGMAC,@FPSAM_ASN,@FPSAM_TID,@FPSAM_TTC,@FDS,@FVER,@FGunNo,@FFuelType,@FGas,@FPrice,@FShiftNo,@FSumGas,@FDCT,
            @FStopDateTime,@FStartWay,@FStopReason,@FStartPress,@FStopPress,@FMediumTemperature,@FMediumDensity,@FDensity,
            @FEquivalent,@FFinanceDate,@FStoredNo,@FPreMoney,@FOperatorCardNo,@FRecMonry,@FRecPrice,@FT_MAC,@FSaveDateTime,'N',@FReGas,@FPriceVer--,@RawData
           )
           
       IF Exists( select 1 from SC_ConsumerDetail where FSerialNo < @FSerialNo)
         Update Base_Card set CardBalance = @FResidualAmount where CardNum = @FUserCardNo;
       delete SC_OmissiveData where FGunNo = @FGunNo and FSerialNo = @FSerialNo;
       set @ReValue=1  
      commit tran      
  end try  
  begin catch        
       --消息 2627:数据重复  
          set @ReValue=@@ERROR  
       if @ReValue=2627    
       begin  
           set @ReValue=1  
       end  
       rollback tran         
  end catch  
  SET NOCOUNT OFF  
  return @ReValue  
END  

GO
