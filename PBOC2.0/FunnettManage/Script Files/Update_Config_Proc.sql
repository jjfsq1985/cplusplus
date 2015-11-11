USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'Pro_Upate_Config') and type in (N'P', N'PC'))
drop procedure Pro_Upate_Config                                                                        
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


Create     PROCEDURE  Pro_Upate_Config(
@filed  varchar(20),
@Value  varchar(10),
@type   int
    ) WITH Encryption
AS
declare @setvalue int
BEGIN

    select @setvalue = FValueI from SC_Config where FCode = @filed      
    if @type = 0
	 update SC_Config set FValueI = @setvalue +1 where  FCode = @filed  
    else if @type = 1
         update SC_Config set FValueI = Convert(int,@Value) where  FCode = @filed   
    else if @type = 2
         update SC_Config set FValueS = @Value where  FCode = @filed   
    else if @type = 3
         update SC_Config set FValueF = Convert(float,@Value) where  FCode = @filed   
	   
END



GO


