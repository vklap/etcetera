using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using etcetera.Auth;
using NUnit.Framework;
using Should;

namespace etcetera.specs
{
    [TestFixture]
    public class CanAuthorizeByRoles : AuthBase
    {
        private const string KEY_1 = "a/keys/key-a";
        private const string KEY_2 = "a/b/keys/key-ab";
        private const string VALUE_1 = "/a/keys/key-a";
        private const string VALUE_2 = "/a/b/keys/key-ab";


        private string USERNAME_OWNER = string.Format("owner-{0}", Guid.NewGuid());
        private string USERNAME_WRITER = string.Format("writer-{0}", Guid.NewGuid());
        private string USERNAME_READER = string.Format("reader-{0}", Guid.NewGuid());

        private string ROLE_WITH_READ_WRITE_PERMISSIONS = string.Format("owner-role-{0}", Guid.NewGuid());
        private string ROLE_WITH_WRITE_PERMISSIONS = string.Format("writer-role-{0}", Guid.NewGuid());
        private string ROLE_WITH_READ_PERMISSIONS = string.Format("reader-role-{0}", Guid.NewGuid());

        private IEtcdClient _owner = null;
        private IEtcdClient _reader = null;
        private IEtcdClient _writer = null;

        [SetUp]
        public void SetUp()
        {
            AuthModule.EnableAuth();
            var rootPermissions = new EtcdPermissions();
            rootPermissions.AddWritePermissions("*");
            rootPermissions.AddReadPermissions("*");    

            CreateRoles();

            CreateUsers();

            InitEtcdClients();

            CreateData();
        }

        [TearDown]
        public void TearDown()
        {
            AuthModule.RemoveUser(USERNAME_OWNER);
            AuthModule.RemoveUser(USERNAME_WRITER);
            AuthModule.RemoveUser(USERNAME_READER);

            AuthModule.RemoveRole(ROLE_WITH_READ_WRITE_PERMISSIONS);
            AuthModule.RemoveRole(ROLE_WITH_READ_PERMISSIONS);
            AuthModule.RemoveRole(ROLE_WITH_WRITE_PERMISSIONS);

            Client.DeleteDir("a", true);
        }

        [Test]
        public void ItShouldReadGivenHasOwnerPermissions()
        {
            var value1 = _owner.Get(KEY_1);
            var value2 = _owner.Get(KEY_2);

            value1.Node.Value.ShouldEqual(VALUE_1);
            value2.Node.Value.ShouldEqual(VALUE_2);
        }

        [Test]
        public void ItShouldReadGivenHasReaderPermissions()
        {
            var value1 = _reader.Get(KEY_1);
            var value2 = _reader.Get(KEY_2);

            value1.Node.Value.ShouldEqual(VALUE_1);
            value2.Node.Value.ShouldEqual(VALUE_2);
        }

        [Test]
        public void ItShouldWriteGivenHasWriterPermissions()
        {
            _writer.Set(KEY_1, "some-other-value");
            _writer.Set(KEY_2, "yet-another-value");

            _reader.Get(KEY_1).Node.Value.ShouldEqual("some-other-value");
            _reader.Get(KEY_2).Node.Value.ShouldEqual("yet-another-value");
        }

        [Test]
        public void ItShouldReadAndWriteGivenHasWriterPermissions()
        {
            _owner.Set(KEY_1, "some-other-value");
            _owner.Set(KEY_2, "yet-another-value");

            _owner.Get(KEY_1).Node.Value.ShouldEqual("some-other-value");
            _owner.Get(KEY_2).Node.Value.ShouldEqual("yet-another-value");
        }

        [Test]
        public void ItShouldFailReadGivenHasNoSufficientPermissions()
        {
            var exKey1 = Assert.Throws<EtceteraException>(() => _writer.Get(KEY_1));
            var exKey2 = Assert.Throws<EtceteraException>(() => _writer.Get(KEY_2));
            
            exKey1.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
            exKey2.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void ItShouldFailWriteGivenHasNoSufficientPermissions()
        {
            var exKey1 = Assert.Throws<EtceteraException>(() => _reader.Set(KEY_1, "some-other-value"));
            var exKey2 = Assert.Throws<EtceteraException>(() => _reader.Set(KEY_2, "yet-another-value"));

            exKey1.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
            exKey2.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

            _reader.Get(KEY_1).Node.Value.ShouldEqual(VALUE_1);
            _reader.Get(KEY_2).Node.Value.ShouldEqual(VALUE_2);
        }

        private void CreateRoles()
        {
            var fullPermissions = new EtcdPermissions();
            fullPermissions.AddWritePermissions("/a/keys/*");
            fullPermissions.AddWritePermissions("/a/b/keys/*");
            fullPermissions.AddReadPermissions("/a/keys/*");
            fullPermissions.AddReadPermissions("/a/b/keys/*");

            var readPermissions = new EtcdPermissions();
            readPermissions.AddReadPermissions("/a/keys/*");
            readPermissions.AddReadPermissions("/a/b/keys/*");

            var writePermissions = new EtcdPermissions();
            writePermissions.AddWritePermissions("/a/keys/*");
            writePermissions.AddWritePermissions("/a/b/keys/*");

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_WRITE_PERMISSIONS,
                permissions = fullPermissions
            });

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_WRITE_PERMISSIONS,
                permissions = writePermissions
            });

            AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = ROLE_WITH_READ_PERMISSIONS,
                permissions = readPermissions
            });
        }

        private void CreateUsers()
        {
            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_OWNER,
                password = "123456",
                roles = new List<string> { ROLE_WITH_READ_WRITE_PERMISSIONS }
            });

            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_WRITER,
                password = "123456",
                roles = new List<string> { ROLE_WITH_WRITE_PERMISSIONS }
            });

            AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = USERNAME_READER,
                password = "123456",
                roles = new List<string> { ROLE_WITH_READ_PERMISSIONS }
            });
        }

        private void InitEtcdClients()
        {
            _owner = new EtcdClient(EtcdLocation);
            _owner.SetBasicAuthentication(USERNAME_OWNER, "123456");
            
            _writer = new EtcdClient(EtcdLocation);
            _writer.SetBasicAuthentication(USERNAME_WRITER, "123456");
            
            _reader = new EtcdClient(EtcdLocation);
            _reader.SetBasicAuthentication(USERNAME_READER, "123456");
        }

        private void CreateData()
        {
            Client.Set(KEY_1, VALUE_1);
            Client.Set(KEY_2, VALUE_2);
        }
    }
}
