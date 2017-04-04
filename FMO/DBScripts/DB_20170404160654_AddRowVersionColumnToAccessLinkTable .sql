 /* User Story RFMO-314 Adding column with datatype timestamp to manange optimistic concurrency*/

ALTER TABLE [FMO].[AccessLink]
ADD [RowVersion] timestamp not null