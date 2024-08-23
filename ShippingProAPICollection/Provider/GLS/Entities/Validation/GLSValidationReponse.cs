using ShippingProAPICollection.Models;

namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    public class GLSValidationReponse : ValidationReponse
    {
        public List<GLSValidationReponseIssue>? ValidationIssues { get; set; }
    }
}
