using AlexAPI.Authentication;
using AlexAPI.Library.Mail;
using AlexAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private ILogger<IdentityController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;
        private IMailService mailService;
        private IFileTemplateService fileTemplateService;
        public IdentityController(ILogger<IdentityController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IFileTemplateService fileTemplateService, IMailService mailService) 
        {
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.fileTemplateService = fileTemplateService;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    roles = userRoles
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            var newUser = await userManager.FindByNameAsync(model.Username);

            var roleResult = await userManager.AddToRoleAsync(newUser, UserRoles.User);

            if (!result.Succeeded || !roleResult.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            var body = fileTemplateService.GetNewUserBody(user.Email, model.Password);
            Mail mail = new Mail()
            {
                ToEmail = user.Email,
                Subject = "New Yachtshop User",
                Body = body

            };

            mailService.SendEmailNow(mail);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterBroker")]
        public async Task<IActionResult> RegisterBroker([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (await roleManager.RoleExistsAsync(UserRoles.Broker))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Broker);
            }

            var body = fileTemplateService.GetNewUserBody(user.Email, model.Password);
            Mail mail = new Mail()
            {
                ToEmail = user.Email,
                Subject = "New Yachtshop Broker",
                Body = body

            };

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            var body = fileTemplateService.GetNewUserBody(user.Email, model.Password);
            Mail mail = new Mail()
            {
                ToEmail = user.Email,
                Subject = "New Yachtshop Admin",
                Body = body

            };

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("Password-Reset-Email")]
        public async Task<IActionResult> ResetPasswordEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var baseAddress = configuration["baseAddress"];
            var URLToken = HttpUtility.UrlEncode(resetToken);
            var resetLink = $"{baseAddress}/my-account-login/reset-password?token={URLToken}&email={email}";
            var body = fileTemplateService.GetPasswordResetLinkBody(user.UserName, resetLink);
            Mail mail = new Mail()
            {
                ToEmail = email,
                Subject = "Yachtshop Password Reset",
                Body = body

            };
            mailService.SendEmailNow(mail);
            return Ok(mail);
        }

        [HttpPost]
        [Route("Password-Reset")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            byte[] bytes = Encoding.Default.GetBytes(model.Token);
            var ASCIIToken = Encoding.ASCII.GetString(bytes);
            var user = await userManager.FindByEmailAsync(model.Email);
            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
