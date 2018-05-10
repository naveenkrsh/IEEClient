using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace nlidbclient.Controllers
{


    [Route("api/[controller]")]
    public class GeneralController : ControllerBase
    {

        private const string constr = "Server=IN-L20053\\MSSQLSERVER17;Initial Catalog=HACKITSM;Persist Security Info=False;User ID=sa;Password=Password@123;MultipleActiveResultSets=False;Connection Timeout=30;";// ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        [HttpGet]
        public List<string> GetSuggetion()
        {
            string sql = "Select  Distinct Text From Generated_Sql";
            DataSet ds = DBHelper.GetDataFromDB(sql);

            List<string> list = new List<string>();
            if(ds!= null && ds.Tables.Count >0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row[0].ToString());
                }
            }
            return list;
        }
    }
}