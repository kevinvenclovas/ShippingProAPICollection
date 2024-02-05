using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities
{
    public enum WeightUom
    {

        [System.Runtime.Serialization.EnumMember(Value = @"g")]
        G = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"kg")]
        Kg = 1,

    }
}
