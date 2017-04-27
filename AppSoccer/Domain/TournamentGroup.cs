using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TournamentGroup
    {
        [Key]
        public int TournamentGroupId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The maximun length for field {0} is {1} characters")]
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Group")]
        public string Name { get; set; }

        [Display(Name = "Tournament")]
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 2)]
        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection <Match> Matches { get; set; }

        public virtual ICollection <TournamentTeam> TournamentTeams { get; set; }


    }
}
