﻿using System;
using Bottles.Services.Messaging;

namespace Bottles.Services.Remote
{
    public class RemoteServicesProxy : MarshalByRefObject
    {
        private BottleServiceRunner _runner;

        public void Start(ServicesToRun services, MarshalByRefObject remoteListener)
        {
            // TODO -- need to handle exceptions gracefully here
            EventAggregator.Start((IRemoteListener) remoteListener);

            var application = new BottleServiceApplication();
            _runner = application.Bootstrap(services.Assemblies);
            _runner.Start();
        }

        // TODO -- send messages to the remote service???

        public void Shutdown()
        {
            EventAggregator.Stop();
            _runner.Stop();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void SendJson(string json)
        {
            EventAggregator.Messaging.SendJson(json);
        }
    }
}