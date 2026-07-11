using System;

namespace CorpInsightsTW.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class CodeAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}