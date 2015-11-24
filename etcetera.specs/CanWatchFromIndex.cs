using NUnit.Framework;

namespace etcetera.specs
{
    using System.Threading;
    using Should;

    [TestFixture]
    public class CanWatchFromIndex :
        EtcdBase
    {
        ManualResetEvent _wasHit;

        [Test]
        public void ActionIsSet()
        {
            Client.Set(AKey, "wassup");
            var response = Client.Get(AKey);

            _wasHit = new ManualResetEvent(false);

            //Should return immediately
            Client.Watch(AKey, resp => _wasHit.Set(), false, null, response.Node.ModifiedIndex);

            _wasHit.WaitOne(1000).ShouldBeTrue();
        }
    }
}
