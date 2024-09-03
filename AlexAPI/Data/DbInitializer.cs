using Microsoft.AspNetCore.Identity;
using AlexAPI.Authentication;
using AlexAPI.Models;

namespace AlexAPI.Data
{
    public interface IDbInitializer
    {
        void Initialize();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            string superadminEmail = _configuration.GetValue<string>("SuperUser:Email");
            string superadminUsername = _configuration.GetValue<string>("SuperUser:Username");
            string superadminPassword = _configuration.GetValue<string>("SuperUser:Password");
            string superadminDefaultRole = _configuration.GetValue<string>("SuperUser:Role");
            ApplicationUser? user = _userManager.FindByEmailAsync(superadminEmail).Result;
            if (user == null)
            {
                var authenticator = new Google.Authenticator.TwoFactorAuthenticator();

                user = new ApplicationUser
                {
                    UserName = superadminUsername,
                    Email = superadminEmail,
                    EmailConfirmed = true,
                    secretKey2FA = Guid.NewGuid()
                };
                _ = _userManager.CreateAsync(user, superadminPassword).Result;
            }

            foreach (var r in _configuration.GetSection("DefaultRoles").Get<string[]>())
            {
                var newRole = new IdentityRole
                {
                    Name = r
                };
                _ = _roleManager.CreateAsync(newRole).Result;
                _ = _userManager.AddToRoleAsync(user, r).Result;
            }

            if (!_userManager.GetRolesAsync(user).Result.Contains(superadminDefaultRole))
            {
                _ = _userManager.AddToRoleAsync(user, superadminDefaultRole).Result;
            }
        }
    }
}
