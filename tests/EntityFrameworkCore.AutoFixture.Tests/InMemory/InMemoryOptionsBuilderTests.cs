using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory
{
    public class InMemoryOptionsBuilderTests
    {
        private abstract class AbstractDbContext : DbContext
        { }

        private static Type GetOptionsType()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "Microsoft.EntityFrameworkCore.InMemory");

            var extensionType = assembly.GetType("Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal.InMemoryOptionsExtension");
            if (extensionType is not null)
                return extensionType;

            var internalExtensionType = assembly.GetType("Microsoft.EntityFrameworkCore.Infrastructure.Internal.InMemoryOptionsExtension");
            if (internalExtensionType is not null)
                return internalExtensionType;

            throw new InvalidOperationException("Unable to find type \"InMemoryOptionsExtension\" in the EF Core InMemory provider assembly");
        }
    }
}
