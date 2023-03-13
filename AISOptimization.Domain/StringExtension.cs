namespace AISOptimization.Core;

public static class StringExtension
{
    public static bool ContainsAny(this string str, IEnumerable<string> symbols)
    {
        if (str is not null)
        {
            return symbols.Any(symbol => str.Contains(symbol));
        }

        return false;
    }
}

