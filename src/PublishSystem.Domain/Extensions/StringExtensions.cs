using PublishSystem.Domain.Enums.StateManagement;

namespace PublishSystem.Domain.Extensions
{
    public static class StringExtensions
    {
        public static State ToEnum(this string stateString)
        {
            return Enum.Parse<State>(stateString);
        }
    }
}
