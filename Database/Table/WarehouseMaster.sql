
/****** Object:  Table [dbo].[WarehouseMaster]    Script Date: 08/17/2019 23:23:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WarehouseMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WareCode] [char](10) NOT NULL,
	[Type] [smallint] NOT NULL,
	[WareDate] [char](10) NOT NULL,
	[EmpCode] [char](8) NULL,
	[EmpName] [nvarchar](100) NULL,
	[CusCode] [char](8) NULL,
	[CusName] [nvarchar](100) NULL,
	[Status] [smallint] NOT NULL CONSTRAINT [DF_WarehouseMaster_Status]  DEFAULT ((0)),
	[WareTitle] [nvarchar](100) NULL,
	[Description] [nvarchar](250) NULL,
	[TotalAmount] [money] NOT NULL CONSTRAINT [DF_WarehouseMaster_TotalAmount]  DEFAULT ((0)),
	[Discount] [money] NOT NULL CONSTRAINT [DF_WarehouseMaster_Discount]  DEFAULT ((0)),
	[SalesAmount] [money] NOT NULL CONSTRAINT [DF_WarehouseMaster_SalesAmount]  DEFAULT ((0)),
	[PaymentType] [smallint] NOT NULL CONSTRAINT [DF_WarehouseMaster_PaymentType]  DEFAULT ((0)),
	[FinishFlag] [smallint] NOT NULL CONSTRAINT [DF_WarehouseMaster_FinishFlag]  DEFAULT ((0)),
	[PaymentDate] [char](10) NULL,
	[FinishDate] [char](10) NULL,
	[Comments] [ntext] NULL,
	[Attachments] [binary](50) NULL,
	[UpdateCount] [smallint] NOT NULL CONSTRAINT [DF_WarehouseMaster_UpdateCount]  DEFAULT ((1)),
	[Retired] [bit] NOT NULL CONSTRAINT [DF_WarehouseMaster_Retired]  DEFAULT ((0)),
	[RetiredDate] [char](10) NULL CONSTRAINT [DF_WarehouseMaster_RetiredDate]  DEFAULT (''),
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
	[UpdateDate] [char](10) NULL,
	[UpdateTime] [char](8) NULL,
	[UpdateUser] [char](8) NULL,
 CONSTRAINT [PK_WarehouseMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF