using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanGetKeysRecursively :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeysRecursively()
        {
            Client.Set(ADirectory + "/" +AKey, "wassup");
            Client.Set(ADirectory + "/" +AKey + "-2", "not-much");
            _getResponse = Client.Get(ADirectory, recursive:true);
        }

        [Test]
        public void ActionIsGet()
        {
            _getResponse.Action.ShouldEqual("get");
        }

        [Test]
        public void ValueIsNotPresent()
        {
            _getResponse.Node.Value.ShouldEqual(null);
        }

        [Test]
        public void ReturnsChildren()
        {
            _getResponse.Node.Nodes.Count.ShouldEqual(2);
        }

        [Test]
        public void KeyIsSet()
        {
            _getResponse.Node.Key.ShouldEqual("/" + ADirectory);
        }

        [Test]
        public void IsADir()
        {
            _getResponse.Node.Dir.ShouldBeTrue();
        }
    }
}