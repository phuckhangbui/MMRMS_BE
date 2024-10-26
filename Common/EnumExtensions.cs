using System.Reflection;

namespace Common
{
    public static class EnumExtensions
    {
        public static string ToVietnamese(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<TranslationAttribute>();

            return attribute?.VietnameseTranslation ?? value.ToString();
        }

        public static string TranslateStatus<TEnum>(string statusString) where TEnum : System.Enum
        {
            if (System.Enum.TryParse(typeof(TEnum), statusString, out var enumValue))
            {
                return ((TEnum)enumValue).ToVietnamese();
            }
            return statusString; // Fallback if the string does not match an enum value
        }
    }
}
