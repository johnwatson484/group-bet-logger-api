using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Results;
using System.Web.Http;
using LJS.GroupBetLogger.Api.Controllers;
using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Models;
using Moq;
using NUnit.Framework;
using LJS.GroupBetLogger.Api.Logging;

namespace LJS.GroupBetLogger.Api.Tests.Controllers
{
    [TestFixture]
    public class BetsControllerTests
    {
        Mock<GroupBetLoggerContext> context;
        Mock<DbSet<Bet>> bets;
        List<Bet> betsData;
        Mock<DbSet<Group>> groups;
        List<Group> groupsData;
        Mock<ILogger> logger;
        BetsController controller;

        Bet bet1;
        Bet bet2;
        Bet bet3;

        Group group1;

        GenericIdentity identity;
        
        [SetUp]
        public void Setup()
        {
            identity = new GenericIdentity("user1");
            Claim claim = new Claim("userId", "1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            bet1 = new Bet
            {
                BetId = 1,
                GroupId = 1,
                Group = new Group
                {
                    GroupId = 1,
                    GroupUsers = new List<GroupUser>
                    {
                        new GroupUser
                        {
                            GroupId = 1,
                            UserId = "1"
                        }
                    }
                }
            };

            bet2 = new Bet
            {
                BetId = 2,
                GroupId = 2,
                Group = new Group
                {
                    GroupId = 2,
                    GroupUsers = new List<GroupUser>
                    {
                        new GroupUser
                        {
                            GroupId = 2,
                            UserId = "1"
                        }
                    }
                }
            };

            bet3 = new Bet
            {
                BetId = 3,
                GroupId = 3,
                Group = new Group
                {
                    GroupId = 3,
                    GroupUsers = new List<GroupUser>
                    {
                        new GroupUser
                        {
                            GroupId = 3,
                            UserId = "2"
                        }
                    }
                }
            };

            betsData = new List<Bet>
            {
                bet1,
                bet2,
                bet3
                
            };

            group1 = new Group
            {
                GroupId = 4,
                GroupUsers = new List<GroupUser>
                {
                    new GroupUser
                    {
                        GroupId = 4,
                        UserId = "1"
                    }
                }
            };

            groupsData = new List<Group>
            {
                group1
            };
            
            bets = new Mock<DbSet<Bet>>().SetupData(betsData);
            groups = new Mock<DbSet<Group>>().SetupData(groupsData);
            context = new Mock<GroupBetLoggerContext>();
            context.Setup(x => x.Bets).Returns(bets.Object);
            context.Setup(x => x.Groups).Returns(groups.Object);
            logger = new Mock<ILogger>();
            controller = new BetsController(context.Object, logger.Object);
        }

        [Test]
        public void Test_Get_Returns_All_Bets_If_No_GroupId()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<Bet>>;

            Assert.IsTrue(result.Content.Contains(bet1));
            Assert.IsTrue(result.Content.Contains(bet2));
        }

        [Test]
        public void Test_Get_Returns_All_Bets_For_Group_If_GroupId()
        {
            var result = controller.Get(1) as OkNegotiatedContentResult<List<Bet>>;

            Assert.IsTrue(result.Content.Contains(bet1));
            Assert.AreEqual(1, result.Content.Count);
        }

        [Test]
        public void Test_Get_Returns_Only_Returns_Bets_For_Current_User()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<Bet>>;

            Assert.IsFalse(result.Content.Contains(bet3));
        }

        [Test]
        public void Test_Post_Creates_Bet()
        {
            var result = controller.Post(new Bet { GroupId = 4 });

            bets.Verify(x => x.Add(It.IsAny<Bet>()));            
        }

        [Test]
        public void Test_Post_Rejects_Unauthorised()
        {
            identity = new GenericIdentity("user2");
            Claim claim = new Claim("userId", "-1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            Assert.Throws<HttpResponseException>(() => controller.Post(new Bet { GroupId = 4 }));
        }
    }
}
