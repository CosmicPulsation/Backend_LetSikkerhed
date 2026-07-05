using LetSikkerhed.Backend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LetSikkerhed.Backend.Database;

public class UserDatabaseContext(DbContextOptions<UserDatabaseContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }
    
    internal DbSet<Token> Tokens { get; set; }
}