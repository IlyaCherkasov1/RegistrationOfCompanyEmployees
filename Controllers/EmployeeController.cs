using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RegistrationOfCompanyEmployees.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationOfCompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration Configuration;

        public EmployeeController(IConfiguration config)
        {
            Configuration = config;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetCompanies()
        {
            List<Employee> employees = new List<Employee>();
            string sqlExpression = "SELECT * FROM Employee";
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
                        int employeeId = Convert.ToInt32(reader["EmployeeId"]);
                        string surname = (reader["Surname"]).ToString();
                        string name = (reader["Name"]).ToString();
                        string middleName = (reader["MiddleName"]).ToString();
                        DateTime EmploymentDate = Convert.ToDateTime(reader["EmploymentDate"]);
                        string Position = (reader["Position"]).ToString();
                        int CompanyId = Convert.ToInt32(reader["CompanyId"]);

                        Employee employee = new Employee(employeeId, surname, name, middleName, EmploymentDate, Position, CompanyId);
                        employees.Add(employee);
                    }
                }
                await reader.CloseAsync();
            }

            return Ok(employees);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            Employee employee = default;
            string sqlExpression = "SELECT * FROM Employee where EmployeeId=(@id)";
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
                        int employeeId = Convert.ToInt32(reader["EmployeeId"]);
                        string surname = (reader["Surname"]).ToString();
                        string name = (reader["Name"]).ToString();
                        string middleName = (reader["MiddleName"]).ToString();
                        DateTime EmploymentDate = Convert.ToDateTime(reader["EmploymentDate"]);
                        string Position = (reader["Position"]).ToString();
                        int CompanyId = Convert.ToInt32(reader["CompanyId"]);

                         employee = new Employee(employeeId, surname, name, middleName, EmploymentDate, Position, CompanyId);
                        
                    }
                }
                await reader.CloseAsync();
            }

            return employee;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            string sqlExpression = "UPDATE Employee SET Surname=(@surname), Name=(@name), MiddleName=(@middleName), " +
                "EmploymentDate=(@employmentDate), Position=(@position), CompanyId=(@companyId) WHERE EmployeeId=(@id)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddWithValue("@surname", employee.Surname);
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@middleName", employee.MiddleName);
                command.Parameters.AddWithValue("@employmentDate", employee.EmploymentDate);
                command.Parameters.AddWithValue("@position", employee.Position);
                command.Parameters.AddWithValue("@companyId", employee.CompanyId);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostCompany(Employee employee)
        {
            string sqlExpression = "INSERT INTO Employee (Surname, Name, MiddleName, EmploymentDate, Position, CompanyId) " +
                        $"VALUES (@surname, @name, @middleName, @employmentDate, @position, @companyId)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@surname", employee.Surname);
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@middleName", employee.MiddleName);
                command.Parameters.AddWithValue("@employmentDate", employee.EmploymentDate);
                command.Parameters.AddWithValue("@position", employee.Position);
                command.Parameters.AddWithValue("@companyId", employee.CompanyId);


                command.ExecuteNonQuery();
            }
            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            string sqlExpression = "Delete from Employee where EmployeeId=(@id)";

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


        //is dupe field
        [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(int employeeId, string fieldName, string fieldValue)
        {

            List<Employee> employees = GetAllEmployees();
            switch (fieldName)
            {
                case "surname": return employees.Any(e => e.Surname == fieldValue && e.EmployeeId != employeeId);
                case "name": return employees.Any(e => e.Name == fieldValue && e.EmployeeId != employeeId);
                case "middleName": return employees.Any(e => e.MiddleName == fieldValue && e.EmployeeId != employeeId);
                case "employmentDate": return employees.Any(e => e.EmploymentDate == DateTime.Parse(fieldValue) && e.EmployeeId != employeeId);
                case "position": return employees.Any(e => e.Position == fieldValue && e.EmployeeId != employeeId);
                case "companyId": return employees.Any(e => e.CompanyId == Int32.Parse(fieldValue) && e.EmployeeId != employeeId);
                default:
                    return false;
            }

        }

        private List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string sqlExpression = "SELECT * FROM Employee";
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                 connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader =  command.ExecuteReader();

                if (reader.HasRows)
                {
                    while ( reader.Read())
                    {
                        int employeeId = Convert.ToInt32(reader["EmployeeId"]);
                        string surname = (reader["Surname"]).ToString();
                        string name = (reader["Name"]).ToString();
                        string middleName = (reader["MiddleName"]).ToString();
                        DateTime EmploymentDate = Convert.ToDateTime(reader["EmploymentDate"]);
                        string Position = (reader["Position"]).ToString();
                        int CompanyId = Convert.ToInt32(reader["CompanyId"]);

                        Employee employee = new Employee(employeeId, surname, name, middleName, EmploymentDate, Position, CompanyId);
                        employees.Add(employee);
                    }
                }
                 reader.Close();
            }

            return employees;
        }
    }
}
