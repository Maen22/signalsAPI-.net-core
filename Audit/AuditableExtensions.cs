using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Server.Audit
{
    public static class AuditableExtensions
    {
        public static void FilterSoftDeletedEntries<TModel>(this ModelBuilder builder) where TModel : Auditable
        {
            builder.Entity<TModel>()
                    .HasQueryFilter(p => p.DeletedAt == null);
        }

        public static void EnsureAudit<TId>(this DbContext context, TId id = default(TId))
        {
            context.ChangeTracker.DetectChanges();

            var markedAsCreated = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var item in markedAsCreated)
            {
                if (item.Entity is Auditable<TId> entity)
                {
                    // Only update the UpdatedAt flag - only this will get sent to the Db
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = id;
                }
                else if (item.Entity is Auditable entity1)
                {
                    entity1.CreatedAt = DateTime.UtcNow;
                }
            }

            var markedAsUpdated = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var item in markedAsUpdated)
            {
                if (item.Entity is Auditable<TId> entity)
                {
                    // Only update the UpdatedAt flag - only this will get sent to the Db
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = id;
                }
                else if (item.Entity is Auditable entity1)
                {
                    entity1.UpdatedAt = DateTime.UtcNow;
                }
            }

            var markedAsDeleted = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is Auditable<TId> entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the DeletedAt flag - only this will get sent to the Db
                    entity.DeletedAt = DateTime.UtcNow;
                    entity.DeletedBy = id;
                }
                else if (item.Entity is Auditable entity1)
                {
                    item.State = EntityState.Unchanged;
                    entity1.DeletedAt = DateTime.UtcNow;
                }
            }
        }

        public static void EnsureAudit(this DbContext context)
        {
            context.ChangeTracker.DetectChanges();

            var markedAsCreated = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var item in markedAsCreated)
            {
                if (item.Entity is Auditable entity)
                {
                    // Only update the UpdatedAt flag - only this will get sent to the Db
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }

            var markedAsUpdated = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var item in markedAsUpdated)
            {
                if (item.Entity is Auditable entity)
                {
                    // Only update the UpdatedAt flag - only this will get sent to the Db
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            var markedAsDeleted = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is Auditable entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the DeletedAt flag - only this will get sent to the Db
                    entity.DeletedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
