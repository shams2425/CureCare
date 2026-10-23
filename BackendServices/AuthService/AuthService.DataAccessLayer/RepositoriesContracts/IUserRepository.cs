using AuthService.DataAccessLayer.Entities;

namespace AuthService.DataAccessLayer.RepositoriesContracts;

public interface IUserRepository
{
    // Finds a user by their unique ID.
    Task<User?> GetByIdAsync(int id);

    // Finds a user by their email address.
    Task<User?> GetByEmailAsync(string email);

    // Saves a new user to the database.
    Task<User> AddAsync(User user);

    // Updates an existing user's information.
    Task UpdateAsync(User user);
}
