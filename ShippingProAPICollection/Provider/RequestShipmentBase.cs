using JsonSubTypes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;

namespace ShippingProAPICollection.Provider
{
    [JsonConverter(typeof(JsonSubtypes), nameof(ProviderType))]
    [JsonSubtypes.KnownSubType(typeof(GLSShipmentRequestModel), "GLS")]
    [JsonSubtypes.KnownSubType(typeof(DPDShipmentRequestModel), "DPD")]
    [JsonSubtypes.KnownSubType(typeof(DHLShipmentRequestModel), "DHL")]
    public abstract class RequestShipmentBase
    {
        public abstract string ProviderType { get; }
        public abstract float MaxPackageWeight { get; }

        public RequestShipmentBase(string contractID)
        {
            ContractID = contractID;
        }

        /// <summary>
        /// Typ des Versandanbieters |
        /// Type of the shipping provider
        /// </summary>
        public string ContractID { get; }

        /// <summary>
        /// Datum wann das Paket frühestens geliefert werden soll |
        /// Earliest delivery date for the package
        /// </summary>
        public DateTime EarliestDeliveryDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Adresszeile 1 des Empfängers |
        /// First address line of reciever
        /// </summary>
        /// <example>Alfa GmbH</example>
        [Required]
        public required string Adressline1 { get; set; }

        /// <summary>
        /// Adresszeile 2 des Empfängers |
        /// Second address line of reciever
        /// </summary>
        /// <example>Kevin Venclovas</example>
        public string? Adressline2 { get; set; }

        /// <summary>
        /// Adresszeile 3 des Empfängers |
        /// Third address line of reciever
        /// </summary>
        /// <example>PO - IT</example>
        public string? Adressline3 { get; set; }

        /// <summary>
        /// Länder Isocode A2 des Empfängers |
        /// Country code in IDOA2 format
        /// </summary>
        /// <example>DE</example>
        [Required]
        public required string Country { get; set; }

        /// <summary>
        /// Postleitzahl des Empfängers |
        /// Postcode of reciever address
        /// </summary>
        /// <example>73479</example>
        [Required]
        public required string PostCode { get; set; }

        /// <summary>
        /// Stadt des Empfängers |
        /// city of reciever address
        /// </summary>
        /// <example>Ellwangen</example>
        [Required]
        public required string City { get; set; }

        /// <summary>
        /// Straße des Empfängers |
        /// street of reciever address
        /// </summary>
        /// <example>Ferdinand-Porsche-Str.</example>
        [Required]
        public required string Street { get; set; }

        /// <summary>
        /// Hausnummer des Empfängers. Hausnummer kann auch '-' sein wenn Hausnummer und Straße in Feld Straße nicht getrennt werden können |
        /// Street number of reciever address. Street number can also be '-' when field street contains the streetnumber
        /// </summary>
        /// <example>10</example>
        public string? StreetNumber { get; set; }

        /// <summary>
        /// Optionaler Kontaktname des Empfängers
        /// optional contact name
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// E-Mail des Empfängers |
        /// EMail of reciever
        /// </summary>
        /// <example>10</example>
        public string? EMail { get; set; }

        /// <summary>
        /// Gewicht der gesamten Fracht in KG |
        /// Weight of the total freight in KG 
        /// </summary>
        /// <example>5</example>
        [Required]
        public required float Weight { get; set; }

        /// <summary>
        /// Auf wieviele Label soll die Fracht aufgeteilt werden? |
        /// How many labels
        /// </summary>
        /// <example>1</example>
        [Range(1, 99)]
        [Required]
        public required uint LabelCount { get; set; }

        /// <summary>
        /// Kundennummer falls vorhanden |
        /// Customernumber if available
        /// </summary>
        /// <example>1256872</example>
        public string? CustomerReference { get; set; }

        /// <summary>
        /// Rechnungsnummer falls vorhanden |
        /// Invoice reference if available
        /// </summary>
        /// <example>V.RE.123456</example>
        public string? InvoiceReference { get; set; }

        /// <summary>
        /// Telefonnummer |
        /// Phone numer of reciever
        /// </summary>
        /// <example>07961 / 5799-0</example>
        public string? Phone { get; set; }

        /// <summary>
        /// IncotermCode wie die Ware am Zoll angemeldet wird |
        /// Incoterm for the customs clearance 
        /// </summary>
        /// <example>10,20,30</example>
        public int? IncotermCode { get; set; }

        /// <summary>
        /// Notiz 1 auf dem Label |
        /// Note 1 printed on the label
        /// </summary>
        /// <example>Ware auf den Briefkasten</example>
        public string? Note1 { get; set; }


        public virtual void Validate()
        {
            if (LabelCount <= 0) throw new ShipmentRequestLabelCountException(LabelCount);
            if (Country.Length != 2) throw new ShipmentRequestNoValidStringLengthException("Country", 2, 2);

            if (Weight <= 0) throw new ShipmentRequestWeightException(1, MaxPackageWeight, 0);
            if (GetPackageWeight() > MaxPackageWeight) throw new ShipmentRequestWeightException(1, MaxPackageWeight, Weight / LabelCount);
        }

        /// <summary>
        /// Get the single package weight of the request
        /// </summary>
        /// <returns></returns>
        public virtual float GetPackageWeight()
        {
            float packageWeight = Weight / LabelCount;
            var result = (float)Math.Round(packageWeight, 2);
            return result < 1 ? 1 : result;
        }

        /// <summary>
        /// Is the booked service express shipping variant
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal virtual bool IsExpress()
        {
            throw new NotImplementedException();
        }
    }
}
