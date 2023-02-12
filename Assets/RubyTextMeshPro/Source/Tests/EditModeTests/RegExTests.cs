using System.Text.RegularExpressions;
using NUnit.Framework;
using TMPro;

namespace RubyTextMeshPro.EditModeTests
{
    public class RegExTests
    {
        // A Test behaves as an ordinary method
        [Test]
        [TestCase("<r>ルビなし</r>", "", "ルビなし")]
        [TestCase("<r=>ルビなし</r>", "", "ルビなし")]
        [TestCase("<ruby>ルビなし</r>", "", "ルビなし")]
        [TestCase("<ruby=>ルビなし</r>", "", "ルビなし")]
        [TestCase("<r>ルビなし</ruby>", "", "ルビなし")]
        [TestCase("<r=>ルビなし</ruby>", "", "ルビなし")]
        [TestCase("<ruby>ルビなし</ruby>", "", "ルビなし")]
        [TestCase("<ruby=>ルビなし</ruby>", "", "ルビなし")]
        [TestCase("<r=ルビあり>ルビあり</r>", "ルビあり", "ルビあり")]
        [TestCase("<r=\"ルビあり\">ルビあり</r>", "ルビあり", "ルビあり")]
        [TestCase("<ruby=ルビあり>ルビあり</r>", "ルビあり", "ルビあり")]
        [TestCase("<ruby=\"ルビあり\">ルビあり</r>", "ルビあり", "ルビあり")]
        [TestCase("<r=ルビあり>ルビあり</ruby>", "ルビあり", "ルビあり")]
        [TestCase("<r=\"ルビあり\">ルビあり</ruby>", "ルビあり", "ルビあり")]
        [TestCase("<ruby=ルビあり>ルビあり</ruby>", "ルビあり", "ルビあり")]
        [TestCase("<ruby=\"ルビあり\">ルビあり</ruby>", "ルビあり", "ルビあり")]
        public void RegExTestsSimplePasses(string testText, string resultRuby, string resultBaseText)
        {
            MatchCollection matches = RubyTextMeshProDefinitions.RUBY_REGEX.Matches(testText);

            foreach (Match match in matches)
            {
                string rubyText = match.Groups["ruby"].ToString();
                string baseText = match.Groups["val"].ToString();
                Assert.IsTrue(rubyText == resultRuby && resultBaseText == baseText);
            }
        }
    }
}