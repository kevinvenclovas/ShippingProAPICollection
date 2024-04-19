using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;

namespace ShippingProAPICollection.RestApi.Helper
{
    public static class TrackingLinkHelper
    {
        public static string CreateTrackingURL(RequestShippingLabelResponse labelReponse, RequestShipmentBase request)
        {
            bool isGermany = request.Country.Equals("DE");

            switch (request)
            {
                case GLSShipmentRequestModel:

                    if (isGermany)
                    {
                        return String.Format("https://gls-group.eu/DE/de/paketverfolgung?match={0}", labelReponse.ParcelNumber);
                    }
                    else
                    {
                        return String.Format("https://www.gls-pakete.de/en/parcel-tracking?match={0}", labelReponse.ParcelNumber);
                    }

                case DHLShipmentRequestModel:

                    if (isGermany)
                    {
                        return String.Format("https://www.dhl.de/de/privatkunden/pakete-empfangen/verfolgen.html?piececode={0}", labelReponse.ParcelNumber);
                    }
                    else
                    {
                        return String.Format("https://nolp.dhl.de/nextt-online-public/set_identcodes.do?lang=en&idc={0}", labelReponse.ParcelNumber);
                    }
                case DPDShipmentRequestModel:

                    var dpdTrackingGroupId = (labelReponse.AdditionalValues?.ContainsKey("REQUEST_ID") ?? false) ? labelReponse.AdditionalValues["REQUEST_ID"].ToString() : null;
                    if (dpdTrackingGroupId == null) return "";

                    if (isGermany)
                    {
                        return String.Format("https://tracking.dpd.de/parcelstatus?query={0}&locale=de_DE", dpdTrackingGroupId);
                    }
                    else
                    {
                        return String.Format("https://tracking.dpd.de/parcelstatus?query={0}&locale=en_DE", dpdTrackingGroupId);
                    }

                default:
                    return "";
            }
        }

    }
}
