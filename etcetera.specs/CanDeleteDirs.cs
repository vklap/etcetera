using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanDeleteDirs :
        EtcdBase
    {
        readonly EtcdResponse _deleteResponse;

        public CanDeleteDirs()
        {
            Client.CreateDir(AKey);
            _deleteResponse = Client.DeleteDir(AKey);
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
        public void IsDir()
        {
            _deleteResponse.Node.Dir.ShouldBeTrue();
        }

        [Test]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + AKey);
        }
    }
}