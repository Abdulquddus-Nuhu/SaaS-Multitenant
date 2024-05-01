using AuthPermissions.AdminCode;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.MultitenantSetup
{
    public class PersonaSyncIndividualAccountUsers : ISyncAuthenticationUsers
    {
        private readonly UserManager<Persona> _userManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userManager"></param>
        public PersonaSyncIndividualAccountUsers(UserManager<Persona> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// This returns the userId, email and UserName of all the users
        /// </summary>
        /// <returns>collection of SyncAuthenticationUser</returns>
        public async Task<IEnumerable<SyncAuthenticationUser>> GetAllActiveUserInfoAsync()
        {
            return await _userManager.Users
                .Select(x => new SyncAuthenticationUser(x.Id, x.Email, x.UserName)).ToListAsync();
        }
    }

}
