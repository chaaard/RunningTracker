using Microsoft.EntityFrameworkCore;
using RunningTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Infrastructure.Data
{
    public class RunningTrackerContext : DbContext
    {
        public RunningTrackerContext(DbContextOptions<RunningTrackerContext> options) : base(options)
        {
            Users = Set<User>();
            RunningActivities = Set<RunningActivity>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RunningActivity> RunningActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("tbl_User")
                .HasKey(u => u.Id);

            modelBuilder.Entity<RunningActivity>()
                .ToTable("tbl_RunningActivity")
                .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RunningActivities)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);
        }
    }
}
