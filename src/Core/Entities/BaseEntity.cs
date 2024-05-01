using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity : IBaseEntity
    {
        //[StringLength(50, MinimumLength = 1)]
        //public Guid Id { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; protected set; } = string.Empty;
        public virtual DateTime? Deleted { get; protected set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public virtual DateTime Created { get; set; }
        public virtual DateTime? Modified { get; protected set; }
        public virtual string? LastModifiedBy { get; protected set; }

        public bool IsActive { get; set; } = true;


        /// <summary>
        /// The DataKey to be used for multi-tenant applications
        /// </summary>
        public string DataKey { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
            Created = DateTime.UtcNow;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
