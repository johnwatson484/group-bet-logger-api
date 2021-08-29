using LJS.GroupBetLogger.Api.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.DAL
{
    public class GroupBetLoggerContext : IdentityDbContext<User>
    {
        public GroupBetLoggerContext() : base("GroupBetLoggerContext") { }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<GroupUser> GroupUsers { get; set; }

        public virtual DbSet<Bet> Bets { get; set; }

        public virtual DbSet<Selection> Selections { get; set; }
    }
}