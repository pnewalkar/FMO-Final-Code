 DECLARE @CategoryID uniqueidentifier

  INSERT INTO [FMO].[ReferenceDataCategory]
  (
	   [CategoryName]
      ,[Maintainable]
      ,[CategoryType]
      ,[ID]
  ) 
  VALUES
  (
	'Object Transparency',
	0,
	2,
	NEWID()
  )


  DECLARE @CategoryID uniqueidentifier
 SELECT @CategoryID = [ID] FROM [FMO].[ReferenceDataCategory] WHERE [CategoryName] = 'Object Transparency'

 INSERT INTO [FMO].[ReferenceData]
  (
	[ReferenceDataName]
      ,[ReferenceDataValue]
      ,[DataDescription]
      ,[ID]
      ,[ReferenceDataCategory_GUID]
      ,[Default]
  )
  VALUES 
  (
	'Object Transparency',
	0.4,
	'Transparency',
	NEWID(),
	@CategoryID,
	0
  )



select * from [FMO].[ReferenceData]