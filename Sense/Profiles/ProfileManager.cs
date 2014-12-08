using System;
using System.Collections.Generic;
using System.Linq;
using MrWindows;
using SharpSenses;

namespace Sense.Profiles {
    public class ProfileManager {
        private static object _sync = new object();

        public static List<Profile> ActiveProfiles = new List<Profile>();
        public static event Action<List<Profile>> ProfileChanged;

        protected static void OnProfileChanged() {
            Action<List<Profile>> handler = ProfileChanged;
            if (handler != null) handler(new List<Profile>(ActiveProfiles));  
        }

        public static bool IsActive(string profileName) {
            lock (_sync) {
                return ActiveProfiles.Any(x => x.Name == profileName);
            }
        }

        public static bool IsEmpty() {
            lock (_sync) {
                return !ActiveProfiles.Any();
            }
        }

        public static bool IsActive(Profile profile) {
            return IsActive(profile.Name);
        }

        public static void Activate(params Profile[] profiles) {
            lock (_sync) {
                foreach (var p in ActiveProfiles) {
                    p.Deactivate();
                }
                ActiveProfiles.Clear();
                ActiveProfiles.AddRange(profiles);
                OnProfileChanged();    
            }
        }

        public static void Deactivate(Profile profile) {
            lock (_sync) {
                if (ActiveProfiles.Contains(profile)) {
                    ActiveProfiles.Remove(profile);
                    profile.Deactivate();
                    OnProfileChanged();
                }
            }
        }

        public static void EnableProfile<T>() where T : Profile {
            var pars = new object[] {
                App.Container.GetInstance<Windows>(), 
                App.Container.GetInstance<ICamera>(),
                App.Container.GetInstance<ProcessMonitor>()
            };
            var profile = (Profile) Activator.CreateInstance(typeof (T), pars);
            profile.Config();
        }
    }
}