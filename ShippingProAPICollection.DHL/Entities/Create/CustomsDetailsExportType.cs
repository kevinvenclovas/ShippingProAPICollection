using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public enum CustomsDetailsExportType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"OTHER")]
        OTHER = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"PRESENT")]
        PRESENT = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"COMMERCIAL_SAMPLE")]
        COMMERCIAL_SAMPLE = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"DOCUMENT")]
        DOCUMENT = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"RETURN_OF_GOODS")]
        RETURN_OF_GOODS = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"COMMERCIAL_GOODS")]
        COMMERCIAL_GOODS = 5,

    }
}
