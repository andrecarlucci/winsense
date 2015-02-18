using System;

namespace Sense.Bob {
    public class WhatTimeIsIt : ISubject {

        public string GetAnswerFor(string sentence) {
            if (!sentence.HasWordsInSequence("what", "time", "is", "it")) {
                return "";
            }
            var now = DateTime.Now;
            return "It's " + now.Hour + " hours and " + now.Minute + " minutes";
        }

        public void OnOtherSubject() {
            
        }
    }
}