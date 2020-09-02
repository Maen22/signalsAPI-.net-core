using System;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(20)]
        public virtual TId CreatedBy { get; set; }

        [MaxLength(20)]
        public virtual TId UpdatedBy { get; set; }

        [MaxLength(20)]
        public virtual TId DeletedBy { get; set; }
    }
}
