using System.Collections;
using System.Reflection;

namespace DietiEstate.WebClient.Data.Requests;

public abstract class BaseFilterDto
{
    public virtual string ToQueryString()
    {
        var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var parts = new List<string>();

        foreach (var prop in props)
        {
            var value = prop.GetValue(this);
            if (value == null)
                continue;

            var name = Uri.EscapeDataString(prop.Name);

            if (value is string str)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                parts.Add($"{name}={Uri.EscapeDataString(str)}");
            }
            else if (value is IEnumerable enumerable and not IDictionary)
            {
                parts.AddRange(enumerable.OfType<object>().Select(item => $"{name}={Uri.EscapeDataString(item.ToString()!)}"));
            }
            else
            {
                parts.Add($"{name}={Uri.EscapeDataString(value.ToString()!)}");
            }
        }

        if (parts.Count == 0)
            return string.Empty;

        return "?" + string.Join("&", parts);
    }
}