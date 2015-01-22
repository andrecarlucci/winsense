﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sense.Storage;
using SharpSenses;

namespace Sense.Lockscreen {
    public class Unlocker {
        private readonly RealSenseCredentialPluginClient _client;
        private readonly ICamera _camera;

        public Unlocker(RealSenseCredentialPluginClient client, ICamera camera) {
            _client = client;
            _camera = camera;
        }

        public void Start() {
            _client.Start();

            Task.Run(async () => {
                while (true) {
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                    int currentUser = _camera.Face.UserId;
                    int registeredUser = Config.Default.GetInt(ConfigKeys.UserId);
                    if (currentUser == registeredUser) {
                        _client.Authorize().Wait();
                        Debug.WriteLine(currentUser + " " + registeredUser);
                    }
                }
            });

            
        }
    }
}