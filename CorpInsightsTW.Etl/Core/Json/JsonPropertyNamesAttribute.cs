using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonPropertyNamesAttribute : JsonAttribute
{
    public string[] Names { get; }

    public JsonPropertyNamesAttribute(string firstName, params string[] additionalNames)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("At least one name is required.", nameof(firstName));

        Names = [firstName, .. additionalNames];
    }
}
