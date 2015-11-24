using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanCreateDirs :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public CanCreateDirs()
        {
            _response = Client.CreateDir(AKey);
        }

        [Test]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Test]
        public void NodeIsDirectory()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Test]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey);
        }

        [Test]
        public void NoValue()
        {
            _response.Node.Value.ShouldBeNull();
        }
    }
}