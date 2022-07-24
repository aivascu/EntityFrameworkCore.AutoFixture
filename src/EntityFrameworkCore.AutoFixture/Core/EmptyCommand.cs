using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

internal class EmptyCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
    }
}
