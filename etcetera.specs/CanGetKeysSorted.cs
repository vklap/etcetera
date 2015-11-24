using NUnit.Framework;

namespace etcetera.specs
{
    using System.Linq;
    using Should;

    [TestFixture]
    public class CanGetKeysSorted :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeysSorted()
        {
            Client.Set(ADirectory + "/" +AKey, "wassup");
            Client.Set(ADirectory + "/" +AKey + "-2", "not-much");
            _getResponse = Client.Get(ADirectory, sorted:true);
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
        public void ReturnsChildrenSorted()
        {
            var a = _getResponse.Node.Nodes.First();
            var b = _getResponse.Node.Nodes.Last();
            a.CreatedIndex.ShouldBeLessThan(b.CreatedIndex);
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