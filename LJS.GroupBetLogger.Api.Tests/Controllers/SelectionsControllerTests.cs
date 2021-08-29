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
    public class SelectionsControllerTests
    {
        Mock<GroupBetLoggerContext> context;
        Mock<DbSet<Selection>> selections;
        Mock<DbSet<Bet>> bets;
        List<Bet> betsData;
        List<Selection> selectionData;
        Mock<ILogger> logger;
        SelectionsController controller;

        Selection selection1;
        Selection selection2;
        Selection selection3;

        Bet bet1;
        Bet bet2;
        Bet bet3;

        GenericIdentity identity;

        [SetUp]
        public void Setup()
        {
            identity = new GenericIdentity("user1");
            Claim claim = new Claim("userId", "1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            selection1 = new Selection
            {
                SelectionId = 1,
                UserId = "1"
            };

            selection2 = new Selection
            {
                SelectionId = 2,
                UserId = "2"
            };

            selection3 = new Selection
            {
                SelectionId = 3,
                UserId = "3"
            };

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
                },
                Selections = new List<Selection>
                {
                    selection1,
                    selection2
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
                },
                Selections = new List<Selection>
                {
                    selection3
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

            selectionData = new List<Selection>
            {
                selection1,
                selection2,
                selection3
            };

            betsData = new List<Bet>
            {
                bet1,
                bet2,
                bet3

            };

            selections = new Mock<DbSet<Selection>>().SetupData(selectionData);
            bets = new Mock<DbSet<Bet>>().SetupData(betsData);
            context = new Mock<GroupBetLoggerContext>();
            context.Setup(x => x.Bets).Returns(bets.Object);
            context.Setup(x => x.Selections).Returns(selections.Object);
            logger = new Mock<ILogger>();
            controller = new SelectionsController(context.Object, logger.Object);
        }

        [Test]
        public void Test_Get_Returns_All_User_Selections_If_No_BetId()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<Selection>>;

            Assert.IsTrue(result.Content.Contains(selection1));
            Assert.IsFalse(result.Content.Contains(selection2));
        }

        [Test]
        public void Test_Get_Returns_All_Selections_For_Bet_If_BetId()
        {
            var result = controller.Get(1) as OkNegotiatedContentResult<List<Selection>>;

            Assert.IsTrue(result.Content.Contains(selection1));
            Assert.IsTrue(result.Content.Contains(selection2));
            Assert.AreEqual(2, result.Content.Count);
        }

        [Test]
        public void Test_Get_Returns_Null_If_No_Selections()
        {
            var result = controller.Get(3) as OkNegotiatedContentResult<List<Selection>>;

            Assert.IsNull(result.Content);
        }

        [Test]
        public void Test_Post_Creates_Selection()
        {
            var result = controller.Post(new Selection { UserId = "1", BetId = 1, Name = "Team" });

            selections.Verify(x => x.Add(It.IsAny<Selection>()));
        }

        [Test]
        public void Test_Post_Rejects_Unauthorised()
        {
            identity = new GenericIdentity("user2");
            Claim claim = new Claim("userId", "-1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            Assert.Throws<HttpResponseException>(() => controller.Post(new Selection { UserId = "1", BetId = 1, Name = "Team" }));
        }
    }
}
