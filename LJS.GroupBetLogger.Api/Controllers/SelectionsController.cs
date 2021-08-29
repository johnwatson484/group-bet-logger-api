using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Logging;
using LJS.GroupBetLogger.Api.Models;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LJS.GroupBetLogger.Api.Controllers
{
    [RoutePrefix("api/Selections")]
    public class SelectionsController : BaseApiController
    {
        GroupBetLoggerContext db;

        public SelectionsController():base()
        {
            db = new GroupBetLoggerContext();
        }

        public SelectionsController(GroupBetLoggerContext context, ILogger logger):base(logger)
        {
            db = context;
        }

        [Authorize]
        [Route("")]
        public IHttpActionResult Get(int betId = -1)
        {
            var tokenUserId = GetUserId();

            if (betId > 0)
            {
                return Ok(db.Bets.Where(x => x.BetId == betId && x.Group.GroupUsers.Any(g => g.UserId == tokenUserId)).FirstOrDefault()?.Selections.ToList());
            }

            return Ok(db.Selections.Where(x => x.UserId == tokenUserId).ToList());
        }

        [Authorize]
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(Selection selection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenUserId = GetUserId();
            var bet = db.Bets.Where(x => x.BetId == selection.BetId && x.Group.GroupUsers.Any(g => g.UserId == tokenUserId)).FirstOrDefault();

            if (bet == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            db.Selections.Add(selection);
            db.SaveChanges();

            return Ok(selection);
        }
    }
}
