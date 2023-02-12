using System.Text.RegularExpressions;

namespace TMPro
{
    public static class RubyTextMeshProDefinitions
    {
        public enum RubyShowType
        {
            RUBY_ALIGNMENT,
            BASE_ALIGNMENT
        }

        // ruby tag
        public static readonly Regex RUBY_REGEX = new Regex(@"<r(uby)?=""?(?<ruby>[\s\S]*?)""?>(?<val>[\s\S]*?)<\/r(uby)?>");
    }
}