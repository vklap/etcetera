﻿namespace etcetera.specs
{
    using System;

    public abstract class EtcdBase
    {
        public string AKey = Guid.NewGuid().ToString();
        public string ADirectory = Guid.NewGuid().ToString();

        public readonly Uri EtcdLocation = new Uri("http://etcd:2379/");

        public EtcdClient Client { get; set; }

        protected EtcdBase()
        {
//            Client = new EtcdClient(new Uri("http://127.0.0.1:4001/"));
            Client = new EtcdClient(EtcdLocation);
            Client.SetBasicAuthentication("root", "Skytap2015!");
        }
    }
}