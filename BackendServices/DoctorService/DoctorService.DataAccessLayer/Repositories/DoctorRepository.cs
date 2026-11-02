using Dapper;
using DoctorService.DataAccessLayer.Data;
using DoctorService.DataAccessLayer.Entities;
using DoctorService.DataAccessLayer.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DoctorService.DataAccessLayer.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly DapperDbContext _db;

    public DoctorRepository(DapperDbContext db)
    {
        _db = db;
    }

    // Add a new doctor profile
    public async Task AddAsync(Doctor doctor)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                INSERT INTO Doctors
                (
                    Id, UserId, FullName, Email, PhoneNumber,
                    Qualification, Specialization, ExperienceYears,
                    ConsultationFee, City, ProfileImageUrl,
                    ApprovalStatus, IsActive, CreatedAt
                )
                VALUES
                (
                    @Id, @UserId, @FullName, @Email, @PhoneNumber,
                    @Qualification, @Specialization, @ExperienceYears,
                    @ConsultationFee, @City, @ProfileImageUrl,
                    @ApprovalStatus, @IsActive, GETDATE()
                )";

        await connection.ExecuteAsync(query, doctor);
    }

    // Check whether a doctor profile already exists for this AuthService user
    public async Task<bool> ExistsByUserIdAsync(Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                SELECT COUNT(1)
                FROM Doctors
                WHERE UserId = @UserId";

        var count = await connection.ExecuteScalarAsync<int>(
            query, new { UserId = userId });

        return count > 0;
    }

    // Get all doctor profiles
    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        const string query = "SELECT * FROM Doctors";

        return await connection.QueryAsync<Doctor>(query);
    }

    public async Task<Doctor?> GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                SELECT *
                FROM Doctors
                WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<Doctor>(
            query, new { Id = id });
    }

    public async Task<Doctor?> GetByUserIdAsync(Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                SELECT *
                FROM Doctors
                WHERE UserId = @UserId";

        return await connection.QuerySingleOrDefaultAsync<Doctor>(
            query, new { UserId = userId });
    }

   
    // Search doctors using optional name, specialization, and city filters
    public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string? name, string? specialization, string? city)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                SELECT *
                FROM Doctors
                WHERE (@Name IS NULL OR FullName LIKE '%' + @Name + '%')
                  AND (@Specialization IS NULL OR Specialization = @Specialization)
                  AND (@City IS NULL OR City = @City)
                  AND ApprovalStatus = 'Approved'
                  AND IsActive = 1";

        return await connection.QueryAsync<Doctor>(
            query,
            new
            {
                Name = string.IsNullOrWhiteSpace(name) ? null : name,
                Specialization = string.IsNullOrWhiteSpace(specialization)
                    ? null : specialization,
                City = string.IsNullOrWhiteSpace(city) ? null : city
            });
    }

    public Task UpdateApprovalStatusAsync(Guid id, string status)
    {
        throw new NotImplementedException();
    }

    // Update existing doctor profile details
    public async Task UpdateAsync(Doctor doctor)
    {
        using var connection = _db.CreateConnection();

        const string query = @"
                UPDATE Doctors
                SET FullName = @FullName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    Qualification = @Qualification,
                    Specialization = @Specialization,
                    ExperienceYears = @ExperienceYears,
                    ConsultationFee = @ConsultationFee,
                    City = @City,
                    ProfileImageUrl = @ProfileImageUrl,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE()
                WHERE Id = @Id";

        await connection.ExecuteAsync(query, doctor);
    }
}

