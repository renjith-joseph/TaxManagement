
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using TaxManagement.Security;
using NUnit.Framework;
using TaxManagement.Models;

namespace TaxManagement.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    [Authorize]
    public class TaxController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaxRepository _taxRepository;
        public TaxController(IUserRepository userRepository,ITaxRepository taxRepository)
        {
            _userRepository = userRepository;
            _taxRepository = taxRepository;
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<string>>> Get()
        //{
            
        //    return await _userRepository.GetUserNames();
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxDetails>>> GetTaxDetails(string Munciplaity, DateTime InputDate)
        {
            return await _taxRepository.GetTaxDetails(Munciplaity, InputDate);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AddTaxResult>>> Post(string UserName, string Munciplaity, string Frequency, Decimal TaxAmount, DateTime FromDate, DateTime ToDate)
        {

            return await _taxRepository.InsertTax(UserName, Munciplaity, Frequency, TaxAmount, FromDate, ToDate);
        }
    }
}
