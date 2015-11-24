using System.Reflection;
using NUnit.Framework;
using RestSharp;
using Should;

namespace etcetera.specs
{
    [Ignore]
    [TestFixture]
    public class CanProvideBasicAuthCredentials : EtcdBase
    {
        [Test]
        public void CanSetUserNameAndPassword()
        {
            GetAuthenticator().ShouldBeNull();
            Client.SetBasicAuthentication("u","p");
            GetAuthenticator().ShouldNotBeNull();
            GetAuthenticator().ShouldBeType<HttpBasicAuthenticator>();
        }

        object GetAuthenticator()
        {
            var cf = typeof(EtcdClient).GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
            var c =  cf.GetValue(Client);
            var af = typeof (RestClient).GetProperty("Authenticator", BindingFlags.Instance | BindingFlags.Public);
            return af.GetValue(c);
        }
    }
}