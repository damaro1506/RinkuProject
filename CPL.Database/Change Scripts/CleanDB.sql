--No. de registros por tabla (opción 1)
SELECT o.name,
  ddps.row_count 
FROM sys.indexes AS i
  INNER JOIN sys.objects AS o ON i.OBJECT_ID = o.OBJECT_ID
  INNER JOIN sys.dm_db_partition_stats AS ddps ON i.OBJECT_ID = ddps.OBJECT_ID
  AND i.index_id = ddps.index_id 
WHERE i.index_id < 2  AND o.is_ms_shipped = 0 
ORDER BY ddps.row_count desc

delete CoverDetail
delete Courtesy
delete CoverData
delete Payments
delete CashRegisterOperation
delete Courtesy
delete Area
delete CoverConfiguration




