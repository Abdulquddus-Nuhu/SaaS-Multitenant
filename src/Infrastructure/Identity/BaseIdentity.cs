using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public abstract class BaseIdentity : IdentityUser, IBaseEntity
    {
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; protected set; } = string.Empty;
        public virtual DateTime? Deleted { get; protected set; }
        public string? CreatedBy { get; protected set; } = string.Empty;
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Modified { get; protected set; }
        public virtual string? LastModifiedBy { get; protected set; }
        public bool IsActive { get; set; } = true;


        /// <summary>
        /// The DataKey to be used for multi-tenant applications
        /// </summary>
        public string? DataKey { get; set; }


        protected BaseIdentity()
        {
            IsDeleted = false;
            Created = DateTime.UtcNow;
        }


        public object Clone() => MemberwiseClone();
    }

}
