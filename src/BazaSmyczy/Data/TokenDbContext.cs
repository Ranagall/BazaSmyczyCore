using BazaSmyczy.Models;
using Microsoft.EntityFrameworkCore;

namespace BazaSmyczy.Data
{
    public class TokenDbContext : DbContext
    {
        public TokenDbContext(DbContextOptions<TokenDbContext> options) : base(options)
        {
        }

        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>().ToTable("Tokens");
        }
    }
}
