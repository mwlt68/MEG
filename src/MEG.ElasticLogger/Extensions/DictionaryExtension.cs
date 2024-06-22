using System.Text;
using Microsoft.Extensions.Primitives;

namespace MEG.ElasticLogger.Extensions;

public static class DictionaryExtension
{
    public static string DictionaryToString(this Dictionary<string, StringValues> dictionary)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var kvp in dictionary)
        {
            var values = kvp.Value.Where(x => x is not null);
            string valuesText = string.Join(",", values);

            sb.AppendLine($"{kvp.Key},{valuesText}");
        }

        return sb.ToString();
    }

    public static string DictionaryToString(this Dictionary<string, string> dictionary)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var kvp in dictionary)
        {
            sb.AppendLine($"{kvp.Key},{kvp.Value}");
        }

        return sb.ToString();
    }
}