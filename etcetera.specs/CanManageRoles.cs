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
    public class CanManageRoles : AuthBase
    {
        private string ROLE_WITH_READ_WRITE_PERMISSIONS = string.Format("owner-{0}", Guid.NewGuid());
        private string ROLE_WITH_WRITE_PERMISSIONS = string.Format("writer-{0}", Guid.NewGuid());
        private string ROLE_WITH_READ_PERMISSIONS = string.Format("reader-{0}", Guid.NewGuid());

        [SetUp]
        public void SetUp()
        {
            AuthModule.EnableAuth();

            var ownerPermissions = new EtcdPermissions();
            ownerPermissions.AddWritePermissions("/fleet/*");
            ownerPermissions.AddReadPermissions("/fleet/*");

            var writerPermissions = new EtcdPermissions();
            writerPermissions.AddWritePermissions("/fleet/*");

            var readerPermissions = new EtcdPermissions();
            readerPermissions.AddReadPermissions("/fleet/*");

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_WRITE_PERMISSIONS,
                permissions = ownerPermissions
            });

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_WRITE_PERMISSIONS,
                permissions = writerPermissions
            });

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_PERMISSIONS,
                permissions = readerPermissions
            });
        }

        [TearDown]
        public void TearDown()
        {
            AuthModule.RemoveRole(ROLE_WITH_READ_WRITE_PERMISSIONS);
            AuthModule.RemoveRole(ROLE_WITH_WRITE_PERMISSIONS);
            AuthModule.RemoveRole(ROLE_WITH_READ_PERMISSIONS);
        }

        [Test]
        public void ShouldReturnRoles()
        {
            var result = AuthModule.GetRoles();

            result.roles.Count.ShouldBeGreaterThanOrEqualTo(3);
            result.roles.ShouldContain(ROLE_WITH_READ_WRITE_PERMISSIONS);
            result.roles.ShouldContain(ROLE_WITH_WRITE_PERMISSIONS);
            result.roles.ShouldContain(ROLE_WITH_READ_PERMISSIONS);
        }

        [Test]
        public void RoleShouldHaveTheRightPermissions()
        {
            var ownerPermissions = AuthModule.GetRoleDetails(ROLE_WITH_READ_WRITE_PERMISSIONS);
            var readerPermissions = AuthModule.GetRoleDetails(ROLE_WITH_READ_PERMISSIONS);
            var writerPermissions = AuthModule.GetRoleDetails(ROLE_WITH_WRITE_PERMISSIONS);

            ownerPermissions.role.ShouldEqual(ROLE_WITH_READ_WRITE_PERMISSIONS);
            ownerPermissions.permissions.kv.write.ShouldContain("/fleet/*");
            ownerPermissions.permissions.kv.read.ShouldContain("/fleet/*");

            readerPermissions.role.ShouldEqual(ROLE_WITH_READ_PERMISSIONS);
            readerPermissions.permissions.kv.read.ShouldContain("/fleet/*");
            readerPermissions.permissions.kv.write.Count.ShouldEqual(0);

            writerPermissions.role.ShouldEqual(ROLE_WITH_WRITE_PERMISSIONS);
            writerPermissions.permissions.kv.write.ShouldContain("/fleet/*");
            writerPermissions.permissions.kv.read.Count.ShouldEqual(0);
        }

        [Test]
        public void ShouldHandleRevokePermissions()
        {
            var permissionsToRevoke = new EtcdPermissions();
            permissionsToRevoke.AddWritePermissions("/fleet/*");
            
            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_WRITE_PERMISSIONS,
                revoke = permissionsToRevoke
            });

            var ownerPermissions = AuthModule.GetRoleDetails(ROLE_WITH_READ_WRITE_PERMISSIONS);

            ownerPermissions.role.ShouldEqual(ROLE_WITH_READ_WRITE_PERMISSIONS);
            ownerPermissions.permissions.kv.read.ShouldContain("/fleet/*");
            ownerPermissions.permissions.kv.write.Count.ShouldEqual(0);
        }

        [Test]
        public void ShouldHandleGrantPermissions()
        {
            var permissionsToGrant = new EtcdPermissions();
            permissionsToGrant.AddWritePermissions("/bar/*");

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_WRITE_PERMISSIONS,
                grant = permissionsToGrant
            });

            var ownerPermissions = AuthModule.GetRoleDetails(ROLE_WITH_READ_WRITE_PERMISSIONS);

            ownerPermissions.role.ShouldEqual(ROLE_WITH_READ_WRITE_PERMISSIONS);
            ownerPermissions.permissions.kv.read.ShouldContain("/fleet/*");
            ownerPermissions.permissions.kv.write.Count.ShouldEqual(2);
            ownerPermissions.permissions.kv.write.ShouldContain("/fleet/*");
            ownerPermissions.permissions.kv.write.ShouldContain("/bar/*");
        }
    }
}
