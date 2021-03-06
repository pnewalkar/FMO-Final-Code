USE [FMO_DEV01_007]
GO
/****** Object:  Schema [FMO]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SCHEMA [FMO]
GO
/****** Object:  UserDefinedFunction [FMO].[InitCap]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [FMO].[InitCap] ( @InputString varchar(4000) ) 
RETURNS VARCHAR(4000)
AS
BEGIN

DECLARE @Index          INT
DECLARE @Char           CHAR(1)
DECLARE @PrevChar       CHAR(1)
DECLARE @OutputString   VARCHAR(255)

SET @OutputString = LOWER(@InputString)
SET @Index = 1

WHILE @Index <= LEN(@InputString)
BEGIN
    SET @Char     = SUBSTRING(@InputString, @Index, 1)
    SET @PrevChar = CASE WHEN @Index = 1 THEN ' '
                         ELSE SUBSTRING(@InputString, @Index - 1, 1)
                    END

    IF @PrevChar IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&', '''', '(')
    BEGIN
        IF @PrevChar != '''' OR UPPER(@Char) != 'S'
            SET @OutputString = STUFF(@OutputString, @Index, 1, UPPER(@Char))
    END

    SET @Index = @Index + 1
END

RETURN @OutputString

END

GO
/****** Object:  Table [FMO].[AccessLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AccessLink](
	[OperationalObjectPoint] [geometry] NOT NULL,
	[NetworkIntersectionPoint] [geometry] NOT NULL,
	[AccessLinkLine] [geometry] NOT NULL,
	[Approved] [bit] NULL,
	[ActualLengthMeter] [numeric](18, 8) NOT NULL,
	[WorkloadLengthMeter] [numeric](18, 8) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AccessLink_ID]  DEFAULT (newsequentialid()),
	[LinkStatus_GUID] [uniqueidentifier] NULL,
	[AccessLinkType_GUID] [uniqueidentifier] NULL,
	[LinkDirection_GUID] [uniqueidentifier] NULL,
	[OperationalObject_GUID] [uniqueidentifier] NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NULL,
	[NetworkLink_GUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_AccessLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Action]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Action](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Action_ID]  DEFAULT (newsequentialid()),
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
/****** Object:  Table [FMO].[AddressLocation]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AddressLocation](
	[UDPRN] [int] NOT NULL,
	[LocationXY] [geometry] NOT NULL,
	[Lattitude] [numeric](38, 8) NOT NULL,
	[Longitude] [numeric](38, 8) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AddressLocat_ID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_AddressLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[AMUChangeRequest]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[AMUChangeRequest](
	[UnitCommentText] [varchar](300) NULL,
	[AddressChanges] [varchar](300) NULL,
	[RaisedDate] [datetime2](7) NULL,
	[ClosedDate] [datetime2](7) NULL,
	[AMUClarificationText] [varchar](300) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[ChangeRequestType_GUID] [uniqueidentifier] NULL,
	[ChangeRequestStatus_GUID] [uniqueidentifier] NULL,
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
/****** Object:  Table [FMO].[AreaHazard]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[AreaHazard](
	[ValidUntilDate] [date] NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[Category_GUID] [uniqueidentifier] NOT NULL,
	[SubCategory_GUID] [uniqueidentifier] NOT NULL,
	[Polygon_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AreaHazard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Block]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Block](
	[BlockType] [char](1) NULL,
	[BlockSpanInMinutes] [numeric](10, 2) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Block_ID]  DEFAULT (newsequentialid()),
	[PairedBlock_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[BlockSequence]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[BlockSequence](
	[OrderIndex] [numeric](16, 8) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_BlockSequenc_ID]  DEFAULT (newsequentialid()),
	[Block_GUID] [uniqueidentifier] NOT NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NULL,
	[DeliveryGroup_GUID] [uniqueidentifier] NULL,
	[OperationalObject_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_BlockActivitySequence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[CollectionRoute]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[CollectionRoute](
	[RouteName] [char](30) NULL,
	[RouteNumber] [char](10) NULL,
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionRoute] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DBVersion]    Script Date: 7/20/2017 7:25:45 AM ******/
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
 CONSTRAINT [PK_DBVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DeliveryGroup]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[DeliveryGroup](
	[Name] [varchar](50) NOT NULL,
	[NumberOfFloors] [tinyint] NULL,
	[InternalDistanceMeters] [float] NULL,
	[WorkloadOverrideTimeMinutes] [float] NULL,
	[WorkloadOverrideReason] [varchar](300) NULL,
	[OverrideApproved] [bit] NULL,
	[ServicePoint] [bit] NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[GroupType_GUID] [uniqueidentifier] NOT NULL,
	[ServicePointType_GUID] [uniqueidentifier] NULL,
	[Polygon_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DeliveryGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DeliveryPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryPoint](
	[LocationXY] [geometry] NULL,
	[Latitude] [numeric](38, 8) NULL,
	[Longitude] [numeric](38, 8) NULL,
	[Positioned] [bit] NOT NULL,
	[AccessLinkPresent] [bit] NOT NULL,
	[RMGDeliveryPointPresent] [bit] NOT NULL,
	[UDPRN] [int] NULL,
	[MultipleOccupancyCount] [smallint] NULL,
	[MailVolume] [int] NULL,
	[IsUnit] [bit] NOT NULL,
	[Temp_DeliveryGroup_Id] [int] NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_DeliveryPoin_ID]  DEFAULT (newsequentialid()),
	[Address_GUID] [uniqueidentifier] NOT NULL,
	[LocationProvider_GUID] [uniqueidentifier] NULL,
	[OperationalStatus_GUID] [uniqueidentifier] NULL,
	[DeliveryGroup_GUID] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[DeliveryPointUseIndicator_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DeliveryPoint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_DeliveryPoint_Address_GUID] UNIQUE NONCLUSTERED 
(
	[Address_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryPoint_BK]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryPoint_BK](
	[ID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryPointAlias]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[DeliveryPointAlias](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_DeliveryPointAlias_ID]  DEFAULT (newsequentialid()),
	[DeliveryPoint_GUID] [uniqueidentifier] NOT NULL,
	[DPAlias] [varchar](60) NOT NULL,
	[Preferred] [bit] NOT NULL CONSTRAINT [DF__DeliveryPointAlias_Preferred]  DEFAULT ((0)),
 CONSTRAINT [PK_DeliveryPointAlias] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DeliveryRoute]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[DeliveryRoute](
	[ExternalId] [int] NULL,
	[RouteName] [char](30) NULL,
	[RouteNumber] [char](10) NULL,
	[TravelOutTimeMin] [numeric](10, 2) NULL,
	[TravelInTimeMin] [numeric](10, 2) NULL,
	[SpanTimeMin] [numeric](10, 2) NULL,
	[DeliveryRouteBarcode] [char](20) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_DeliveryRout_ID]  DEFAULT (newsequentialid()),
	[OperationalStatus_GUID] [uniqueidentifier] NOT NULL,
	[RouteMethodType_GUID] [uniqueidentifier] NOT NULL,
	[TravelOutTransportType_GUID] [uniqueidentifier] NULL,
	[TravelInTransportType_GUID] [uniqueidentifier] NULL,
	[DeliveryScenario_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Route] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[DeliveryRouteActivity]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryRouteActivity](
	[RouteActivityOrderIndex] [numeric](8, 8) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[DeliveryRoute_GUID] [uniqueidentifier] NULL,
	[ActivityType_GUID] [uniqueidentifier] NULL,
	[Block_GUID] [uniqueidentifier] NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NULL,
	[DeliveryGroup_GUID] [uniqueidentifier] NULL,
	[OperationalObject_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RouteActivitySequence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryRouteBlock]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryRouteBlock](
	[ID] [uniqueidentifier] NOT NULL,
	[DeliveryRoute_GUID] [uniqueidentifier] NOT NULL,
	[Block_GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_DeliveryRouteBlock_Block_GUID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_DeliveryRouteBlock_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryRouteNetworkLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryRouteNetworkLink](
	[LinkOrderIndex] [numeric](8, 8) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[DeliveryRouteActivity_GUID] [uniqueidentifier] NULL,
	[NetworkLink_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_DeliveryRouteNetworkLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[DeliveryRoutePostcode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[DeliveryRoutePostcode](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_DeliveryRoutePostcode_ID]  DEFAULT (newsequentialid()),
	[DeliveryRoute_GUID] [uniqueidentifier] NOT NULL,
	[Postcode_GUID] [uniqueidentifier] NOT NULL,
	[IsPrimaryRoute] [bit] NOT NULL CONSTRAINT [DF_DeliveryRoutePostcode_PrimaryRoute]  DEFAULT ((0)),
 CONSTRAINT [PK_DeliveryRoutePostcode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Function]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Function](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Function_ID]  DEFAULT (newsequentialid()),
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[Action_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[GroupHazard]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[GroupHazard](
	[Description] [varchar](300) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[DeliveryGroup_GUID] [uniqueidentifier] NOT NULL,
	[Category_GUID] [uniqueidentifier] NOT NULL,
	[SubCategory_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GroupHazard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkLink](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_NetworkLink_Id]  DEFAULT (newsequentialid()),
	[TOID] [char](20) NULL,
	[LinkGeometry] [geometry] NOT NULL,
	[LinkLength] [numeric](18, 4) NOT NULL,
	[LinkGradientType] [int] NULL,
	[NetworkLinkType_GUID] [uniqueidentifier] NULL,
	[DataProvider_GUID] [uniqueidentifier] NULL,
	[RoadName_GUID] [uniqueidentifier] NULL,
	[StreetName_GUID] [uniqueidentifier] NULL,
	[StartNode_GUID] [uniqueidentifier] NULL,
	[EndNode_GUID] [uniqueidentifier] NULL,
	[LinkName] [varchar](255) NULL,
 CONSTRAINT [PK_NetworkLink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkLinkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkLinkReference](
	[OSRoadLinkTOID] [char](20) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[OSRoadLink_GUID] [uniqueidentifier] NULL,
	[NetworkReference_GUID] [uniqueidentifier] NOT NULL,
	[NetworkLink_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_NetworkLinkReference] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkNode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkNode](
	[DataProvider] [char](1) NULL,
	[NodeGeometry] [geometry] NOT NULL,
	[TOID] [char](20) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_NetworkNode_ID]  DEFAULT (newsequentialid()),
	[NetworkNodeType_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_NetworkNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[NetworkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[NetworkReference](
	[ReferenceType] [char](10) NULL,
	[NodeReferenceTOID] [char](20) NULL,
	[NodeReferenceLocation] [geometry] NULL,
	[PointReferenceLocation] [geometry] NULL,
	[PointReferenceRoadLinkTOID] [char](20) NULL,
	[ExternalNetworkRef] [varchar](50) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[NetworkNode_GUID] [uniqueidentifier] NULL,
	[PointReferenceNetworkLink_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_NetworkReference] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Notification]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Notification](
	[Notification Heading] [varchar](50) NULL,
	[Notification Message] [varchar](300) NULL,
	[NotificationDueDate] [datetime2](7) NULL,
	[NotificationActionLink] [nvarchar](2000) NULL,
	[NotificationSource] [varchar](50) NULL,
	[PostcodeDistrict] [char](4) NULL,
	[PostcodeSector] [char](6) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[NotificationType_GUID] [uniqueidentifier] NULL,
	[NotificationPriority_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSAccessRestriction]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSAccessRestriction](
	[TOID] [char](20) NOT NULL,
	[RestrictionValue] [varchar](21) NULL,
	[InclusionVehicleQualifier] [xml] NULL,
	[ExclusionVehicleQualifier] [xml] NULL,
	[TimeInterval] [xml] NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[NetworkReference_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OSAccessRestriction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSConnectingLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSConnectingLink](
	[TOID] [char](20) NOT NULL,
	[Fictitious] [bit] NULL,
	[ConnectingNodeTOID] [char](20) NOT NULL,
	[PathNodeTOID] [char](20) NOT NULL,
	[Geometry] [geometry] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OSConnecting_ID]  DEFAULT (newsequentialid()),
	[ConnectingNode_GUID] [uniqueidentifier] NOT NULL,
	[PathNode_GUID] [uniqueidentifier] NULL,
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
/****** Object:  Table [FMO].[OSConnectingNode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSConnectingNode](
	[TOID] [char](20) NOT NULL,
	[Location] [geometry] NOT NULL,
	[RoadLinkTOID] [char](20) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[RoadLink_GUID] [uniqueidentifier] NULL,
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
/****** Object:  Table [FMO].[OSPathLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSPathLink](
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
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OSPathLink_ID]  DEFAULT (newsequentialid()),
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
/****** Object:  Table [FMO].[OSPathNode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSPathNode](
	[TOID] [char](20) NOT NULL,
	[Location] [geometry] NULL,
	[formOfRoadNode] [varchar](21) NULL,
	[Classification] [varchar](19) NULL,
	[ReasonForChange] [varchar](32) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OSPathNode_ID]  DEFAULT (newsequentialid()),
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
/****** Object:  Table [FMO].[OSRestrictionForVehicles]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRestrictionForVehicles](
	[TOID] [char](20) NOT NULL,
	[MeasureInMeters] [float] NULL,
	[RestrictionType] [xml] NULL,
	[SourceofMeasure] [char](10) NULL,
	[Inclusion] [xml] NULL,
	[Exclusion] [xml] NULL,
	[Structure] [varchar](50) NULL,
	[TrafficSign] [varchar](120) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[NetworkReference_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RestrictionForVehicles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[OSRoadLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRoadLink](
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
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OSRoadLink_ID]  DEFAULT (newsequentialid()),
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
/****** Object:  Table [FMO].[OSRoadNode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSRoadNode](
	[TOID] [char](20) NOT NULL,
	[validFrom] [datetime] NULL,
	[Location] [geometry] NULL,
	[formOfRoadNode] [varchar](21) NULL,
	[Classification] [varchar](19) NULL,
	[access] [char](5) NULL,
	[junctionName] [varchar](120) NULL,
	[JunctionNumber] [varchar](30) NULL,
	[ReasonForChange] [varchar](32) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OSRoadNode_ID]  DEFAULT (newsequentialid()),
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
/****** Object:  Table [FMO].[OSTurnRestriction]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[OSTurnRestriction](
	[TOID] [char](20) NOT NULL,
	[Restriction] [varchar](34) NULL,
	[inclusion] [varchar](50) NULL,
	[Exclusion] [varchar](50) NULL,
	[TimeInterval] [varchar](50) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[NetworkReference_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TurnRestriction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[POBox]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[POBox](
	[UDPRN ] [int] NOT NULL,
	[POBoxNumber] [bigint] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[POBoxType_GUID] [uniqueidentifier] NULL,
	[POBoxLinkedAddress_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_POBox_Id] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_POBox_UDPRN] UNIQUE NONCLUSTERED 
(
	[UDPRN ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[PointHazard]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PointHazard](
	[Description] [varchar](300) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[Category_GUID] [uniqueidentifier] NOT NULL,
	[SubCategory_GUID] [uniqueidentifier] NOT NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NOT NULL,
	[OperationalObject_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PointHazard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Polygon]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[Polygon](
	[PolygonGeometry] [geometry] NOT NULL,
	[PolygonCentroid] [geometry] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[PolygonType_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Polygon] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[PolygonObject]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[PolygonObject](
	[ObjectExcluded] [bit] NOT NULL,
	[GroupOrderIndex] [numeric](8, 8) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[OperationalObject_GUID] [uniqueidentifier] NOT NULL,
	[OperationalObjectType_GUID] [uniqueidentifier] NOT NULL,
	[Polygon_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PolygonObject] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[PostalAddress]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostalAddress](
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
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PostalAddres_ID]  DEFAULT (newsequentialid()),
	[PostCodeGUID] [uniqueidentifier] NOT NULL,
	[AddressType_GUID] [uniqueidentifier] NOT NULL,
	[AddressStatus_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Postcode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Postcode](
	[PostcodeUnit] [char](8) NOT NULL,
	[OutwardCode] [char](4) NOT NULL,
	[InwardCode] [char](3) NOT NULL,
	[Sector] [char](5) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Postcode_ID]  DEFAULT (newsequentialid()),
	[SectorGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Postcode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostcodeArea]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostcodeArea](
	[Area] [char](2) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PostcodeArea_ID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_PostcodeArea] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostcodeDistrict]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostcodeDistrict](
	[District] [char](4) NOT NULL,
	[Area] [char](2) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PostcodeDist_ID]  DEFAULT (newsequentialid()),
	[AreaGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PostcodeDistrict] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[PostcodeSector]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[PostcodeSector](
	[Sector] [char](5) NOT NULL,
	[District] [char](4) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PostcodeSect_ID]  DEFAULT (newsequentialid()),
	[DistrictGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PostcodeSector] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[ReferenceData](
	[ReferenceDataName] [varchar](100) NULL,
	[ReferenceDataValue] [varchar](100) NULL,
	[DataDescription] [varchar](300) NULL,
	[DisplayText] [varchar](100) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ReferenceDat_ID]  DEFAULT (newsequentialid()),
	[ReferenceDataCategory_GUID] [uniqueidentifier] NOT NULL,
	[DataParent_GUID] [uniqueidentifier] NULL,
	[OrderingIndex] [int] NULL,
	[Default] [bit] NULL CONSTRAINT [DF_ReferenceData_Default]  DEFAULT ((0)),
 CONSTRAINT [PK_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[ReferenceDataCategory]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[ReferenceDataCategory](
	[CategoryName] [varchar](50) NOT NULL,
	[Maintainable] [bit] NOT NULL,
	[CategoryType] [int] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ReferenceDataCategory_ID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_ReferenceDataCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RMGDeliveryPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RMGDeliveryPoint](
	[LocationXY] [geometry] NULL,
	[Latitude] [numeric](38, 8) NULL,
	[Longitude] [numeric](38, 8) NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[DeliveryPoint_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RMGDeliveryPoint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [FMO].[RMGLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RMGLink](
	[LinkType] [char](1) NULL,
	[StartNodeReference] [char](20) NULL,
	[StartNodeType] [char](2) NULL,
	[EndNodeType] [char](2) NULL,
	[EndNodeReference] [char](20) NULL,
	[Geometry] [geometry] NULL,
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RMGLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RMGNode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RMGNode](
	[Location] [geometry] NULL,
	[FormofNode] [bigint] NULL,
	[NodeOnLink] [bit] NULL,
	[OSLinkReference] [char](20) NULL,
	[OSLinkType] [char](1) NULL,
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RMGNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RoadName]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[RoadName](
	[TOID] [char](20) NULL,
	[NationalRoadCode] [char](10) NULL,
	[roadClassification] [char](21) NULL,
	[DesignatedName] [varchar](255) NULL,
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Road] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[Role]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Role](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Role_ID]  DEFAULT (newsequentialid()),
	[RoleName] [varchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[RoleFunction]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[RoleFunction](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_RoleFunction_ID]  DEFAULT (newsequentialid()),
	[Role_GUID] [uniqueidentifier] NOT NULL,
	[Function_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RoleFunction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_RoleIDFunctionID] UNIQUE NONCLUSTERED 
(
	[Role_GUID] ASC,
	[Function_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [FMO].[Scenario]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[Scenario](
	[ScenarioName] [varchar](50) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Scenario_ID]  DEFAULT (newsequentialid()),
	[OperationalState_GUID] [uniqueidentifier] NULL,
	[Unit_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Scenario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[SpecialInstruction]    Script Date: 7/20/2017 7:25:45 AM ******/
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
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
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
/****** Object:  Table [FMO].[StreetName]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[StreetName](
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
	[OperationalStateStartTime] [datetime2](7) NULL,
	[OperationalStateEndTime] [datetime2](7) NULL,
	[Locality] [varchar](35) NULL,
	[Town] [varchar](30) NULL,
	[AdministrativeArea] [varchar](30) NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StreetName_ID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_OSStreet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[StreetNameNetworkLink]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[StreetNameNetworkLink](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StreetNameNetworkLink_ID]  DEFAULT (newsequentialid()),
	[StreetName_GUID] [uniqueidentifier] NULL,
	[NetworkLink_GUID] [uniqueidentifier] NULL,
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
/****** Object:  Table [FMO].[UnitLocation]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[UnitLocation](
	[ExternalId] [varchar](50) NOT NULL,
	[UnitName] [varchar](50) NULL,
	[UnitAddressUDPRN] [int] NOT NULL,
	[UnitBoundryPolygon] [geometry] NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Unit_ID]  DEFAULT (newsequentialid()),
	[LocationType_GUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UnitLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[UnitLocationPostcode]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [FMO].[UnitLocationPostcode](
	[PostcodeUnit] [char](8) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UnitLocationPostcode_ID]  DEFAULT (newsequentialid()),
	[Unit_GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UnitLocationPostcode_Unit_GUID]  DEFAULT (newsequentialid()),
	[PoscodeUnit_GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UnitLocationPostcode_PoscodeUnit_GUID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_UnitLocationPostcode_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[UnitPostcodeSector]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[UnitPostcodeSector](
	[PostcodeSector] [char](5) NOT NULL,
	[PostcodeSector_GUID] [uniqueidentifier] NOT NULL,
	[Unit_GUID] [uniqueidentifier] NOT NULL,
	[UnitStatus_GUID] [uniqueidentifier] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UnitPostcodeSector_ID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_UnitPostcodeSector] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[User]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [FMO].[User](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_User_ID]  DEFAULT (newsequentialid()),
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[UserName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [FMO].[UserRoleUnit]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FMO].[UserRoleUnit](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserRoleUnit_ID]  DEFAULT (newsequentialid()),
	[User_GUID] [uniqueidentifier] NOT NULL,
	[Role_GUID] [uniqueidentifier] NOT NULL,
	[Unit_GUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserRoleUnit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_UserRoleUnit] UNIQUE NONCLUSTERED 
(
	[User_GUID] ASC,
	[Role_GUID] ASC,
	[Unit_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [FMO].[Vw_GetAccessFunction]    Script Date: 7/20/2017 7:25:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [FMO].[Vw_GetAccessFunction] 
	as
 Select r.RoleName, uru.Unit_GUID, u.UserName, f.Name as FunctionName, a.DisplayText as ActionName, u.ID as UserId
  from [FMO].[Function] f 
inner join [FMO].[RoleFunction] rf on f.ID = rf.Function_GUID
inner join [FMO].[Action] a on a.ID = f.Action_GUID
inner join [FMO].[UserRoleUnit] uru on uru.Role_GUID = rf.Role_GUID
inner join [FMO].[User] u on u.ID = uru.User_GUID
inner join [FMO].[Role] r on r.ID = uru.Role_GUID


GO
/****** Object:  Index [IXFK_AccessLink_DeliveryPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLink_DeliveryPoint] ON [FMO].[AccessLink]
(
	[OperationalObject_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AccessLink_NetworkLink]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AccessLink_NetworkLink] ON [FMO].[AccessLink]
(
	[NetworkLink_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_PostalAddress]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_PostalAddress] ON [FMO].[AMUChangeRequest]
(
	[CurrentAddress_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_PostalAddress_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_PostalAddress_02] ON [FMO].[AMUChangeRequest]
(
	[NewAddress_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_Postcode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_Postcode] ON [FMO].[AMUChangeRequest]
(
	[RequestPostcode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData] ON [FMO].[AMUChangeRequest]
(
	[ChangeRequestType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData_02] ON [FMO].[AMUChangeRequest]
(
	[ChangeRequestStatus_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AMUChangeRequest_ReferenceData_03]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AMUChangeRequest_ReferenceData_03] ON [FMO].[AMUChangeRequest]
(
	[ChangeReasonCode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AreaHazard_Polygon]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AreaHazard_Polygon] ON [FMO].[AreaHazard]
(
	[Polygon_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AreaHazard_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AreaHazard_ReferenceData] ON [FMO].[AreaHazard]
(
	[Category_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_AreaHazard_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_AreaHazard_ReferenceData_02] ON [FMO].[AreaHazard]
(
	[SubCategory_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryGroup_Polygon]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryGroup_Polygon] ON [FMO].[DeliveryGroup]
(
	[Polygon_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryGroup_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryGroup_ReferenceData] ON [FMO].[DeliveryGroup]
(
	[GroupType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPoint_Address]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPoint_Address] ON [FMO].[DeliveryPoint]
(
	[Address_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryPoint_DeliveryGroup]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryPoint_DeliveryGroup] ON [FMO].[DeliveryPoint]
(
	[DeliveryGroup_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Route_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Route_ReferenceData] ON [FMO].[DeliveryRoute]
(
	[RouteMethodType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Route_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Route_ReferenceData_02] ON [FMO].[DeliveryRoute]
(
	[TravelOutTransportType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Route_ReferenceData_03]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Route_ReferenceData_03] ON [FMO].[DeliveryRoute]
(
	[TravelInTransportType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivitySequence_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivitySequence_ReferenceData] ON [FMO].[DeliveryRouteActivity]
(
	[ActivityType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RouteActivitySequence_Route]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_RouteActivitySequence_Route] ON [FMO].[DeliveryRouteActivity]
(
	[DeliveryRoute_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryRouteBlock_Block]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryRouteBlock_Block] ON [FMO].[DeliveryRouteBlock]
(
	[Block_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_DeliveryRouteBlock_Route]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_DeliveryRouteBlock_Route] ON [FMO].[DeliveryRouteBlock]
(
	[DeliveryRoute_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_GroupHazard_DeliveryGroup]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_GroupHazard_DeliveryGroup] ON [FMO].[GroupHazard]
(
	[DeliveryGroup_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_GroupHazard_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_GroupHazard_ReferenceData] ON [FMO].[GroupHazard]
(
	[Category_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_GroupHazard_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_GroupHazard_ReferenceData_02] ON [FMO].[GroupHazard]
(
	[SubCategory_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_NetworkNode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_NetworkNode] ON [FMO].[NetworkLink]
(
	[StartNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_NetworkNode_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_NetworkNode_02] ON [FMO].[NetworkLink]
(
	[EndNode_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_RoadName]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_RoadName] ON [FMO].[NetworkLink]
(
	[RoadName_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkLink_StreetName]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkLink_StreetName] ON [FMO].[NetworkLink]
(
	[StreetName_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReferenceLink_NetworkLink]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReferenceLink_NetworkLink] ON [FMO].[NetworkLinkReference]
(
	[NetworkLink_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_NetworkReferenceLink_NetworkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_NetworkReferenceLink_NetworkReference] ON [FMO].[NetworkLinkReference]
(
	[NetworkReference_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Access Restriction_NetworkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Access Restriction_NetworkReference] ON [FMO].[OSAccessRestriction]
(
	[NetworkReference_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSConnectingLink_OSConnectingNode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSConnectingLink_OSConnectingNode] ON [FMO].[OSConnectingLink]
(
	[ConnectingNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode] ON [FMO].[OSPathLink]
(
	[StartNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSPathLink_OSPathNode_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSPathLink_OSPathNode_02] ON [FMO].[OSPathLink]
(
	[EndNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RestrictionForVehicles_NetworkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_RestrictionForVehicles_NetworkReference] ON [FMO].[OSRestrictionForVehicles]
(
	[NetworkReference_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode] ON [FMO].[OSRoadLink]
(
	[StartNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_OSRoadLink_OSRoadNode_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_OSRoadLink_OSRoadNode_02] ON [FMO].[OSRoadLink]
(
	[EndNodeTOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_TurnRestriction_NetworkReference]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_TurnRestriction_NetworkReference] ON [FMO].[OSTurnRestriction]
(
	[NetworkReference_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_POBox_Address]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_POBox_Address] ON [FMO].[POBox]
(
	[UDPRN ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PointHazard_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PointHazard_ReferenceData] ON [FMO].[PointHazard]
(
	[Category_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PointHazard_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PointHazard_ReferenceData_02] ON [FMO].[PointHazard]
(
	[SubCategory_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PointHazard_ReferenceData_03]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PointHazard_ReferenceData_03] ON [FMO].[PointHazard]
(
	[OperationalObjectType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Polygon_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Polygon_ReferenceData] ON [FMO].[Polygon]
(
	[PolygonType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PolygonObject_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PolygonObject_ReferenceData] ON [FMO].[PolygonObject]
(
	[OperationalObjectType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PolygonOperationalObject_Polygon]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PolygonOperationalObject_Polygon] ON [FMO].[PolygonObject]
(
	[Polygon_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Address_Postcode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Address_Postcode] ON [FMO].[PostalAddress]
(
	[PostCodeGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Address_UDPRN]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IXFK_Address_UDPRN] ON [FMO].[PostalAddress]
(
	[UDPRN] ASC
)
WHERE ([UDPRN] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddress_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddress_ReferenceData] ON [FMO].[PostalAddress]
(
	[AddressStatus_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostalAddress_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostalAddress_ReferenceData_02] ON [FMO].[PostalAddress]
(
	[AddressType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_Postcode_PostcodeSector]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Postcode_PostcodeSector] ON [FMO].[Postcode]
(
	[Sector] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Postcode_PostcodeSector02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Postcode_PostcodeSector02] ON [FMO].[Postcode]
(
	[SectorGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_PostcodeDistrict_PostcodeArea]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostcodeDistrict_PostcodeArea] ON [FMO].[PostcodeDistrict]
(
	[Area] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostcodeDistrict_PostcodeArea02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostcodeDistrict_PostcodeArea02] ON [FMO].[PostcodeDistrict]
(
	[AreaGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_PostcodeSector_PostcodeDistrict]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostcodeSector_PostcodeDistrict] ON [FMO].[PostcodeSector]
(
	[District] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_PostcodeSector_PostcodeDistrict02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_PostcodeSector_PostcodeDistrict02] ON [FMO].[PostcodeSector]
(
	[DistrictGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ReferenceData_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_ReferenceData_ReferenceData] ON [FMO].[ReferenceData]
(
	[DataParent_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_ReferenceData_ReferenceDataCategory]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_ReferenceData_ReferenceDataCategory] ON [FMO].[ReferenceData]
(
	[ReferenceDataCategory_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_RMGDeliveryPoint_DeliveryPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_RMGDeliveryPoint_DeliveryPoint] ON [FMO].[RMGDeliveryPoint]
(
	[DeliveryPoint_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Scenario_Unit]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_Scenario_Unit] ON [FMO].[Scenario]
(
	[Unit_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_SpecialInstruction_ReferenceData]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_SpecialInstruction_ReferenceData] ON [FMO].[SpecialInstruction]
(
	[OperationalObjectType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_SpecialInstruction_ReferenceData_02]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_SpecialInstruction_ReferenceData_02] ON [FMO].[SpecialInstruction]
(
	[InstructionType_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UnitLocPostCode_Postcode]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_UnitLocPostCode_Postcode] ON [FMO].[UnitLocationPostcode]
(
	[PoscodeUnit_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UnitLocPostCode_Unit]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_UnitLocPostCode_Unit] ON [FMO].[UnitLocationPostcode]
(
	[Unit_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IXFK_UnitPostcodeSector_PostcodeSector]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_UnitPostcodeSector_PostcodeSector] ON [FMO].[UnitPostcodeSector]
(
	[PostcodeSector] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_UnitPostcodeSector_UnitLocation]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE NONCLUSTERED INDEX [IXFK_UnitPostcodeSector_UnitLocation] ON [FMO].[UnitPostcodeSector]
(
	[Unit_GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [FMO].[AMUChangeRequest] ADD  CONSTRAINT [DF_AMUChangeReq_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[AreaHazard] ADD  CONSTRAINT [DF_AreaHazard_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[CollectionRoute] ADD  CONSTRAINT [DF_CollectionRo_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[DeliveryGroup] ADD  CONSTRAINT [DF_DeliveryGrou_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[DeliveryRouteNetworkLink] ADD  CONSTRAINT [DF_RouteNetworkID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[GroupHazard] ADD  CONSTRAINT [DF_GroupHazard_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[NetworkLinkReference] ADD  CONSTRAINT [DF_NetworkLinkR_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[NetworkLinkReference] ADD  CONSTRAINT [DF_NetworkLinkReference_NetworkReference]  DEFAULT (newsequentialid()) FOR [NetworkReference_GUID]
GO
ALTER TABLE [FMO].[NetworkReference] ADD  CONSTRAINT [DF_NetworkRefer_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[Notification] ADD  CONSTRAINT [DF_Notification_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSAccessRestriction] ADD  CONSTRAINT [DF_OSAccessRest_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSRestrictionForVehicles] ADD  CONSTRAINT [DF_OSRestrictio_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[OSTurnRestriction] ADD  CONSTRAINT [DF_OSTurnRestri_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[POBox] ADD  CONSTRAINT [DF_POBox_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[PointHazard] ADD  CONSTRAINT [DF_PointHazard_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[Polygon] ADD  CONSTRAINT [DF_Polygon_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[PolygonObject] ADD  CONSTRAINT [DF_PolygonObjec_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[PolygonObject] ADD  CONSTRAINT [DF_PolygonObject_Polygon_GUID]  DEFAULT (newsequentialid()) FOR [Polygon_GUID]
GO
ALTER TABLE [FMO].[RMGDeliveryPoint] ADD  CONSTRAINT [DF_RMGDeliveryP_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RMGLink] ADD  CONSTRAINT [DF_RMGLink_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RMGNode] ADD  CONSTRAINT [DF_RMGNode_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[RoadName] ADD  CONSTRAINT [DF_RoadName_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[SpecialInstruction] ADD  CONSTRAINT [DF_SpecialInstr_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [FMO].[AccessLink]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessLink_DeliveryGroup] FOREIGN KEY([ID])
REFERENCES [FMO].[DeliveryGroup] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] NOCHECK CONSTRAINT [FK_AccessLink_DeliveryGroup]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_DeliveryPoint] FOREIGN KEY([OperationalObject_GUID])
REFERENCES [FMO].[DeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_DeliveryPoint]
GO
ALTER TABLE [FMO].[AccessLink]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessLink_NetworkLink] FOREIGN KEY([NetworkLink_GUID])
REFERENCES [FMO].[NetworkLink] ([Id])
GO
ALTER TABLE [FMO].[AccessLink] NOCHECK CONSTRAINT [FK_AccessLink_NetworkLink]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData] FOREIGN KEY([LinkStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData_02] FOREIGN KEY([AccessLinkType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData_02]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData_03] FOREIGN KEY([LinkDirection_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData_03]
GO
ALTER TABLE [FMO].[AccessLink]  WITH CHECK ADD  CONSTRAINT [FK_AccessLink_ReferenceData_04] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] CHECK CONSTRAINT [FK_AccessLink_ReferenceData_04]
GO
ALTER TABLE [FMO].[AccessLink]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessLink_RMGDeliveryPoint] FOREIGN KEY([ID])
REFERENCES [FMO].[RMGDeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[AccessLink] NOCHECK CONSTRAINT [FK_AccessLink_RMGDeliveryPoint]
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
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData] FOREIGN KEY([ChangeRequestType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData_02] FOREIGN KEY([ChangeRequestStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData_02]
GO
ALTER TABLE [FMO].[AMUChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_AMUChangeRequest_ReferenceData_03] FOREIGN KEY([ChangeReasonCode_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AMUChangeRequest] CHECK CONSTRAINT [FK_AMUChangeRequest_ReferenceData_03]
GO
ALTER TABLE [FMO].[AreaHazard]  WITH CHECK ADD  CONSTRAINT [FK_AreaHazard_Polygon] FOREIGN KEY([Polygon_GUID])
REFERENCES [FMO].[Polygon] ([ID])
GO
ALTER TABLE [FMO].[AreaHazard] CHECK CONSTRAINT [FK_AreaHazard_Polygon]
GO
ALTER TABLE [FMO].[AreaHazard]  WITH CHECK ADD  CONSTRAINT [FK_AreaHazard_ReferenceData] FOREIGN KEY([Category_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AreaHazard] CHECK CONSTRAINT [FK_AreaHazard_ReferenceData]
GO
ALTER TABLE [FMO].[AreaHazard]  WITH CHECK ADD  CONSTRAINT [FK_AreaHazard_ReferenceData_02] FOREIGN KEY([SubCategory_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[AreaHazard] CHECK CONSTRAINT [FK_AreaHazard_ReferenceData_02]
GO
ALTER TABLE [FMO].[Block]  WITH CHECK ADD  CONSTRAINT [FK_Block_Block] FOREIGN KEY([PairedBlock_GUID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[Block] CHECK CONSTRAINT [FK_Block_Block]
GO
ALTER TABLE [FMO].[BlockSequence]  WITH CHECK ADD  CONSTRAINT [FK_BlockSequence_Block] FOREIGN KEY([Block_GUID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[BlockSequence] CHECK CONSTRAINT [FK_BlockSequence_Block]
GO
ALTER TABLE [FMO].[BlockSequence]  WITH CHECK ADD  CONSTRAINT [FK_BlockSequence_DeliveryGroup] FOREIGN KEY([DeliveryGroup_GUID])
REFERENCES [FMO].[DeliveryGroup] ([ID])
GO
ALTER TABLE [FMO].[BlockSequence] CHECK CONSTRAINT [FK_BlockSequence_DeliveryGroup]
GO
ALTER TABLE [FMO].[BlockSequence]  WITH CHECK ADD  CONSTRAINT [FK_BlockSequence_ReferenceData] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[BlockSequence] CHECK CONSTRAINT [FK_BlockSequence_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryGroup]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryGroup_Polygon] FOREIGN KEY([Polygon_GUID])
REFERENCES [FMO].[Polygon] ([ID])
GO
ALTER TABLE [FMO].[DeliveryGroup] CHECK CONSTRAINT [FK_DeliveryGroup_Polygon]
GO
ALTER TABLE [FMO].[DeliveryGroup]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryGroup_ReferenceData] FOREIGN KEY([GroupType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryGroup] CHECK CONSTRAINT [FK_DeliveryGroup_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryGroup]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryGroup_ReferenceData_02] FOREIGN KEY([ServicePointType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryGroup] CHECK CONSTRAINT [FK_DeliveryGroup_ReferenceData_02]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_Address_02] FOREIGN KEY([Address_GUID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_Address_02]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_DeliveryGroup] FOREIGN KEY([DeliveryGroup_GUID])
REFERENCES [FMO].[DeliveryGroup] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_DeliveryGroup]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_ReferenceData] FOREIGN KEY([LocationProvider_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_ReferenceData_02] FOREIGN KEY([OperationalStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_ReferenceData_02]
GO
ALTER TABLE [FMO].[DeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPoint_ReferenceData_03] FOREIGN KEY([DeliveryPointUseIndicator_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPoint] CHECK CONSTRAINT [FK_DeliveryPoint_ReferenceData_03]
GO
ALTER TABLE [FMO].[DeliveryPointAlias]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryPointAlias_DeliveryPoint] FOREIGN KEY([DeliveryPoint_GUID])
REFERENCES [FMO].[DeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[DeliveryPointAlias] CHECK CONSTRAINT [FK_DeliveryPointAlias_DeliveryPoint]
GO
ALTER TABLE [FMO].[DeliveryRoute]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRoute_DeliveryScenario] FOREIGN KEY([DeliveryScenario_GUID])
REFERENCES [FMO].[Scenario] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoute] CHECK CONSTRAINT [FK_DeliveryRoute_DeliveryScenario]
GO
ALTER TABLE [FMO].[DeliveryRoute]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRoute_ReferenceData] FOREIGN KEY([OperationalStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoute] CHECK CONSTRAINT [FK_DeliveryRoute_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryRoute]  WITH CHECK ADD  CONSTRAINT [FK_Route_ReferenceData] FOREIGN KEY([RouteMethodType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoute] CHECK CONSTRAINT [FK_Route_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryRoute]  WITH CHECK ADD  CONSTRAINT [FK_Route_ReferenceData_02] FOREIGN KEY([TravelOutTransportType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoute] CHECK CONSTRAINT [FK_Route_ReferenceData_02]
GO
ALTER TABLE [FMO].[DeliveryRoute]  WITH CHECK ADD  CONSTRAINT [FK_Route_ReferenceData_03] FOREIGN KEY([TravelInTransportType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoute] CHECK CONSTRAINT [FK_Route_ReferenceData_03]
GO
ALTER TABLE [FMO].[DeliveryRouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteActivity_Block] FOREIGN KEY([Block_GUID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteActivity] CHECK CONSTRAINT [FK_DeliveryRouteActivity_Block]
GO
ALTER TABLE [FMO].[DeliveryRouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteActivity_DeliveryGroup] FOREIGN KEY([DeliveryGroup_GUID])
REFERENCES [FMO].[DeliveryGroup] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteActivity] CHECK CONSTRAINT [FK_DeliveryRouteActivity_DeliveryGroup]
GO
ALTER TABLE [FMO].[DeliveryRouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteActivity_ReferenceData] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteActivity] CHECK CONSTRAINT [FK_DeliveryRouteActivity_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryRouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivitySequence_ReferenceData] FOREIGN KEY([ActivityType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteActivity] CHECK CONSTRAINT [FK_RouteActivitySequence_ReferenceData]
GO
ALTER TABLE [FMO].[DeliveryRouteActivity]  WITH CHECK ADD  CONSTRAINT [FK_RouteActivitySequence_Route] FOREIGN KEY([DeliveryRoute_GUID])
REFERENCES [FMO].[DeliveryRoute] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteActivity] CHECK CONSTRAINT [FK_RouteActivitySequence_Route]
GO
ALTER TABLE [FMO].[DeliveryRouteBlock]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteActivityBlock_Block] FOREIGN KEY([Block_GUID])
REFERENCES [FMO].[Block] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteBlock] CHECK CONSTRAINT [FK_DeliveryRouteActivityBlock_Block]
GO
ALTER TABLE [FMO].[DeliveryRouteBlock]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteBlock_DeliveryRoute] FOREIGN KEY([DeliveryRoute_GUID])
REFERENCES [FMO].[DeliveryRoute] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteBlock] CHECK CONSTRAINT [FK_DeliveryRouteBlock_DeliveryRoute]
GO
ALTER TABLE [FMO].[DeliveryRouteNetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteNetworkLink_DeliveryRouteActivity] FOREIGN KEY([DeliveryRouteActivity_GUID])
REFERENCES [FMO].[DeliveryRouteActivity] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRouteNetworkLink] CHECK CONSTRAINT [FK_DeliveryRouteNetworkLink_DeliveryRouteActivity]
GO
ALTER TABLE [FMO].[DeliveryRouteNetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRouteNetworkLink_NetworkLink] FOREIGN KEY([NetworkLink_GUID])
REFERENCES [FMO].[NetworkLink] ([Id])
GO
ALTER TABLE [FMO].[DeliveryRouteNetworkLink] CHECK CONSTRAINT [FK_DeliveryRouteNetworkLink_NetworkLink]
GO
ALTER TABLE [FMO].[DeliveryRoutePostcode]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRoutePostcode_DeliveryRoute] FOREIGN KEY([DeliveryRoute_GUID])
REFERENCES [FMO].[DeliveryRoute] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoutePostcode] CHECK CONSTRAINT [FK_DeliveryRoutePostcode_DeliveryRoute]
GO
ALTER TABLE [FMO].[DeliveryRoutePostcode]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRoutePostcode_Postcode] FOREIGN KEY([Postcode_GUID])
REFERENCES [FMO].[Postcode] ([ID])
GO
ALTER TABLE [FMO].[DeliveryRoutePostcode] CHECK CONSTRAINT [FK_DeliveryRoutePostcode_Postcode]
GO
ALTER TABLE [FMO].[Function]  WITH CHECK ADD  CONSTRAINT [FK_Function_Action] FOREIGN KEY([Action_GUID])
REFERENCES [FMO].[Action] ([ID])
GO
ALTER TABLE [FMO].[Function] CHECK CONSTRAINT [FK_Function_Action]
GO
ALTER TABLE [FMO].[GroupHazard]  WITH CHECK ADD  CONSTRAINT [FK_GroupHazard_DeliveryGroup] FOREIGN KEY([DeliveryGroup_GUID])
REFERENCES [FMO].[DeliveryGroup] ([ID])
GO
ALTER TABLE [FMO].[GroupHazard] CHECK CONSTRAINT [FK_GroupHazard_DeliveryGroup]
GO
ALTER TABLE [FMO].[GroupHazard]  WITH CHECK ADD  CONSTRAINT [FK_GroupHazard_ReferenceData] FOREIGN KEY([Category_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[GroupHazard] CHECK CONSTRAINT [FK_GroupHazard_ReferenceData]
GO
ALTER TABLE [FMO].[GroupHazard]  WITH CHECK ADD  CONSTRAINT [FK_GroupHazard_ReferenceData_02] FOREIGN KEY([SubCategory_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[GroupHazard] CHECK CONSTRAINT [FK_GroupHazard_ReferenceData_02]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_NetworkNode] FOREIGN KEY([StartNode_GUID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_NetworkNode]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_NetworkNode_02] FOREIGN KEY([EndNode_GUID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_NetworkNode_02]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_OSStreet] FOREIGN KEY([StreetName_GUID])
REFERENCES [FMO].[StreetName] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_OSStreet]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_ReferenceData] FOREIGN KEY([NetworkLinkType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_ReferenceData]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_ReferenceData_02] FOREIGN KEY([DataProvider_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_ReferenceData_02]
GO
ALTER TABLE [FMO].[NetworkLink]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLink_Road] FOREIGN KEY([RoadName_GUID])
REFERENCES [FMO].[RoadName] ([ID])
GO
ALTER TABLE [FMO].[NetworkLink] CHECK CONSTRAINT [FK_NetworkLink_Road]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_NetworkLink] FOREIGN KEY([NetworkLink_GUID])
REFERENCES [FMO].[NetworkLink] ([Id])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_NetworkLink]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_NetworkReference] FOREIGN KEY([NetworkReference_GUID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_NetworkReference]
GO
ALTER TABLE [FMO].[NetworkLinkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReferenceLink_OSRoadLink] FOREIGN KEY([OSRoadLink_GUID])
REFERENCES [FMO].[OSRoadLink] ([ID])
GO
ALTER TABLE [FMO].[NetworkLinkReference] CHECK CONSTRAINT [FK_NetworkReferenceLink_OSRoadLink]
GO
ALTER TABLE [FMO].[NetworkNode]  WITH CHECK ADD  CONSTRAINT [FK_NetworkNode_ReferenceData] FOREIGN KEY([NetworkNodeType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[NetworkNode] CHECK CONSTRAINT [FK_NetworkNode_ReferenceData]
GO
ALTER TABLE [FMO].[NetworkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReference_NetworkLink] FOREIGN KEY([PointReferenceNetworkLink_GUID])
REFERENCES [FMO].[NetworkLink] ([Id])
GO
ALTER TABLE [FMO].[NetworkReference] CHECK CONSTRAINT [FK_NetworkReference_NetworkLink]
GO
ALTER TABLE [FMO].[NetworkReference]  WITH CHECK ADD  CONSTRAINT [FK_NetworkReference_NetworkNode] FOREIGN KEY([NetworkNode_GUID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[NetworkReference] CHECK CONSTRAINT [FK_NetworkReference_NetworkNode]
GO
ALTER TABLE [FMO].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_ReferenceData] FOREIGN KEY([NotificationType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Notification] CHECK CONSTRAINT [FK_Notification_ReferenceData]
GO
ALTER TABLE [FMO].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_ReferenceData_02] FOREIGN KEY([NotificationPriority_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Notification] CHECK CONSTRAINT [FK_Notification_ReferenceData_02]
GO
ALTER TABLE [FMO].[OSAccessRestriction]  WITH CHECK ADD  CONSTRAINT [FK_Access Restriction_NetworkReference] FOREIGN KEY([NetworkReference_GUID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[OSAccessRestriction] CHECK CONSTRAINT [FK_Access Restriction_NetworkReference]
GO
ALTER TABLE [FMO].[OSConnectingLink]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingLink_OSConnectingNode] FOREIGN KEY([ConnectingNode_GUID])
REFERENCES [FMO].[OSConnectingNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingLink] CHECK CONSTRAINT [FK_OSConnectingLink_OSConnectingNode]
GO
ALTER TABLE [FMO].[OSConnectingLink]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingLink_OSPathNode] FOREIGN KEY([PathNode_GUID])
REFERENCES [FMO].[OSPathNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingLink] CHECK CONSTRAINT [FK_OSConnectingLink_OSPathNode]
GO
ALTER TABLE [FMO].[OSConnectingNode]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingNode_NetworkNode] FOREIGN KEY([ID])
REFERENCES [FMO].[NetworkNode] ([ID])
GO
ALTER TABLE [FMO].[OSConnectingNode] CHECK CONSTRAINT [FK_OSConnectingNode_NetworkNode]
GO
ALTER TABLE [FMO].[OSConnectingNode]  WITH CHECK ADD  CONSTRAINT [FK_OSConnectingNode_OSRoadLink] FOREIGN KEY([RoadLink_GUID])
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
ALTER TABLE [FMO].[OSTurnRestriction]  WITH CHECK ADD  CONSTRAINT [FK_TurnRestriction_NetworkReference] FOREIGN KEY([NetworkReference_GUID])
REFERENCES [FMO].[NetworkReference] ([ID])
GO
ALTER TABLE [FMO].[OSTurnRestriction] CHECK CONSTRAINT [FK_TurnRestriction_NetworkReference]
GO
ALTER TABLE [FMO].[POBox]  WITH CHECK ADD  CONSTRAINT [FK_POBox_Address_02] FOREIGN KEY([POBoxLinkedAddress_GUID])
REFERENCES [FMO].[PostalAddress] ([ID])
GO
ALTER TABLE [FMO].[POBox] CHECK CONSTRAINT [FK_POBox_Address_02]
GO
ALTER TABLE [FMO].[POBox]  WITH CHECK ADD  CONSTRAINT [FK_POBox_ReferenceData] FOREIGN KEY([POBoxType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[POBox] CHECK CONSTRAINT [FK_POBox_ReferenceData]
GO
ALTER TABLE [FMO].[PointHazard]  WITH CHECK ADD  CONSTRAINT [FK_PointHazard_ReferenceData] FOREIGN KEY([Category_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PointHazard] CHECK CONSTRAINT [FK_PointHazard_ReferenceData]
GO
ALTER TABLE [FMO].[PointHazard]  WITH CHECK ADD  CONSTRAINT [FK_PointHazard_ReferenceData_02] FOREIGN KEY([SubCategory_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PointHazard] CHECK CONSTRAINT [FK_PointHazard_ReferenceData_02]
GO
ALTER TABLE [FMO].[PointHazard]  WITH CHECK ADD  CONSTRAINT [FK_PointHazard_ReferenceData_03] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PointHazard] CHECK CONSTRAINT [FK_PointHazard_ReferenceData_03]
GO
ALTER TABLE [FMO].[Polygon]  WITH CHECK ADD  CONSTRAINT [FK_Polygon_ReferenceData] FOREIGN KEY([PolygonType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Polygon] CHECK CONSTRAINT [FK_Polygon_ReferenceData]
GO
ALTER TABLE [FMO].[PolygonObject]  WITH CHECK ADD  CONSTRAINT [FK_PolygonObject_ReferenceData] FOREIGN KEY([OperationalObjectType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PolygonObject] CHECK CONSTRAINT [FK_PolygonObject_ReferenceData]
GO
ALTER TABLE [FMO].[PolygonObject]  WITH CHECK ADD  CONSTRAINT [FK_PolygonOperationalObject_Polygon] FOREIGN KEY([Polygon_GUID])
REFERENCES [FMO].[Polygon] ([ID])
GO
ALTER TABLE [FMO].[PolygonObject] CHECK CONSTRAINT [FK_PolygonOperationalObject_Polygon]
GO
ALTER TABLE [FMO].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_Address_Postcode] FOREIGN KEY([PostCodeGUID])
REFERENCES [FMO].[Postcode] ([ID])
GO
ALTER TABLE [FMO].[PostalAddress] CHECK CONSTRAINT [FK_Address_Postcode]
GO
ALTER TABLE [FMO].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddress_ReferenceData] FOREIGN KEY([AddressStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostalAddress] CHECK CONSTRAINT [FK_PostalAddress_ReferenceData]
GO
ALTER TABLE [FMO].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_PostalAddress_ReferenceData_02] FOREIGN KEY([AddressType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[PostalAddress] CHECK CONSTRAINT [FK_PostalAddress_ReferenceData_02]
GO
ALTER TABLE [FMO].[Postcode]  WITH CHECK ADD  CONSTRAINT [FK_Postcode_PostcodeSector] FOREIGN KEY([SectorGUID])
REFERENCES [FMO].[PostcodeSector] ([ID])
GO
ALTER TABLE [FMO].[Postcode] CHECK CONSTRAINT [FK_Postcode_PostcodeSector]
GO
ALTER TABLE [FMO].[PostcodeDistrict]  WITH CHECK ADD  CONSTRAINT [FK_PostcodeDistrict_PostcodeArea] FOREIGN KEY([AreaGUID])
REFERENCES [FMO].[PostcodeArea] ([ID])
GO
ALTER TABLE [FMO].[PostcodeDistrict] CHECK CONSTRAINT [FK_PostcodeDistrict_PostcodeArea]
GO
ALTER TABLE [FMO].[PostcodeSector]  WITH CHECK ADD  CONSTRAINT [FK_PostcodeSector_PostcodeDistrict] FOREIGN KEY([DistrictGUID])
REFERENCES [FMO].[PostcodeDistrict] ([ID])
GO
ALTER TABLE [FMO].[PostcodeSector] CHECK CONSTRAINT [FK_PostcodeSector_PostcodeDistrict]
GO
ALTER TABLE [FMO].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_ReferenceData] FOREIGN KEY([DataParent_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_ReferenceData]
GO
ALTER TABLE [FMO].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_ReferenceDataCategory] FOREIGN KEY([ReferenceDataCategory_GUID])
REFERENCES [FMO].[ReferenceDataCategory] ([ID])
GO
ALTER TABLE [FMO].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_ReferenceDataCategory]
GO
ALTER TABLE [FMO].[RMGDeliveryPoint]  WITH CHECK ADD  CONSTRAINT [FK_RMGDeliveryPoint_DeliveryPoint] FOREIGN KEY([DeliveryPoint_GUID])
REFERENCES [FMO].[DeliveryPoint] ([ID])
GO
ALTER TABLE [FMO].[RMGDeliveryPoint] CHECK CONSTRAINT [FK_RMGDeliveryPoint_DeliveryPoint]
GO
ALTER TABLE [FMO].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Function] FOREIGN KEY([Function_GUID])
REFERENCES [FMO].[Function] ([ID])
GO
ALTER TABLE [FMO].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Function]
GO
ALTER TABLE [FMO].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Role] FOREIGN KEY([Role_GUID])
REFERENCES [FMO].[Role] ([ID])
GO
ALTER TABLE [FMO].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Role]
GO
ALTER TABLE [FMO].[Scenario]  WITH CHECK ADD  CONSTRAINT [FK_Scenario_ReferenceData] FOREIGN KEY([OperationalState_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[Scenario] CHECK CONSTRAINT [FK_Scenario_ReferenceData]
GO
ALTER TABLE [FMO].[Scenario]  WITH CHECK ADD  CONSTRAINT [FK_Scenario_UnitLocation] FOREIGN KEY([Unit_GUID])
REFERENCES [FMO].[UnitLocation] ([ID])
GO
ALTER TABLE [FMO].[Scenario] CHECK CONSTRAINT [FK_Scenario_UnitLocation]
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
ALTER TABLE [FMO].[UnitLocation]  WITH CHECK ADD  CONSTRAINT [FK_UnitLocation_ReferenceData] FOREIGN KEY([LocationType_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[UnitLocation] CHECK CONSTRAINT [FK_UnitLocation_ReferenceData]
GO
ALTER TABLE [FMO].[UnitLocationPostcode]  WITH CHECK ADD  CONSTRAINT [FK_UnitLocationPostcode_Postcode] FOREIGN KEY([PoscodeUnit_GUID])
REFERENCES [FMO].[Postcode] ([ID])
GO
ALTER TABLE [FMO].[UnitLocationPostcode] CHECK CONSTRAINT [FK_UnitLocationPostcode_Postcode]
GO
ALTER TABLE [FMO].[UnitLocationPostcode]  WITH CHECK ADD  CONSTRAINT [FK_UnitLocationPostcode_UnitLocation] FOREIGN KEY([Unit_GUID])
REFERENCES [FMO].[UnitLocation] ([ID])
GO
ALTER TABLE [FMO].[UnitLocationPostcode] CHECK CONSTRAINT [FK_UnitLocationPostcode_UnitLocation]
GO
ALTER TABLE [FMO].[UnitPostcodeSector]  WITH CHECK ADD  CONSTRAINT [FK_UnitPostcodeSector_PostcodeSector] FOREIGN KEY([PostcodeSector_GUID])
REFERENCES [FMO].[PostcodeSector] ([ID])
GO
ALTER TABLE [FMO].[UnitPostcodeSector] CHECK CONSTRAINT [FK_UnitPostcodeSector_PostcodeSector]
GO
ALTER TABLE [FMO].[UnitPostcodeSector]  WITH CHECK ADD  CONSTRAINT [FK_UnitPostcodeSector_ReferenceData] FOREIGN KEY([UnitStatus_GUID])
REFERENCES [FMO].[ReferenceData] ([ID])
GO
ALTER TABLE [FMO].[UnitPostcodeSector] CHECK CONSTRAINT [FK_UnitPostcodeSector_ReferenceData]
GO
ALTER TABLE [FMO].[UnitPostcodeSector]  WITH CHECK ADD  CONSTRAINT [FK_UnitPostcodeSector_UnitLocation] FOREIGN KEY([Unit_GUID])
REFERENCES [FMO].[UnitLocation] ([ID])
GO
ALTER TABLE [FMO].[UnitPostcodeSector] CHECK CONSTRAINT [FK_UnitPostcodeSector_UnitLocation]
GO
ALTER TABLE [FMO].[UserRoleUnit]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleUnit_Role] FOREIGN KEY([Role_GUID])
REFERENCES [FMO].[Role] ([ID])
GO
ALTER TABLE [FMO].[UserRoleUnit] CHECK CONSTRAINT [FK_UserRoleUnit_Role]
GO
ALTER TABLE [FMO].[UserRoleUnit]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleUnit_UnitLocation] FOREIGN KEY([Unit_GUID])
REFERENCES [FMO].[UnitLocation] ([ID])
GO
ALTER TABLE [FMO].[UserRoleUnit] CHECK CONSTRAINT [FK_UserRoleUnit_UnitLocation]
GO
ALTER TABLE [FMO].[UserRoleUnit]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleUnit_User] FOREIGN KEY([User_GUID])
REFERENCES [FMO].[User] ([ID])
GO
ALTER TABLE [FMO].[UserRoleUnit] CHECK CONSTRAINT [FK_UserRoleUnit_User]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [IDX_Spatial_AccessLink_AccessLinkLine]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_AccessLink_AccessLinkLine] ON [FMO].[AccessLink]
(
	[AccessLinkLine]
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
/****** Object:  Index [IDX_Spatial_AccessLink_NetworkIntersectionPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_AccessLink_NetworkIntersectionPoint] ON [FMO].[AccessLink]
(
	[NetworkIntersectionPoint]
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
/****** Object:  Index [IDX_Spatial_AccessLink_OperationalObjectPoint]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_AccessLink_OperationalObjectPoint] ON [FMO].[AccessLink]
(
	[OperationalObjectPoint]
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
/****** Object:  Index [IDX_Spatial_AddressLocation_LocationXY]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_DeliveryPoint_LocationXY]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_DeliveryPoint_LocationXY] ON [FMO].[DeliveryPoint]
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
/****** Object:  Index [IDX_Spatial_NetworkLink_LinkGeometry]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_NetworkNode_NodeGeometry]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_NetworkNode_NodeGeometry] ON [FMO].[NetworkNode]
(
	[NodeGeometry]
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
/****** Object:  Index [IDX_Spatial_NetworkReference_NodeReferenceLocation]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_NetworkReference_PointReferenceLocation]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_OSConnectingLink_Geometry]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_OSConnectingNode_Location]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_OSPathLink_CentreLineGeometry]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_OSPathNode_Location]    Script Date: 7/20/2017 7:25:45 AM ******/
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
/****** Object:  Index [IDX_Spatial_OSRoadLink_CentreLineGeometry]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSRoadLink_CentreLineGeometry] ON [FMO].[OSRoadLink]
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
/****** Object:  Index [IDX_Spatial_RoadLink]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_RoadLink] ON [FMO].[OSRoadLink]
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
/****** Object:  Index [IDX_Spatial_OSRoadNode_Location]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_OSRoadNode_Location] ON [FMO].[OSRoadNode]
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
/****** Object:  Index [IDX_Spatial_Polygon_PolygonCentroid]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_Polygon_PolygonCentroid] ON [FMO].[Polygon]
(
	[PolygonCentroid]
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
/****** Object:  Index [IDX_Spatial_Polygon_PolygonGeometry]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_Polygon_PolygonGeometry] ON [FMO].[Polygon]
(
	[PolygonGeometry]
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
/****** Object:  Index [IDX_Spatial_RMGDeliveryPoint_LocationXY]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_RMGDeliveryPoint_LocationXY] ON [FMO].[RMGDeliveryPoint]
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
/****** Object:  Index [IDX_Spatial_RMGLink_Geometry]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_RMGLink_Geometry] ON [FMO].[RMGLink]
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
/****** Object:  Index [IDX_Spatial_RMGNode_Location]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_RMGNode_Location] ON [FMO].[RMGNode]
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
/****** Object:  Index [IDX_Spatial_StreetName_Geometry]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_StreetName_Geometry] ON [FMO].[StreetName]
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
/****** Object:  Index [IDX_Spatial_UnitLocation_UnitBoundryPolygon]    Script Date: 7/20/2017 7:25:45 AM ******/
CREATE SPATIAL INDEX [IDX_Spatial_UnitLocation_UnitBoundryPolygon] ON [FMO].[UnitLocation]
(
	[UnitBoundryPolygon]
)USING  GEOMETRY_GRID 
WITH (BOUNDING_BOX =(1393.0196, 13494.9764, 671196.3657, 1230275.0454), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
