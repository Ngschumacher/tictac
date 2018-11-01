using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TypeFaster.Models
{
    public class GameContext : DbContext
    {
        
        public DbSet<Move> Move { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Game;Integrated Security=True;User Id=localhost;Password=localhost");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Game)
                .WithMany(g => g.Moves);
            modelBuilder.Entity<Game>()
                .HasMany<Move>(g => g.Moves)
                .WithOne(m => m.Game)
                .HasForeignKey(k => k.GameId)
                .IsRequired()
                ;
            
            
            base.OnModelCreating(modelBuilder);
        }

      
    }
}