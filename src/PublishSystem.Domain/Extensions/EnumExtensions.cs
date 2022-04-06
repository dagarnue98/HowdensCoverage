using System.ComponentModel;

namespace PublishSystem.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute descriptionAttribute = value.GetType()
                                                             .GetField(value.ToString())
                                                             .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)
                                                             .FirstOrDefault() as DescriptionAttribute;
            if (descriptionAttribute == null)
            {
                return value.ToString();
            }

            return descriptionAttribute.Description;
        }
    }
}
