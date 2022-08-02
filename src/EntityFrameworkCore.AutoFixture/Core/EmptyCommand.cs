using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class EmptyCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
    }
}
