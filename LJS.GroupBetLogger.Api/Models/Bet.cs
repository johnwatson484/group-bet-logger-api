using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LJS.GroupBetLogger.Api.Models
{
    [Table("Bets", Schema = "Group")]
    public class Bet
    {
        public int BetId { get; set; }

        [Required]
        public int GroupId { get; set; }
        
        public virtual Group Group { get; set; }

        public virtual IList<Selection> Selections { get; set; }
    }
}