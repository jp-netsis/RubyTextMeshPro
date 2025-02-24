using System.Text;
using jp.netsis.RubyText;

namespace TMPro
{
    public static class RubyTextUtilities
    {
        #region SET_UNEDITED_TEXTS
        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0)
        {
            rubyText.SetUneditedText(sourceText, arg0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, arg2, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        /// <param name="arg3">Forth float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2, float arg3)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, arg2, arg3, 0, 0, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        /// <param name="arg3">Forth float value.</param>
        /// <param name="arg4">Fifth float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, arg2, arg3, arg4, 0, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        /// <param name="arg3">Forth float value.</param>
        /// <param name="arg4">Fifth float value.</param>
        /// <param name="arg5">Sixth float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, arg2, arg3, arg4, arg5, 0, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        /// <param name="arg3">Forth float value.</param>
        /// <param name="arg4">Fifth float value.</param>
        /// <param name="arg5">Sixth float value.</param>
        /// <param name="arg6">Seventh float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5, float arg6)
        {
            rubyText.SetUneditedText(sourceText, arg0, arg1, arg2, arg3, arg4, arg5, arg6, 0);
        }

        /// <summary>
        /// <para>Formatted string containing a pattern and a value representing the text to be rendered.</para>
        /// <para>Ex. TMP_Text.SetText("A = {0}, B = {1:00}, C = {2:000.0}", 10.75f, 10.75f, 10.75f);</para>
        /// <para>Results "A = 10.75, B = 11, C = 010.8."</para>
        /// </summary>
        /// <param name="sourceText">String containing the pattern.</param>
        /// <param name="arg0">First float value.</param>
        /// <param name="arg1">Second float value.</param>
        /// <param name="arg2">Third float value.</param>
        /// <param name="arg3">Forth float value.</param>
        /// <param name="arg4">Fifth float value.</param>
        /// <param name="arg5">Sixth float value.</param>
        /// <param name="arg6">Seventh float value.</param>
        /// <param name="arg7">Eighth float value.</param>
        public static void SetUneditedText(this IRubyText rubyText, string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5, float arg6, float arg7)
        {
            rubyText.uneditedText = sourceText;
            if (rubyText is TMP_Text tmpText)
            {
                tmpText.SetText(tmpText.text,arg0,arg1,arg2,arg3,arg4,arg5,arg6,arg7);
            }
        }

        /// <summary>
        /// Set the text using a StringBuilder object as the source.
        /// </summary>
        /// <description>
        /// Using a StringBuilder instead of concatenating strings prevents memory allocations with temporary objects.
        /// </description>
        /// <param name="sourceText">The StringBuilder object containing the source text.</param>
        public static void SetUneditedText(this IRubyText rubyText, StringBuilder sourceText)
        {
            int srcLength = sourceText == null ? 0 : sourceText.Length;

            rubyText.SetUneditedText(sourceText, 0, srcLength);
        }

        /// <summary>
        /// Set the text using a StringBuilder object and specifying the starting character index and length.
        /// </summary>
        /// <param name="sourceText">The StringBuilder object containing the source text.</param>
        /// <param name="start">The index of the first character to read from in the array.</param>
        /// <param name="length">The number of characters in the array to be read.</param>
        public static void SetUneditedText(this IRubyText rubyText, StringBuilder sourceText, int start, int length)
        {
            rubyText.uneditedText = sourceText.ToString(start, length);
        }

        /// <summary>
        /// Set the text using a char array.
        /// </summary>
        /// <param name="sourceText">Source char array containing the Unicode characters of the text.</param>
        public static void SetUneditedText(this IRubyText rubyText, char[] sourceText)
        {
            int srcLength = sourceText == null ? 0 : sourceText.Length;

            rubyText.SetUneditedCharArray(sourceText, 0, srcLength);
        }

        /// <summary>
        /// Set the text using a char array and specifying the starting character index and length.
        /// </summary>
        /// <param name="sourceText">Source char array containing the Unicode characters of the text.</param>
        /// <param name="start">Index of the first character to read from in the array.</param>
        /// <param name="length">The number of characters in the array to be read.</param>
        public static void SetUneditedText(this IRubyText rubyText, char[] sourceText, int start, int length)
        {
            rubyText.SetUneditedCharArray(sourceText, start, length);
        }

        /// <summary>
        /// Set the text using a char array.
        /// </summary>
        /// <param name="sourceText">Source char array containing the Unicode characters of the text.</param>
        public static void SetUneditedCharArray(this IRubyText rubyText, char[] sourceText)
        {
            int srcLength = sourceText == null ? 0 : sourceText.Length;

            rubyText.SetUneditedCharArray(sourceText, 0, srcLength);
        }
        
        /// <summary>
        /// Set the text using a char array and specifying the starting character index and length.
        /// </summary>
        /// <param name="sourceText">Source char array containing the Unicode characters of the text.</param>
        /// <param name="start">The index of the first character to read from in the array.</param>
        /// <param name="length">The number of characters in the array to be read.</param>
        public static void SetUneditedCharArray(this IRubyText rubyText, char[] sourceText, int start, int length)
        {
            rubyText.uneditedText = new string(sourceText, start, length);
        }

        #endregion
    }
}