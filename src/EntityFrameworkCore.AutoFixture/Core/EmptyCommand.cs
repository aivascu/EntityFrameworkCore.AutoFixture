using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Does nothing.
/// </summary>
public class EmptyCommand : ISpecimenCommand
{
    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="specimen">The specimen.</param>
    /// <param name="context">The context.</param>
    public void Execute(object specimen, ISpecimenContext context)
    {
    }
}
