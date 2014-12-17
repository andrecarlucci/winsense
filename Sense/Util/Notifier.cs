using System;

namespace Sense.Util {
    public static class Messenger {

        public static event Action<string> NewMesssage;

        public static void Send(string message) {
            OnNewMesssage(message);    
        }
        private static void OnNewMesssage(string obj) {
            var handler = NewMesssage;
            if (handler != null) handler(obj);
        }
    }
}
