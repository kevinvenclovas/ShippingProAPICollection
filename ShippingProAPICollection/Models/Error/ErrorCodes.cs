using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.Models.Error
{
    public enum ErrorCode
    {
        UNKNOW = 9999,
        UNAUTHORIZED,
        TO_MANY_REQUESTS,
        INTERNAL_SERVER_ERROR,
        BAD_REQUEST_ERROR,
        CANNOT_CONVERT_RESPONSE,
        DPD_LOGIN_ERROR,
        DPD_SHIPMENT_REQUEST_ERROR,
    }
}
