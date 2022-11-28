using System.Runtime.Serialization;

namespace TaxManagement.Models
{   
        public class MuncipalityTax
        {
            public int UserId { get; set; }
            
            public int MId { get; set; }

            public int FrequencyId { get; set; }

            public Decimal TaxAmount { get; set; }

            public DateTime FromDate { get; set; }
            
            public DateTime ToDate { get; set; }
           
            public int TaxId { get; set; }

        }
    
}
