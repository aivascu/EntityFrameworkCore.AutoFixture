using System;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public interface IOptionsBuilder
    {
        object Build(Type type);
    }
}
