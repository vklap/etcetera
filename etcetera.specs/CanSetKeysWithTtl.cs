using NUnit.Framework;

namespace etcetera.specs
{
    using System;
    using Should;

    [TestFixture]
    public class CanSetKeysWithTtl :
        EtcdBase
    {
        readonly EtcdResponse _response;
        readonly int _ttl = 30;
        readonly DateTime _now;

        public CanSetKeysWithTtl()
        {
            _response = Client.Set(AKey, "wassup", _ttl);
            _now = DateTime.Now;
        }

        [Test]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
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
        public void ExpirationIsSet()
        {
            _response.Node.Expiration.ShouldBeGreaterThan(_now.AddSeconds(_ttl).AddSeconds(-1));
            _response.Node.Expiration.ShouldBeLessThan(_now.AddSeconds(_ttl).AddSeconds(1));
        }
    }
}