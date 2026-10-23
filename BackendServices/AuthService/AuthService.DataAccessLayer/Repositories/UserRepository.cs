using AuthService.DataAccessLayer.Data;
using AuthService.DataAccessLayer.Entities;
using AuthService.DataAccessLayer.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccessLayer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _db;

    public UserRepository(AuthDbContext db)
    {
        _db = db;
    }

    public async Task<User> AddAsync(User user)
    {
       await _db.Users.AddAsync(user);
       await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
       return await _db.Users.FirstOrDefaultAsync(user => user.Email == email);
        
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
         _db.Users.Update(user);
        await _db.SaveChangesAsync();

    }
}
