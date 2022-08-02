using System.Text;
using System.Text.Encodings.Web;

namespace SmsMan.Helpers;

// https://github.com/dotnet/runtime/issues/32606
// https://github.com/dotnet/runtime/issues/18874
internal static class QueryHelpers
{
    public static string AddQueryString(
        string uri,
        IEnumerable<KeyValuePair<string, string?>> queryString)
    {
        if (uri == null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (queryString == null)
        {
            throw new ArgumentNullException(nameof(queryString));
        }

        var anchorIndex = uri.IndexOf('#');
        var uriToBeAppended = uri;
        var anchorText = "";
        // If there is an anchor, then the query string must be inserted before its first occurrence.
        if (anchorIndex != -1)
        {
            anchorText = uri[anchorIndex..];
            uriToBeAppended = uri[..anchorIndex];
        }

        var queryIndex = uriToBeAppended.IndexOf('?');
        var hasQuery = queryIndex != -1;

        var sb = new StringBuilder();
        sb.Append(uriToBeAppended);
        foreach (var (key, value) in queryString)
        {
            if (value == null)
            {
                continue;
            }

            sb.Append(hasQuery ? '&' : '?');
            sb.Append(UrlEncoder.Default.Encode(key));
            sb.Append('=');
            sb.Append(UrlEncoder.Default.Encode(value));
            hasQuery = true;
        }

        sb.Append(anchorText);
        return sb.ToString();
    }
}