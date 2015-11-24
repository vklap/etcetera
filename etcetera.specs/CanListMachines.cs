using NUnit.Framework;
using Should;

namespace etcetera.specs
{
    [Ignore]
    [TestFixture]
    public class CanListMachines : EtcdBase
    {
        [Test]
        public void CanGetAListOfMachines()
        {
            var resp = Client.Machine.List();
            resp.ShouldNotBeEmpty();
        }
    }
}