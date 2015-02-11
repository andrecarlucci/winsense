using System;
using Sense.Storage;

namespace Sense.Bob {
    public static class StringExtensions {
        public static bool HasWordsInSequence(this string sentence, params string[] required) {
            string[] words = sentence.ToLower().Split(' ');
            int j = 0;
            for (int i = 0; i < words.Length; i++) {
                if (words[i].Equals(required[j])) {
                    j++;
                }
                if (j == required.Length) {
                    return true;
                }
            }
            return false;
        }

        public static string AppendUser(this string sentence) {
            var user = Config.Default.Get(ConfigKeys.Username);
            if(!String.IsNullOrEmpty(user)) {
                return sentence + ", " + user;
            }
            return sentence;
        }
    }
}