using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [Ignore]
    [TestFixture]
    public class KeysWithSlashes :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public KeysWithSlashes()
        {
            _response = Client.Set("/folder1/bill", "wassup");
        }

        [Test]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Test]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Test]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/folder1/bill");
        }
    }
}