using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanGetKeys :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeys()
        {
            Client.Set(AKey, "wassup");
            _getResponse = Client.Get(AKey);
        }

        [Test]
        public void ActionIsSet()
        {
            _getResponse.Action.ShouldEqual("get");
        }

        [Test]
        public void ValueIsWassup()
        {
            _getResponse.Node.Value.ShouldEqual("wassup");
        }

        [Test]
        public void KeyIsSet()
        {
            _getResponse.Node.Key.ShouldEqual("/" + AKey);
        }

        [Test]
        public void IsNotADir()
        {
            _getResponse.Node.Dir.ShouldBeFalse();
        }
    }
}