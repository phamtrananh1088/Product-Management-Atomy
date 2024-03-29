
/****** Object:  Table [dbo].[PropLog]    Script Date: 08/17/2019 23:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PropLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogType] [smallint] NOT NULL,
	[LogCreateDate] [char](10) NULL,
	[LogCreateTime] [char](8) NULL,
	[LogCreateUser] [char](8) NULL,
	[PropCode] [char](8) NOT NULL,
	[PropName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Category] [nvarchar](50) NULL CONSTRAINT [DF_PropLog_Category]  DEFAULT (N'Atomy'),
	[Condition] [nvarchar](50) NULL CONSTRAINT [DF_PropLog_Condition]  DEFAULT (N'Tốt'),
	[AcquiredDate] [char](10) NULL,
	[Unit] [nvarchar](50) NULL,
	[PurchasePrice] [money] NULL CONSTRAINT [DF_PropLog_PurchasePrice]  DEFAULT ((0)),
	[SalesPrice] [money] NULL CONSTRAINT [DF_PropLog_SalesPrice]  DEFAULT ((0)),
	[CurrentValue] [money] NULL CONSTRAINT [DF_PropLog_CurrentValue]  DEFAULT ((0)),
	[Location] [nvarchar](255) NULL,
	[Manufacturer] [nvarchar](50) NULL CONSTRAINT [DF_PropLog_Manufacturer]  DEFAULT (N'Atomy.Co.,Ld (Korea)'),
	[Model] [varchar](20) NULL,
	[Comments] [nvarchar](1000) NULL,
	[Attachments] [binary](50) NULL,
	[Retired] [bit] NOT NULL CONSTRAINT [DF_PropLog_Retired]  DEFAULT ((0)),
	[RetiredDate] [char](10) NULL,
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
	[UpdateDate] [char](10) NULL,
	[UpdateTime] [char](8) NULL,
	[UpdateUser] [char](8) NULL,
 CONSTRAINT [PK_PropLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF