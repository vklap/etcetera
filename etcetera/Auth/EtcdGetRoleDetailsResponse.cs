using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdGetRoleDetailsResponse
    {
        public string role { get; set; }

        public EtcdPermissions permissions { get; set; }
    }
}
