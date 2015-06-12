USE [FunnettStation]
GO

/****** 客户单位信息表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Base_Client](
	[ClientId] int NOT NULL,
	[ClientName] [nvarchar](50) NOT NULL,
	[ParentID] int NULL,
	[ParentName] [nvarchar](50) NULL,
	[Linkman] [nvarchar](12) NULL,
	[Telephone] [varchar](15) NULL,
	[FaxNum] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Zipcode] [varchar](10) NULL,
	[Address] [nvarchar](50) NULL,
	[Bank] [nvarchar](50) NULL,
	[BankAccountNum] [varchar](25) NULL,	
	[Remark] [nvarchar](50) NULL,
	[IsSelfUnit] [bit] NULL,
	[UpLoadStatus] [int] NOT NULL,
 CONSTRAINT [PK_Base_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'ClientId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'ClientName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级单位编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'ParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'ParentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Linkman'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Telephone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'传真' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'FaxNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Email'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Zipcode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Address'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户银行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'Bank'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'BankAccountNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否自定单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'IsSelfUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经上传' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Client', @level2type=N'COLUMN',@level2name=N'UpLoadStatus'
GO

insert into Base_Client values(1,N'测试单位',0,N'',N'admin','051288888888',N'',N'','215600',N'张家港市杨舍镇晨新路19号', N'',N'',N'测试发卡使用',0,0);
GO