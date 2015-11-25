using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace etcetera.specs
{
    [TestFixture]
    public class CanAuthorizeByRoles : AuthBase
    {
        private string USERNAME_OWNER = string.Format("owner-{0}", Guid.NewGuid());
        private string USERNAME_WRITER = string.Format("writer-{0}", Guid.NewGuid());
        private string USERNAME_READER = string.Format("reader-{0}", Guid.NewGuid());

        private string ROLE_WITH_READ_WRITE_PERMISSIONS = string.Format("owner-role-{0}", Guid.NewGuid());
        private string ROLE_WITH_WRITE_PERMISSIONS = string.Format("writer-role-{0}", Guid.NewGuid());
        private string ROLE_WITH_READ_PERMISSIONS = string.Format("reader-role-{0}", Guid.NewGuid());


        [SetUp]
        public void SetUp()
        {
            AuthModule.EnableAuth();
        }


        [TearDown]
        public void TearDown()
        {
            
        }

        
    }
}
