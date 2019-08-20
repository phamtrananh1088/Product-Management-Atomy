
/****** Object:  Table [dbo].[Customer]    Script Date: 08/17/2019 23:20:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CusCode] [char](8) NOT NULL,
	[Company] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[EmailAddress] [varchar](50) NULL,
	[JobTitle] [nvarchar](50) NULL,
	[BusinessPhone] [varchar](15) NULL,
	[HomePhone] [varchar](15) NULL,
	[MobilePhone] [varchar](15) NOT NULL,
	[FaxNumber] [varchar](15) NULL,
	[Address] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[StateProvince] [nvarchar](50) NULL,
	[ZIPPostalCode] [varchar](20) NULL,
	[CountryRegion] [nvarchar](50) NULL,
	[WebPage] [varchar](50) NULL,
	[FacebookID] [varchar](50) NULL,
	[Notes] [ntext] NULL,
	[Attachments] [binary](50) NULL,
	[Retired] [bit] NOT NULL CONSTRAINT [DF_Customer_Retired]  DEFAULT ((0)),
	[RetiredDate] [char](10) NULL CONSTRAINT [DF_Customer_RetiredDate]  DEFAULT (''),
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
	[UpdateDate] [char](10) NULL,
	[UpdateTime] [char](8) NULL,
	[UpdateUser] [char](8) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF