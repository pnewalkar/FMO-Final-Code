/****** Object:  Table [FMO].[AuditLog]    Script Date: 4/12/2017 8:54:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [FMO].[AuditLog](
	[AuditLog_Id] [uniqueidentifier] NOT NULL,	
	[TableName] [varchar](50) NULL,
	[RecordId] [varchar](50) NULL,	
	[ColumnName] [varchar](50) NULL,
	[OriginalValue] [nvarchar](MAX) NULL,
	[NewValue] [nvarchar](MAX) NULL,
	[UserId] [varchar](50) NULL,
	[EventType] [varchar](50) NULL,
	[EventTimeStamp] [datetime] NULL
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[AuditLog_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
