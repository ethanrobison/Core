using System.Text;

public static class StringBuilderExtensions
{
    public static string ToStringAndClear(this StringBuilder sb) {
        var result = sb.ToString();
        sb.Clear();
        return result;
    }
}
