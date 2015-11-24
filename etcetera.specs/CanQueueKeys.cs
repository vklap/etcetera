using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [Ignore]
    [TestFixture]
    public class CanQueueKeys :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public CanQueueKeys()
        {
            _response = Client.Queue(AKey, "wassup");
        }

        [Test]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("create");
        }

        [Test]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Test]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey + "/" + _response.Node.CreatedIndex);
        }

        [Test]
        public void TtlIsNotSet()
        {
            _response.Node.Ttl.HasValue.ShouldBeFalse();
        }
    }
}