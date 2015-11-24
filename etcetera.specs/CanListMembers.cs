using NUnit.Framework;

namespace etcetera.specs
{
    using Should;

    [TestFixture]
    public class CanListMembers : EtcdBase
    {
        [Test]
        public void CanGetListOfMembers()
        {
            var resp = Client.Members.List();
            resp.Members.ShouldNotBeEmpty();
        }
    }
}