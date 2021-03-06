
  DECLARE @Id uniqueidentifier

  INSERT INTO [FMO].[ReferenceDataCategory]
  (	
	   [ID] 	
	  ,[CategoryName]
      ,[Maintainable]
      ,[CategoryType]
  )
  values
  (
	NEWID(),
	'Delivery Point Color',
	0,
	2
  )

  SELECT @Id = [ID] FROM [FMO].[ReferenceDataCategory] WHERE [CategoryName] = 'Delivery Point Color'


  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ffff00',
	'DP Color',
	NEWID(),
	@Id,
	1,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#00ff00',
	'DP Color',
	NEWID(),
	@Id,
	2,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#9999ff',
	'DP Color',
	NEWID(),
	@Id,
	3,
	0
  )
  
  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ffff99',
	'DP Color',
	NEWID(),
	@Id,
	4,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ff99cc',
	'DP Color',
	NEWID(),
	@Id,
	5,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ff8080',
	'DP Color',
	NEWID(),
	@Id,
	6,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#00ccff',
	'DP Color',
	NEWID(),
	@Id,
	7,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#008000',
	'DP Color',
	NEWID(),
	@Id,
	8,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ff6600',
	'DP Color',
	NEWID(),
	@Id,
	9,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#c0c0c0',
	'DP Color',
	NEWID(),
	@Id,
	10,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#808000',
	'DP Color',
	NEWID(),
	@Id,
	11,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ff9900',
	'DP Color',
	NEWID(),
	@Id,
	12,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ccffcc',
	'DP Color',
	NEWID(),
	@Id,
	13,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#cc99ff',
	'DP Color',
	NEWID(),
	@Id,
	14,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#0000ff',
	'DP Color',
	NEWID(),
	@Id,
	15,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#008080',
	'DP Color',
	NEWID(),
	@Id,
	16,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#993300',
	'DP Color',
	NEWID(),
	@Id,
	17,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#ff0000',
	'DP Color',
	NEWID(),
	@Id,
	18,
	0
  )

  INSERT INTO [FMO].[ReferenceData]
  (
	   [ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[OrderingIndex]
      ,[Default]
  )
  VALUES 
  (
	'Delivery Point Color',
	'#da202a',
	'DP Color',
	NEWID(),
	@Id,
	19,
	0
  )