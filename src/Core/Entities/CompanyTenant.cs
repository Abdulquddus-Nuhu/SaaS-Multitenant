using AuthPermissions.BaseCode.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CompanyTenant : IDataKeyFilterReadWrite 
    {
        public int CompanyTenantId { get; set; }

        /// <summary>
        /// This contains the fullname of the AuthP Tenant
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// This contains the datakey from the AuthP's Tenant
        /// </summary>
        public string DataKey { get; set; }

        //public string Acronymn { get; set; }

        /// <summary>
        /// This contains the Primary key from the AuthP's Tenant
        /// </summary>
        public int AuthPTenantId { get; set; }
    }

}
