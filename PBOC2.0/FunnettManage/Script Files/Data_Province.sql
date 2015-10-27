USE [FunnettStation]
GO

/****** 加气站所在省代码 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Data_Province](
	[ProvinceCode] [char](2) NOT NULL,
	[ProvinceName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Data_Province] PRIMARY KEY CLUSTERED 
(
	[ProvinceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_Province', @level2type=N'COLUMN',@level2name=N'ProvinceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Data_Province', @level2type=N'COLUMN',@level2name=N'ProvinceName'
GO
     

insert into Data_Province values('11','北京市');
insert into Data_Province values('12','天津市');
insert into Data_Province values('13','河北省');
insert into Data_Province values('14','山西省');
insert into Data_Province values('15','内蒙古自治区');
insert into Data_Province values('21','辽宁省');
insert into Data_Province values('22','吉林省');
insert into Data_Province values('23','黑龙江');
insert into Data_Province values('31','上海市');
insert into Data_Province values('32','江苏省');
insert into Data_Province values('33','浙江省');
GO

insert into Data_Province values('34','安徽省');
insert into Data_Province values('35','福建省');
insert into Data_Province values('36','江西省');
insert into Data_Province values('37','山东省');
insert into Data_Province values('41','河南省');
insert into Data_Province values('42','湖北省');
insert into Data_Province values('43','湖南省');
insert into Data_Province values('44','广东省');
insert into Data_Province values('45','广西壮族自治区');
insert into Data_Province values('46','海南省');
insert into Data_Province values('50','重庆市');
GO

insert into Data_Province values('51','四川省');
insert into Data_Province values('52','贵州省');
insert into Data_Province values('53','云南省');
insert into Data_Province values('54','西藏自治区');
insert into Data_Province values('61','陕西省');
insert into Data_Province values('62','甘肃省');
insert into Data_Province values('63','青海省');
insert into Data_Province values('64','宁夏回族自治区');
insert into Data_Province values('65','新疆维吾尔自治区');
insert into Data_Province values('71','台湾省');
insert into Data_Province values('81','香港特别行政区');
insert into Data_Province values('82','澳门特别行政区');
GO

