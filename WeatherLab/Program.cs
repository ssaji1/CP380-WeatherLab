using System;
using System.Linq;


namespace WeatherLab
{
    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var total_2020_precipitation = (from values in measurements where values.year == 2020 select values.precipitation).Sum();

            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation} mm\n");

            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //

            //
            // Cooling degree days have a mean temp of >=18C
            //

            var meanValue = measurements.GroupBy(
                row => row.year).Select(
                meanValue => new {
                    Year = meanValue.Key, 
                    Hdd = meanValue.Where(
                        row => row.meantemp < 18).Count(),
                    Cdd = meanValue.Where(
                        row => row.meantemp >= 18).Count()
                });

            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //
            Console.WriteLine("Year\tHDD\tCDD");

            foreach (var i in meanValue)
            {
                Console.WriteLine($"{i.Year}\t{i.Hdd}\t{i.Cdd}");
            }

            var variableDays = from value in measurements
                           orderby (value.maxtemp - value.mintemp) descending
                           select new
                           {
                               yearMonthDay = $"{value.year}-{value.month:d2}-{value.day:d2}",
                               delta = (value.maxtemp - value.mintemp)
                           };

            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");
            var count = 0;
            foreach (var i in variableDays)
            {
                switch(count)
                {
                    case 0:
                        Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                        count++;
                        break;
                    case 1:
                        Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                        count++;
                        break;
                    case 2:
                        Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                        count++;
                        break;
                    case 3:
                        Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                        count++;
                        break;
                    case 4:
                        Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                        count++;
                        break;
                    default:
                        break;
                }
                //                while (count < 5)
                //              {
                //                Console.WriteLine($"{i.yearMonthDay}\t{i.delta}");
                //              count++;
                //        }
            }
        }
    }
}
