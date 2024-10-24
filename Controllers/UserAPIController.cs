using Microsoft.AspNetCore.Mvc;
using CarRent.Models;
using CarRent.Repositories;
using CarRent.DTOs;

namespace CarRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserAPIController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
              return NotFound();
            
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
              return BadRequest(ModelState); 

            await _userRepository.AddUserAsync(userDto);

            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto updatedUserDto)
        {
            if (id != updatedUserDto.Id)
              return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
              return BadRequest(ModelState); 

            await _userRepository.UpdateUserAsync(updatedUserDto);

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
              return NotFound();

            await _userRepository.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
