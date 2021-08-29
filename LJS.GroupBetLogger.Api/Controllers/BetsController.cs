using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Logging;
using LJS.GroupBetLogger.Api.Models;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LJS.GroupBetLogger.Api.Controllers
{
    [RoutePrefix("api/Bets")]
    public class BetsController : BaseApiController
    {
        GroupBetLoggerContext db;

        public BetsController():base()
        {
            db = new GroupBetLoggerContext();
        }

        public BetsController(GroupBetLoggerContext context, ILogger logger):base(logger)
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
                return Ok(db.Bets.Where(x => x.GroupId == groupId && x.Group.GroupUsers.Any(g => g.UserId == tokenUserId)).ToList());
            }

            return Ok(db.Bets.Where(x => x.Group.GroupUsers.Any(g => g.UserId == tokenUserId)).ToList());
        }

        [Authorize]
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(Bet bet)
        {
            if (!ModelState.IsValid)
            {                
                return BadRequest(ModelState);
            }

            var tokenUserId = GetUserId();
            var group = db.Groups.Where(x => x.GroupId == bet.GroupId && x.GroupUsers.Any(g => g.UserId == tokenUserId)).FirstOrDefault();

            if (group == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            db.Bets.Add(bet);
            db.SaveChanges();

            return Ok(bet);
        }
    }
}
