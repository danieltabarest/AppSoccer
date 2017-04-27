using Domain;

namespace Backend.Models
{
    public class DataContextLocal :  DataContext
    {
        public System.Data.Entity.DbSet<Domain.Date> Dates { get; set; }

        public System.Data.Entity.DbSet<Domain.TournamentTeam> TournamentTeams { get; set; }

        public System.Data.Entity.DbSet<Domain.UserType> UserTypes { get; set; }

        public System.Data.Entity.DbSet<Domain.User> Users { get; set; }
    }
}