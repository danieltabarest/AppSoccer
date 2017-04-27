using Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class MatchView :  Match
    {
        [DataType(DataType.Date)]
        public string  DateString{ get; set; }

        [DataType(DataType.Time)]
        public string TimeString { get; set; }

        [Display(Name = "Local League")]
        public int LocalLeagueId { get; set; }

        [Display(Name = "Visitor League")]
        public int VisitorLeagueId { get; set; }
    }
}