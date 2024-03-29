
/****** Object:  Table [dbo].[Employee]    Script Date: 08/17/2019 23:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [char](8) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Department] [nvarchar](50) NULL,
	[Position] [nvarchar](50) NULL,
	[EmailAddress] [varchar](50) NULL,
	[BusinessPhone] [varchar](15) NULL,
	[HomePhone] [varchar](15) NULL,
	[MobilePhone] [varchar](15) NOT NULL,
	[FaxNumber] [varchar](15) NULL,
	[Address] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[StateProvince] [nvarchar](50) NULL,
	[ZIPPostalCode] [varchar](20) NULL,
	[CountryRegion] [nvarchar](50) NULL,
	[FacebookID] [varchar](50) NULL,
	[Notes] [ntext] NULL,
	[Attachments] [binary](50) NULL,
	[Retired] [bit] NOT NULL CONSTRAINT [DF_Employee_Retired]  DEFAULT ((0)),
	[RetiredDate] [char](10) NULL CONSTRAINT [DF_Employee_RetiredDate]  DEFAULT (''),
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
	[UpdateDate] [char](10) NULL,
	[UpdateTime] [char](8) NULL,
	[UpdateUser] [char](8) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF