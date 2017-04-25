---Route selection type insert-----  

Declare @ID UniqueIdentifier

insert into [FMO].[ReferenceDataCategory] ([ReferenceDataCategory_Id]
      ,[CategoryName]
      ,[Maintainable]
      ,[CategoryType]) values(19,'Route Log Selection Type',0,2)

Select @ID = ID from [FMO].[ReferenceDataCategory] WHere CategoryName = 'Route Log Selection Type'


INSERT [FMO].[ReferenceData] ([ReferenceDataCategory_Id], [ReferenceDataName], [ReferenceDataValue], [DataDescription], [DisplayText], [DataParentId], [ID], [ReferenceDataCategory_GUID], [DataParent_GUID]) VALUES ( 19, NULL, NULL, N'Multiple', N'Multiple', NULL, NEWID(), @ID, NULL)


INSERT [FMO].[ReferenceData] ([ReferenceDataCategory_Id], [ReferenceDataName], [ReferenceDataValue], [DataDescription], [DisplayText], [DataParentId], [ID], [ReferenceDataCategory_GUID], [DataParent_GUID]) VALUES ( 19, NULL, NULL, N'Single', N'Single', NULL, NEWID(), @ID, NULL)


