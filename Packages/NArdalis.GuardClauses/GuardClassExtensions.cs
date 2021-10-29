using System;

namespace NArdalis.GuardClauses
{
    /// <summary>
    /// See <see href="https://github.com/ardalis/GuardClauses/blob/main/src/GuardClauses/GuardClauseExtensions.cs">here</see>.
    /// </summary>
    public static class GuardClassExtensions
    {
        public static T Null<T>(this IGuardClass guardClass, T input, string parameterName, string? message = null)
        {
            if (input is null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentNullException(parameterName);
                }

                throw new ArgumentNullException(parameterName, message);
            }

            return input;
        }
    }
}
