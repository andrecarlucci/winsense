using System;
using System.Configuration;
using System.Linq;
using Sense.Storage;

namespace Sense.Bob {
    public static class StringExtensions {
        public static bool HasWordsInSequence(this string sentence, params string[] required) {
            string[] sequence = sentence.ToLower().Split(' ');
            int j = 0;
            for (int i = 0; i < sequence.Length; i++) {
                var requiredOptions = required[j].Split('|');
                if (requiredOptions.Contains(sequence[i])) {
                    j++;                
                }
                if (j == required.Length) {
                    return true;
                }
            }
            return false;
        }

        public static string GetWordAfter(this string sentence, string word) {
            var words = sentence.Split(' ');
            for (int i = 0; i < words.Length-1; i++) {
                if (words[i] == word) {
                    return words[i + 1];
                }
            }
            return "";
        }

        public static bool StartsWithOneOf(this string sentence, string words) {
            var wordOptions = words.Trim().Split('|');
            sentence = sentence.Trim();
            foreach (var wordOption in wordOptions) {
                if (sentence.StartsWith(wordOption)) {
                    return true;
                }
            }
            return false;
        }

        public static string GetAllAfter(this string sentence, string word) {
            int index = sentence.IndexOf(word);
            if (index < 0 | index + word.Length > sentence.Length) {
                return "";
            } 
            return sentence.Substring(index + word.Length + 1);
        }
        //Go-go

        public static string AppendUser(this string sentence) {
            var user = WinSenseConfig.GetUser().Name;
            if(!String.IsNullOrEmpty(user)) {
                return sentence + " " + user;
            }
            return sentence;
        }
    }
}