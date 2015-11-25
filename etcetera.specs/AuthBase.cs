using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using etcetera.Auth;

namespace etcetera.specs
{
    public class AuthBase : EtcdBase
    {
        public IEtcdAuthModule AuthModule { get; set; }

        public AuthBase()
        {
            AuthModule = new EtcdAuthModule(new Uri("http://etcd:2379/"), "root", "Skytap2015!");
        }
    }
}
