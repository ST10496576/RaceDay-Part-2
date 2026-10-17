using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Event> Events => Set<Event>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Enrolment> Enrolments => Set<Enrolment>();

        public DbSet<Result> Results => Set<Result>();

        public DbSet<RaceDay.API.Models.Route> Routes
        => Set<RaceDay.API.Models.Route>();
    }
}