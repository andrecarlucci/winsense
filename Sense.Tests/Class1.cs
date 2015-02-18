using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sense.Bob;

namespace Sense.Tests {
    public class StringExtensionsTests {

        [Test]
        public void Test() {
            Assert.IsTrue("please open word".HasWordsInSequence("open","word"));
            Assert.IsTrue("please open word".HasWordsInSequence("open","word|world"));
            Assert.IsTrue("I'm fine".HasWordsInSequence("i'm","fine"));
            Assert.IsFalse("please open word".HasWordsInSequence("word|world","open"));
            Assert.IsFalse("please open word".HasWordsInSequence("foo"));

        }
    }
}
