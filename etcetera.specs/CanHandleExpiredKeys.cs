using NUnit.Framework;

namespace etcetera.specs
{
    using System.Threading;
    using Should;

    [TestFixture]
    public class CanHandleExpiredKeys :
        EtcdBase
    {
        const int _ttl = 1;

        public CanHandleExpiredKeys()
        {
            Client.Set(AKey, "wassup", _ttl);
        }

        [Test]
        public void ActionIsSet()
        {
            Thread.Sleep(2000);
            var response = Client.Get(AKey);

            response.ErrorCode.ShouldEqual(100);
        }


    }
}