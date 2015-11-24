using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanDeleteKeys :
        EtcdBase
    {
        readonly EtcdResponse _deleteResponse;

        public CanDeleteKeys()
        {
            Client.Set(AKey, "wassup");
            _deleteResponse = Client.Delete(AKey);
        }

        [Test]
        public void ActionIsSet()
        {
            _deleteResponse.Action.ShouldEqual("delete");
        }

        [Test]
        public void ValueIsWassup()
        {
            _deleteResponse.Node.Value.ShouldBeNull();
        }

        [Test]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + AKey);
        }
    }
}