USE [FMODB03]
GO
/****** Object:  Schema [FMO]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SCHEMA [FMO]
GO
/****** Object:  Table [FMO].[AccessLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AccessLink](
	[ID] [uniqueidentifier] NOT NULL,
	[Approved] [bit] NULL,
	[WorkloadLengthMeter] [numeric](18, 8) NOT NULL,
	[AccessLinkTypeGUID] [uniqueidentifier] NULL,
	[LinkDirectionGUID] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
	[ConnectedNetworkLinkID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AccessLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[AccessLinkStatus]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AccessLinkStatus](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[NetworkLinkID] [uniqueidentifier] NOT NULL,
	[AccessLinkStatusGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_AccessLinkStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Action]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Action](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[DisplayText] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[AddressLocation]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AddressLocation](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[UDPRN] [int] NOT NULL,
	[LocationXY] [geometry] NOT NULL,
	[Lattitude] [numeric](38, 8) NOT NULL,
	[Longitude] [numeric](38, 8) NOT NULL,
 CONSTRAINT [PK_AddressLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[AMUChangeRequest]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[AMUChangeRequest](
	[ID] [uniqueidentifier] NOT NULL,
	[UnitCommentText] [varchar](300) NULL,
	[AddressChanges] [varchar](300) NULL,
	[RaisedDate] [datetime] NULL,
	[ClosedDate] [datetime] NULL,
	[AMUClarificationText] [varchar](300) NULL,
	[ChangeRequestTypeGUID] [uniqueidentifier] NULL,
	[ChangeRequestStatusGUID] [uniqueidentifier] NULL,
	[CurrentAddress_GUID] [uniqueidentifier] NULL,
	[NewAddress_GUID] [uniqueidentifier] NULL,
	[ChangeReasonCode_GUID] [uniqueidentifier] NULL,
	[RequestPostcode_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AMUChangeRequest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Block]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[Block](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[BlockTypeGUID] [uniqueidentifier] NOT NULL,
	[BlockSpanMinute] [numeric](10, 2) NULL,
	[PairedBlockID] [uniqueidentifier] NULL,
	[GeoRouteBlockID] [int] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[BlockSequence]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[BlockSequence](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[OrderIndex] [numeric](16, 8) NULL,
	[BlockID] [uniqueidentifier] NOT NULL,
	[LocationID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_BlockActivitySequence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DBVersion]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[DBVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SchemaVersion] [varchar](100) NOT NULL,
	[DataVersion] [varchar](100) NOT NULL,
	[VersionDesc] [varchar](max) NOT NULL,
	[CreationTime] [datetime] NOT NULL CONSTRAINT [DF_DBVersion_CreationTime]  DEFAULT (getdate()),
	[LastUpdatedTime] [datetime] NOT NULL CONSTRAINT [DF_DBVersion_LastUpdatedTime]  DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DeliveryPoint]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryPoint](
	[ID] [uniqueidentifier] NOT NULL,
	[PostalAddressID] [uniqueidentifier] NOT NULL,
	[MultipleOccupancyCount] [smallint] NULL,
	[MailVolume] [int] NULL,
	[DeliveryPointUseIndicatorGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_DeliveryPoint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryPointStatus]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryPointStatus](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationID] [uniqueidentifier] NOT NULL,
	[DeliveryPointStatusGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_DeliveryPointStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Function]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Function](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[ActionID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Location]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[Location](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[AlternateID] [int] IDENTITY(1,1) NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
	[Shape] [geometry] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[LocationOffering]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[LocationOffering](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationID] [uniqueidentifier] NOT NULL,
	[OfferingID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_LocationOffering] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[LocationPostcodeHierarchy]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[LocationPostcodeHierarchy](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationID] [uniqueidentifier] NOT NULL,
	[PostcodeHierarchyID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_LocationPostcodeHierarchy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[LocationReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[LocationReferenceData](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationID] [uniqueidentifier] NOT NULL,
	[ReferenceDataID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_LocationReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[LocationRelationship]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[LocationRelationship](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationID] [uniqueidentifier] NOT NULL,
	[RelatedLocationID] [uniqueidentifier] NOT NULL,
	[RelationshipTypeGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_LocationRelationship] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[NetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkLink](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NULL,
	[LinkGeometry] [geometry] NOT NULL,
	[LinkLength] [numeric](18, 4) NOT NULL,
	[LinkGradientType] [int] NULL,
	[NetworkLinkTypeGUID] [uniqueidentifier] NULL,
	[DataProviderGUID] [uniqueidentifier] NULL,
	[RoadNameGUID] [uniqueidentifier] NULL,
	[StreetNameGUID] [uniqueidentifier] NULL,
	[StartNodeID] [uniqueidentifier] NULL,
	[EndNodeID] [uniqueidentifier] NULL,
	[LinkName] [varchar](255) NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_NetworkLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkLinkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkLinkReference](
	[ID] [uniqueidentifier] NOT NULL,
	[OSRoadLinkTOID] [char](20) NULL,
	[OSRoadLinkID] [uniqueidentifier] NULL,
	[NetworkReferenceID] [uniqueidentifier] NOT NULL,
	[NetworkLinkID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_NetworkLinkReference] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkNode](
	[ID] [uniqueidentifier] NOT NULL,
	[NetworkNodeType_GUID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NULL,
	[DataProviderGUID] [uniqueidentifier] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_NetworkNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkReference](
	[ID] [uniqueidentifier] NOT NULL,
	[ReferenceType] [char](10) NULL,
	[NodeReferenceTOID] [char](20) NULL,
	[NodeReferenceLocation] [geometry] NULL,
	[PointReferenceLocation] [geometry] NULL,
	[PointReferenceRoadLinkTOID] [char](20) NULL,
	[ExternalNetworkRef] [varchar](50) NULL,
	[NetworkNodeID] [uniqueidentifier] NULL,
	[PointReferenceNetworkLinkID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_NetworkReference] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Notification]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Notification](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Notification Heading] [varchar](50) NULL,
	[Notification Message] [varchar](300) NULL,
	[NotificationDueDate] [datetime] NULL,
	[NotificationActionLink] [nvarchar](2000) NULL,
	[NotificationSource] [varchar](50) NULL,
	[PostcodeDistrict] [char](4) NULL,
	[PostcodeSector] [char](6) NULL,
	[NotificationTypeGUID] [uniqueidentifier] NULL,
	[NotificationPriorityGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Offering]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Offering](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[OfferingDescription] [varchar](50) NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Offering] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSAccessRestriction]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSAccessRestriction](
	[ID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NOT NULL,
	[RestrictionValue] [varchar](21) NULL,
	[InclusionVehicleQualifier] [text] NULL,
	[ExclusionVehicleQualifier] [text] NULL,
	[TimeInterval] [text] NULL,
	[NetworkReferenceID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OSAccessRestriction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSConnectingLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSConnectingLink](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NOT NULL,
	[Fictitious] [bit] NULL,
	[ConnectingNodeTOID] [char](20) NOT NULL,
	[PathNodeTOID] [char](20) NOT NULL,
	[Geometry] [geometry] NOT NULL,
	[ConnectingNodeID] [uniqueidentifier] NOT NULL,
	[PathNodeID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OSConnectingLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSConnectingLink_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSConnectingNode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSConnectingNode](
	[ID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NOT NULL,
	[Location] [geometry] NOT NULL,
	[RoadLinkTOID] [char](20) NULL,
	[RoadLinkID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OSConnectingNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSConnectingNode_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSPathLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSPathLink](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NOT NULL,
	[CentreLineGeometry] [geometry] NULL,
	[Ficticious] [binary](1) NULL,
	[FormOfWay] [varchar](42) NULL,
	[PathName] [varchar](255) NULL,
	[AlternateName] [varchar](255) NULL,
	[LengthInMeters] [numeric](38, 8) NOT NULL,
	[StartNodeTOID] [char](20) NULL,
	[EndNodeTOID] [char](20) NULL,
	[StartGradeSeparation] [tinyint] NOT NULL,
	[EndGradeSeparation] [tinyint] NOT NULL,
	[FormPartOf] [char](20) NULL,
	[StartNode_GUID] [uniqueidentifier] NULL,
	[EndNode_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PathLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSPathLink_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSPathNode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSPathNode](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NOT NULL,
	[Location] [geometry] NULL,
	[formOfRoadNode] [varchar](21) NULL,
	[Classification] [varchar](19) NULL,
	[ReasonForChange] [varchar](32) NULL,
 CONSTRAINT [PK_OSPathNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSPathNode_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSRestrictionForVehicles]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRestrictionForVehicles](
	[ID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NOT NULL,
	[MeasureInMeters] [float] NULL,
	[RestrictionType] [text] NULL,
	[SourceofMeasure] [char](10) NULL,
	[Inclusion] [text] NULL,
	[Exclusion] [text] NULL,
	[Structure] [varchar](50) NULL,
	[TrafficSign] [varchar](120) NULL,
	[NetworkReference_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RestrictionForVehicles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSRoadLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRoadLink](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NOT NULL,
	[CentreLineGeometry] [geometry] NOT NULL,
	[Ficticious] [bit] NULL,
	[RoadClassificaton] [varchar](21) NULL,
	[RouteHierarchy] [varchar](32) NULL,
	[FormOfWay] [varchar](42) NULL,
	[TrunkRoad] [binary](1) NULL,
	[PrimaryRoute] [binary](1) NULL,
	[RoadClassificationNumber] [char](10) NULL,
	[RoadName] [varchar](255) NULL,
	[AlternateName] [varchar](255) NULL,
	[Directionality] [varchar](21) NULL,
	[LengthInMeters] [numeric](38, 8) NOT NULL,
	[StartNodeTOID] [char](20) NULL,
	[EndNodeTOID] [char](20) NULL,
	[StartGradeSeparation] [tinyint] NOT NULL,
	[EndGradeSeparation] [tinyint] NOT NULL,
	[OperationalState] [varchar](19) NULL,
	[StartNode_GUID] [uniqueidentifier] NULL,
	[EndNode_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OSRoadLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSRoadLink_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSRoadNode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRoadNode](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TOID] [char](20) NOT NULL,
	[validFrom] [datetime] NULL,
	[Location] [geometry] NULL,
	[formOfRoadNode] [varchar](21) NULL,
	[Classification] [varchar](19) NULL,
	[access] [char](5) NULL,
	[junctionName] [varchar](120) NULL,
	[JunctionNumber] [varchar](30) NULL,
	[ReasonForChange] [varchar](32) NULL,
 CONSTRAINT [PK_OSRoadNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_OSRoadNode_TOID] UNIQUE NONCLUSTERED 
(
	[TOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSTurnRestriction]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSTurnRestriction](
	[ID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NOT NULL,
	[Restriction] [varchar](34) NULL,
	[inclusion] [varchar](50) NULL,
	[Exclusion] [varchar](50) NULL,
	[TimeInterval] [varchar](50) NULL,
	[NetworkReferenceID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TurnRestriction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[POBox]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[POBox](
	[ID] [uniqueidentifier] NOT NULL,
	[POBoxNumber] [bigint] NOT NULL,
	[POBoxTypeGUID] [uniqueidentifier] NULL,
	[POBoxLinkedPostalAddressID] [uniqueidentifier] NULL,
	[RowCreateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_POBox_Id] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[PostalAddress]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostalAddress](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[PostcodeType] [char](1) NOT NULL,
	[OrganisationName] [varchar](60) NULL,
	[DepartmentName] [varchar](60) NULL,
	[BuildingName] [varchar](50) NULL,
	[BuildingNumber] [smallint] NULL,
	[SubBuildingName] [varchar](50) NULL,
	[Thoroughfare] [varchar](80) NULL,
	[DependentThoroughfare] [varchar](80) NULL,
	[DependentLocality] [varchar](35) NULL,
	[DoubleDependentLocality] [varchar](35) NULL,
	[PostTown] [varchar](30) NOT NULL,
	[Postcode] [char](8) NOT NULL,
	[DeliveryPointSuffix] [char](2) NULL,
	[SmallUserOrganisationIndicator] [char](1) NULL,
	[UDPRN] [int] NULL,
	[AMUApproved] [bit] NULL,
	[POBoxNumber] [char](6) NULL,
	[AddressType_GUID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostalAddressIdentifier]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostalAddressIdentifier](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[PostalAddressID] [uniqueidentifier] NULL,
	[IdentifierTypeGUID] [uniqueidentifier] NULL,
	[ExternalID] [varchar](100) NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PostalAddressIdentifier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostalAddressStatus]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[PostalAddressStatus](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[PostalAddressGUID] [uniqueidentifier] NOT NULL,
	[OperationalStatusGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_PostalAddressStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Postcode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Postcode](
	[ID] [uniqueidentifier] NOT NULL,
	[PostcodeUnit] [char](8) NOT NULL,
	[OutwardCode] [char](4) NOT NULL,
	[InwardCode] [char](3) NOT NULL,
	[PrimaryRouteGUID] [uniqueidentifier] NULL,
	[SecondaryRouteGUID] [uniqueidentifier] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Postcode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Postcode_PostcodeUnit] UNIQUE NONCLUSTERED 
(
	[PostcodeUnit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostcodeHierarchy]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostcodeHierarchy](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Postcode] [char](8) NOT NULL,
	[ParentPostcode] [char](8) NULL,
	[PostcodeTypeGUID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_PostcodeArea] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[ProductOrServiceOffering]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[ProductOrServiceOffering](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[OfferingID] [uniqueidentifier] NOT NULL,
	[ProductOrServiceID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_ProductOrServiceOffering] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[ReferenceData](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ReferenceDataName] [varchar](1000) NULL,
	[ReferenceDataValue] [varchar](1000) NULL,
	[DataDescription] [varchar](1000) NULL,
	[DisplayText] [varchar](1000) NULL,
	[ReferenceDataCategoryID] [uniqueidentifier] NOT NULL,
	[ParentReferenceDataID] [uniqueidentifier] NULL,
	[OrderingIndex] [int] NULL,
	[Default] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[ReferenceDataCategory]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[ReferenceDataCategory](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[CategoryName] [varchar](50) NOT NULL,
	[Maintainable] [bit] NOT NULL,
	[CategoryType] [int] NOT NULL,
 CONSTRAINT [PK_ReferenceDataCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RMGLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RMGLink](
	[ID] [uniqueidentifier] NOT NULL,
	[LinkType] [char](1) NULL,
	[StartNodeReference] [char](20) NULL,
	[StartNodeType] [char](2) NULL,
	[EndNodeType] [char](2) NULL,
	[EndNodeReference] [char](20) NULL,
	[Geometry] [geometry] NULL,
 CONSTRAINT [PK_RMGLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RMGNode]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RMGNode](
	[ID] [uniqueidentifier] NOT NULL,
	[Location] [geometry] NULL,
	[FormofNode] [bigint] NULL,
	[NodeOnLink] [bit] NULL,
	[OSLinkReference] [char](20) NULL,
	[OSLinkType] [char](1) NULL,
 CONSTRAINT [PK_RMGNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RoadName]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RoadName](
	[ID] [uniqueidentifier] NOT NULL,
	[TOID] [char](20) NULL,
	[NationalRoadCode] [char](10) NULL,
	[roadClassification] [char](21) NULL,
	[DesignatedName] [varchar](255) NULL,
 CONSTRAINT [PK_Road] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Role]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Role](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[RoleName] [varchar](50) NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RoleFunction]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RoleFunction](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[RoleID] [uniqueidentifier] NOT NULL,
	[FunctionID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_RoleFunction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_RoleIDFunctionID] UNIQUE NONCLUSTERED 
(
	[RoleID] ASC,
	[FunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Route]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Route](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[GeoRouteID] [int] NULL,
	[RouteName] [char](30) NULL,
	[RouteNumber] [char](10) NULL,
	[SpanTimeMinute] [numeric](10, 2) NULL,
	[RouteBarcode] [char](20) NULL,
	[RouteMethodTypeGUID] [uniqueidentifier] NOT NULL,
	[TotalDistanceMeter] [float] NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[LastModifiedDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
	[PairedRouteID] [uniqueidentifier] NULL,
	[UnSequencedBlockID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Route] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RouteActivity]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RouteActivity](
	[ID] [uniqueidentifier] NOT NULL,
	[ActivityTypeGUID] [uniqueidentifier] NULL,
	[RouteActivityOrderIndex] [numeric](16, 8) NULL,
	[BlockID] [uniqueidentifier] NULL,
	[LocationID] [uniqueidentifier] NULL,
	[RouteID] [uniqueidentifier] NOT NULL,
	[ActivityDurationMinute] [float] NULL,
	[ResourceGUID] [uniqueidentifier] NULL,
	[DistanceToNextLocationMeter] [float] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_RouteActivitySequence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[RouteNetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RouteNetworkLink](
	[ID] [uniqueidentifier] NOT NULL,
	[OrderIndex] [numeric](8, 8) NULL,
	[RouteActivityID] [uniqueidentifier] NULL,
	[NetworkLinkID] [uniqueidentifier] NULL,
	[RowCreateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_RouteNetworkLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[RouteStatus]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RouteStatus](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[RouteID] [uniqueidentifier] NOT NULL,
	[RouteStatusGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_RouteStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Scenario]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Scenario](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ScenarioName] [varchar](50) NOT NULL,
	[LocationID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[LastModifiedDateTime] [datetime] NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Scenario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[ScenarioDayOfTheWeek]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[ScenarioDayOfTheWeek](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ScenarioID] [uniqueidentifier] NOT NULL,
	[DayOfTheWeekGUID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_Table1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[ScenarioRoute]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[ScenarioRoute](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ScenarioID] [uniqueidentifier] NOT NULL,
	[RouteID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime())
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[ScenarioStatus]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[ScenarioStatus](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ScenarioID] [uniqueidentifier] NOT NULL,
	[ScenarioStatusGUID] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_ScenarioStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[SpecialInstruction]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[SpecialInstruction](
	[InstructionText] [varchar](300) NULL,
	[OperationalObject_Id] [int] NOT NULL,
	[DaysofTheWeek] [smallint] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NOT NULL,
	[InstructionType_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SpecialInstruction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[StreetName]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[StreetName](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[USRN] [char](12) NULL,
	[NationalRoadCode] [char](10) NULL,
	[DesignatedName] [varchar](120) NULL,
	[LocalName] [varchar](120) NULL,
	[Descriptor] [varchar](120) NULL,
	[RoadClassification] [char](21) NULL,
	[StreetType] [varchar](35) NULL,
	[Geometry] [geometry] NULL,
	[StreetNameProvider] [char](1) NOT NULL,
	[OperationalState] [varchar](35) NULL,
	[OperationalStateReason] [varchar](120) NULL,
	[OperationalStateStartTime] [datetime] NULL,
	[OperationalStateEndTime] [datetime] NULL,
	[Locality] [varchar](35) NULL,
	[Town] [varchar](30) NULL,
	[AdministrativeArea] [varchar](30) NULL,
 CONSTRAINT [PK_OSStreet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[StreetNameNetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[StreetNameNetworkLink](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[StreetNameID] [uniqueidentifier] NULL,
	[NetworkLinkID] [uniqueidentifier] NULL,
	[USRN] [char](12) NULL,
	[RoadLinkTOID] [char](20) NULL,
 CONSTRAINT [PK_StreetNameNetworkLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[SupportingDeliveryPoint]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[SupportingDeliveryPoint](
	[ID] [uniqueidentifier] NOT NULL,
	[GroupName] [varchar](50) NULL,
	[NumberOfFloors] [tinyint] NULL,
	[InternalDistanceMeters] [float] NULL,
	[WorkloadTimeOverrideMinutes] [float] NULL,
	[TimeOverrideReason] [varchar](300) NULL,
	[TimeOverrideApproved] [bit] NULL,
	[ServicePoint] [bit] NULL,
	[GroupTypeGUID] [uniqueidentifier] NULL,
	[ServicePointTypeGUID] [uniqueidentifier] NULL,
	[SupportDeliveryPointTypeGUID] [uniqueidentifier] NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SupportingDeliveryPoint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[User]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[User](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[UserName] [varchar](50) NOT NULL,
	[RowCreateDateTime] [datetime] NOT NULL DEFAULT (sysdatetime()),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[UserRoleLocation]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[UserRoleLocation](
	[ID] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[UserID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[LocationID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserRoleLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_UserRoleLocation] UNIQUE NONCLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC,
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [FMO].[Vw_GetAccessFunction]    Script Date: 7/18/2017 2:24:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 CREATE View [FMO].[Vw_GetAccessFunction] 
 as
 Select r.RoleName, url.LocationID,UserName, f.Name as FunctionName, a.DisplayText as ActionName, u.ID as UserId
 from [FMO].[Function] f 
 inner join [FMO].[RoleFunction] rf on f.ID = rf.FunctionID
 inner join [FMO].[Action] a on a.ID = f.ActionID
 inner join [FMO].[UserRoleLocation] url on url.RoleID = rf.RoleID
 inner join [FMO].[User] u on u.ID = url.UserID
 inner join [FMO].[Role] r on r.ID = url.RoleID



GO
/****** Object:  Index [IXFK_AccessLink_NetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLink_NetworkLink] ON [FMO].[AccessLink]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AccessLink_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLink_ReferenceData_02] ON [FMO].[AccessLink]
(
	[AccessLinkTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AccessLink_ReferenceData_03]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLink_ReferenceData_03] ON [FMO].[AccessLink]
(
	[LinkDirectionGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AccessLinkStatus_AccessLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLinkStatus_AccessLink] ON [FMO].[AccessLinkStatus]
(
	[NetworkLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AccessLinkStatus_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLinkStatus_ReferenceData] ON [FMO].[AccessLinkStatus]
(
	[AccessLinkStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AddressLocation_UDPRN]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IDX_AddressLocation_UDPRN] ON [FMO].[AddressLocation]
(
	[UDPRN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_PostalAddress]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_PostalAddress] ON [FMO].[AMUChangeRequest]
(
	[CurrentAddress_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_PostalAddress_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_PostalAddress_02] ON [FMO].[AMUChangeRequest]
(
	[NewAddress_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_Postcode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_Postcode] ON [FMO].[AMUChangeRequest]
(
	[RequestPostcode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData] ON [FMO].[AMUChangeRequest]
(
	[ChangeRequestTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData_02] ON [FMO].[AMUChangeRequest]
(
	[ChangeRequestStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData_03]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData_03] ON [FMO].[AMUChangeRequest]
(
	[ChangeReasonCode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Block_Block]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Block_Block] ON [FMO].[Block]
(
	[PairedBlockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Block_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Block_ReferenceData] ON [FMO].[Block]
(
	[BlockTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_BlockSequence_Block]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_BlockSequence_Block] ON [FMO].[BlockSequence]
(
	[BlockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_BlockSequence_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_BlockSequence_Location] ON [FMO].[BlockSequence]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPoint_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPoint_NetworkNode] ON [FMO].[DeliveryPoint]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPoint_PostalAddress_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPoint_PostalAddress_02] ON [FMO].[DeliveryPoint]
(
	[PostalAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPoint_ReferenceData_03]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPoint_ReferenceData_03] ON [FMO].[DeliveryPoint]
(
	[DeliveryPointUseIndicatorGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPointStatus_DeliveryPoint]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPointStatus_DeliveryPoint] ON [FMO].[DeliveryPointStatus]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPointStatus_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPointStatus_ReferenceData] ON [FMO].[DeliveryPointStatus]
(
	[DeliveryPointStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Function_Action]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Function_Action] ON [FMO].[Function]
(
	[ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationOffering_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationOffering_Location] ON [FMO].[LocationOffering]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationOffering_Offering]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationOffering_Offering] ON [FMO].[LocationOffering]
(
	[OfferingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationPostcodeHierarchy_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationPostcodeHierarchy_Location] ON [FMO].[LocationPostcodeHierarchy]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationPostcodeHierarchy_PostcodeHierarchy]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationPostcodeHierarchy_PostcodeHierarchy] ON [FMO].[LocationPostcodeHierarchy]
(
	[PostcodeHierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationReferenceData_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationReferenceData_Location] ON [FMO].[LocationReferenceData]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationReferenceData_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationReferenceData_ReferenceData] ON [FMO].[LocationReferenceData]
(
	[ReferenceDataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationRelationship_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationRelationship_Location] ON [FMO].[LocationRelationship]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationRelationship_Location_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationRelationship_Location_02] ON [FMO].[LocationRelationship]
(
	[RelatedLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_LocationRelationship_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_LocationRelationship_ReferenceData] ON [FMO].[LocationRelationship]
(
	[RelationshipTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_NetworkNode] ON [FMO].[NetworkLink]
(
	[StartNodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_NetworkNode_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_NetworkNode_02] ON [FMO].[NetworkLink]
(
	[EndNodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_ReferenceData] ON [FMO].[NetworkLink]
(
	[NetworkLinkTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_ReferenceData_02] ON [FMO].[NetworkLink]
(
	[DataProviderGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_RoadName]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_RoadName] ON [FMO].[NetworkLink]
(
	[RoadNameGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_StreetName]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_StreetName] ON [FMO].[NetworkLink]
(
	[StreetNameGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReferenceLink_NetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReferenceLink_NetworkLink] ON [FMO].[NetworkLinkReference]
(
	[NetworkLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReferenceLink_NetworkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReferenceLink_NetworkReference] ON [FMO].[NetworkLinkReference]
(
	[NetworkReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReferenceLink_OSRoadLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReferenceLink_OSRoadLink] ON [FMO].[NetworkLinkReference]
(
	[OSRoadLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkNode_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkNode_Location] ON [FMO].[NetworkNode]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkNode_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkNode_ReferenceData] ON [FMO].[NetworkNode]
(
	[NetworkNodeType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReference_NetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReference_NetworkLink] ON [FMO].[NetworkReference]
(
	[PointReferenceNetworkLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReference_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReference_NetworkNode] ON [FMO].[NetworkReference]
(
	[NetworkNodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Notification_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Notification_ReferenceData] ON [FMO].[Notification]
(
	[NotificationTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Notification_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Notification_ReferenceData_02] ON [FMO].[Notification]
(
	[NotificationPriorityGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Access Restriction_NetworkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Access Restriction_NetworkReference] ON [FMO].[OSAccessRestriction]
(
	[NetworkReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSConnectingLink_OSConnectingNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingLink_OSConnectingNode] ON [FMO].[OSConnectingLink]
(
	[ConnectingNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSConnectingLink_OSConnectingNode_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingLink_OSConnectingNode_02] ON [FMO].[OSConnectingLink]
(
	[ConnectingNodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSConnectingLink_OSPathNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingLink_OSPathNode] ON [FMO].[OSConnectingLink]
(
	[PathNodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSConnectingNode_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingNode_NetworkNode] ON [FMO].[OSConnectingNode]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSConnectingNode_OSRoadLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingNode_OSRoadLink] ON [FMO].[OSConnectingNode]
(
	[RoadLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode] ON [FMO].[OSPathLink]
(
	[StartNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode_02] ON [FMO].[OSPathLink]
(
	[EndNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode_03]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode_03] ON [FMO].[OSPathLink]
(
	[StartNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode_04]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode_04] ON [FMO].[OSPathLink]
(
	[EndNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSPathNode_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathNode_NetworkNode] ON [FMO].[OSPathNode]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RestrictionForVehicles_NetworkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RestrictionForVehicles_NetworkReference] ON [FMO].[OSRestrictionForVehicles]
(
	[NetworkReference_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode] ON [FMO].[OSRoadLink]
(
	[StartNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode_02] ON [FMO].[OSRoadLink]
(
	[EndNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode_03]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode_03] ON [FMO].[OSRoadLink]
(
	[StartNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode_04]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode_04] ON [FMO].[OSRoadLink]
(
	[EndNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_OSRoadNode_NetworkNode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadNode_NetworkNode] ON [FMO].[OSRoadNode]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_TurnRestriction_NetworkReference]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_TurnRestriction_NetworkReference] ON [FMO].[OSTurnRestriction]
(
	[NetworkReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_POBox_Address_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_POBox_Address_02] ON [FMO].[POBox]
(
	[POBoxLinkedPostalAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_POBox_PostalAddress]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_POBox_PostalAddress] ON [FMO].[POBox]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_POBox_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_POBox_ReferenceData] ON [FMO].[POBox]
(
	[POBoxTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Address_UDPRN]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IXFK_Address_UDPRN] ON [FMO].[PostalAddress]
(
	[UDPRN] ASC
)
WHERE ([UDPRN] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_PostalAddress_Postcode]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddress_Postcode] ON [FMO].[PostalAddress]
(
	[Postcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddress_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddress_ReferenceData_02] ON [FMO].[PostalAddress]
(
	[AddressType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddressIdentifier_PostalAddress]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddressIdentifier_PostalAddress] ON [FMO].[PostalAddressIdentifier]
(
	[PostalAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddressIdentifier_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddressIdentifier_ReferenceData] ON [FMO].[PostalAddressIdentifier]
(
	[IdentifierTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddressStatus_PostalAddress]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddressStatus_PostalAddress] ON [FMO].[PostalAddressStatus]
(
	[PostalAddressGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddressStatus_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddressStatus_ReferenceData] ON [FMO].[PostalAddressStatus]
(
	[OperationalStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Postcode_PostcodeHierarchy]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Postcode_PostcodeHierarchy] ON [FMO].[Postcode]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Postcode_Route]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Postcode_Route] ON [FMO].[Postcode]
(
	[PrimaryRouteGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Postcode_Route_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Postcode_Route_02] ON [FMO].[Postcode]
(
	[SecondaryRouteGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostcodeHierarchy_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostcodeHierarchy_ReferenceData] ON [FMO].[PostcodeHierarchy]
(
	[PostcodeTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ProductOrServiceOffering_Offering]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ProductOrServiceOffering_Offering] ON [FMO].[ProductOrServiceOffering]
(
	[OfferingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ProductOrServiceOffering_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ProductOrServiceOffering_ReferenceData] ON [FMO].[ProductOrServiceOffering]
(
	[ProductOrServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ReferenceData_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ReferenceData_ReferenceData] ON [FMO].[ReferenceData]
(
	[ParentReferenceDataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ReferenceData_ReferenceDataCategory]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ReferenceData_ReferenceDataCategory] ON [FMO].[ReferenceData]
(
	[ReferenceDataCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RoleFunction_Function]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RoleFunction_Function] ON [FMO].[RoleFunction]
(
	[FunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RoleFunction_Role]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RoleFunction_Role] ON [FMO].[RoleFunction]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivity_Block]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivity_Block] ON [FMO].[RouteActivity]
(
	[BlockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivity_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivity_Location] ON [FMO].[RouteActivity]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivity_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivity_ReferenceData] ON [FMO].[RouteActivity]
(
	[ResourceGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivity_Route]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivity_Route] ON [FMO].[RouteActivity]
(
	[RouteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivitySequence_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivitySequence_ReferenceData] ON [FMO].[RouteActivity]
(
	[ActivityTypeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteNetworkLink_NetworkLink]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteNetworkLink_NetworkLink] ON [FMO].[RouteNetworkLink]
(
	[NetworkLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteNetworkLink_RouteActivity]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteNetworkLink_RouteActivity] ON [FMO].[RouteNetworkLink]
(
	[RouteActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteStatus_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteStatus_ReferenceData] ON [FMO].[RouteStatus]
(
	[RouteStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteStatus_Route]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteStatus_Route] ON [FMO].[RouteStatus]
(
	[RouteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Scenario_Unit]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_Scenario_Unit] ON [FMO].[Scenario]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioDayOfTheWeek_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioDayOfTheWeek_ReferenceData] ON [FMO].[ScenarioDayOfTheWeek]
(
	[DayOfTheWeekGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioDayOfTheWeek_Scenario]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioDayOfTheWeek_Scenario] ON [FMO].[ScenarioDayOfTheWeek]
(
	[ScenarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioRoute_Route]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioRoute_Route] ON [FMO].[ScenarioRoute]
(
	[RouteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioRoute_Scenario]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioRoute_Scenario] ON [FMO].[ScenarioRoute]
(
	[ScenarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioStatus_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioStatus_ReferenceData] ON [FMO].[ScenarioStatus]
(
	[ScenarioStatusGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ScenarioStatus_Scenario]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_ScenarioStatus_Scenario] ON [FMO].[ScenarioStatus]
(
	[ScenarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_SpecialInstruction_ReferenceData]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_SpecialInstruction_ReferenceData] ON [FMO].[SpecialInstruction]
(
	[OperationalObjectType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_SpecialInstruction_ReferenceData_02]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_SpecialInstruction_ReferenceData_02] ON [FMO].[SpecialInstruction]
(
	[InstructionType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_SupportingDeliveryPoint_DeliveryPoint]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_SupportingDeliveryPoint_DeliveryPoint] ON [FMO].[SupportingDeliveryPoint]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UserRoleLocation_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_UserRoleLocation_Location] ON [FMO].[UserRoleLocation]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UserRoleLocation_Role]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_UserRoleLocation_Role] ON [FMO].[UserRoleLocation]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UserRoleLocation_User]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE NONCLUSTERED INDEX [IXFK_UserRoleLocation_User] ON [FMO].[UserRoleLocation]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [FMO].[AMUChangeRequest] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[NetworkLinkReference] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[NetworkLinkReference] ADD  DEFAULT (newsequentialid()) FOR [NetworkReferenceID]
GO
ALTER TABLE [FMO].[NetworkReference] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSAccessRestriction] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSRestrictionForVehicles] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSTurnRestriction] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[POBox] ADD  DEFAULT (sysdatetime()) FOR [RowCreateDateTime]
GO
ALTER TABLE [FMO].[RMGLink] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RMGNode] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RoadName] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RouteNetworkLink] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RouteNetworkLink] ADD  DEFAULT (sysdatetime()) FOR [RowCreateDateTime]
GO
ALTER TABLE [FMO].[SpecialInstruction] ADD  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[SupportingDeliveryPoint] ADD  DEFAULT (sysdatetime()) FOR [RowCreateDateTime]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_NetworkLink] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkLink] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_NetworkLink]
GO
ALTER TABLE [FMO].[AccessLink]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessLink_NetworkLink2] FOREIGN KEY([ConnectedNetworkLinkID])
REFERENCES [FMO].[NetworkLink] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_NetworkLink2]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData_02] FOREIGN KEY([AccessLinkTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData_02]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData_03] FOREIGN KEY([LinkDirectionGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData_03]
GO
ALTER TABLE [FMO].[AccessLinkStatus]  WITH CHECK ADD  CONSTRAINT [FK_AccessLinkStatus_AccessLink] FOREIGN KEY([NetworkLinkID])
REFERENCES [FMO].[AccessLink] ([ID])
GO
ALTER TABLE [FMO].[AccessLinkStatus] CHECK CONSTRAINT [FK_AccessLinkStatus_AccessLink]
GO
ALTER TABLE [FMO].[AccessLinkStatus]  WITH CHECK ADD  CONSTRAINT [FK_AccessLinkStatus_ReferenceData] FOREIGN KEY([AccessLinkStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLinkStatus] CHECK CONSTRAINT [FK_AccessLinkStatus_ReferenceData]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_PostalAddress] FOREIGN KEY([CurrentAddress_GUID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_PostalAddress]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_PostalAddress_02] FOREIGN KEY([NewAddress_GUID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_PostalAddress_02]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_Postcode] FOREIGN KEY([RequestPostcode_GUID])
REFERENCES [FMO].[Postcode] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_Postcode]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData] FOREIGN KEY([ChangeRequestTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData_02] FOREIGN KEY([ChangeRequestStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData_02]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData_03] FOREIGN KEY([ChangeReasonCode_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData_03]
GO
ALTER TABLE [FMO].[Block]  WITH CHECK ADD  CONSTRAINT [FK_Block_Block] FOREIGN KEY([PairedBlockID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[Block] CHECK CONSTRAINT [FK_Block_Block]
GO
ALTER TABLE [FMO].[Block]  WITH CHECK ADD  CONSTRAINT [FK_Block_ReferenceData] FOREIGN KEY([BlockTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Block] CHECK CONSTRAINT [FK_Block_ReferenceData]
GO
ALTER TABLE [FMO].[BlockSequence]  WITH CHECK ADD  CONSTRAINT [FK_BlockSequence_Block] FOREIGN KEY([BlockID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[BlockSequence] CHECK CONSTRAINT [FK_BlockSequence_Block]
GO
ALTER TABLE [FMO].[BlockSequence]  WITH CHECK ADD  CONSTRAINT [FK_BlockSequence_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[BlockSequence] CHECK CONSTRAINT [FK_BlockSequence_Location]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_NetworkNode] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_NetworkNode]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_PostalAddress] FOREIGN KEY([PostalAddressID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_PostalAddress]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_ReferenceData_03] FOREIGN KEY([DeliveryPointUseIndicatorGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_ReferenceData_03]
GO
ALTER TABLE [FMO].[DeliveryPointStatus]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPointStatus_DeliveryPoint] FOREIGN KEY([LocationID])
REFERENCES [FMO].[DeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPointStatus] CHECK CONSTRAINT [FK_DeliveryPointStatus_DeliveryPoint]
GO
ALTER TABLE [FMO].[DeliveryPointStatus]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPointStatus_ReferenceData] FOREIGN KEY([DeliveryPointStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPointStatus] CHECK CONSTRAINT [FK_DeliveryPointStatus_ReferenceData]
GO
ALTER TABLE [FMO].[Function]  WITH CHECK ADD  CONSTRAINT [FK_Function_Action] FOREIGN KEY([ActionID])
REFERENCES [FMO].[Action] ([ID])
GO
ALTER TABLE [FMO].[Function] CHECK CONSTRAINT [FK_Function_Action]
GO
ALTER TABLE [FMO].[LocationOffering]  WITH CHECK ADD  CONSTRAINT [FK_LocationOffering_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[LocationOffering] CHECK CONSTRAINT [FK_LocationOffering_Location]
GO
ALTER TABLE [FMO].[LocationOffering]  WITH CHECK ADD  CONSTRAINT [FK_LocationOffering_Offering] FOREIGN KEY([OfferingID])
REFERENCES [FMO].[Offering] ([ID])
GO
ALTER TABLE [FMO].[LocationOffering] CHECK CONSTRAINT [FK_LocationOffering_Offering]
GO
ALTER TABLE [FMO].[LocationPostcodeHierarchy]  WITH CHECK ADD  CONSTRAINT [FK_LocationPostcodeHierarchy_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[LocationPostcodeHierarchy] CHECK CONSTRAINT [FK_LocationPostcodeHierarchy_Location]
GO
ALTER TABLE [FMO].[LocationPostcodeHierarchy]  WITH CHECK ADD  CONSTRAINT [FK_LocationPostcodeHierarchy_PostcodeHierarchy] FOREIGN KEY([PostcodeHierarchyID])
REFERENCES [FMO].[PostcodeHierarchy] ([ID])
GO
ALTER TABLE [FMO].[LocationPostcodeHierarchy] CHECK CONSTRAINT [FK_LocationPostcodeHierarchy_PostcodeHierarchy]
GO
ALTER TABLE [FMO].[LocationReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_LocationReferenceData_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[LocationReferenceData] CHECK CONSTRAINT [FK_LocationReferenceData_Location]
GO
ALTER TABLE [FMO].[LocationReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_LocationReferenceData_ReferenceData] FOREIGN KEY([ReferenceDataID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[LocationReferenceData] CHECK CONSTRAINT [FK_LocationReferenceData_ReferenceData]
GO
ALTER TABLE [FMO].[LocationRelationship]  WITH CHECK ADD  CONSTRAINT [FK_LocationRelationship_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[LocationRelationship] CHECK CONSTRAINT [FK_LocationRelationship_Location]
GO
ALTER TABLE [FMO].[LocationRelationship]  WITH CHECK ADD  CONSTRAINT [FK_LocationRelationship_Location_02] FOREIGN KEY([RelatedLocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[LocationRelationship] CHECK CONSTRAINT [FK_LocationRelationship_Location_02]
GO
ALTER TABLE [FMO].[LocationRelationship]  WITH CHECK ADD  CONSTRAINT [FK_LocationRelationship_ReferenceData] FOREIGN KEY([RelationshipTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[LocationRelationship] CHECK CONSTRAINT [FK_LocationRelationship_ReferenceData]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_NetworkNode] FOREIGN KEY([StartNodeID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_NetworkNode]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_NetworkNode_02] FOREIGN KEY([EndNodeID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_NetworkNode_02]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_OSStreet] FOREIGN KEY([StreetNameGUID])
REFERENCES [FMO].[StreetName] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_OSStreet]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_ReferenceData] FOREIGN KEY([NetworkLinkTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_ReferenceData]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_ReferenceData_02] FOREIGN KEY([DataProviderGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_ReferenceData_02]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_Road] FOREIGN KEY([RoadNameGUID])
REFERENCES [FMO].[RoadName] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_Road]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_NetworkLink] FOREIGN KEY([NetworkLinkID])
REFERENCES [FMO].[NetworkLink] ([ID])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_NetworkLink]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_NetworkReference] FOREIGN KEY([NetworkReferenceID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_NetworkReference]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_OSRoadLink] FOREIGN KEY([OSRoadLinkID])
REFERENCES [FMO].[OSRoadLink] ([ID])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_OSRoadLink]
GO
ALTER TABLE [FMO].[NetworkNode]  WITH CHECK ADD  CONSTRAINT [FK_NetworkNode_Location] FOREIGN KEY([ID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[NetworkNode] CHECK CONSTRAINT [FK_NetworkNode_Location]
GO
ALTER TABLE [FMO].[NetworkNode]  WITH CHECK ADD  CONSTRAINT [FK_NetworkNode_ReferenceData] FOREIGN KEY([NetworkNodeType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkNode] CHECK CONSTRAINT [FK_NetworkNode_ReferenceData]
GO
ALTER TABLE [FMO].[NetworkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReference_NetworkLink] FOREIGN KEY([PointReferenceNetworkLinkID])
REFERENCES [FMO].[NetworkLink] ([ID])
GO
ALTER TABLE [FMO].[NetworkReference] CHECK CONSTRAINT [FK_NetworkReference_NetworkLink]
GO
ALTER TABLE [FMO].[NetworkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReference_NetworkNode] FOREIGN KEY([NetworkNodeID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkReference] CHECK CONSTRAINT [FK_NetworkReference_NetworkNode]
GO
ALTER TABLE [FMO].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_ReferenceData] FOREIGN KEY([NotificationTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Notification] CHECK CONSTRAINT [FK_Notification_ReferenceData]
GO
ALTER TABLE [FMO].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_ReferenceData_02] FOREIGN KEY([NotificationPriorityGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Notification] CHECK CONSTRAINT [FK_Notification_ReferenceData_02]
GO
ALTER TABLE [FMO].[OSAccessRestriction]  WITH CHECK ADD  CONSTRAINT [FK_Access Restriction_NetworkReference] FOREIGN KEY([NetworkReferenceID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[OSAccessRestriction] CHECK CONSTRAINT [FK_Access Restriction_NetworkReference]
GO
ALTER TABLE [FMO].[OSConnectingLink]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingLink_OSConnectingNode] FOREIGN KEY([ConnectingNodeID])
REFERENCES [FMO].[OSConnectingNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingLink] CHECK CONSTRAINT [FK_OSConnectingLink_OSConnectingNode]
GO
ALTER TABLE [FMO].[OSConnectingLink]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingLink_OSPathNode] FOREIGN KEY([PathNodeID])
REFERENCES [FMO].[OSPathNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingLink] CHECK CONSTRAINT [FK_OSConnectingLink_OSPathNode]
GO
ALTER TABLE [FMO].[OSConnectingNode]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingNode_NetworkNode] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingNode] CHECK CONSTRAINT [FK_OSConnectingNode_NetworkNode]
GO
ALTER TABLE [FMO].[OSConnectingNode]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingNode_OSRoadLink] FOREIGN KEY([RoadLinkID])
REFERENCES [FMO].[OSRoadLink] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingNode] CHECK CONSTRAINT [FK_OSConnectingNode_OSRoadLink]
GO
ALTER TABLE [FMO].[OSPathLink]  WITH CHECK ADD  CONSTRAINT [FK_OSPathLink_OSPathNode] FOREIGN KEY([StartNode_GUID])
REFERENCES [FMO].[OSPathNode] ([ID])
GO
ALTER TABLE [FMO].[OSPathLink] CHECK CONSTRAINT [FK_OSPathLink_OSPathNode]
GO
ALTER TABLE [FMO].[OSPathLink]  WITH CHECK ADD  CONSTRAINT [FK_OSPathLink_OSPathNode_02] FOREIGN KEY([EndNode_GUID])
REFERENCES [FMO].[OSPathNode] ([ID])
GO
ALTER TABLE [FMO].[OSPathLink] CHECK CONSTRAINT [FK_OSPathLink_OSPathNode_02]
GO
ALTER TABLE [FMO].[OSPathNode]  WITH CHECK ADD  CONSTRAINT [FK_OSPathNode_NetworkNode] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[OSPathNode] CHECK CONSTRAINT [FK_OSPathNode_NetworkNode]
GO
ALTER TABLE [FMO].[OSRestrictionForVehicles]  WITH CHECK ADD  CONSTRAINT [FK_RestrictionForVehicles_NetworkReference] FOREIGN KEY([NetworkReference_GUID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[OSRestrictionForVehicles] CHECK CONSTRAINT [FK_RestrictionForVehicles_NetworkReference]
GO
ALTER TABLE [FMO].[OSRoadLink]  WITH CHECK ADD  CONSTRAINT [FK_OSRoadLink_OSRoadNode] FOREIGN KEY([StartNode_GUID])
REFERENCES [FMO].[OSRoadNode] ([ID])
GO
ALTER TABLE [FMO].[OSRoadLink] CHECK CONSTRAINT [FK_OSRoadLink_OSRoadNode]
GO
ALTER TABLE [FMO].[OSRoadLink]  WITH CHECK ADD  CONSTRAINT [FK_OSRoadLink_OSRoadNode_02] FOREIGN KEY([EndNode_GUID])
REFERENCES [FMO].[OSRoadNode] ([ID])
GO
ALTER TABLE [FMO].[OSRoadLink] CHECK CONSTRAINT [FK_OSRoadLink_OSRoadNode_02]
GO
ALTER TABLE [FMO].[OSRoadNode]  WITH CHECK ADD  CONSTRAINT [FK_OSRoadNode_NetworkNode] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[OSRoadNode] CHECK CONSTRAINT [FK_OSRoadNode_NetworkNode]
GO
ALTER TABLE [FMO].[OSTurnRestriction]  WITH CHECK ADD  CONSTRAINT [FK_TurnRestriction_NetworkReference] FOREIGN KEY([NetworkReferenceID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[OSTurnRestriction] CHECK CONSTRAINT [FK_TurnRestriction_NetworkReference]
GO
ALTER TABLE [FMO].[POBox]  WITH CHECK ADD  CONSTRAINT [FK_POBox_Address_02] FOREIGN KEY([POBoxLinkedPostalAddressID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[POBox] CHECK CONSTRAINT [FK_POBox_Address_02]
GO
ALTER TABLE [FMO].[POBox]  WITH CHECK ADD  CONSTRAINT [FK_POBox_PostalAddress] FOREIGN KEY([ID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[POBox] CHECK CONSTRAINT [FK_POBox_PostalAddress]
GO
ALTER TABLE [FMO].[POBox]  WITH CHECK ADD  CONSTRAINT [FK_POBox_ReferenceData] FOREIGN KEY([POBoxTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[POBox] CHECK CONSTRAINT [FK_POBox_ReferenceData]
GO
ALTER TABLE [FMO].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddress_Postcode] FOREIGN KEY([Postcode])
REFERENCES [FMO].[Postcode] ([PostcodeUnit])
GO
ALTER TABLE [FMO].[PostalAddress] CHECK CONSTRAINT [FK_PostalAddress_Postcode]
GO
ALTER TABLE [FMO].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddress_ReferenceData_02] FOREIGN KEY([AddressType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostalAddress] CHECK CONSTRAINT [FK_PostalAddress_ReferenceData_02]
GO
ALTER TABLE [FMO].[PostalAddressIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddressIdentifier_PostalAddress] FOREIGN KEY([PostalAddressID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[PostalAddressIdentifier] CHECK CONSTRAINT [FK_PostalAddressIdentifier_PostalAddress]
GO
ALTER TABLE [FMO].[PostalAddressIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddressIdentifier_ReferenceData] FOREIGN KEY([IdentifierTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostalAddressIdentifier] CHECK CONSTRAINT [FK_PostalAddressIdentifier_ReferenceData]
GO
ALTER TABLE [FMO].[PostalAddressStatus]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddressStatus_PostalAddress] FOREIGN KEY([PostalAddressGUID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[PostalAddressStatus] CHECK CONSTRAINT [FK_PostalAddressStatus_PostalAddress]
GO
ALTER TABLE [FMO].[PostalAddressStatus]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddressStatus_ReferenceData] FOREIGN KEY([OperationalStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostalAddressStatus] CHECK CONSTRAINT [FK_PostalAddressStatus_ReferenceData]
GO
ALTER TABLE [FMO].[Postcode]  WITH CHECK ADD  CONSTRAINT [FK_Postcode_PostcodeHierarchy] FOREIGN KEY([ID])
REFERENCES [FMO].[PostcodeHierarchy] ([ID])
GO
ALTER TABLE [FMO].[Postcode] CHECK CONSTRAINT [FK_Postcode_PostcodeHierarchy]
GO
ALTER TABLE [FMO].[Postcode]  WITH CHECK ADD  CONSTRAINT [FK_Postcode_Route] FOREIGN KEY([PrimaryRouteGUID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[Postcode] CHECK CONSTRAINT [FK_Postcode_Route]
GO
ALTER TABLE [FMO].[Postcode]  WITH CHECK ADD  CONSTRAINT [FK_Postcode_Route_02] FOREIGN KEY([SecondaryRouteGUID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[Postcode] CHECK CONSTRAINT [FK_Postcode_Route_02]
GO
ALTER TABLE [FMO].[PostcodeHierarchy]  WITH CHECK ADD  CONSTRAINT [FK_PostcodeHierarchy_ReferenceData] FOREIGN KEY([PostcodeTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostcodeHierarchy] CHECK CONSTRAINT [FK_PostcodeHierarchy_ReferenceData]
GO
ALTER TABLE [FMO].[ProductOrServiceOffering]  WITH CHECK ADD  CONSTRAINT [FK_ProductOrServiceOffering_Offering] FOREIGN KEY([OfferingID])
REFERENCES [FMO].[Offering] ([ID])
GO
ALTER TABLE [FMO].[ProductOrServiceOffering] CHECK CONSTRAINT [FK_ProductOrServiceOffering_Offering]
GO
ALTER TABLE [FMO].[ProductOrServiceOffering]  WITH CHECK ADD  CONSTRAINT [FK_ProductOrServiceOffering_ReferenceData] FOREIGN KEY([ProductOrServiceID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[ProductOrServiceOffering] CHECK CONSTRAINT [FK_ProductOrServiceOffering_ReferenceData]
GO
ALTER TABLE [FMO].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_ReferenceData] FOREIGN KEY([ParentReferenceDataID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_ReferenceData]
GO
ALTER TABLE [FMO].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_ReferenceDataCategory] FOREIGN KEY([ReferenceDataCategoryID])
REFERENCES [FMO].[ReferenceDataCategory] ([ID])
GO
ALTER TABLE [FMO].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_ReferenceDataCategory]
GO
ALTER TABLE [FMO].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Function] FOREIGN KEY([FunctionID])
REFERENCES [FMO].[Function] ([ID])
GO
ALTER TABLE [FMO].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Function]
GO
ALTER TABLE [FMO].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Role] FOREIGN KEY([RoleID])
REFERENCES [FMO].[Role] ([ID])
GO
ALTER TABLE [FMO].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Role]
GO
ALTER TABLE [FMO].[Route]  WITH NOCHECK ADD  CONSTRAINT [FK_Route_Route] FOREIGN KEY([PairedRouteID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[Route] CHECK CONSTRAINT [FK_Route_Route]
GO
ALTER TABLE [FMO].[RouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivity_Block] FOREIGN KEY([BlockID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[RouteActivity] CHECK CONSTRAINT [FK_RouteActivity_Block]
GO
ALTER TABLE [FMO].[RouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivity_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[RouteActivity] CHECK CONSTRAINT [FK_RouteActivity_Location]
GO
ALTER TABLE [FMO].[RouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivity_ReferenceData] FOREIGN KEY([ResourceGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[RouteActivity] CHECK CONSTRAINT [FK_RouteActivity_ReferenceData]
GO
ALTER TABLE [FMO].[RouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivity_Route] FOREIGN KEY([RouteID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[RouteActivity] CHECK CONSTRAINT [FK_RouteActivity_Route]
GO
ALTER TABLE [FMO].[RouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivitySequence_ReferenceData] FOREIGN KEY([ActivityTypeGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[RouteActivity] CHECK CONSTRAINT [FK_RouteActivitySequence_ReferenceData]
GO
ALTER TABLE [FMO].[RouteNetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_RouteNetworkLink_NetworkLink] FOREIGN KEY([NetworkLinkID])
REFERENCES [FMO].[NetworkLink] ([ID])
GO
ALTER TABLE [FMO].[RouteNetworkLink] CHECK CONSTRAINT [FK_RouteNetworkLink_NetworkLink]
GO
ALTER TABLE [FMO].[RouteNetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_RouteNetworkLink_RouteActivity] FOREIGN KEY([RouteActivityID])
REFERENCES [FMO].[RouteActivity] ([ID])
GO
ALTER TABLE [FMO].[RouteNetworkLink] CHECK CONSTRAINT [FK_RouteNetworkLink_RouteActivity]
GO
ALTER TABLE [FMO].[RouteStatus]  WITH CHECK ADD  CONSTRAINT [FK_RouteStatus_ReferenceData] FOREIGN KEY([RouteStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[RouteStatus] CHECK CONSTRAINT [FK_RouteStatus_ReferenceData]
GO
ALTER TABLE [FMO].[RouteStatus]  WITH CHECK ADD  CONSTRAINT [FK_RouteStatus_Route] FOREIGN KEY([RouteID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[RouteStatus] CHECK CONSTRAINT [FK_RouteStatus_Route]
GO
ALTER TABLE [FMO].[Scenario]  WITH CHECK ADD  CONSTRAINT [FK_Scenario_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[Scenario] CHECK CONSTRAINT [FK_Scenario_Location]
GO
ALTER TABLE [FMO].[ScenarioDayOfTheWeek]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioDayOfTheWeek_ReferenceData] FOREIGN KEY([DayOfTheWeekGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[ScenarioDayOfTheWeek] CHECK CONSTRAINT [FK_ScenarioDayOfTheWeek_ReferenceData]
GO
ALTER TABLE [FMO].[ScenarioDayOfTheWeek]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioDayOfTheWeek_Scenario] FOREIGN KEY([ScenarioID])
REFERENCES [FMO].[Scenario] ([ID])
GO
ALTER TABLE [FMO].[ScenarioDayOfTheWeek] CHECK CONSTRAINT [FK_ScenarioDayOfTheWeek_Scenario]
GO
ALTER TABLE [FMO].[ScenarioRoute]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioRoute_Route] FOREIGN KEY([RouteID])
REFERENCES [FMO].[Route] ([ID])
GO
ALTER TABLE [FMO].[ScenarioRoute] CHECK CONSTRAINT [FK_ScenarioRoute_Route]
GO
ALTER TABLE [FMO].[ScenarioRoute]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioRoute_Scenario] FOREIGN KEY([ScenarioID])
REFERENCES [FMO].[Scenario] ([ID])
GO
ALTER TABLE [FMO].[ScenarioRoute] CHECK CONSTRAINT [FK_ScenarioRoute_Scenario]
GO
ALTER TABLE [FMO].[ScenarioStatus]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioStatus_ReferenceData] FOREIGN KEY([ScenarioStatusGUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[ScenarioStatus] CHECK CONSTRAINT [FK_ScenarioStatus_ReferenceData]
GO
ALTER TABLE [FMO].[ScenarioStatus]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioStatus_Scenario] FOREIGN KEY([ScenarioID])
REFERENCES [FMO].[Scenario] ([ID])
GO
ALTER TABLE [FMO].[ScenarioStatus] CHECK CONSTRAINT [FK_ScenarioStatus_Scenario]
GO
ALTER TABLE [FMO].[SpecialInstruction]  WITH CHECK ADD  CONSTRAINT [FK_SpecialInstruction_ReferenceData] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[SpecialInstruction] CHECK CONSTRAINT [FK_SpecialInstruction_ReferenceData]
GO
ALTER TABLE [FMO].[SpecialInstruction]  WITH CHECK ADD  CONSTRAINT [FK_SpecialInstruction_ReferenceData_02] FOREIGN KEY([InstructionType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[SpecialInstruction] CHECK CONSTRAINT [FK_SpecialInstruction_ReferenceData_02]
GO
ALTER TABLE [FMO].[SupportingDeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_SupportingDeliveryPoint_DeliveryPoint] FOREIGN KEY([ID])
REFERENCES [FMO].[DeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[SupportingDeliveryPoint] CHECK CONSTRAINT [FK_SupportingDeliveryPoint_DeliveryPoint]
GO
ALTER TABLE [FMO].[UserRoleLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleLocation_Location] FOREIGN KEY([LocationID])
REFERENCES [FMO].[Location] ([ID])
GO
ALTER TABLE [FMO].[UserRoleLocation] CHECK CONSTRAINT [FK_UserRoleLocation_Location]
GO
ALTER TABLE [FMO].[UserRoleLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleLocation_Role] FOREIGN KEY([RoleID])
REFERENCES [FMO].[Role] ([ID])
GO
ALTER TABLE [FMO].[UserRoleLocation] CHECK CONSTRAINT [FK_UserRoleLocation_Role]
GO
ALTER TABLE [FMO].[UserRoleLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleLocation_User] FOREIGN KEY([UserID])
REFERENCES [FMO].[User] ([ID])
GO
ALTER TABLE [FMO].[UserRoleLocation] CHECK CONSTRAINT [FK_UserRoleLocation_User]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_AddressLocation_LocationXY]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_AddressLocation_LocationXY] ON [FMO].[AddressLocation]
(
	[LocationXY]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_Location_Shape]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_Location_Shape] ON [FMO].[Location]
(
	[Shape]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_NetworkLink_LinkGeometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_NetworkLink_LinkGeometry] ON [FMO].[NetworkLink]
(
	[LinkGeometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_NetworkReference_NodeReferenceLocation]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_NetworkReference_NodeReferenceLocation] ON [FMO].[NetworkReference]
(
	[NodeReferenceLocation]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_NetworkReference_PointReferenceLocation]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_NetworkReference_PointReferenceLocation] ON [FMO].[NetworkReference]
(
	[PointReferenceLocation]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_OSConnectingLink_Geometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSConnectingLink_Geometry] ON [FMO].[OSConnectingLink]
(
	[Geometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_OSConnectingNode_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSConnectingNode_Location] ON [FMO].[OSConnectingNode]
(
	[Location]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_OSPathLink_CentreLineGeometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSPathLink_CentreLineGeometry] ON [FMO].[OSPathLink]
(
	[CentreLineGeometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_OSPathNode_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSPathNode_Location] ON [FMO].[OSPathNode]
(
	[Location]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_OSRoadLink_CentreLineGeometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_OSRoadLink_CentreLineGeometry] ON [FMO].[OSRoadLink]
(
	[CentreLineGeometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_OSRoadNode_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_OSRoadNode_Location] ON [FMO].[OSRoadNode]
(
	[Location]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_RMGLink_Geometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_RMGLink_Geometry] ON [FMO].[RMGLink]
(
	[Geometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_RMGNode_Location]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_RMGNode_Location] ON [FMO].[RMGNode]
(
	[Location]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_StreetName_Geometry]    Script Date: 7/18/2017 2:24:56 PM ******/
CREATE SPATIAL INDEX [IDX_StreetName_Geometry] ON [FMO].[StreetName]
(
	[Geometry]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [FMO].[PostalAddressAlias]
(
	[ID] uniqueidentifier NOT NULL DEFAULT newsequentialid(),
	[PostalAddressID] uniqueidentifier NOT NULL,
	[AliasTypeGUID] uniqueidentifier NOT NULL,
	[PostalAddressIdentifierID] uniqueidentifier NULL,
	[AliasName] varchar(100) NULL,
	[PreferenceOrderIndex] numeric(16,8) NULL,
	[StartDateTime] datetime2(7) NULL,
	[EndDateTime] datetime2(7) NULL,
	[RowCreateDateTime] datetime2(7) NOT NULL DEFAULT sysdatetime()
)
GO

ALTER TABLE [FMO].[PostalAddressAlias] 
 ADD CONSTRAINT [PK_PostalAddressAlias]
	PRIMARY KEY CLUSTERED ([ID] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_PostalAddressAlias_PostalAddress] 
 ON [FMO].[PostalAddressAlias] ([PostalAddressID] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_PostalAddressAlias_PostalAddressIdentifier] 
 ON [FMO].[PostalAddressAlias] ([PostalAddressIdentifierID] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_PostalAddressAlias_ReferenceData] 
 ON [FMO].[PostalAddressAlias] ([AliasTypeGUID] ASC)
GO

ALTER TABLE [FMO].[PostalAddressAlias] ADD CONSTRAINT [FK_PostalAddressAlias_PostalAddress]
	FOREIGN KEY ([PostalAddressID]) REFERENCES [FMO].[PostalAddress] ([ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [FMO].[PostalAddressAlias] ADD CONSTRAINT [FK_PostalAddressAlias_PostalAddressIdentifier]
	FOREIGN KEY ([PostalAddressIdentifierID]) REFERENCES [FMO].[PostalAddressIdentifier] ([ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [FMO].[PostalAddressAlias] ADD CONSTRAINT [FK_PostalAddressAlias_ReferenceData]
	FOREIGN KEY ([AliasTypeGUID]) REFERENCES [FMO].[ReferenceData] ([ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [FMO].[NetworkLink]
ADD AlternateID INT IDENTITY(1,1)
GO

ALTER TABLE [FMO].[Block]
DROP COLUMN [GeoRouteBlockID]
ALTER TABLE [FMO].[Block]
ADD [GeoRouteBlockID] int IDENTITY(1,1)
GO

ALTER TABLE [FMO].[Route]
DROP COLUMN [GeoRouteID]
ALTER TABLE [FMO].[Route]
ADD [GeoRouteID] int IDENTITY(1,1)
GO

ALTER TABLE [FMO].[PostalAddressIdentifier]
DROP COLUMN [ExternalID]
ALTER TABLE [FMO].[PostalAddressIdentifier]
ADD [ExternalID] int IDENTITY(1,1)
GO