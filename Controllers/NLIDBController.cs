using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nlidbclient.Models;

namespace nlidbclient.Controllers
{
    public class IIEModel{

        public IIEModel()
        {
            Text="";
            Table = new DataTable();
        }
        public string Text{get;set;}
        public DataTable Table{get;set;}
    }
    public class NLIDBController : Controller
    {
        [HttpPost]
        public ActionResult CreateQuery(string query)
        {
            var data = new NLPQuery()
            {
                Text = query// "what is Symptom and CategoryName for all Incident whose ID is 1",
            };

            string URL = "http://localhost:5000/query";
            var client = new HttpClient();

            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            DataTable dt = new DataTable();
            try
            {
                var response = client.PostAsync(URL, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync().Result;
                    DataSet ds = DBHelper.GetDataFromDB(contents);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                        DBHelper.SaveSqlQuery(query, contents);
                    else
                        ErrorMessage(dt);
                }
                else
                {
                    ErrorMessage(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(dt);
            }
            IIEModel model = new IIEModel()
            {
                Text = query,
                Table = dt
            };
            return View("Client", model);
        }

        private void ErrorMessage(DataTable dt)
        {
            dt.Clear();
            dt.Columns.Clear();
            dt.Columns.Add("INFO:");
            DataRow row = dt.NewRow();
            row[0] = "Our database does not contain the information you are seeking.";
            dt.Rows.Add(row);
        }

        public IActionResult Client()
        {
            ViewData["Message"] = "Client";
            IIEModel model = new IIEModel();
            return View(model);
        }
    }
}