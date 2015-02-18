using System;
using System.Diagnostics;

namespace Sense.Util {
    public static class Try {
        public static void Action(Action action) {
            try {
                action.Invoke();
            }
            catch (Exception ex) {
                Debug.WriteLine("Exception invoking action: " + action + ". Ex: " + ex);
            }
        }
    }
}