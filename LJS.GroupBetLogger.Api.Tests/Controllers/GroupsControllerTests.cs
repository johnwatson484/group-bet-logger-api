using LJS.GroupBetLogger.Api.Controllers;
using LJS.GroupBetLogger.Api.DAL;
using LJS.GroupBetLogger.Api.Logging;
using LJS.GroupBetLogger.Api.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace LJS.GroupBetLogger.Api.Tests.Controllers
{
    [TestFixture]
    public class GroupsControllerTests
    {
        Mock<GroupBetLoggerContext> context;
        Mock<DbSet<Group>> groups;
        List<Group> groupsData;
        Mock<ILogger> logger;
        GroupsController controller;

        Group group1;
        Group group2;
        Group group3;

        GenericIdentity identity;

        [SetUp]
        public void Setup()
        {
            identity = new GenericIdentity("user1");
            Claim claim = new Claim("userId", "1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            group1 = new Group
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
            };

            group2 = new Group
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

            };

            group3 = new Group
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

            };
            

            groupsData = new List<Group>
            {
                group1,
                group2,
                group3
            };

            groups = new Mock<DbSet<Group>>().SetupData(groupsData);
            context = new Mock<GroupBetLoggerContext>();
            context.Setup(x => x.Groups).Returns(groups.Object);
            logger = new Mock<ILogger>();
            controller = new GroupsController(context.Object, logger.Object);
        }

        [Test]
        public void Test_Get_Returns_All_Groups_If_No_GroupId()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<Group>>;

            Assert.IsTrue(result.Content.Contains(group1));
            Assert.IsTrue(result.Content.Contains(group2));
        }

        [Test]
        public void Test_Get_Returns_Group_If_GroupId()
        {
            var result = controller.Get(1) as OkNegotiatedContentResult<Group>;

            Assert.AreEqual(group1, result.Content);
        }

        [Test]
        public void Test_Get_Returns_Only_Returns_Groups_For_Current_User()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<Group>>;

            Assert.IsFalse(result.Content.Contains(group3));
        }

        [Test]
        public void Test_Post_Creates_Group()
        {
            var result = controller.Post("1");

            groups.Verify(x => x.Add(It.IsAny<Group>()));
        }

        [Test]
        public void Test_Post_Rejects_Unauthorised()
        {
            identity = new GenericIdentity("user2");
            Claim claim = new Claim("userId", "-1");
            identity.AddClaim(claim);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            Assert.Throws<HttpResponseException>(() => controller.Post("1"));
        }
    }
}
