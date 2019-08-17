
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 08/17/2019 23:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Message] [nvarchar](250) NULL,
	[Source] [varchar](50) NULL,
	[StackTrace] [ntext] NULL,
	[InnerException] [varchar](250) NULL,
	[Window] [varchar](50) NULL,
	[CreateDate] [char](10) NULL,
	[CreateTime] [char](8) NULL,
	[CreateUser] [char](8) NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF