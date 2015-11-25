using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;

namespace etcetera.specs
{
    [TestFixture]
    public class CanEnableAndDisableAuth : AuthBase
    {
        [SetUp]
        public void SetUp()
        {
            AuthModule.DisableAuth();   
        }

        [TearDown]
        public void TearDown()
        {
            AuthModule.EnableAuth();
        }

        [Test]
        public void EnableAuth()
        {
            AuthModule.EnableAuth();

            var result = AuthModule.GetAuthStatus();
            result.enabled.ShouldBeTrue();
        }

        [Test]
        public void DisableAuth()
        {
            AuthModule.DisableAuth();

            var result = AuthModule.GetAuthStatus();
            result.enabled.ShouldBeFalse();    
        }
    }
}
