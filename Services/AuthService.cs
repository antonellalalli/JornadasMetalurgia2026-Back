using AutoMapper;
using Jornadas_Metalurgia_2026.Models.User;
using Jornadas_Metalurgia_2026.Models.User.DTO;
using Jornadas_Metalurgia_2026.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Jornadas_Metalurgia_2026.Services
{
    public class AuthService
    {

        private readonly UserService _userService;
        private readonly IEncoderServices _encoderServices;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        internal readonly string _secret;


        public AuthService(UserService userService, IEncoderServices encoderService, IMapper mapper, IConfiguration config)
        {
            _userService = userService;
            _encoderServices = encoderService;
            _mapper = mapper;
            _config = config;
            _secret = _config.GetSection("Secret:JWT")?.Value?.ToString() ?? string.Empty;

        }




        //Traer todos los usuarios
        async public Task<List<UserWithoutPasswordDTO>> GetUsers()
        {
            return await _userService.GetAll();

        }

        ///-------------------------------------------------------------------
        async public Task<UserWithoutPasswordDTO> UpdateUser(string id, UpdateUserDTO dto)
        {
            var updatedUser = await _userService.UpdateUser(id, dto);
            return _mapper.Map<User, UserWithoutPasswordDTO>(updatedUser);

        }
        //--------------------------------------------------------------------
        async public Task<LoginResponseDTO> Login(LoginDTO login, HttpContext context)
        {
            string datum = login.EmailOrUsername;
            var user = await _userService.GetOneByEmailOrUsername(datum, datum);


            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid Credentials");
            }
            
            bool isMatch = _encoderServices.Verify(login.Password, user.Password);
            
            
            if (!isMatch)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid Credentials");
            }

            await SetCookie(user, context);
            string token = GenerateJwt(user);
            var userDto = _mapper.Map<UserWithoutPasswordDTO>(user);
            userDto.Roles = user.Roles.Select(r => r.Name).ToList();
            return new LoginResponseDTO
            {
                Token = token,
                User = userDto

            };
        }

        async public Task SetCookie(User user, HttpContext context)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString())
            };
            if (user.Roles != null || user.Roles?.Count > 0)
            {
                foreach (var role in user.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role.Name);
                    claims.Add(claim);
                }
            }
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                }
                );
        }

        public string GenerateJwt(User user)
        {
            var key = Encoding.UTF8.GetBytes(_secret);
            var symmetrickKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(symmetrickKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("id", user.Id.ToString()));
            if (user.Roles != null || user.Roles?.Count > 0)
            {
                foreach (var role in user.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role.Name);
                    claims.AddClaim(claim);
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(tokenConfig);
            return token;

        }

        public async Task LogOut(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }









    }
}
