using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public class EtcdPermissions
    {
        public Dictionary<string, List<string>> kv { get; set; }

        public void AddReadPermissions(string dir)
        {
            EnsureKeyValueDictionaryExists();
            if (!kv.ContainsKey("read"))
            {
                kv.Add("read", new List<string>());
            }
            kv["read"].Add(dir);
        }

        public void AddWritePermissions(string dir)
        {
            EnsureKeyValueDictionaryExists();
            if (!kv.ContainsKey("write"))
            {
                kv.Add("write", new List<string>());
            }
            kv["write"].Add(dir);
        }

        private void EnsureKeyValueDictionaryExists()
        {
            if (kv == null)
            {
                kv = new Dictionary<string, List<string>>();
            }
        }
    }
}
