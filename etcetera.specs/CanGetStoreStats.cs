using NUnit.Framework;
using Should;

namespace etcetera.specs
{
    [TestFixture]
    public class CanGetStoreStats : EtcdBase
    {
        [Test]
        public void CanSeeStats()
        {
            var resp = Client.Statistics.Store();
            resp.ShouldNotBeNull();
        }
    }
}