using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Logging;
using LJS.GroupBetLogger.Api.Models;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LJS.GroupBetLogger.Api.Controllers
{
    [RoutePrefix("api/Groups")]
    public class GroupsController : BaseApiController
    {
        GroupBetLoggerContext db;

        public GroupsController():base()
        {
            db = new GroupBetLoggerContext();
        }

        public GroupsController(GroupBetLoggerContext context, ILogger logger):base(logger)
        {
            db = context;
        }

        [Authorize]
        [Route("")]
        public IHttpActionResult Get(int groupId = -1)
        {
            var tokenUserId = GetUserId();

            if (groupId > 0)
            {
                return Ok(db.Groups.Where(x => x.GroupId == groupId && x.GroupUsers.Any(g => g.UserId == tokenUserId)).FirstOrDefault());
            }

            return Ok(db.Groups.Where(x => x.GroupUsers.Any(g => g.UserId == tokenUserId)).ToList());            
        }

        [Authorize]
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(string userId)
        {
            var tokenUserId = GetUserId();

            if (userId != tokenUserId)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var group = new Group();
            group.GroupUsers.Add(new GroupUser
            {
                UserId = userId,
                Admin = true
            });

            db.Groups.Add(group);
            db.SaveChanges();

            return Ok(group);
        }
    }
}