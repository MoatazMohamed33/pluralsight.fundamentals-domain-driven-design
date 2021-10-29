namespace NArdalis.GuardClauses
{
    /// <summary>
    /// See <see href="https://github.com/ardalis/GuardClauses/blob/main/src/GuardClauses/Guard.cs">here</see>.
    /// </summary>
    public class Guard : IGuardClass
    {
        public static IGuardClass Against { get; } = new Guard();

        private Guard()
        {
        }
    }
}
