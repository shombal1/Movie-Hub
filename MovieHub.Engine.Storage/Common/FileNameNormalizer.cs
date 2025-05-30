using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieHub.Engine.Storage.Common;

public static class FileNameNormalizer
{
    private static readonly Dictionary<char, string> CyrillicToLatinMap = new()
    {
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"}, {'е', "e"}, {'ё', "e"},
        {'ж', "zh"}, {'з', "z"}, {'и', "i"}, {'й', "i"}, {'к', "k"}, {'л', "l"}, {'м', "m"},
        {'н', "n"}, {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
        {'ф', "f"}, {'х', "h"}, {'ц', "ts"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""},
        {'ы', "y"}, {'ь', ""}, {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
    };

    public static string NormalizeForFileName(string input, string fallback = "file")
    {
        if (string.IsNullOrEmpty(input))
            return fallback;
        
        string result = TransliterateCyrillic(input.ToLower());
        result = RemoveDiacritics(result);
        
        result = Regex.Replace(result, @"[^\p{L}\p{N}\-]", "-"); 
    
        result = Regex.Replace(result, @"\s+", "-");
        result = Regex.Replace(result, @"-+", "-");
        result = result.Trim('-');

        if (string.IsNullOrEmpty(result))
            return fallback;

        return result.ToLowerInvariant();
    }

    private static string TransliterateCyrillic(string text)
    {
        var result = new StringBuilder();

        foreach (char c in text)
        {
            if (CyrillicToLatinMap.TryGetValue(char.ToLower(c), out string latinChar))
            {
                result.Append(latinChar);
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var result = new StringBuilder();

        foreach (char c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                result.Append(c);
            }
        }

        return result.ToString().Normalize(NormalizationForm.FormC);
    }
}