using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RM.Common.Notification.WebAPI.Entities
{
    /// <summary>
    /// Base class to enable audit logging for the application.
    /// </summary>
    public abstract class AuditContext : DbContext
    {
        protected AuditContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database and logs the changelog.
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <returns>The number of state entries written to the underlying database.</returns>
        public int SaveChanges(string userId)
        {
            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);

            foreach (var entry in modifiedEntities)
            {
                // For each changed record, get the audit record entries and add them
                foreach (AuditLog auditLog in GetAuditRecordsForChange(entry, userId))
                {
                    AuditLogs.Add(auditLog);
                }
            }

            // Call the original SaveChanges(), which will save both the changes made and the audit records
            return base.SaveChanges();
        }

        /// <summary>
        /// Gets the primary key value for the respective DB record.
        /// </summary>
        /// <param name="entry">The <see cref="DbEntityEntry"/>.</param>
        /// <returns>Primary key.</returns>
        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        /// <summary>
        /// Gets all the change logs for an entity.
        /// </summary>
        /// <param name="dbEntry">The <see cref="DbEntityEntry"/>.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>Collection of audit logs.</returns>
        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<AuditLog> auditLogs = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            var primaryKey = GetPrimaryKeyValue(dbEntry);
            string keyName;
            if (primaryKey == null)
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList();

                keyName = keyNames[0].Name;
            }
            else
            {
                keyName = primaryKey.ToString();
            }

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                {
                    auditLogs.Add(new AuditLog()
                    {
                        AuditLog_Id = Guid.NewGuid(),
                        UserId = userId,
                        EventTimeStamp = changeTime,
                        EventType = EntityState.Added.ToString(),    // Added
                        TableName = tableName,
                        RecordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                        ColumnName = propertyName,
                        NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                        OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString()
                    });
                }
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                auditLogs.Add(new AuditLog()
                {
                    AuditLog_Id = Guid.NewGuid(),
                    UserId = userId,
                    EventTimeStamp = changeTime,
                    EventType = EntityState.Deleted.ToString(), // Deleted
                    TableName = tableName,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    ColumnName = null,
                    NewValue = dbEntry.OriginalValues.ToObject().ToString(),
                    OriginalValue = dbEntry.OriginalValues.ToObject().ToString()
                });
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                    // For updates, we only want to capture the columns that actually changed
                    if (!Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        auditLogs.Add(new AuditLog()
                        {
                            AuditLog_Id = Guid.NewGuid(),
                            UserId = userId,
                            EventTimeStamp = changeTime,
                            EventType = EntityState.Modified.ToString(),    // Modified
                            TableName = tableName,
                            RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                            ColumnName = propertyName,
                            OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                            NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        });
                    }
                }
            }

            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            return auditLogs;
        }
    }
}