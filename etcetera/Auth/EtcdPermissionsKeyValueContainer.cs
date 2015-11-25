using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdPermissionsKeyValueContainer
    {
        public List<string> read { get; set; }
        
        public List<string> write { get; set; }
    }
}
