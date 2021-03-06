/****** Script for SelectTopNRows command from SSMS  ******/

INSERT INTO [FMO_DEV_031].[FMO].[RouteActivity]([ID]
      ,[ActivityTypeGUID]  
      ,[RouteActivityOrderIndex] --BS
      ,[BlockID]
      ,[LocationID]
      ,[RouteID])
SELECT NEWID()
      ,'F22DC917-F066-E711-80ED-000D3A22173B' --'Service Demand Activity'
      ,[OrderIndex]
      ,[BlockID]
      ,[LocationID]
      ,[DeliveryRoute_GUID]
  FROM [FMO_DEV_031].[FMO].[BlockSequence] BS97429, [FMO_DEV_030].[FMO].[DeliveryRouteBlock] DRB470
  WHERE [BlockID]=[Block_GUID]

  UPDATE [FMO_DEV_031].[FMO].[RouteActivity]
  SET [RouteActivityOrderIndex]=[RouteActivityOrderIndex]+1

  DELETE FROM [FMO_DEV_031].[FMO].[RouteActivity]
  WHERE [RouteActivityOrderIndex] IS NULL

  /****** Script for SelectTopNRows command from SSMS  ******/

INSERT INTO [FMO_DEV_031].[FMO].[RouteActivity]([ID]
      ,[ActivityTypeGUID]  --'Service Demand Activity'
      ,[RouteActivityOrderIndex] --BS
      ,[BlockID]
      ,[LocationID]
      ,[RouteID]
      ,[ActivityDurationMinute]
      ,[ResourceGUID]
      ,[DistanceToNextLocationMeter])
SELECT newid()
      ,'7803247C-F866-E711-80ED-000D3A22173B' --Travel Out
      ,1
      ,[BlockID]
      ,[LocationID]
      ,[RouteID]
      ,[ActivityDurationMinute]
      ,[ResourceGUID]
      ,[DistanceToNextLocationMeter]
  FROM [FMO_DEV_031].[FMO].[RouteActivity]
  where [RouteActivityOrderIndex]=2

  
  

INSERT INTO [FMO_DEV_031].[FMO].[RouteActivity]([ID]
      ,[ActivityTypeGUID]  --'Service Demand Activity'
      ,[RouteActivityOrderIndex] --BS
      ,[BlockID]
      ,[LocationID]
      ,[RouteID]
      ,[ActivityDurationMinute]
      ,[ResourceGUID]
      ,[DistanceToNextLocationMeter])
SELECT newid()
      ,'9CC0D090-F866-E711-80ED-000D3A22173B'--Travel In
      ,RA1.[RouteActivityOrderIndex]+1
      ,[BlockID]
      ,[LocationID]
      ,RA1.[RouteID]
      ,[ActivityDurationMinute]
      ,[ResourceGUID]
      ,[DistanceToNextLocationMeter]
  FROM [FMO_DEV_031].[FMO].[RouteActivity] RA1, (SELECT MAX([RouteActivityOrderIndex]) AS [RouteActivityOrderIndex]
													  ,[RouteID]
												  FROM [FMO_DEV_031].[FMO].[RouteActivity]
												  GROUP BY [RouteID]) RA2
  where RA1.RouteID=RA2.RouteID AND RA1.[RouteActivityOrderIndex]=RA2.[RouteActivityOrderIndex] 
