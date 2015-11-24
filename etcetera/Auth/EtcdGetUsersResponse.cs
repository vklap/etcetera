using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdGetUsersResponse
    {
        public List<EtcdUser> users { get; set; } 
    }
}
