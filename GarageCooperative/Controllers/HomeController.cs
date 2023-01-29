using GarageCooperative.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Request(string request)
        {
            // string connectionString = $"Server=DESKTOP-I75L3P7;Database=GarageCooperative;Trusted_Connection=True;Encrypt=False;";
            string connectionString = $"Server=DESKTOP-KIV92L3;Database=GarageCooperative;Trusted_Connection=True;Encrypt=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(request, connection);
                    var result = new RequestViewModel();
                    var reader = command.ExecuteReader();
                    result.Displays = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Displays[i] = reader.GetName(i);
                    }

                    while (reader.Read())
                    {
                        string[] value = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            value[i] = reader.GetValue(i).ToString();
                        }

                        result.Result.Add(value);
                    }

                    return View(result);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}
