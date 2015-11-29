using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using etcetera.Auth;
using NUnit.Framework;
using Should;

namespace etcetera.specs
{
    [TestFixture]
    public class CanManageUsers : AuthBase
    {
        private string ROLE_1 = string.Format("role1-{0}", Guid.NewGuid()); 
        private string ROLE_2 = string.Format("role2-{0}", Guid.NewGuid());

        private string USERNAME_1 = string.Format("username1-{0}", Guid.NewGuid());
        private string USERNAME_2 = string.Format("username2-{0}", Guid.NewGuid());

        [SetUp]
        public void SetUp()
        {
            AuthModule.EnableAuth();

            AuthModule.SetRole(new EtcdSetRoleRequest {role = ROLE_1});
            AuthModule.SetRole(new EtcdSetRoleRequest {role = ROLE_2});

            AuthModule.SetUser(new EtcdSetUserRequest 
            {
                user = USERNAME_1,
                password = "123456",
                roles = new List<string> { ROLE_1 }
            });

            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_2,
                password = "123456",
                roles = new List<string> { ROLE_2 }
            });
        }

        [TearDown]
        public void TearDown()
        {
            AuthModule.RemoveRole(ROLE_1);
            AuthModule.RemoveRole(ROLE_2);

            AuthModule.RemoveUser(USERNAME_1);
            AuthModule.RemoveUser(USERNAME_2);
        }

        [Test]
        public void ShouldGetUsers()
        {
            var result = AuthModule.GetUsers();

            result.users.Count.ShouldBeGreaterThan(2);
            result.users.FirstOrDefault(username => username == USERNAME_1).ShouldNotBeNull();
            result.users.FirstOrDefault(username => username == USERNAME_2).ShouldNotBeNull();
        }

        [Test]
        public void ShouldGetUserDetails()
        {
            var user1 = AuthModule.GetUser(USERNAME_1);
            var user2 = AuthModule.GetUser(USERNAME_2);

            user1.roles.ShouldContain(ROLE_1);
            user2.roles.ShouldContain(ROLE_2);
        }

        [Test]
        public void ShouldGrantRole()
        {
            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_1,
                grant = new List<string> {ROLE_2}
            });

            var userDetails = AuthModule.GetUser(USERNAME_1);

            userDetails.roles.Count.ShouldEqual(2);
            userDetails.roles.ShouldContain(ROLE_1);
            userDetails.roles.ShouldContain(ROLE_2);
        }

        [Test]
        public void ShouldRevokeRole()
        {
            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_1,
                revoke = new List<string> { ROLE_1 }
            });

            var userDetails = AuthModule.GetUser(USERNAME_1);

            userDetails.roles.Count.ShouldEqual(0);
        }
    }
}
