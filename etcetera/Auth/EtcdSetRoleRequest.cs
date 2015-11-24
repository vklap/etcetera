using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdSetRoleRequest
    {
        public string role { get; set; }

        public EtcdPermissions permissions { get; set; }

        public EtcdPermissions grant { get; set; }
        
        public EtcdPermissions revoke { get; set; }
    }
}
