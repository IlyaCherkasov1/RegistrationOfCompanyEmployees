using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RegistrationOfCompanyEmployees.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationOfCompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // private readonly string connectionString = ConfigurationManager.AppSettings["DefaultConnection"];

        public IConfiguration Configuration { get; }
        public CompanyController(IConfiguration config)
        {
            Configuration = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            List<Company> companies = new List<Company>();
            string sqlExpression = "SELECT * FROM Company";
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int idCompany = Convert.ToInt32(reader["CompanyId"]);
                        string name = (reader["Name"]).ToString();
                        string legalForm = (reader["LegalForm"]).ToString();

                        Company company = new Company(idCompany, name, legalForm);
                        companies.Add(company);
                    }
                }
                await reader.CloseAsync();
            }

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            Company company = default;
            string sqlExpression = "SELECT * FROM Company where CompanyId=(@id)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int idCompany = Convert.ToInt32(reader["CompanyId"]);
                        string name = (reader["Name"]).ToString();
                        string legalForm = (reader["LegalForm"]).ToString();

                        company = new Company(idCompany, name, legalForm);

                    }
                }
                await reader.CloseAsync();
            }

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            string sqlExpression = "UPDATE Company SET Name=(@name), LegalForm=(@legalForm) WHERE CompanyId=(@id)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", company.Name);
                command.Parameters.AddWithValue("@legalForm", company.LegalForm);


                command.ExecuteNonQuery();
            }
            return NoContent();
        }



        [HttpPost]
        public async Task<IActionResult> PostCompany(Company company)
        {
            string sqlExpression = "INSERT INTO Company (Name, LegalForm) " +
                        $"VALUES (@name, @legalForm)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
         
                command.Parameters.AddWithValue("@name", company.Name);
                command.Parameters.AddWithValue("@legalForm", company.LegalForm);

                command.ExecuteNonQuery();
            }
            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            string sqlExpression = "Delete from Company where CompanyId=(@id)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
            return NoContent();
        }


    }
}
