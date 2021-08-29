using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LJS.GroupBetLogger.Api.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : BaseApiController
    {
        GroupBetLoggerContext db;

        public UsersController():base()
        {
            db = new GroupBetLoggerContext();
        }

        public UsersController(GroupBetLoggerContext context, ILogger logger):base(logger)
        {
            db = context;
        }

        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            var tokenUserId = GetUserId();            

            return Ok(db.Users.Where(x => x.Id == tokenUserId).FirstOrDefault());
        }        
    }
}
