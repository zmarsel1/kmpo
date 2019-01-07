using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Concrete;
using DAL.Entities;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Objects.SqlClient;

namespace Import
{
    partial class Program
    {

        /// <summary>
        /// Функция расичтывает показатели и записывает в БД
        /// </summary>
        /// <param name="in_sum">список входящих тэгов, которые будут суммироваться</param>
        /// <param name="in_div">список входящих тэгов, на сумму которых будут делить</param>
        /// <param name="outtag">тэг, по которому будет записан результат</param>
        public static void CalculateAndExport_v2(string[] in_sum, string[] in_div, string outtag)
        {
            TagDbContext context = new TagDbContext();

            Tag tag = context.Tags.Where(t => t.TagName == outtag).First();
            var values = context.TagValues
                .Where(t => t.Tag.TagName == outtag)
                .OrderByDescending(t => t.DateTime)
                .Select(t => t.DateTime);
            DateTime date = values.Count() > 0 ? values.First() : new DateTime(2012, 10, 1);//DateTime.Today; // <---- заменить DateTime на Today

            string query1 = string.Format("SELECT DateTime, SUM(Value) as Value FROM dbo.AnalogHistory WHERE TagName in ( '{0}' ) AND DateTime > @date AND Quality = 0 AND wwRetrievalMode = 'Cyclic' AND wwResolution=3600000 GROUP BY DateTime", string.Join("','", in_sum));
            string query2 = string.Format("SELECT DateTime, SUM(Value) as Value FROM dbo.AnalogHistory WHERE TagName in ( '{0}' ) AND DateTime > @date AND Quality = 0 AND wwRetrievalMode = 'Cyclic' AND wwResolution=3600000 GROUP BY DateTime", string.Join("','", in_div));
            
            List<TagValue> data1 = new List<TagValue>();
            List<TagValue> data2 = new List<TagValue>();
            
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SourceDB"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query1, conn))
                {
                    command.CommandTimeout = 240000;
                    command.Parameters.AddWithValue("@date", date);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DateTime d = reader.GetDateTime(0);
                        data1.Add(new TagValue() { DateTime = reader.GetDateTime(0), Value = reader.GetDouble(1) });
                    }
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(query2, conn))
                {
                    command.CommandTimeout = 240000;
                    command.Parameters.AddWithValue("@date", date);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        data2.Add(new TagValue() { DateTime = reader.GetDateTime(0), Value = reader.GetDouble(1) });
                    }
                    reader.Close();
                }
            }

            var data_out = data1
                .Join(data2,
                    d1 => d1.DateTime,
                    d2 => d2.DateTime,
                    (d1, d2) => new { DateTime = d1.DateTime, Value = (d2.Value == 0 ? 0 : d1.Value / d2.Value) });

            foreach (var row in data_out)
            {
                TagValue value = new TagValue() { DateTime = row.DateTime, Tag = tag, Value = row.Value };
                context.TagValues.Add(value);
            }
            context.SaveChanges();
        }

        public static void ImportVTI(int vti, string tagname)
        {
            var context = new TagDbContext();
            Tag tag = context.Tags.Where(t => t.TagName == tagname).First();

            var values = context.TagValues
                .Where(t => t.Tag.TagName == tagname)
                .OrderByDescending(t => t.DateTime)
                .Select(t => t.DateTime);
            DateTime date = values.Count() > 0 ? values.First() : new DateTime(2012, 10, 1);//DateTime.Today; // <---- заменить DateTime на Today

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["SourceDB"].ConnectionString))
            {
                sql.Open();
                using (SqlCommand cmdClear = new SqlCommand("ep_AskVTIdata", sql))
                {
                    cmdClear.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdClear.Parameters.AddWithValue("@cmd", "Clear");
                    cmdClear.Parameters.AddWithValue("@idReq", 60000);

                    cmdClear.ExecuteNonQuery();
                }

                using (SqlCommand cmdSelect = new SqlCommand("ep_AskVTIdata", sql))
                {
                    cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdSelect.Parameters.AddWithValue("@cmd", "ListAdd");
                    cmdSelect.Parameters.AddWithValue("@idVTI", vti);
                    cmdSelect.Parameters.AddWithValue("@TimeStart", date);
                    cmdSelect.Parameters.AddWithValue("@ShiftBeg", 60);
                    cmdSelect.Parameters.AddWithValue("@idReq", 60000);

                    cmdSelect.ExecuteNonQuery();
                }

                string query = @"SELECT v1.TimeSQL, v1.idVTI, v1.ValueFl+ v2.ValueFl as Value
                                FROM VTIdataList v1
                                INNER JOIN VTIdataList v2 ON v1.TimeSQL = DATEADD(minute, 30, v2.TimeSQL)
                                                             and DATEPART(minute, v1.TimeSQL) != 30
                                                             and v1.idVTI = v2.idVTI
                                WHERE v1.idVTI=@vti and v1.idReq=@idReq and v1.ValueFl IS NOT NULL and v2.ValueFl IS NOT NULL";
                using (SqlCommand cmdGetData = new SqlCommand(query, sql))
                {
                    cmdGetData.Parameters.AddWithValue("@vti", vti);
                    cmdGetData.Parameters.AddWithValue("@idReq", 60000);

                    var reader = cmdGetData.ExecuteReader();
                    while (reader.Read())
                    {
                        context.TagValues.Add(new TagValue() { DateTime = (DateTime)reader["TimeSQL"], TagID = tag.TagID, Value = (double)reader["Value"] });
                    }
                    context.SaveChanges();
                }
            };
        }

        public static void ImportVTISum(int[] vtis, string tagname)
        {
            Console.WriteLine("Импорт факта...");
            using (var context = new TagDbContext()) {
                Tag tag = context.Tags.Where(t => t.TagName == tagname).First();

                var values = context.TagValues
                    .Where(t => t.Tag.TagName == tagname)
                    .OrderByDescending(t => t.DateTime)
                    .Select(t => t.DateTime);
                DateTime date = values.Count() > 0 ? values.First() : new DateTime(2012, 10, 1);//DateTime.Today; // <---- заменить DateTime на Today

                using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["SourceDB"].ConnectionString)) {
                    sql.Open();
                    using (SqlCommand cmdClear = new SqlCommand("ep_AskVTIdata", sql)) {
                        cmdClear.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdClear.Parameters.AddWithValue("@cmd", "Clear");
                        cmdClear.Parameters.AddWithValue("@idReq", 60000);

                        cmdClear.ExecuteNonQuery();
                    }
                    foreach (var vti in vtis) {
                        using (SqlCommand cmdSelect = new SqlCommand("ep_AskVTIdata", sql)) {
                            cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdSelect.Parameters.AddWithValue("@cmd", "ListAdd");
                            cmdSelect.Parameters.AddWithValue("@idVTI", vti);
                            cmdSelect.Parameters.AddWithValue("@TimeStart", date);
                            cmdSelect.Parameters.AddWithValue("@ShiftBeg", 60);
                            cmdSelect.Parameters.AddWithValue("@idReq", 60000);

                            cmdSelect.ExecuteNonQuery();
                        }
                    }
                    string query = @"SELECT v1.TimeSQL, SUM(v1.ValueFl+ v2.ValueFl) as Value
                                FROM VTIdataList v1
                                INNER JOIN VTIdataList v2 ON v1.TimeSQL = DATEADD(minute, 30, v2.TimeSQL)
                                                             and DATEPART(minute, v1.TimeSQL) != 30
                                                             and v1.idVTI = v2.idVTI
                                WHERE v1.idReq=@idReq and v1.ValueFl IS NOT NULL and v2.ValueFl IS NOT NULL
                                GROUP BY v1.TimeSQL";
                    using (SqlCommand cmdGetData = new SqlCommand(query, sql)) {
                        cmdGetData.Parameters.AddWithValue("@idReq", 60000);
                        var reader = cmdGetData.ExecuteReader();
                        while (reader.Read()) {
                            context.TagValues.Add(new TagValue() { DateTime = (DateTime)reader["TimeSQL"], TagID = tag.TagID, Value = (double)reader["Value"] });
                        }
                        context.SaveChanges();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            /*
             * Вариант 1 
            Dictionary<int, string> VTIs = new Dictionary<int,string>()
            {
                {123123, "1231"},
                {123123, "1231"},
                {123123, "1231"},
                {123123, "1231"},
            };
            StreamWriter log = new StreamWriter(File.Open("log.txt", FileMode.OpenOrCreate));
            foreach (var vti in VTIs.Keys)
            {
                try
                {
                    ImportVTI(vti, VTIs[vti]);
                }
                catch(Exception e)
                {
                    Console.WriteLine("{0} расчёт факта {1}: {2}/n{3}", DateTime.Now, e.Message);
                    log.WriteLine(string.Format("{0} расчёт факта {1}: {2}/n{3}", DateTime.Now, e.Message));
                }
            }
            */
            StreamWriter log = new StreamWriter(File.Open("log.txt", FileMode.OpenOrCreate));
            int[] vtis = new int[] { 1024, 1016, 1032, 1040, 1004, 1048, 1056, 1064, 1072, 1080, 1088, 1684, 1748 };
            try
            {
                ImportVTISum(vtis, "ENERGY_FACTORY_SUM_FACT");
                //ForecastAndExport_v2("ENERGY_FACTORY_SUM_FORECAST", "ENERGY_FACTORY_SUM_FACT", 2);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} импорт факт: {1}", DateTime.Now, e.Message);
                log.WriteLine(string.Format("{0} импорт факт: {1}", DateTime.Now, e.Message));
                log.WriteLine(e.ToString());
            }
        }
    }
}
