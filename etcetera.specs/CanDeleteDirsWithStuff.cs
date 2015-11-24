using NUnit.Framework;

namespace etcetera.specs
{
    using System;
    using Should;

    [TestFixture]
    public class CanDeleteDirsWithStuff :
        EtcdBase
    {
        EtcdResponse _deleteResponse;

        public CanDeleteDirsWithStuff()
        {
            Client.Set(AKey +"/bob", "hi");
            _deleteResponse = Client.DeleteDir(AKey, true);
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