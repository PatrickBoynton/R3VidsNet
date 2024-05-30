using Microsoft.EntityFrameworkCore;
using R3vids.Models;

namespace R3vids.Data;

public class VideoDbContext(DbContextOptions<VideoDbContext> options) : DbContext(options)
{
    public DbSet<Video> Videos { get; set; } = null!;
    public DbSet<VideoStatus> VideoStatus { get; set; } = null!;

    public DbSet<VideoNavigation> VideoNavigations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>()
            .HasOne(v => v.VideoStatus)
            .WithOne(vs => vs.Video)
            .HasForeignKey<VideoStatus>(vs => vs.Id);
    }
}