using Microsoft.AspNetCore.Mvc;

namespace TaxManagement.Models
{
    public interface ITaxRepository
    {
        Task<ActionResult<IEnumerable<AddTaxResult>>> InsertTax(string UserName, string Munciplaity, string Frequency, Decimal TaxAmount,DateTime FromDate,DateTime ToDate);
        Task<ActionResult<IEnumerable<TaxDetails>>> GetTaxDetails(string Munciplaity, DateTime InputDate);
    }
}
