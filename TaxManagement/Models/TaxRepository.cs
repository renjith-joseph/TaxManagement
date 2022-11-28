using TaxManagement.Security;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Forms;

namespace TaxManagement.Models
{
    public class TaxRepository:ITaxRepository
    {
        private readonly IConfiguration _configuration;
        public SqlConnection conn = new SqlConnection();
        enum TaxFrequency{Yearly=1,Monthly=2,Weekly=3,Daily=4};
        enum Muncipality {Copenhagen=1};
        enum UserLogin {danskeuser=1};

        public TaxRepository(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<AddTaxResult>>> InsertTax(string UserName, string  Munciplaity, string Frequency, Decimal TaxAmount, DateTime FromDate, DateTime ToDate)
        {
            List<AddTaxResult> TaxResults = new List<AddTaxResult>();
            try
            {
                int UserId = 0, MId = 0, FrequencyId = 0;

                if (UserName == "danskeuser")
                    UserId = (int)UserLogin.danskeuser;
                if (Munciplaity == "Copenhagen")
                    MId = (int)Muncipality.Copenhagen;
                if (Frequency == "Yearly")
                    FrequencyId = (int)TaxFrequency.Yearly;
                else if (Frequency == "Monthly")
                    FrequencyId = (int)TaxFrequency.Monthly;
                else if (Frequency == "Weekly")
                    FrequencyId = (int)TaxFrequency.Weekly;
                else if (Frequency == "Daily")
                    FrequencyId = (int)TaxFrequency.Daily;



                using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandText = "AddTax";
                        SqlParameter param;

                        param = sqlCommand.Parameters.Add("@UserId", SqlDbType.Int);
                        param.Value = 1;

                        param = sqlCommand.Parameters.Add("@MId", SqlDbType.Int);
                        param.Value = 1;

                        param = sqlCommand.Parameters.Add("@FrequencyId", SqlDbType.Int);
                        param.Value = FrequencyId;

                        param = sqlCommand.Parameters.Add("@TaxAmount", SqlDbType.Decimal, 6);
                        param.Value = TaxAmount;

                        param = sqlCommand.Parameters.Add("@FromDate", SqlDbType.DateTime);
                        param.Value = FromDate;

                        param = sqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime);
                        param.Value = ToDate;

                        param = sqlCommand.Parameters.Add("@TaxId", SqlDbType.Int);

                        param.Direction = ParameterDirection.Output;


                        sqlConnection.Open();

                        sqlCommand.ExecuteNonQuery();

                        sqlConnection.Close();


                        if (Convert.ToInt32(param.Value) > 0)
                        {

                            TaxResults.Add(new AddTaxResult
                            {
                                TaxId = Convert.ToInt32(param.Value),
                                Message = "Tax Details Inserted Successfully"
                            });
                        }
                        else
                        {
                            TaxResults.Add(new AddTaxResult
                            {
                                TaxId = Convert.ToInt32(param.Value),
                                Message = "Tax Details Already Exist"
                            });
                        }

                    }
                }
               
            }
            catch(Exception ex)
            {
                TaxResults.Add(new AddTaxResult
                {
                    TaxId = 0,
                    Message = ex.Message
                }); ;

            }
            return TaxResults;
        }

        public async Task<ActionResult<IEnumerable<TaxDetails>>> GetTaxDetails(string MunciplaityName, DateTime InputDate)
        {
            List<TaxDetails> Taxes = new List<TaxDetails>();
            int MId = 0;
            try
            {
                if (MunciplaityName == "Copenhagen")
                    MId = (int)Muncipality.Copenhagen;


                using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandText = "GetTaxDetails";
                        SqlParameter param;



                        param = sqlCommand.Parameters.Add("@MId", SqlDbType.Int);
                        param.Value = 1;


                        param = sqlCommand.Parameters.Add("@Date", SqlDbType.DateTime);
                        param.Value = InputDate;


                        sqlConnection.Open();

                        using (SqlDataReader sdr = sqlCommand.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    Taxes.Add(new TaxDetails
                                    {
                                        MuncipalityName = Convert.ToString(sdr["MName"]),
                                        TaxDate = Convert.ToDateTime(sdr["GivenDate"]),
                                        Frequency = Convert.ToString(sdr["FName"]),
                                        TaxAmount = Convert.ToInt32(sdr["TaxAmount"]),
                                        Message = "Success"
                                    });
                                }
                            }
                            else
                            {
                                Taxes.Add(new TaxDetails
                                {
                                    MuncipalityName = MunciplaityName,
                                    TaxDate = InputDate,
                                    Frequency = "",
                                    TaxAmount = 0,
                                    Message = "No Records "
                                });

                            }

                        }
                        sqlConnection.Close();
                    }
                }

               
            }
            catch(Exception ex)
            {
                Taxes.Add(new TaxDetails
                {
                    MuncipalityName = "",
                    TaxDate = DateTime.Now,
                    Frequency = "",
                    TaxAmount = 0,
                    Message = ex.Message
                });
            }
            return Taxes;
        }
    }
}
