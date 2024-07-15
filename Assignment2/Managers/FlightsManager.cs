using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment2.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Assignment2.Managers
{
    public static class FlightsManager
    {
        // save flights info
        public static List<Flight> Flights;
        public static List<Flight> Results;
        public static Flight SelectedFlight;

        // all search keys
        public static List<string> AllFrom => Flights.Select(f => f.From).Distinct().ToList();
        public static List<string> AllTo => Flights.Select(f => f.To).Distinct().ToList();
        public static List<string> AllDay => Flights.Select(f => f.Day).Distinct().ToList();

        // current selected key
        public static string selectedFrom = "";
        public static string selectedTo = "";
        public static string selectedDay = "";

        // make pagenations
        //public static int currentCount = 0;
        public static int pageSize = 10;
        public static int currentPage = 1;
        public static List<Flight> pagedFlights => Results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        public static int totalPages => (int)Math.Ceiling((double)Results.Count / pageSize);

        public static void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
        }

        public static void NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
            }
        }

        public static async Task Init()
        {
            Flights = await LoadFlights("Flights.csv");
            Results = Flights;
        }

        /// <summary>
        /// load flights from csv
        /// the flights.csv is saved at Resources/raw/
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<List<Flight>> LoadFlights(string filePath)
        {

            Flights = new List<Flight>();
            using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);  // handle app resources

            try
            {
                using (var reader = new StreamReader(stream))
                {
                    // Read the header line
                    var headerLine = reader.ReadLine();

                    if (headerLine == null)
                    {
                        throw new InvalidOperationException("CSV file is empty.");
                    }
                    else
                    {
                        var values = headerLine.Split(',');

                        var flight = new Flight(values[0], values[1], values[2], values[3], values[4],
                            values[5], Convert.ToInt32(values[6]), Convert.ToDouble(values[7]));

                        Flights.Add(flight);
                    }

                    // Read the remaining lines
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var values = line.Split(',');

                            var flight = new Flight(values[0], values[1], values[2], values[3], values[4],
                                values[5], Convert.ToInt32(values[6]), Convert.ToDouble(values[7]));

                            Flights.Add(flight);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access to the path is denied: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                throw;
            }

            return Flights;
        }

        /// <summary>
        /// multi-keys search, use linq and lambada expression to query.
        /// </summary>
        /// <param name="flights"></param>
        /// <returns></returns>
        public static async Task<List<Flight>> FilterFlights()
        {

            var query = Flights.AsQueryable();
            if (!string.IsNullOrEmpty(selectedFrom) && !selectedFrom.Equals("Any"))
            {
                query = query.Where(flight => flight.From == selectedFrom);
            }
            if (!string.IsNullOrEmpty(selectedTo) && !selectedTo.Equals("Any"))
            {
                query = query.Where(flight => flight.To == selectedTo);
            }
            if (!string.IsNullOrEmpty(selectedDay) && !selectedDay.Equals("Any"))
            {
                query = query.Where(flight => flight.Day == selectedDay);
            }

            Results = query.ToList();

            currentPage = 1;

            return Results;
        }
    }
}
