/****** Script for SelectTopNRows command from SSMS  ******/

UPDATE [FMO_DEV_031].[FMO].[Postcode]
SET [PrimaryRouteGUID]=[DeliveryRoute_GUID]
FROM [FMO_DEV_031].[FMO].[Postcode] PO, (SELECT [Postcode_GUID]
											  ,[DeliveryRoute_GUID]
										  FROM [FMO_DEV_030].[FMO].[DeliveryRoutePostcode]
										  WHERE [IsPrimaryRoute]=1) RO
WHERE PO.ID=RO.Postcode_GUID


UPDATE [FMO_DEV_031].[FMO].[Postcode]
SET [SecondaryRouteGUID]=[DeliveryRoute_GUID]
FROM [FMO_DEV_031].[FMO].[Postcode] PO, (SELECT [Postcode_GUID]
											  ,[DeliveryRoute_GUID]
										  FROM [FMO_DEV_030].[FMO].[DeliveryRoutePostcode]
										  WHERE [IsPrimaryRoute]=0) RO
WHERE PO.ID=RO.Postcode_GUID