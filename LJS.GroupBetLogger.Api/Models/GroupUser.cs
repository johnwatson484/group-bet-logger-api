using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.Models
{
    [Table("GroupUsers", Schema = "Group")]
    public class GroupUser
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public bool Admin { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}