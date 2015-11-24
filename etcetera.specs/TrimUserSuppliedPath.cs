using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    public class TrimUserSuppliedPath :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public TrimUserSuppliedPath()
        {
            _response = Client.Set(AKey, "wassup");
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
            _response.Node.Key.ShouldEqual("/"+AKey);
        }
    }
}