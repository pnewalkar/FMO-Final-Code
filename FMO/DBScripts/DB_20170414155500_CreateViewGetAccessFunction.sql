
/****** Object:  View [FMO].[Vw_GetAccessFunction]    Script Date: 4/14/2017 3:53:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE View [FMO].[Vw_GetAccessFunction] 
	as
 Select r.RoleName, uru.Unit_GUID, u.UserName, f.Name as FunctionName, a.DisplayText as ActionName 
  from [FMO].[Function] f 
inner join [FMO].[RoleFunction] rf on f.ID = rf.Function_GUID
inner join [FMO].[Action] a on a.ID = f.Action_GUID
inner join [FMO].[UserRoleUnit] uru on uru.Role_GUID = rf.Role_GUID
inner join [FMO].[User] u on u.ID = uru.User_GUID
inner join [FMO].[Role] r on r.ID = uru.Role_GUID

GO


