---Route selection type insert-----  

Declare @ID UniqueIdentifier

insert into [FMO].[ReferenceDataCategory] ([ID]
      ,[CategoryName]
      ,[Maintainable]
      ,[CategoryType]) values(NEWID(),'Route Log Selection Type',0,2)

Select @ID = ID from [FMO].[ReferenceDataCategory] WHere CategoryName = 'Route Log Selection Type'

INSERT [FMO].[ReferenceData] ( [ReferenceDataName], [ReferenceDataValue], [DataDescription], [DisplayText],  [ID], [ReferenceDataCategory_GUID], [DataParent_GUID]) VALUES ( NULL, NULL, N'Single', N'Single',  NEWID(), @ID, NULL)

INSERT [FMO].[ReferenceData] ( [ReferenceDataName], [ReferenceDataValue], [DataDescription], [DisplayText],  [ID], [ReferenceDataCategory_GUID], [DataParent_GUID]) VALUES ( NULL, NULL, N'Multiple', N'Multiple',  NEWID(), @ID, NULL)


