using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Serialization;
using System.IO;



namespace TableToJSON
{
    class Program
    {
  
        
        static void Main(string[] args)
        {
         

            foreach (string arg in args )
            { 
                
                
                DataTable ttt = fillDataTable(arg); 
                String json = JsonConvert.SerializeObject(ttt  , new Serialization.DataTableConverter());
               // Console.Write(json);
                if (File.Exists(Path.Combine(Environment.CurrentDirectory,arg+".json")))
                {
                    File.Delete(Path.Combine(Environment.CurrentDirectory, arg + ".json"));
                }
                using (StreamWriter sw = new StreamWriter(arg +".json",true,Encoding.GetEncoding("utf-8")))
                {
                    sw.Write(json);
                }
            }
        }

        // Fill any datatable
        public static DataTable fillDataTable(string table)
        {

            String conSTR = Properties.Settings.Default.farmaConnectionString;
            string query = "SELECT * FROM " + Properties.Settings.Default.Database+".dbo." + table;
            SqlConnection sqlConn = new SqlConnection(conSTR);
            sqlConn.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            sqlConn.Close();
            return dt;
        }
       
    }
}
