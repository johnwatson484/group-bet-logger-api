using LJS.GroupBetLogger.Api.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace LJS.GroupBetLogger.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        ILogger logger;

        public BaseApiController()
        {
            logger = new Logger();
        }

        public BaseApiController(ILogger logger)
        {
            this.logger = logger;
        }

        public string GetUserId()
        {
            return ((ClaimsIdentity)User.Identity)?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;            
        }
    }
}
