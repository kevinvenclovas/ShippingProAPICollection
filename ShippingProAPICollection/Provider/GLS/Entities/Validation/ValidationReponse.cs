﻿
namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    public class ValidationReponse
    {
        public required bool Success {  get; set; }
        public List<ValidationReponseIssue>? ValidationIssues { get; set; }
    }
}
