using System.Data;
using System.Data.SqlClient;

namespace nlidbclient.Controllers
{
    public static class DBHelper
    {
        private const string constr = "Server=IN-L20053\\MSSQLSERVER17;Initial Catalog=HACKITSM;Persist Security Info=False;User ID=sa;Password=Password@123;MultipleActiveResultSets=False;Connection Timeout=30;";// ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        public static DataSet GetDataFromDB(string contents)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                //string query = "SELECT * FROM Customers";
                using (SqlCommand cmd = new SqlCommand(contents,con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public static void SaveSqlQuery(string query, string sqlQuery)
        {
            sqlQuery = sqlQuery.Replace("'","''");
            string sqlStatement = $"Insert Into Generated_Sql(Text,Sql) Values('{query}','{sqlQuery}')";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement,con))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}