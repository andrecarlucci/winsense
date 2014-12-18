using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MrWindows;
using Sense.Behaviors;
using SharpSenses;

namespace Sense.Profiles {
    public static class ProfileManager {

        public static event Action<Profile> ProfileChanged;
        public static Profile Current;

        public static void Init(ICamera camera, Windows windows, ProcessMonitor processMonitor) {
            var behaviors = CreateAll<Behavior>(windows, camera);
            var profiles = CreateAll<Profile>(behaviors.ToDictionary(b => b.GetType(), b => b));

            processMonitor.ActiveProcessChanged += processName => {
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

        private static void OnProfileChanged(Profile profile) {
            var handler = ProfileChanged;
            if (handler != null) handler(profile);
        }
    }
}