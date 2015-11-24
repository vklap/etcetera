using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdSetUserRequest
    {
        public string user { get; set; }
        
        public string password { get; set; }

        public List<string> roles { get; set; }

        public EtcdPermissions grant { get; set; }
        
        public EtcdPermissions revoke { get; set; }
    }
}
