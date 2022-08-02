using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.AutoFixture.Tests;

public static class ReflectionExtensions
{
    public static IEnumerable<Type> FindTypesByName(this Assembly source, string typeName)
    {
        return source.GetExportedTypes().Where(x => x.FullName is not null && x.FullName.EndsWith(typeName));
    }
}
