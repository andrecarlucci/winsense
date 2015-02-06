using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MrWindows;
using Sense.Behaviors;
using Sense.Services;
using SharpSenses;

namespace Sense.Profiles {
    public class ProfileManager {
        private readonly ICamera _camera;
        private readonly Windows _windows;
        private readonly ProcessMonitor _processMonitor;

        public event Action<Profile> ProfileChanged;
        public Profile Current;

        public ProfileManager(ICamera camera, Windows windows, ProcessMonitor processMonitor) {
            _camera = camera;
            _windows = windows;
            _processMonitor = processMonitor;
        }

        public void Start() {
            var behaviors = CreateAll<Behavior>(_windows, _camera);
            var profiles = CreateAll<Profile>(behaviors.ToDictionary(b => b.GetType(), b => b));

            _processMonitor.ActiveProcessChanged += processName => {
                var profile = profiles.FirstOrDefault(p => p.Name.ToUpper() == processName.ToUpper());
                if (Current != null) {
                    Current.Deactivate();
                }
                if (profile != null) {
                    Current = profile;
                    Current.Activate();
                }
                OnProfileChanged(Current);
            };
        }

        private static List<T> CreateAll<T>(params object[] pars) {
            var types =
                Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(T)));
            return types.Select(type => ((T)Activator.CreateInstance(type, pars))).ToList();
        }

        private void OnProfileChanged(Profile profile) {
            var handler = ProfileChanged;
            if (handler != null) handler(profile);
        }
    }
}