﻿using System;
using System.Text.RegularExpressions;
using Bridge.Test;

namespace Bridge.ClientTest.Text.RegularExpressions
{
    [Category(Constants.MODULE_REGEX)]
    [TestFixture(TestNameFormat = "Regex: Lookahead - {0}")]
    public class RegexLookaheadTests : RegexTestBase
    {
        #region MSDN

        [Test]
        public void MsdnPositiveLookaheadTest()
        {
            string[] inputs =
            {
                "The dog is a Malamute.",
                "The island has beautiful birds.",
                "The pitch missed home plate.",
                "Sunday is a weekend day."
            };
            var expected = new[]
            {
                "dog",
                null,
                null,
                "Sunday"
            };

            const string pattern = @"\b\w+(?=\sis\b)";
            var rgx = new Regex(pattern);

            for (int i = 0; i < inputs.Length; i++)
            {
                var m = rgx.Match(inputs[i]);
                if (expected[i] == null)
                {
                    Assert.False(m.Success);
                }
                else
                {
                    Assert.True(m.Success);
                    if (m.Success)
                    {
                        Assert.AreEqual(expected[i], m.Value);
                    }
                }
            }
        }

        [Test]
        public void MsdnNegativeLookaheadTest()
        {
            const string pattern = @"\b(?!un)\w+\b";
            const string text = "unite one unethical ethics use untie ultimate";
            var rgx = new Regex(pattern);
            var ms = rgx.Matches(text);

            Assert.AreEqual(4, ms.Count, "Matches count is correct.");

            // Match #0:
            Assert.NotNull(ms[0], "Match[0] is not null.");
            ValidateMatch(ms[0], 6, 3, "one", 1, true);

            ValidateGroup(ms[0], 0, 6, 3, true, "one", 1);
            ValidateCapture(ms[0], 0, 0, 6, 3, "one");

            // Match #1:
            Assert.NotNull(ms[1], "Match[1] is not null.");
            ValidateMatch(ms[1], 20, 6, "ethics", 1, true);

            ValidateGroup(ms[1], 0, 20, 6, true, "ethics", 1);
            ValidateCapture(ms[1], 0, 0, 20, 6, "ethics");

            // Match #2:
            Assert.NotNull(ms[2], "Match[2] is not null.");
            ValidateMatch(ms[2], 27, 3, "use", 1, true);

            ValidateGroup(ms[2], 0, 27, 3, true, "use", 1);
            ValidateCapture(ms[2], 0, 0, 27, 3, "use");

            // Match #3:
            Assert.NotNull(ms[3], "Match[3] is not null.");
            ValidateMatch(ms[3], 37, 8, "ultimate", 1, true);

            ValidateGroup(ms[3], 0, 37, 8, true, "ultimate", 1);
            ValidateCapture(ms[3], 0, 0, 37, 8, "ultimate");
        }

        [Test]
        public void MsdnPositiveLookbehindTest()
        {
            const string pattern = @"(?<=\b20)\d{2}\b";
            const string text = "2010 1999 1861 2140 2009";
            var rgx = new Regex(pattern);
            var ms = rgx.Matches(text);

            Assert.AreEqual(2, ms.Count, "Matches count is correct.");

            // Match #0:
            Assert.NotNull(ms[0], "Match[0] is not null.");
            ValidateMatch(ms[0], 2, 2, "10", 1, true);

            ValidateGroup(ms[0], 0, 2, 2, true, "10", 1);
            ValidateCapture(ms[0], 0, 0, 2, 2, "10");

            // Match #1:
            Assert.NotNull(ms[1], "Match[1] is not null.");
            ValidateMatch(ms[1], 22, 2, "09", 1, true);

            ValidateGroup(ms[1], 0, 22, 2, true, "09", 1);
            ValidateCapture(ms[1], 0, 0, 22, 2, "09");
        }

        [Test]
        public void MsdnNegativeLookbehindTest()
        {
            var inputs = new[]
            {
                "Monday February 1, 2010",
                "Wednesday February 3, 2010",
                "Saturday February 6, 2010",
                "Sunday February 7, 2010",
                "Monday, February 8, 2010"
            };
            var expected = new[]
            {
                "February 1, 2010",
                "February 3, 2010",
                null,
                null,
                "February 8, 2010"
            };

            const string pattern = @"(?<!(Saturday|Sunday) )\b\w+ \d{1,2}, \d{4}\b";
            var rgx = new Regex(pattern);

            for (int i = 0; i < inputs.Length; i++)
            {
                var m = rgx.Match(inputs[i]);
                if (expected[i] == null)
                {
                    Assert.False(m.Success);
                }
                else
                {
                    Assert.True(m.Success);
                    if (m.Success)
                    {
                        Assert.AreEqual(expected[i], m.Value);
                    }
                }
            }
        }

        #endregion

        [Test]
        public void PositiveLookaheadTest1()
        {
            const string pattern = @"abc(?=def)de";
            const string text = "abcde";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 0, "", 1, false);

            ValidateGroup(m, 0, 0, 0, false, "", 0);
        }

        [Test]
        public void PositiveLookaheadTest2()
        {
            const string pattern = @"abc(?=de)def";
            const string text = "abcdef";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 6, "abcdef", 1, true);

            ValidateGroup(m, 0, 0, 6, true, "abcdef", 1);
            ValidateCapture(m, 0, 0, 0, 6, "abcdef");
        }

        [Test]
        public void NegativeLookaheadTest1()
        {
            const string pattern = @"ab(?![\\d\\D])";
            const string text = "ab";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 2, "ab", 1, true);

            ValidateGroup(m, 0, 0, 2, true, "ab", 1);
            ValidateCapture(m, 0, 0, 0, 2, "ab");
        }

        [Test]
        public void NegativeLookaheadTest2()
        {
            const string pattern = @"ab(?!\D)";
            const string text = "abc";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 0, "", 1, false);

            ValidateGroup(m, 0, 0, 0, false, "", 0);
        }

        [Test]
        public void PositiveLookaheadWithGroupTest()
        {
            const string pattern = @"(ab)(?=(d)e)(def)";
            const string text = "abdef";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 5, "abdef", 4, true);

            ValidateGroup(m, 0, 0, 5, true, "abdef", 1);
            ValidateCapture(m, 0, 0, 0, 5, "abdef");

            ValidateGroup(m, 1, 0, 2, true, "ab", 1);
            ValidateCapture(m, 1, 0, 0, 2, "ab");

            ValidateGroup(m, 2, 2, 1, true, "d", 1);
            ValidateCapture(m, 2, 0, 2, 1, "d");

            ValidateGroup(m, 3, 2, 3, true, "def", 1);
            ValidateCapture(m, 3, 0, 2, 3, "def");
        }

        [Test]
        public void NegativeLookaheadWithGroupTest()
        {
            const string pattern = @"(abc)(?!(d)x)(def)";
            const string text = "abcdef";
            var rgx = new Regex(pattern);
            var m = rgx.Match(text);

            ValidateMatch(m, 0, 6, "abcdef", 4, true);

            ValidateGroup(m, 0, 0, 6, true, "abcdef", 1);
            ValidateCapture(m, 0, 0, 0, 6, "abcdef");

            ValidateGroup(m, 1, 0, 3, true, "abc", 1);
            ValidateCapture(m, 1, 0, 0, 3, "abc");

            ValidateGroup(m, 2, 0, 0, false, "", 0);

            ValidateGroup(m, 3, 3, 3, true, "def", 1);
            ValidateCapture(m, 3, 0, 3, 3, "def");
        }
    }
}