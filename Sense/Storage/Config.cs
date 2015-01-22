using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Sense.Storage {
    public class Config : IConfig {

        public static IConfig Default = new Config("config.ini");
        
        public string Filename { get; private set; }
        private Dictionary<string, string> _dic = new Dictionary<string, string>();
        private static object _sync = new object();

        public Config(string filename) {
            Filename = filename;
            EnsureDb();
        }

        public string Get(string key, string defaultValue = "") {
            lock (_sync) {
                return !_dic.ContainsKey(key) ? defaultValue : _dic[key];
            }
        }

        public int GetInt(string key, int defaultValue = 0) {
            lock (_sync) {
                return !_dic.ContainsKey(key) ? defaultValue : Convert.ToInt32(_dic[key], CultureInfo.InvariantCulture);
            }
        }

        public void Set(string key, string value) {
            lock (_sync) {
                _dic[key] = value;
                Persist();
            }
        }

        public void Set(string key, int value) {
            lock (_sync) {
                _dic[key] = value.ToString(CultureInfo.InvariantCulture);
                Persist();
            }
        }

        private void EnsureDb() {
            if (!File.Exists(Filename)) {
                File.Create(Filename);
            }
            var lines = File.ReadAllLines(Filename, Encoding.UTF8);
            foreach (var line in lines) {
                var keyValue = line.Split('=');
                if (keyValue.Length != 2) continue;
                _dic[keyValue[0]] = keyValue[1];
            }
        }

        private void Persist() {
            File.WriteAllLines(Filename, _dic.Select(x => x.Key + "=" + x.Value), Encoding.UTF8);
        }
    }
}
