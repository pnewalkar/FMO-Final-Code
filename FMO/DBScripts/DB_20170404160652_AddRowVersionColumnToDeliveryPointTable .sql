 /* User Story RFMO-314 Adding column with datatype timestamp to manange optimistic concurrency*/

ALTER TABLE [FMO].[DeliveryPoint]
ADD [RowVersion] timestamp not null