using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdPermissions
    {
        public EtcdPermissionsKeyValueContainer kv { get; set; }

        public void AddReadPermissions(string dir)
        {
            EnsureKeyValueExists();

            if (kv.read == null)
            {
                kv.read = new List<string>();
            }

            if (kv.read.Contains(dir))
            {
                return;
            }

            kv.read.Add(dir);
        }

        public void AddWritePermissions(string dir)
        {
            EnsureKeyValueExists();

            if (kv.write == null)
            {
                kv.write = new List<string>();    
            }

            if (kv.write.Contains(dir))
            {
                return;
            }

            kv.write.Add(dir);
        }

        private void EnsureKeyValueExists()
        {
            if (kv == null)
            {
                kv = new EtcdPermissionsKeyValueContainer();
            }
        }
    }
}
