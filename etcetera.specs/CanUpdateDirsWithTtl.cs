using NUnit.Framework;

namespace etcetera.specs
{
    using System;
    using Should;

    [TestFixture]
    public class CanUpdateDirsWithTtl :
        EtcdBase
    {
        EtcdResponse _response;
        int _ttl = 30;
        DateTime _now;

        public CanUpdateDirsWithTtl()
        {
            Client.CreateDir(AKey);
            _response = Client.CreateDir(AKey, _ttl, true);
            _now = DateTime.Now;
        }

        [Test]
        public void ActionIsUpdate()
        {
            _response.Action.ShouldEqual("update");
        }

        [Test]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey);
        }

        [Test]
        public void TtlIsSet()
        {
            _response.Node.Ttl.ShouldEqual(_ttl);
        }

        [Test]
        public void IsDir()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Test]
        public void ExpirationIsSet()
        {
            _response.Node.Expiration.ShouldBeGreaterThan(_now.AddSeconds(_ttl).AddSeconds(-1));
            _response.Node.Expiration.ShouldBeLessThan(_now.AddSeconds(_ttl).AddSeconds(1));
        }

        [Test]
        public void PrevKeyIsSet()
        {
            _response.PrevNode.Key.ShouldEqual("/" + AKey);
        }

        [Test]
        public void PrevTtlIsNotSet()
        {
            _response.PrevNode.Ttl.ShouldBeNull();
        }

        [Test]
        public void PrevIsDir()
        {
            _response.PrevNode.Dir.ShouldBeTrue();
        }

        [Test]
        public void PrevExpirationIsNotSet()
        {
            _response.PrevNode.Expiration.ShouldBeNull();
        }
    }
}
