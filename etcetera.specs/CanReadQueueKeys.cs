using NUnit.Framework;

namespace etcetera.specs
{
    using System.Linq;
    using Should;
    
    [TestFixture]
    public class CanReadQueueKeys :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public CanReadQueueKeys()
        {
            Client.Queue(AKey, "wassup1");
            Client.Queue(AKey, "wassup2");

            _response = Client.Get(AKey, sorted:true);
        }

        [Test]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("get");
        }

        [Test]
        public void NodeIsDir()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Test]
        public void NodesHas2Values()
        {
            _response.Node.Nodes.Count().ShouldEqual(2);
        }
    }
}