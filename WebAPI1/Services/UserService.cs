using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebAPI1.Data;
using WebAPI1.Models;
using WebAPI1.Models.DTOs;

namespace WebAPI1.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        public UserService(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }
        public async Task<List<User>> GetAllUserAsync()
        {
            var user = _appDbContext.Users.ToListAsync();
            return await user;
        }
        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            if (await _appDbContext.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email ya registrado");
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = HashPassword(dto.Password)
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public string HashPassword(string password) {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<User> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _appDbContext.Users.FindAsync(dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = HashPassword(dto.Password);
            }

            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(DeleteUserDto dto)
        {
            var user = await _appDbContext.Users.FindAsync(dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User> UpdateEmailAsync(int userId, string newEmail)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (await _appDbContext.Users.AnyAsync(u => u.Email == newEmail))
                throw new Exception("El nuevo correo electrónico ya está registrado");

            user.Email = newEmail;
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }
    }
}
