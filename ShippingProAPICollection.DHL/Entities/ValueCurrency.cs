using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities
{
    public enum ValueCurrency
    {

        [System.Runtime.Serialization.EnumMember(Value = @"AED")]
        AED = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AFN")]
        AFN = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"ALL")]
        ALL = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"AMD")]
        AMD = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"ANG")]
        ANG = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"AOA")]
        AOA = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"ARS")]
        ARS = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"AUD")]
        AUD = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"AWG")]
        AWG = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"AZN")]
        AZN = 9,

        [System.Runtime.Serialization.EnumMember(Value = @"BAM")]
        BAM = 10,

        [System.Runtime.Serialization.EnumMember(Value = @"BBD")]
        BBD = 11,

        [System.Runtime.Serialization.EnumMember(Value = @"BDT")]
        BDT = 12,

        [System.Runtime.Serialization.EnumMember(Value = @"BGN")]
        BGN = 13,

        [System.Runtime.Serialization.EnumMember(Value = @"BHD")]
        BHD = 14,

        [System.Runtime.Serialization.EnumMember(Value = @"BIF")]
        BIF = 15,

        [System.Runtime.Serialization.EnumMember(Value = @"BMD")]
        BMD = 16,

        [System.Runtime.Serialization.EnumMember(Value = @"BND")]
        BND = 17,

        [System.Runtime.Serialization.EnumMember(Value = @"BOB")]
        BOB = 18,

        [System.Runtime.Serialization.EnumMember(Value = @"BOV")]
        BOV = 19,

        [System.Runtime.Serialization.EnumMember(Value = @"BRL")]
        BRL = 20,

        [System.Runtime.Serialization.EnumMember(Value = @"BSD")]
        BSD = 21,

        [System.Runtime.Serialization.EnumMember(Value = @"BTN")]
        BTN = 22,

        [System.Runtime.Serialization.EnumMember(Value = @"BWP")]
        BWP = 23,

        [System.Runtime.Serialization.EnumMember(Value = @"BYR")]
        BYR = 24,

        [System.Runtime.Serialization.EnumMember(Value = @"BZD")]
        BZD = 25,

        [System.Runtime.Serialization.EnumMember(Value = @"CAD")]
        CAD = 26,

        [System.Runtime.Serialization.EnumMember(Value = @"CDF")]
        CDF = 27,

        [System.Runtime.Serialization.EnumMember(Value = @"CHE")]
        CHE = 28,

        [System.Runtime.Serialization.EnumMember(Value = @"CHF")]
        CHF = 29,

        [System.Runtime.Serialization.EnumMember(Value = @"CHW")]
        CHW = 30,

        [System.Runtime.Serialization.EnumMember(Value = @"CLF")]
        CLF = 31,

        [System.Runtime.Serialization.EnumMember(Value = @"CLP")]
        CLP = 32,

        [System.Runtime.Serialization.EnumMember(Value = @"CNY")]
        CNY = 33,

        [System.Runtime.Serialization.EnumMember(Value = @"COP")]
        COP = 34,

        [System.Runtime.Serialization.EnumMember(Value = @"COU")]
        COU = 35,

        [System.Runtime.Serialization.EnumMember(Value = @"CRC")]
        CRC = 36,

        [System.Runtime.Serialization.EnumMember(Value = @"CUC")]
        CUC = 37,

        [System.Runtime.Serialization.EnumMember(Value = @"CUP")]
        CUP = 38,

        [System.Runtime.Serialization.EnumMember(Value = @"CVE")]
        CVE = 39,

        [System.Runtime.Serialization.EnumMember(Value = @"CZK")]
        CZK = 40,

        [System.Runtime.Serialization.EnumMember(Value = @"DJF")]
        DJF = 41,

        [System.Runtime.Serialization.EnumMember(Value = @"DKK")]
        DKK = 42,

        [System.Runtime.Serialization.EnumMember(Value = @"DOP")]
        DOP = 43,

        [System.Runtime.Serialization.EnumMember(Value = @"DZD")]
        DZD = 44,

        [System.Runtime.Serialization.EnumMember(Value = @"EGP")]
        EGP = 45,

        [System.Runtime.Serialization.EnumMember(Value = @"ERN")]
        ERN = 46,

        [System.Runtime.Serialization.EnumMember(Value = @"ETB")]
        ETB = 47,

        [System.Runtime.Serialization.EnumMember(Value = @"EUR")]
        EUR = 48,

        [System.Runtime.Serialization.EnumMember(Value = @"FJD")]
        FJD = 49,

        [System.Runtime.Serialization.EnumMember(Value = @"FKP")]
        FKP = 50,

        [System.Runtime.Serialization.EnumMember(Value = @"GBP")]
        GBP = 51,

        [System.Runtime.Serialization.EnumMember(Value = @"GEL")]
        GEL = 52,

        [System.Runtime.Serialization.EnumMember(Value = @"GHS")]
        GHS = 53,

        [System.Runtime.Serialization.EnumMember(Value = @"GIP")]
        GIP = 54,

        [System.Runtime.Serialization.EnumMember(Value = @"GMD")]
        GMD = 55,

        [System.Runtime.Serialization.EnumMember(Value = @"GNF")]
        GNF = 56,

        [System.Runtime.Serialization.EnumMember(Value = @"GTQ")]
        GTQ = 57,

        [System.Runtime.Serialization.EnumMember(Value = @"GYD")]
        GYD = 58,

        [System.Runtime.Serialization.EnumMember(Value = @"HKD")]
        HKD = 59,

        [System.Runtime.Serialization.EnumMember(Value = @"HNL")]
        HNL = 60,

        [System.Runtime.Serialization.EnumMember(Value = @"HRK")]
        HRK = 61,

        [System.Runtime.Serialization.EnumMember(Value = @"HTG")]
        HTG = 62,

        [System.Runtime.Serialization.EnumMember(Value = @"HUF")]
        HUF = 63,

        [System.Runtime.Serialization.EnumMember(Value = @"IDR")]
        IDR = 64,

        [System.Runtime.Serialization.EnumMember(Value = @"ILS")]
        ILS = 65,

        [System.Runtime.Serialization.EnumMember(Value = @"INR")]
        INR = 66,

        [System.Runtime.Serialization.EnumMember(Value = @"IQD")]
        IQD = 67,

        [System.Runtime.Serialization.EnumMember(Value = @"IRR")]
        IRR = 68,

        [System.Runtime.Serialization.EnumMember(Value = @"ISK")]
        ISK = 69,

        [System.Runtime.Serialization.EnumMember(Value = @"JMD")]
        JMD = 70,

        [System.Runtime.Serialization.EnumMember(Value = @"JOD")]
        JOD = 71,

        [System.Runtime.Serialization.EnumMember(Value = @"JPY")]
        JPY = 72,

        [System.Runtime.Serialization.EnumMember(Value = @"KES")]
        KES = 73,

        [System.Runtime.Serialization.EnumMember(Value = @"KGS")]
        KGS = 74,

        [System.Runtime.Serialization.EnumMember(Value = @"KHR")]
        KHR = 75,

        [System.Runtime.Serialization.EnumMember(Value = @"KMF")]
        KMF = 76,

        [System.Runtime.Serialization.EnumMember(Value = @"KPW")]
        KPW = 77,

        [System.Runtime.Serialization.EnumMember(Value = @"KRW")]
        KRW = 78,

        [System.Runtime.Serialization.EnumMember(Value = @"KWD")]
        KWD = 79,

        [System.Runtime.Serialization.EnumMember(Value = @"KYD")]
        KYD = 80,

        [System.Runtime.Serialization.EnumMember(Value = @"KZT")]
        KZT = 81,

        [System.Runtime.Serialization.EnumMember(Value = @"LAK")]
        LAK = 82,

        [System.Runtime.Serialization.EnumMember(Value = @"LBP")]
        LBP = 83,

        [System.Runtime.Serialization.EnumMember(Value = @"LKR")]
        LKR = 84,

        [System.Runtime.Serialization.EnumMember(Value = @"LRD")]
        LRD = 85,

        [System.Runtime.Serialization.EnumMember(Value = @"LSL")]
        LSL = 86,

        [System.Runtime.Serialization.EnumMember(Value = @"LTL")]
        LTL = 87,

        [System.Runtime.Serialization.EnumMember(Value = @"LVL")]
        LVL = 88,

        [System.Runtime.Serialization.EnumMember(Value = @"LYD")]
        LYD = 89,

        [System.Runtime.Serialization.EnumMember(Value = @"MAD")]
        MAD = 90,

        [System.Runtime.Serialization.EnumMember(Value = @"MDL")]
        MDL = 91,

        [System.Runtime.Serialization.EnumMember(Value = @"MGA")]
        MGA = 92,

        [System.Runtime.Serialization.EnumMember(Value = @"MKD")]
        MKD = 93,

        [System.Runtime.Serialization.EnumMember(Value = @"MMK")]
        MMK = 94,

        [System.Runtime.Serialization.EnumMember(Value = @"MNT")]
        MNT = 95,

        [System.Runtime.Serialization.EnumMember(Value = @"MOP")]
        MOP = 96,

        [System.Runtime.Serialization.EnumMember(Value = @"MRO")]
        MRO = 97,

        [System.Runtime.Serialization.EnumMember(Value = @"MUR")]
        MUR = 98,

        [System.Runtime.Serialization.EnumMember(Value = @"MVR")]
        MVR = 99,

        [System.Runtime.Serialization.EnumMember(Value = @"MWK")]
        MWK = 100,

        [System.Runtime.Serialization.EnumMember(Value = @"MXN")]
        MXN = 101,

        [System.Runtime.Serialization.EnumMember(Value = @"MXV")]
        MXV = 102,

        [System.Runtime.Serialization.EnumMember(Value = @"MYR")]
        MYR = 103,

        [System.Runtime.Serialization.EnumMember(Value = @"MZN")]
        MZN = 104,

        [System.Runtime.Serialization.EnumMember(Value = @"NAD")]
        NAD = 105,

        [System.Runtime.Serialization.EnumMember(Value = @"NGN")]
        NGN = 106,

        [System.Runtime.Serialization.EnumMember(Value = @"NIO")]
        NIO = 107,

        [System.Runtime.Serialization.EnumMember(Value = @"NOK")]
        NOK = 108,

        [System.Runtime.Serialization.EnumMember(Value = @"NPR")]
        NPR = 109,

        [System.Runtime.Serialization.EnumMember(Value = @"NZD")]
        NZD = 110,

        [System.Runtime.Serialization.EnumMember(Value = @"OMR")]
        OMR = 111,

        [System.Runtime.Serialization.EnumMember(Value = @"PAB")]
        PAB = 112,

        [System.Runtime.Serialization.EnumMember(Value = @"PEN")]
        PEN = 113,

        [System.Runtime.Serialization.EnumMember(Value = @"PGK")]
        PGK = 114,

        [System.Runtime.Serialization.EnumMember(Value = @"PHP")]
        PHP = 115,

        [System.Runtime.Serialization.EnumMember(Value = @"PKR")]
        PKR = 116,

        [System.Runtime.Serialization.EnumMember(Value = @"PLN")]
        PLN = 117,

        [System.Runtime.Serialization.EnumMember(Value = @"PYG")]
        PYG = 118,

        [System.Runtime.Serialization.EnumMember(Value = @"QAR")]
        QAR = 119,

        [System.Runtime.Serialization.EnumMember(Value = @"RON")]
        RON = 120,

        [System.Runtime.Serialization.EnumMember(Value = @"RSD")]
        RSD = 121,

        [System.Runtime.Serialization.EnumMember(Value = @"RUB")]
        RUB = 122,

        [System.Runtime.Serialization.EnumMember(Value = @"RWF")]
        RWF = 123,

        [System.Runtime.Serialization.EnumMember(Value = @"SAR")]
        SAR = 124,

        [System.Runtime.Serialization.EnumMember(Value = @"SBD")]
        SBD = 125,

        [System.Runtime.Serialization.EnumMember(Value = @"SCR")]
        SCR = 126,

        [System.Runtime.Serialization.EnumMember(Value = @"SDG")]
        SDG = 127,

        [System.Runtime.Serialization.EnumMember(Value = @"SEK")]
        SEK = 128,

        [System.Runtime.Serialization.EnumMember(Value = @"SGD")]
        SGD = 129,

        [System.Runtime.Serialization.EnumMember(Value = @"SHP")]
        SHP = 130,

        [System.Runtime.Serialization.EnumMember(Value = @"SLL")]
        SLL = 131,

        [System.Runtime.Serialization.EnumMember(Value = @"SOS")]
        SOS = 132,

        [System.Runtime.Serialization.EnumMember(Value = @"SRD")]
        SRD = 133,

        [System.Runtime.Serialization.EnumMember(Value = @"SSP")]
        SSP = 134,

        [System.Runtime.Serialization.EnumMember(Value = @"STD")]
        STD = 135,

        [System.Runtime.Serialization.EnumMember(Value = @"SYP")]
        SYP = 136,

        [System.Runtime.Serialization.EnumMember(Value = @"SZL")]
        SZL = 137,

        [System.Runtime.Serialization.EnumMember(Value = @"THB")]
        THB = 138,

        [System.Runtime.Serialization.EnumMember(Value = @"TJS")]
        TJS = 139,

        [System.Runtime.Serialization.EnumMember(Value = @"TMT")]
        TMT = 140,

        [System.Runtime.Serialization.EnumMember(Value = @"TND")]
        TND = 141,

        [System.Runtime.Serialization.EnumMember(Value = @"TOP")]
        TOP = 142,

        [System.Runtime.Serialization.EnumMember(Value = @"TRY")]
        TRY = 143,

        [System.Runtime.Serialization.EnumMember(Value = @"TTD")]
        TTD = 144,

        [System.Runtime.Serialization.EnumMember(Value = @"TWD")]
        TWD = 145,

        [System.Runtime.Serialization.EnumMember(Value = @"TZS")]
        TZS = 146,

        [System.Runtime.Serialization.EnumMember(Value = @"UAH")]
        UAH = 147,

        [System.Runtime.Serialization.EnumMember(Value = @"UGX")]
        UGX = 148,

        [System.Runtime.Serialization.EnumMember(Value = @"USD")]
        USD = 149,

        [System.Runtime.Serialization.EnumMember(Value = @"USN")]
        USN = 150,

        [System.Runtime.Serialization.EnumMember(Value = @"USS")]
        USS = 151,

        [System.Runtime.Serialization.EnumMember(Value = @"UYI")]
        UYI = 152,

        [System.Runtime.Serialization.EnumMember(Value = @"UYU")]
        UYU = 153,

        [System.Runtime.Serialization.EnumMember(Value = @"UZS")]
        UZS = 154,

        [System.Runtime.Serialization.EnumMember(Value = @"VEF")]
        VEF = 155,

        [System.Runtime.Serialization.EnumMember(Value = @"VND")]
        VND = 156,

        [System.Runtime.Serialization.EnumMember(Value = @"VUV")]
        VUV = 157,

        [System.Runtime.Serialization.EnumMember(Value = @"WST")]
        WST = 158,

        [System.Runtime.Serialization.EnumMember(Value = @"XAF")]
        XAF = 159,

        [System.Runtime.Serialization.EnumMember(Value = @"XAG")]
        XAG = 160,

        [System.Runtime.Serialization.EnumMember(Value = @"XAU")]
        XAU = 161,

        [System.Runtime.Serialization.EnumMember(Value = @"XBA")]
        XBA = 162,

        [System.Runtime.Serialization.EnumMember(Value = @"XBB")]
        XBB = 163,

        [System.Runtime.Serialization.EnumMember(Value = @"XBC")]
        XBC = 164,

        [System.Runtime.Serialization.EnumMember(Value = @"XBD")]
        XBD = 165,

        [System.Runtime.Serialization.EnumMember(Value = @"XCD")]
        XCD = 166,

        [System.Runtime.Serialization.EnumMember(Value = @"XDR")]
        XDR = 167,

        [System.Runtime.Serialization.EnumMember(Value = @"XFU")]
        XFU = 168,

        [System.Runtime.Serialization.EnumMember(Value = @"XOF")]
        XOF = 169,

        [System.Runtime.Serialization.EnumMember(Value = @"XPD")]
        XPD = 170,

        [System.Runtime.Serialization.EnumMember(Value = @"XPF")]
        XPF = 171,

        [System.Runtime.Serialization.EnumMember(Value = @"XPT")]
        XPT = 172,

        [System.Runtime.Serialization.EnumMember(Value = @"XXX")]
        XXX = 173,

        [System.Runtime.Serialization.EnumMember(Value = @"YER")]
        YER = 174,

        [System.Runtime.Serialization.EnumMember(Value = @"ZAR")]
        ZAR = 175,

        [System.Runtime.Serialization.EnumMember(Value = @"ZMW")]
        ZMW = 176,

        [System.Runtime.Serialization.EnumMember(Value = @"UNKNOWN")]
        UNKNOWN = 177,

    }
}
