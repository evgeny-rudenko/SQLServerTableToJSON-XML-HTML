using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

using ClosedXML.Excel;

namespace ExportToXLS
{
    class Program
    {
        static void Main(string[] args)
        {

            string TableName = "sales";
            if (args.Length !=0)
            {
                TableName = args[0];
            }


            string gsql, fname;

            if (TableName.ToUpper().Contains(".SQL"))
            {
                gsql = File.ReadAllText(TableName);
                fname = TableName.Replace(".sql","");

            }
            else
            {
                gsql = "select * from " + TableName;
                fname = TableName;
            }
            
            AddLine(gsql, "Log.SQL");
            Console.WriteLine("Формирую файл ");
            DataTable goods = fillDataTable(gsql);
            DataSet ds = new DataSet();
            ds.Tables.Add(goods);
            

            DataTable dt;

            /*var workbook = new XLWorkbook("Номенклатура.xlsx");
 var worksheet = workbook.Worksheet(1);*/
            dt = fillDataTable(gsql);
            ExportDataToExcel(dt,  fname + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
        }


        /// <summary>
        /// Выгрузка отчета в файл
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="destination"></param>
        private static void ExportDataToExcel(DataTable dt, string destination)
        {
            var workbook = new ClosedXML.Excel.XLWorkbook();
            //var worksheet = workbook.Worksheets.Add(dt.TableName);
            //var worksheet = workbook.Worksheet(1);
            var worksheet = workbook.Worksheets.Add("Лист1");
            worksheet.Cell(1, 1).InsertTable(dt);
            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(Path.Combine(Properties.Settings.Default.DestinationFolder, destination));
            workbook.Dispose();
        }

        /// <summary>
        /// Формируем Datatable из запроса или названия таблицы/представления
        /// </summary>
        /// <param name="table">Название таблицы или представления. Или запрс SQL </param>
        /// <returns></returns>

        public static DataTable fillDataTable(string table)
        {
            string query = table;

            //костыль - так лучше не делать
            if (table.ToUpper().Contains("SELECT") == true)
            {
                query = table;
            }
            else
            {
                query = "SELECT * FROM " + Properties.Settings.Default.Database + ".dbo." + table;
            }



            String conSTR = Properties.Settings.Default.farmaConnectionString;
            SqlConnection sqlConn = new SqlConnection(conSTR);

            sqlConn.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConn);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            sqlConn.Close();
            return dt;
        }
        /// <summary>
        /// Загрузка из ресурсов текстовых файлов - чтобы скрипты были внутри программы
        /// </summary>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static string GetSQLFromResource(string ResName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = ResName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
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
            using (StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory().ToString(), sFile), true, Encoding.GetEncoding(1251)))
            {


                sw.Write(LineToAdd);

            }


            return 0;
        }

    }
}
