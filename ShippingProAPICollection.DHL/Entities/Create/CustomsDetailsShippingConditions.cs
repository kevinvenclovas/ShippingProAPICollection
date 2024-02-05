using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public enum CustomsDetailsShippingConditions
    {

        [System.Runtime.Serialization.EnumMember(Value = @"DDU")]
        DDU = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"DAP")]
        DAP = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"DDP")]
        DDP = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"DDX")]
        DDX = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"DXV")]
        DXV = 4,

    }
}
