using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UsersMap());
            modelBuilder.Configurations.Add(new GroupsMap());
            modelBuilder.Configurations.Add(new MatchesMap());

        }
        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentGroup> TournamentGroups { get; set; }

        public System.Data.Entity.DbSet<Domain.Status> Status { get; set; }

        public System.Data.Entity.DbSet<Domain.Match> Matches { get; set; }

        public System.Data.Entity.DbSet<Domain.Date> Dates { get; set; }
    }

    internal class MatchesMap : EntityTypeConfiguration<Match>
    {
        public MatchesMap()
        {

                 HasRequired(o => o.Local).WithMany(m => m.Locals).HasForeignKey(m => m.LocalId);

                 HasRequired(o => o.Visitor).WithMany(m => m.Visitors).HasForeignKey(m => m.VisitorId);

        }
    }

    internal class GroupsMap : EntityTypeConfiguration<Group>
    {
        public GroupsMap()
        {

            HasRequired(o => o.Owner).WithMany(m => m.UserGroups).HasForeignKey(m => m.OwnerId);

        }
    }

    internal class UsersMap : EntityTypeConfiguration<User>
    {
        public UsersMap()
        {

            HasRequired(o => o.FavoriteTeam).WithMany(m => m.Fans).HasForeignKey(m => m.FavoriteTeamId);

        }
    }
}
