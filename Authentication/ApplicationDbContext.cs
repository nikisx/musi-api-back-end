using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicApi.Entiites;
using System.Linq;

namespace MusicApi.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       public DbSet<Track> Tracks { get; set; }
       public DbSet<Playlist> Playlists { get; set; }
       public DbSet<Album> Albums { get; set; }
       public DbSet<TrackPlaylist> TrackPlaylists { get; set; }

       public int SaveChanges(string userId)
       {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added));

            foreach (var entity in entities)
            {
                ((BaseEntity)entity.Entity).CreatedById = userId;
                ((BaseEntity)entity.Entity).Created = System.DateTime.Now;

            }

            return SaveChanges();
       }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
