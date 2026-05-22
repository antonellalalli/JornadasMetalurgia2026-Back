using Jornadas_Metalurgia_2026.Enum;
using Jornadas_Metalurgia_2026.Models.User;
using Jornadas_Metalurgia_2026.Models.User.DTO;
using Jornadas_Metalurgia_2026.Services;
using Jornadas_Metalurgia_2026.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Jornadas_Metalurgia_2026.Controllers
{
    [Route("api/JornadaMetalurgica")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }




        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]

        async public Task<ActionResult<User>> Login([FromBody] LoginDTO login)
        {
            try
            {
                var res = await _authService.Login(login, HttpContext);
                return Ok(res);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }



        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserWithoutPasswordDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]

        async public Task<ActionResult<UserWithoutPasswordDTO>> GetSelf()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
                if (userId == null)
                {
                    return Unauthorized();
                }
                if (!int.TryParse(userId, out int userIdINT))
                {
                    return BadRequest(new HttpMessage("Invalid ID token"));
                }

                var users = await _authService.GetUsers();
                var user = users.FirstOrDefault(u => u.Id == userIdINT);
                if (user == null)
                {
                    return NotFound(new HttpMessage("User doesn´t exists"));
                }

                return Ok(user);

            } catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }

        }

        [HttpPut("users/{id}")]
        [Authorize(Roles = ROLE.ADMIN)]
        [ProducesResponseType(typeof(UserWithoutPasswordDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]

        async public Task<ActionResult<UserWithoutPasswordDTO>> UpdateUser(string id, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                var updatedUser = await _authService.UpdateUser(id, dto);
                return Ok(updatedUser);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }





        [HttpPost("logout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<User>> LogOut()
        {
            try
            {
                await _authService.LogOut(HttpContext);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }


        [HttpGet("health")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool Health()
        {
            return true;
        }

        [HttpGet("users")]
        [Authorize(Roles = $" {ROLE.ADMIN}")]

        async public Task<ActionResult<List<UserWithoutPasswordDTO>>> GetUsers()
        {
            try
            {
                var users = await _authService.GetUsers();
                return Ok(users);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }



    }
}
