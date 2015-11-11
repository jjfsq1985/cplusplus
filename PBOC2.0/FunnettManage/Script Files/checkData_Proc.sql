USE [FunnettStation]
GO

if exists (select * from sysobjects where id = object_id(N'Pro_checkData') and type in (N'P', N'PC'))
drop procedure Pro_checkData                                                                        
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE  Pro_checkData With Encryption
As
BEGIN
  declare @SerialNo int,@curCount int,@i Int,@FGuno int,@Gun int
  set @curCount = 0
  SET @Gun = 0
  truncate table SC_OmissiveData
  DECLARE My_Cursor CURSOR 
	FOR select FSerialNo,FGunno from SC_ConsumerDetail order by FGunno,FSerialNo  
	OPEN My_Cursor; 
	FETCH NEXT FROM My_Cursor into @SerialNo,@FGuno; 
	WHILE @@FETCH_STATUS = 0
		BEGIN
		   if @curCount = 0 
		   begin
		       SET @curCount = @SerialNo
		       SET @Gun = @FGuno;
		   end
		    else if ((@SerialNo - @curCount) > 1) and (@Gun = @FGuno)
		    Begin		        
		      Set @i = @curCount + 1
		      while(@i < @SerialNo)
		      BEGIN
		         insert into SC_OmissiveData(FSerialNo,FGunNo)
		         values(@i,@FGuno) 
		        set @i=@i+1
		      END 
		    end 
		    SET @curCount = @SerialNo
		    Set @Gun = @FGuno 
			FETCH NEXT FROM My_Cursor into @SerialNo,@FGuno;
		END
	CLOSE My_Cursor; 
	DEALLOCATE My_Cursor;
END
GO


