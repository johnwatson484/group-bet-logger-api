using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.Models
{
    public class User: IdentityUser
    {
        public virtual IList<GroupUser> GroupUsers { get; set; }
    }
}