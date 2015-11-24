﻿using System.Diagnostics;
using NUnit.Framework;

namespace etcetera.specs
{
    using System.Threading;
    using Should;

    [Ignore]
    [TestFixture]
    public class CanWatchKeys :
        EtcdBase
    {
        ManualResetEvent _wasHit;

        [Test]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            Client.Set(AKey, "wassup");

            Client.Watch(AKey, resp =>
            {
                _wasHit.Set();
            });

            Client.Set(AKey, "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }

        [Test]
        public void WhenWatchTimesOut()
        {
            _wasHit = new ManualResetEvent(false);
            Client.Set(AKey, "wassup");

            var watch = new Stopwatch();
            watch.Start();
            Client.Watch(AKey, resp =>
            {
                watch.Stop();
                _wasHit.Set();
            }, timeout: 1);

            _wasHit.WaitOne(2000).ShouldBeTrue();
            watch.ElapsedMilliseconds.ShouldBeLessThan(2000);
        }
    }
}