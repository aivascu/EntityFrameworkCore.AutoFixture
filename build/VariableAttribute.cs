using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Nuke.Common.ValueInjection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class VariableAttribute : ValueInjectionAttributeBase
{
    readonly string Name;

    public VariableAttribute()
        : this(null)
    {
    }

    public VariableAttribute([AllowNull] string name)
    {
        this.Name = name;
    }

    public override object GetValue(MemberInfo member, object instance)
    {
        var variableName = this.Name ?? member.Name;
        return Environment.GetEnvironmentVariable(variableName);
    }
}