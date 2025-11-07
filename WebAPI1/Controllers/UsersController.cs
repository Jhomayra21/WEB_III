using Microsoft.AspNetCore.Mvc;
using WebAPI1.Models.DTOs;
using WebAPI1.Services;

namespace WebAPI1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService) {
            _userService = userService;
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var user = await _userService.GetAllUserAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(dto);
                return Ok(new { message = "Usuario creado", user.Id, user.UserName });
            }
            catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(dto);
                return Ok(new { message = "Usuario actualizado", user.Id, user.UserName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteUserDto dto)
        {
            try
            {
                await _userService.DeleteUserAsync(dto);
                return Ok(new { message = "Usuario eliminado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
