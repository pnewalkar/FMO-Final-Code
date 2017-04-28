/*Create File processing log to exception while inserting PAF ,NYB,USR details into Database*/

CREATE TABLE [FMO].[FileProcessingLog](
	[FileID] [uniqueidentifier] NOT NULL,
	[UDPRN] [int] NOT NULL,
	[FileType] [varchar](25) NOT NULL,
	[FileProcessing_TimeStamp] [datetime] NOT NULL,
	[FileName] [varchar](25) NOT NULL,
	[NatureOfError] [varchar](max) NOT NULL,
	[AmendmentType] [char](1) NOT NULL,
	[Comments] [varchar](max) NULL,
 CONSTRAINT [PK_FileProcessing_Log] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

-------------------------Alter Script -------------------


ALTER TABLE [FMO].[FileProcessingLog]
ALTER COLUMN [FileName] VARCHAR(100)