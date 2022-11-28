namespace TaxManagement.Models
{
    public class TaxDetails
    {      

            public string  MuncipalityName { get; set; }
            public DateTime TaxDate { get; set; }
            public string Frequency { get; set; }
            public int TaxAmount { get; set; }
            public string Message { get; set; }

    }
}
