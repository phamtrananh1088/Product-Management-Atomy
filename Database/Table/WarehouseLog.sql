
/****** Object:  Table [dbo].[Warehouse]    Script Date: 08/17/2019 23:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Warehouse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WareCode] [char](10) NOT NULL,
	[Type] [smallint] NOT NULL,
	[WareDate] [char](10) NOT NULL,
	[PropCode] [char](10) NOT NULL,
	[PropName] [char](10) NOT NULL,
	[Category] [nvarchar](50) NULL,
	[Status] [smallint] NOT NULL CONSTRAINT [DF_Warehouse_Status]  DEFAULT ((0)),
	[Description] [nvarchar](50) NULL,
	[Unit] [nvarchar](50) NULL,
	[UnitPrice] [money] NOT NULL CONSTRAINT [DF_Warehouse_UnitPrice]  DEFAULT ((0)),
	[CurrentPrice] [money] NOT NULL CONSTRAINT [DF_Warehouse_CurrentPrice]  DEFAULT ((0)),
	[Quantity] [smallint] NOT NULL CONSTRAINT [DF_Warehouse_Quantity]  DEFAULT ((0)),
	[Amount] [money] NOT NULL CONSTRAINT [DF_Warehouse_Amount]  DEFAULT ((0)),
	[DiscountDistribute] [money] NOT NULL CONSTRAINT [DF_Warehouse_DiscountDistribute]  DEFAULT ((0)),
	[Comments] [ntext] NULL,
	[Attachments] [binary](50) NULL,
	[UpdateCount] [smallint] NOT NULL CONSTRAINT [DF_Warehouse_UpdateCount]  DEFAULT ((1)),
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
	[UpdateDate] [char](10) NULL,
	[UpdateTime] [char](8) NULL,
	[UpdateUser] [char](8) NULL,
 CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF