namespace ShippingProAPICollection.Models.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Is string between min and max length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool RangeLenghtValidation(this string? value, int minLength, int maxLength)
        {
            if (value == null)
            {
                if (minLength == 0) return true;
                return false;
            }

            return value.Trim().Length >= minLength && value.Trim().Length <= maxLength;
        }

        /// <summary>
        /// Is string size not greater than max length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool MaxLenghtValidation(this string? value, int maxLength)
        {

            if (value == null) return true;
            return value.Length <= maxLength;
        }
    }
}
