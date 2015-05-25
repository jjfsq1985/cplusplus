USE [FunnettStation]
GO

/****** 用户名、密码表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserDb](
	[UserId] [int] IDENTITY(0,1) NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[Password] [varchar](64) NOT NULL,
	[Authority] [int] NULL,
	[Status]   [int] NULL,
CONSTRAINT [PK_UserDb] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDb', @level2type=N'COLUMN',@level2name=N'Authority'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态(0未登录，1登录，2停用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDb', @level2type=N'COLUMN',@level2name=N'Status'
GO

--增加初始账户admin,密码admin(MD5格式)
insert into UserDb values('admin','21232F297A57A5A743894A0E4A801FC3',0x7FFFFFFF,0);