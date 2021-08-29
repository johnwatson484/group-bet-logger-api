using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.Models
{
    [Table("Groups", Schema = "Group")]
    public class Group
    {
        public int GroupId { get; set; }

        public virtual IList<Bet> Bets { get; set; }

        public virtual IList<GroupUser> GroupUsers { get; set; }

        public Group()
        {
            Bets = new List<Bet>();
            GroupUsers = new List<GroupUser>();
        }
    }
}