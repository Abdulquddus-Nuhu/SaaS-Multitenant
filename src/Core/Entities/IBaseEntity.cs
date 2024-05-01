namespace Core.Entities
{
    public interface IBaseEntity : ICloneable
    {
        public bool IsDeleted { get; set; }


        /// <summary>
        /// The DataKey to be used for multi-tenant applications
        /// </summary>
        public string DataKey { get; }
    }

}