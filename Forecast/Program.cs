using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
using DAL.Concrete;
using System.Data.Objects.SqlClient;
using System.IO;

namespace Forecast
{
    partial class Program
    {
        public static void ForecastAndExport_v5(string forecast_tag, string data_tag, int forecastPerspective = 2)
        {
            string tag_name = forecast_tag;
            DateTime fdate = DateTime.Today.AddDays(forecastPerspective); // дата прогноза

            List<double> input = new List<double>(); // входной вектор
            Dictionary<DateTime, double> solution = new Dictionary<DateTime, double>(); // прогноз

            var context = new TagDbContext();

            var model = CreateSomModel_v3(data_tag, fdate.DayOfWeek);

            var data = context.TagValues
                                   .Where(t => t.Tag.TagName.Trim() == data_tag && (SqlFunctions.DatePart("weekday", t.DateTime) - 1) == (int)fdate.DayOfWeek)
                                   .OrderBy(v => v.DateTime)
                                   .Select(v => v.Value)
                                   .Take(24 + retrospective + 1)
                                   .AsEnumerable()
                                   .Reverse()
                                   .ToArray();

            for (int i = 0; i < 24; i++)
            {
                var vector = data.Skip(i).Take(retrospective).ToArray();
                var value = model.Compute(vector)[0];
                solution[fdate.AddHours(i + 1)] = value;
            }

            ExportToDB(tag_name, solution, context);
        }
        public static void ForecastAndExport_v4(string forecast_tag, string data_tag, int forecastPerspective = 2)
        {
            string tag_name = forecast_tag;
            DateTime fdate = DateTime.Today.AddDays(forecastPerspective); // дата прогноза

            List<double> input = new List<double>(); // входной вектор
            Dictionary<DateTime, double> solution = new Dictionary<DateTime, double>(); // прогноз

            var context = new TagDbContext();

            var model = CreateSomModel_v2(data_tag);

            var data = context.TagValues
                                   .Where(t => t.Tag.TagName.Trim() == data_tag && (SqlFunctions.DatePart("weekday", t.DateTime) - 1) == (int)fdate.DayOfWeek)
                                   .OrderBy(v => v.DateTime)
                                   .Select(v => v.Value)
                                   .Take(24 + retrospective + 1)
                                   .AsEnumerable()
                                   .Reverse()
                                   .ToArray();

            for (int i = 0; i < 24; i++)
            {
                var vector = data.Skip(i).Take(retrospective).ToArray();
                var value = model.Compute(vector)[0];
                solution[fdate.AddHours(i+1)] = value;
            }

            ExportToDB(tag_name, solution, context);
        }

        private static void ExportToDB(string tag_name, Dictionary<DateTime, double> solution, TagDbContext context)
        {
            foreach (DateTime key in solution.Keys)
            {
                var value = context.TagValues.Where(v => v.Tag.TagName == tag_name && v.DateTime == key).FirstOrDefault();
                if (value != null)
                {
                    value.Value = Math.Round(solution[key], 2);
                }
                else
                {
                    Tag tag = context.Tags.Where(t => t.TagName == tag_name).First();
                    value = new TagValue() { DateTime = key, Tag = tag, Value = Math.Round(solution[key], 2) };
                    context.TagValues.Add(value);
                }
                context.SaveChanges();
            }
        }
        public static void ForecastAndExport_v3(string forecast_tag, string data_tag, int forecastPerspective = 2)
        {
            string tag_name = forecast_tag;
            double[] vMax, vMin;
            DateTime fdate = DateTime.Today.AddDays(forecastPerspective); // дата прогноза
            SOM model = CreateSomModel(data_tag, out vMax, out vMin);
            Dictionary<DateTime, double> solution = new Dictionary<DateTime, double>(); // прогноз

            TagDbContext context = new TagDbContext();

            var data = context.TagValues
                                .Where(t => t.Tag.TagName.Trim() == data_tag)
                                .OrderByDescending(v => v.DateTime)
                                .Select(v => new { v.DateTime.Hour, Day = SqlFunctions.DatePart("weekday", v.DateTime) + 1, Value = v.Value })
                                .Take(24 + 3 + 1)
                                .AsEnumerable()
                                .Reverse()
                                .ToArray();

            for (int index = 0; index < 24; index++)
            {
                var vector = new double[] { 
                    (double)(vMax[0] - (double)data[index].Hour) / (vMax[0] - vMin[0]),
                    (double)(vMax[1] - (double)data[index].Day) /  (vMax[1] - vMin[1]),
                    (double)(vMax[2] - data[index + 1].Value) / (vMax[2] - vMin[2]),
                    (double)(vMax[3] - data[index + 2].Value) / (vMax[3] - vMin[3]),
                    (double)(vMax[4] - data[index + 3].Value) / (vMax[4] - vMin[4]) };
                var value = model.Compute(vector)[0];
                solution[fdate.AddHours(data[index].Hour)] = value;
            }

            ExportToDB(tag_name, solution, context);
        }

        public static void ForecastAndExport_v2(string forecast_tag, string data_tag, int forecastPerspective = 2)
        {
            string tag_name = forecast_tag;
            DateTime fdate = DateTime.Today.AddDays(forecastPerspective); // дата прогноза

            List<double> input = new List<double>(); // входной вектор
            Dictionary<DateTime, double> solution = new Dictionary<DateTime, double>(); // прогноз

            var context = new TagDbContext();
            
            //double yMax, yMin;
            //if (fdate.DayOfWeek == DayOfWeek.Saturday || fdate.DayOfWeek == DayOfWeek.Sunday) //составляем модель на субботу и воскресенье
            //{
            //    Console.WriteLine("Создание модели.");
            //    var model = CreateTimeSeriesModel_v2(data_tag, out yMax, out yMin, fdate.DayOfWeek);
            //    Console.WriteLine("Модели создана.");
            //    Console.WriteLine("Подготовка входных данных.");
            //    input = context.TagValues
            //        .Where(t => t.Tag.TagName == data_tag && (SqlFunctions.DatePart("weekday", t.DateTime) - 1) == (int)fdate.DayOfWeek)
            //        .OrderByDescending(t => t.DateTime)
            //        .Select(v => (yMax - v.Value) / (yMax - yMin))
            //        .Take(retrospective)
            //        .AsEnumerable()
            //        .Reverse()
            //        .ToList();

            //    Console.WriteLine("Входные данные готовы.");
            //    Console.WriteLine("Прогнозирование.");

            //    for (int i = 1; i <= 24; i++)
            //    {
            //        double model_result = model.Compute(input.ToArray())[0];
            //        double result = yMax - model_result * (yMax - yMin);
            //        solution[fdate.AddHours(i)] = result;
            //        input.RemoveAt(0);
            //        input.Add(model_result);
            //    }
            //}
            //else // составляем модель на будние дни
            //{
                DateTime startDate = fdate.AddMonths(-1); //вычитаем 
                var tender = context.TagValues
                             .Where(tv => tv.Tag.TagName == data_tag && tv.DateTime > startDate
                                   && (SqlFunctions.DatePart("weekday", tv.DateTime) - 1) == (int)fdate.DayOfWeek)
                             .GroupBy(tv => SqlFunctions.DatePart("Hour", tv.DateTime))
                             .Select(t => new { Hour = t.Key, Value = t.Average(tv => tv.Value) })
                             .ToArray();
                foreach (var entity in tender)
                {
                    solution[fdate.AddHours((int)entity.Hour)] = entity.Value;
                }
            //}

            Console.WriteLine("Запись прогноза...");
            ExportToDB(tag_name, solution, context);
        }

        public static void ForecastAndExport(string forecast_tag, string data_tag, int forecastPerspective = 2)
        {
            string tag_name = forecast_tag;

            double yMax, yMin;
            var model = CreateTimeSeriesModel(data_tag, out yMax, out yMin);
            var context = new TagDbContext();

            var values = context.TagValues
                .Where(t => t.Tag.TagName == data_tag)
                .OrderByDescending(t => t.DateTime)
                .Select(t => t.DateTime);
            DateTime date = values.First();
            DateTime perspective = date.AddHours((forecastPerspective + 1) * 24); // прогнозируем на forecastPerspective часов вперёд


            List<double> input = new List<double>();
            input = context.TagValues
                .Where(t => t.Tag.TagName == data_tag)
                .OrderByDescending(t => t.DateTime)
                .Select(v => (yMax - v.Value) / (yMax - yMin))
                .Take(retrospective)
                .AsEnumerable()
                .Reverse()
                .ToList();
            Dictionary<DateTime, double> solution = new Dictionary<DateTime, double>();
            while (date <= perspective)
            {
                date = date.AddHours(1);
                double model_result = model.Compute(input.ToArray())[0];
                double result = yMax - model_result * (yMax - yMin);
                solution[date] = result;
                input.RemoveAt(0);
                input.Add(model_result);
            }
            ExportToDB(tag_name, solution, context);
        }


        static void Main(string[] args)
        {
            StreamWriter log = new StreamWriter(File.Open("forecast.log", FileMode.OpenOrCreate));
            try
            {
                string model = args.Length > 0 ? args[0] : "default"; 
                switch(model){
                    case "model1":
                        ForecastAndExport("ENERGY_FACTORY_SUM_FORECAST", "ENERGY_FACTORY_SUM_FACT", 2);
                        break;
                    case "model2":
                        ForecastAndExport_v2("ENERGY_FACTORY_SUM1_FORECAST", "ENERGY_FACTORY_SUM_FACT", 2);
                        break;
                    case "model3":
                        ForecastAndExport_v4("ENERGY_FACTORY_SUM2_FORECAST", "ENERGY_FACTORY_SUM_FACT", 2);
                        break;
                    default:
                        ForecastAndExport_v5("ENERGY_FACTORY_SUM_FORECAST", "ENERGY_FACTORY_SUM_FACT", 2);
                        break;
                }
            }
            catch (Exception e)
            {
                log.WriteLine(string.Format("{0} : {1}", DateTime.Now, e.Message));
                log.WriteLine(e.ToString());
            }
        }
    }
}
