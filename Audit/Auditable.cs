using System;

namespace Server.Audit
{
    public abstract class Auditable
    {
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }
    }

    public abstract class Auditable<TId> : Auditable
    {
        public virtual TId CreatedBy { get; set; }
        public virtual TId UpdatedBy { get; set; }
        public virtual TId DeletedBy { get; set; }
    }
}
