using System;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Implements argument validation checks.
/// </summary>
public static class Check
{
    /// <summary>
    /// Validates if <paramref name="value" /> is of type <typeparamref name="T" />.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <typeparam name="T">The target type.</typeparam>
    /// <returns>
    /// Returns an instance of type <typeparamref name="T" /> if <paramref name="value" /> matches the target type.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value" /> is not of type <typeparamref name="T" />.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value" /> is <see langword="null"/>.
    /// </exception>
    public static T IsOfType<T>(object value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        if (value is not T result)
        {
            throw new ArgumentException(
                $"Argument expected to be of type \"{typeof(T)}\", but was \"{value.GetType()}\".",
                paramName);
        }

        return result;
    }

    /// <summary>
    /// Validates if <paramref name="value" /> is <see langword="null"/>.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value" /> is <see langword="null"/>.
    /// </exception>
    public static void NotNull(object value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
    }

    /// <summary>
    /// Validates if <paramref name="value" /> is <see langword="null"/> or an empty string.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value" /> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value" /> is an empty string.
    /// </exception>
    public static void NotEmpty(string value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        if (value.Trim().Length == 0)
            throw new ArgumentException("Value cannot be empty.", paramName);
    }
}
