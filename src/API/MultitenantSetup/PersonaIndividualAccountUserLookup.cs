using AuthPermissions.BaseCode.SetupCode;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace API.MultitenantSetup
{
    public class PersonaIndividualAccountUserLookup : IFindUserInfoService
    {
        private readonly UserManager<Persona> _userManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userManager"></param>
        public PersonaIndividualAccountUserLookup(UserManager<Persona> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// This should find an user in the authentication provider using the <see cref="BulkLoadUserWithRolesTenant.UniqueUserName"/>.
        /// It returns userId and its user name (if no user found with that uniqueName, then
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns>a class containing a UserIf and UserName property, or null if not found</returns>
        public async Task<FindUserInfoResult> FindUserInfoAsync(string uniqueName)
        {
            var user = await _userManager.FindByNameAsync(uniqueName);
            return (user == null ? null : new FindUserInfoResult(user.Id, user.UserName));
        }
    }

}
