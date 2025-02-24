using System.Globalization;
using ShippingProAPICollection.Provider.DHL.Entities;

namespace ShippingProAPICollection.Provider.DHL
{
    internal static class ThreeLetterCountryCodeHelper
    {
        static Dictionary<string, string> _countryCodeMatches = new Dictionary<string, string>();

        internal static string GetThreeLetterCountryCode(string isaoA2code)
        {
            if (_countryCodeMatches.Count == 0) buildCountryMatches();
            if (!_countryCodeMatches.ContainsKey(isaoA2code)) throw new ThreeLetterCountryCodeResolvingException($"Cannot resolve three letter country code from two letter country code {isaoA2code}");

            return _countryCodeMatches[isaoA2code];
        }

        internal static void buildCountryMatches()
        {
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo regionInfo;
                try
                {
                    regionInfo = new RegionInfo(cultureInfo.Name);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (!_countryCodeMatches.ContainsKey(regionInfo.TwoLetterISORegionName))
                {
                    _countryCodeMatches.Add(regionInfo.TwoLetterISORegionName, regionInfo.ThreeLetterISORegionName);
                }

            }
        }
    }
}
