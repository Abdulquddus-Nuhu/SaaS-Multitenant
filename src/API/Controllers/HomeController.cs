using API.MultitenantSetup;
using AuthPermissions.SupportCode.AddUsersServices;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Shared.Constants.AuthConstants;
using static Shared.Constants.StringConstants;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<Persona> _userManager;
        private readonly AppDbContext _dbContext;
        public HomeController(UserManager<Persona> userManager
            , AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromServices] ISignInAndCreateTenant userRegisterInvite,
       string tenantName, string email, string password, string version, bool isPersistent)
        {
            var newUserData = new AddNewUserDto { Email = email, Password = password, IsPersistent = isPersistent };
            var newTenantData = new AddNewTenantDto { TenantName = tenantName, Version = version };
            var status = await userRegisterInvite.SignUpNewTenantWithVersionAsync(newUserData, newTenantData,
                CreateTenantVersions.TenantSetupData);


            if (status.HasErrors)
            {
                return BadRequest(status.Errors);
            }

            var newTenant = _dbContext.Companies.FirstOrDefault(x => x.CompanyName == tenantName);

            var admin = await _userManager.FindByEmailAsync(email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);

            var dd = await _userManager.ResetPasswordAsync(admin, token, password);

            admin.PesonaType = Shared.Enums.PersonaType.Admin;
            admin.EmailConfirmed = true;
            admin.DataKey = newTenant.DataKey;

            await _userManager.UpdateAsync(admin);

            await _userManager.AddToRoleAsync(admin, Roles.ADMIN);

            //var admin = new Persona()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Email = email,
            //    FirstName = tenantName,
            //    LastName = "",
            //    PhoneNumber = "",
            //    UserName = email,
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true,
            //    DataKey = newTenant.DataKey,
            //};

            //var result = await _userManager.CreateAsync(admin, password);
            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(admin, Roles.ADMIN);
            //}

            return Ok( new { message = status.Message });
        }
    }
}
