using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //with this we take the implementation from the base and we can add our extra configurations to the metod

            modelBuilder.Entity<Movie>() 
                .Property(x => x.Description) 
                .HasMaxLength(250);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Year)
                .IsRequired();

            //relation
            modelBuilder.Entity<Movie>() //the entity Note
                .Property(x => x.Genre) //has one user
                 .IsRequired();

           
        }
    }
}
