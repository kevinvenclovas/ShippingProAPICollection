namespace ShippingProAPICollection.Models.Utils
{
    internal static class StringUtils
    {
        /// <summary>
        /// Is string between min and max length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        internal static bool RangeLenghtValidation(this string? value, int minLength, int maxLength)
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
        internal static bool MaxLenghtValidation(this string? value, int maxLength)
        {

            if (value == null) return true;
            return value.Length <= maxLength;
        }

        /// <summary>
        /// FIll the string to a specified length
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string FillString(this string input, int length, char fillChar)
        {
            if (input.Length >= length)
                return input;

            int fillCount = length - input.Length;
            string filledString = input.PadRight(input.Length + fillCount, fillChar);
            return filledString;
        }
    }
}
