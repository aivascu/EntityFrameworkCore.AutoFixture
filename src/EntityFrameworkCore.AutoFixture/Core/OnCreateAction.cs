namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Defines which action is going to be invoked
/// on <see cref="Microsoft.EntityFrameworkCore.DbContext"/> after creation.
/// </summary>
public enum OnCreateAction
{
    /// <summary>
    /// Does not perform any action after context is created.
    /// </summary>
    None = 0,

    /// <summary>
    /// Runs the EnsureCreated method on context after it is created.
    /// </summary>
    EnsureCreated = 1,

    /// <summary>
    /// Runs the Migrate method on context after it is created.
    /// </summary>
    Migrate = 2
}
