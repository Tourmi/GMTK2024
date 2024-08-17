using System.Collections.Generic;

public static class Extensions
{
    public static IEnumerable<T> Collect<T>(this IEnumerator<T> enumerator)
    {
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
