using System;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore.Common
{
    public interface IOptionsBuilder
    {
        object Build(Type type);
    }

    public interface IOptionsBuilder<TContext> where TContext : DbContext
    {
        DbContextOptions<TContext> Build();
    }
}