using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace SQLfunctionapp
{
    public static class Getnames
    {
        [FunctionName("Getnames")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            /*
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
            */
            List<Names> _name_list = new List<Names>();
            //string _connection_string = "SQL_CONNECTION_STRING"
            //Make sure you add connection string in Azure function app settings with name Sql_connection_string
            string _connection_string = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Sql_connection_string");
            string _statememnt = "SELECT * from webapp";
            SqlConnection _connection = new SqlConnection(_connection_string);
            _connection.Open();
            SqlCommand _sqlcommand = new SqlCommand(_statememnt, _connection);
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while(_reader.Read())
                {
                    Names name = new Names()
                    {
                        Name = _reader.GetString(0)
                    };
                    
                    _name_list.Add(name);
                }
            }
            _connection.Close();
            return new OkObjectResult(_name_list);
        }
    }
}
