using System.ComponentModel.DataAnnotations;
using CarRent.DTOs;
using CarRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber
            });
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ValidationException(errors);
            }
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user != null)
            {
                user.UserName = userDto.UserName;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new ValidationException(errors);
                }
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }

            return null;
        }
    }
}
