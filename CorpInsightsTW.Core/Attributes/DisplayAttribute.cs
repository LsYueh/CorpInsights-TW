namespace CorpInsightsTW.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DisplayAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}