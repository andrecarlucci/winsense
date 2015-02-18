using System;
using System.IO;
using Sense.Models;

namespace Sense.Storage {
    public static class WinSenseConfig {

        private static object _syncRoot = new object();
        private static string _filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "WinSenseConfig.txt";

        public static User GetUser() {
            if (File.Exists(_filename)) {
                lock (_syncRoot) {
                    return User.Deserialize(File.ReadAllText(_filename));
                }
            }
            return new User();
        }

        public static void SetUser(User user) {
            lock (_syncRoot) {
                File.WriteAllText(_filename, user.Serialize());            
            }
        }
    }
}