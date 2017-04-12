/****** Object:  Table [FMO].[AuditLog]    Script Date: 4/12/2017 8:54:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [FMO].[AuditLog](
	[AuditLog_Id] [uniqueidentifier] NOT NULL,
	[EventTimeStamp] [datetime] NULL,
	[EventType] [varchar](50) NULL,
	[RecordId] [varchar](50) NULL,
	[TableName] [varchar](50) NULL,
	[ColumnName] [varchar](50) NULL,
	[OriginalValue] [varchar](500) NULL,
	[NewValue] [varchar](500) NULL,
	[UserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[AuditLog_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
