using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TableToXML
{
    class Program
    {

        static void Main(string[] args)
        {


            foreach (string arg in args)
            {

                //это костыль
                String DtName = arg;
                DataTable ttt = fillDataTable(arg);
                if (DtName == "v_goods_json")
                {
                    DtName = "goods";
                }

                if (DtName=="v_remains")
                {
                    DtName = "remains";
                }
                
                ttt.TableName = DtName;
                ttt.WriteXml(DtName + ".xml");
                //  String json = JsonConvert.SerializeObject(ttt, new Serialization.DataTableConverter());
                //Console.Write(json);
            }
        }

        // Fill any datatable
        public static DataTable fillDataTable(string table)
        {
            String DBaseName = Properties.Settings.Default.dBaseName;
            String conSTR = Properties.Settings.Default.farmaConnectionString;
            string query = "SELECT * FROM "+DBaseName+".dbo." + table;
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
