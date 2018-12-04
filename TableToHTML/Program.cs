using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TableToHTML
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

                if (DtName == "v_remains")
                {
                    DtName = "remains";
                }

                ttt.TableName = DtName;
                ///ttt.WriteXml(DtName + ".xml");
                //  String json = JsonConvert.SerializeObject(ttt, new Serialization.DataTableConverter());
                //Console.Write(json);

                String HtmlOut = toHTML_Table(ttt);

                AddLine(HtmlOut, DtName + ".html");
            }

        }



        /// <summary>
        /// Записываем в файл строку
        /// </summary>
        /// <param name="LineToAdd">Строка или строки для записи</param>
        /// <param name="sFile">Наименование файла</param>
        /// <returns></returns>
        private static int AddLine(String LineToAdd, String sFile)
        {

            Encoding enc = Encoding.GetEncoding(1251);
            //using (StreamWriter sw = File.AppendText(Path.Combine( Environment.CurrentDirectory, sFile)))
            using (StreamWriter sw = new StreamWriter(sFile, true, Encoding.GetEncoding(1251)))
            {


                sw.WriteLine(LineToAdd);

            }


            return 0;
        }



        /// <summary>
        /// Build HTML from Data Table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string toHTML_Table(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";///`enter code here`

            StringBuilder builder = new StringBuilder();
            builder.Append("<html>");
            builder.Append("<head>");
            builder.Append("<title>");
            builder.Append("Page-");
            builder.Append(Guid.NewGuid().ToString());
            builder.Append("</title>");
            builder.Append("</head>");
            builder.Append("<body>");
            builder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
            builder.Append("style='border: solid 1px Silver; font-size: x-small;'>");
            builder.Append("<tr align='left' valign='top'>");
            foreach (DataColumn c in dt.Columns)
            {
                builder.Append("<td align='left' valign='top'><b>");
                builder.Append(c.ColumnName);
                builder.Append("</b></td>");
            }
            builder.Append("</tr>");
            foreach (DataRow r in dt.Rows)
            {
                builder.Append("<tr align='left' valign='top'>");
                foreach (DataColumn c in dt.Columns)
                {
                    builder.Append("<td align='left' valign='top'>");
                    builder.Append(r[c.ColumnName]);
                    builder.Append("</td>");
                }
                builder.Append("</tr>");
            }
            builder.Append("</table>");
            builder.Append("</body>");
            builder.Append("</html>");

            return builder.ToString();
        }

        /// <summary>
        /// Fill Data Table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        // Fill any datatable

        public static DataTable fillDataTable(string table)
        {

            String conSTR = Properties.Settings.Default.farmaConnectionString;
            string query = "SELECT * FROM " + table;
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
