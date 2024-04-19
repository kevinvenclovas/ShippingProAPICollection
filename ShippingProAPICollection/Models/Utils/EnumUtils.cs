namespace ShippingProAPICollection.Models.Utils
{
    internal static class EnumUtils
    {
        /// <summary>
        /// Convert string to enum, if enum not exist, return default enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            try
            {
                T enumValue;
                if (!Enum.TryParse(value, true, out enumValue))
                {
                    return defaultValue;
                }
                return enumValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
