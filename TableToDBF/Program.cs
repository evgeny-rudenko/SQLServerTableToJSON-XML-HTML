using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfWriter; //прямая запись DBF без сторонних драйверов
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.InteropServices;


namespace TableToDBF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Не передан файл с запросом");
                return;
            }

            string gsql;
            DataTable dt;
            Console.WriteLine("Загрузка запроса");
            Console.WriteLine(DateTime.Now.ToString());
            gsql = File.ReadAllText(args[0]); //GetSQLFromResource("MedisenDocExport.DocumentData.sql");
            
            Console.WriteLine("Исполнение запроса");
            Console.WriteLine(DateTime.Now.ToString());
            dt = fillDataTable(gsql);
            Console.WriteLine(DateTime.Now.ToString());
            Console.WriteLine("Загрузка завершена");

            


                //Console.WriteLine(dt.Rows.Count);
                string fname = "result.dbf";

            
                //DocEntry de = new DocEntry(doc);
                //if (docs.Contains(de)
                Console.WriteLine(fname);
            // Console.ReadKey();

            //фильтруем по уникальному номеру документа
            //DataRow[] dr = dt.Select("N_NACL = '" + doc + "'");
            //DataTable dt1 = dr.CopyToDataTable();
            Console.WriteLine("Начали записывать DBF");

                DbfWriter.DbfWriterFast.Write(dt, Properties.Settings.Default.DestinationFolder, fname);
            Console.WriteLine("Записали DBF");


        }
        /// <summary>
        /// Формируем Datatable из запроса или названия таблицы/представления
        /// </summary>
        /// <param name="table">Название таблицы или представления. Или запрс SQL </param>
        /// <returns></returns>
        public static DataTable fillDataTable(string table)
        {
            string query = table;

                query = table;
            



            String conSTR = Properties.Settings.Default.farmaConnectionString;
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
