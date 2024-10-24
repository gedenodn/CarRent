using CarRent.DTOs;

namespace CarRent.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync(); 
        Task<UserDto> GetUserByIdAsync(string id); 
        Task AddUserAsync(UserDto userDto); 
        Task UpdateUserAsync(UserDto userDto); 
        Task DeleteUserAsync(string id); 
        Task<UserDto> AuthenticateAsync(string username, string password); 
    }

}

