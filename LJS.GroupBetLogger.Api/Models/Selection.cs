using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.Models
{
    [Table("Selections", Schema = "Group")]
    public class Selection
    {
        public int SelectionId { get; set; }

        [Required]
        public int BetId { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Bet Bet { get; set; }

        public virtual User User { get; set; }
    }
}