﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    class EtcdGetUserDetailsResponse
    {
        public string user { get; set; }
        public List<string> roles { get; set; }
    }
}
