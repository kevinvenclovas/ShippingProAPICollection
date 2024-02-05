using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public enum VASEndorsement
    {

        [System.Runtime.Serialization.EnumMember(Value = @"RETURN")]
        RETURN = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ABANDON")]
        ABANDON = 1,

    }
}
