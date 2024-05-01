using AuthPermissions.SupportCode.AddUsersServices;

namespace API.MultitenantSetup
{
    public static class CreateTenantVersions
    {
        public static readonly MultiTenantVersionData TenantSetupData = new()
        {
            TenantRolesForEachVersion = new Dictionary<string, List<string>>()
            {
                { "Free", null },
                { "Pro", null },
                { "Enterprise", new List<string> { "Tenant Admin", "Enterprise" } },
            },
            TenantAdminRoles = new Dictionary<string, List<string>>()
            {
                { "Free", new List<string> { "Invoice Reader", "Invoice Creator" } },
                { "Pro", null },
                { "Enterprise", new List<string> { "Invoice Reader", "Invoice Creator", "Tenant Admin" } }
            }
            //No settings for HasOwnDbForEachVersion as this isn't a sharding 
        };
    }
}
