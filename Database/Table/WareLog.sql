
/****** Object:  Table [dbo].[WareLog]    Script Date: 08/17/2019 23:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WareLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogType] [smallint] NOT NULL,
	[LogCreateDate] [char](10) NULL,
	[LogCreateTime] [char](8) NULL,
	[LogCreateUser] [char](8) NULL,
	[WareCodeM] [char](10) NOT NULL,
	[TypeM] [smallint] NOT NULL,
	[WareDateM] [char](10) NOT NULL,
	[EmpCodeM] [char](8) NULL,
	[EmpNameM] [nvarchar](100) NULL,
	[CusCodeM] [char](8) NULL,
	[CusNameM] [nvarchar](100) NULL,
	[StatusM] [smallint] NOT NULL CONSTRAINT [DF_WareLog_Status]  DEFAULT ((0)),
	[WareTitleM] [nvarchar](100) NULL,
	[DescriptionM] [nvarchar](250) NULL,
	[TotalAmountM] [money] NOT NULL CONSTRAINT [DF_WareLog_TotalAmount]  DEFAULT ((0)),
	[DiscountM] [money] NOT NULL CONSTRAINT [DF_WareLog_Discount]  DEFAULT ((0)),
	[SalesAmountM] [money] NOT NULL CONSTRAINT [DF_WareLog_SalesAmount]  DEFAULT ((0)),
	[PaymentTypeM] [smallint] NOT NULL CONSTRAINT [DF_WareLog_PaymentType]  DEFAULT ((0)),
	[FinishFlagM] [smallint] NOT NULL CONSTRAINT [DF_WareLog_FinishFlag]  DEFAULT ((0)),
	[PaymentDateM] [char](10) NULL,
	[FinishDateM] [char](10) NULL,
	[CommentsM] [ntext] NULL,
	[AttachmentsM] [binary](50) NULL,
	[UpdateCountM] [smallint] NOT NULL CONSTRAINT [DF_WareLog_UpdateCount]  DEFAULT ((1)),
	[RetiredM] [bit] NOT NULL CONSTRAINT [DF_WareLog_Retired]  DEFAULT ((0)),
	[RetiredDateM] [char](10) NULL,
	[CreateDateM] [char](10) NULL,
	[CreateTimeM] [char](8) NULL,
	[CreateUserM] [char](8) NULL,
	[UpdateDateM] [char](10) NULL,
	[UpdateTimeM] [char](8) NULL,
	[UpdateUserM] [char](8) NULL,
	[WareCodeD] [char](10) NOT NULL,
	[TypeD] [smallint] NOT NULL,
	[WareDateD] [char](10) NOT NULL,
	[PropCodeD] [char](10) NOT NULL,
	[PropNameD] [char](10) NOT NULL,
	[CategoryD] [nvarchar](50) NULL,
	[StatusD] [smallint] NOT NULL CONSTRAINT [DF_WareLog_Status_1]  DEFAULT ((0)),
	[DescriptionD] [nvarchar](50) NULL,
	[UnitD] [nvarchar](50) NULL,
	[UnitPriceD] [money] NOT NULL CONSTRAINT [DF_WareLog_UnitPrice]  DEFAULT ((0)),
	[CurrentPriceD] [money] NOT NULL CONSTRAINT [DF_WareLog_CurrentPrice]  DEFAULT ((0)),
	[QuantityD] [smallint] NOT NULL CONSTRAINT [DF_WareLog_Quantity]  DEFAULT ((0)),
	[AmountD] [money] NOT NULL CONSTRAINT [DF_WareLog_Amount]  DEFAULT ((0)),
	[DiscountDistributeD] [money] NOT NULL CONSTRAINT [DF_WareLog_DiscountDistribute]  DEFAULT ((0)),
	[CommentsD] [ntext] NULL,
	[AttachmentsD] [binary](50) NULL,
	[UpdateCountD] [smallint] NOT NULL CONSTRAINT [DF_WareLog_UpdateCount_1]  DEFAULT ((1)),
	[CreateDateD] [char](10) NULL,
	[CreateTimeD] [char](8) NULL,
	[CreateUserD] [char](8) NULL,
	[UpdateDateD] [char](10) NULL,
	[UpdateTimeD] [char](8) NULL,
	[UpdateUserD] [char](8) NULL,
 CONSTRAINT [PK_WareLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF