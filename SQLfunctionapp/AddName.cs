using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace SQLfunctionapp
{
    public static class AddName
    {
        [FunctionName("AddName")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string names_list = data?.name;
            //string _connection_string = "SQL_CONNECTION_STRING"
            string _connection_string = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Sql_connection_string");
            string _statememnt = "INSERT INTO webapp(Name) VALUES(@param1)";
            SqlConnection _connection = new SqlConnection(_connection_string);
            _connection.Open();
            //SqlCommand _sqlcommand = new SqlCommand(_statememnt, _connection);
            using (SqlCommand _sqlcommand = new SqlCommand(_statememnt, _connection))
            {
                _sqlcommand.Parameters.Add("@param1", SqlDbType.VarChar, 30).Value = data.name;
                _sqlcommand.CommandType = CommandType.Text;
                _sqlcommand.ExecuteNonQuery();
            }
            _connection.Close();
            return new OkObjectResult("Name Added");
        }
    }
}
