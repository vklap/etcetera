using NUnit.Framework;

namespace etcetera.specs
{
    using System.Threading;
    using Should;

    [Ignore]
    [TestFixture]
    public class CanWatchKeysRecursively :
        EtcdBase
    {
        ManualResetEvent _wasHit;

        [Test]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            Client.Set("bob/" + AKey, "wassup");

            Client.Watch("bob", resp =>
            {
                _wasHit.Set();
            }, true);

            Client.Set("bob/" + AKey, "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }
    }
}