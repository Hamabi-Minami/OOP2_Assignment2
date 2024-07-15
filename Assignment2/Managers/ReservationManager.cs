using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Assignment2.Models;

namespace Assignment2.Managers
{
    public static class ReservationManager
    {
        public static List<Reservation> Reservations = new List<Reservation>();
        public static List<Reservation> Results;
        public static Reservation SelectedReservation;

        // all search keys
        public static List<string> allCode => Reservations.Select(f => f.ReservationCode).Distinct().ToList();
        public static List<string> allAirline => Reservations.Select(f => f.Flight.AirLine).Distinct().ToList();
        public static List<string> allName => Reservations.Select(f => f.Name).Distinct().ToList();

        // current selected key
        public static string selectedCode = "";
        public static string selectedAirline = "";
        public static string selectedName = "";

        // make pagenations
        public static int currentCount = 0;
        public static int pageSize = 10;
        public static int currentPage = 1;
        public static List<Reservation> pagedReservations => Results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
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
            Reservations = await LoadReservations();
            Results = Reservations;
        }

        /// <summary>
        /// format LDDDD (e.g., I1234)
        /// </summary>
        /// <returns></returns>
        private static string GenerateReservationCode()
        {
            var random = new Random();
            char letter = (char)random.Next('A', 'Z' + 1);
            int number = random.Next(1000, 10000);

            return $"{letter}{number}";
        }

        public static void MakeReservation(Flight selectedFlight, string name, string citizenship)
        {
            var reservation = Reservations.Where(x => x.Flight.Seats == selectedFlight.Seats);

            if (reservation.Count()!=0)
            {
                throw new Exception("This seat is taken!");
            }

            Reservations.Add(new Reservation(selectedFlight, name, citizenship, GenerateReservationCode()));
        }

        public static async Task<List<Reservation>> FilterReservations()
        {
            var query = Reservations.AsQueryable();
            if (!string.IsNullOrEmpty(selectedCode) && !selectedCode.Equals("Any"))
            {
                query = query.Where(reservation => reservation.ReservationCode == selectedCode);
            }
            if (!string.IsNullOrEmpty(selectedAirline) && !selectedAirline.Equals("Any"))
            {
                query = query.Where(reservation => reservation.Flight.AirLine == selectedAirline);
            }
            if (!string.IsNullOrEmpty(selectedName) && !selectedName.Equals("Any"))
            {
                query = query.Where(reservation => reservation.Name == selectedName);
            }

            Results = query.ToList();

            currentPage = 1;

            return Results;
        }

        /// <summary>
        /// load reservations from csv, file saved at AppDataDirectory
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static async Task<List<Reservation>> LoadReservations(string fileName = "reservations.csv")
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (!File.Exists(filePath))
                {

                    return Reservations;
                }

                // Ensure the directory exists
                Directory.CreateDirectory(FileSystem.AppDataDirectory);

                System.Diagnostics.Debug.WriteLine("load:" + filePath);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    var headerLine = reader.ReadLine();

                    if (headerLine == null)
                    {
                        throw new InvalidOperationException("CSV file is empty.");
                    }
                    else
                    {
                        var values = headerLine.Split(',');

                        var reservation = new Reservation(new Flight(values[0], values[1], values[2], values[3], values[4],
                            values[5], Convert.ToInt32(values[6]), Convert.ToDouble(values[7])), values[8], values[9], values[10], values[11]);

                        Reservations.Add(reservation);
                    }

                    // Read the remaining lines
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var values = line.Split(',');

                            var reservation = new Reservation(new Flight(values[0], values[1], values[2], values[3], values[4],
                                values[5], Convert.ToInt32(values[6]), Convert.ToDouble(values[7])), values[8], values[9], values[10], values[11]);

                            Reservations.Add(reservation);
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

            return Reservations;
        }

        public static async Task SaveReservations(string fileName = "reservations.csv")
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Ensure the directory exists
                Directory.CreateDirectory(FileSystem.AppDataDirectory);

                System.Diagnostics.Debug.WriteLine("save:" + filePath);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var reservation in Reservations)
                    {
                        string csvLine = reservation.ToCsvString();
                        await writer.WriteLineAsync(csvLine);
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
        }

        /// <summary>
        /// get reservation by code
        /// </summary>
        /// <param name="reservationCode"></param>
        /// <returns></returns>
        private static Reservation GetReservationByCode(string reservationCode)
        {
            var reservation = Reservations.Where(x => x.ReservationCode == reservationCode).FirstOrDefault();

            return reservation;
        }

        /// <summary>
        /// soft delete reservation
        /// </summary>
        /// <param name="selectedReservation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Boolean DeleteReservation(Reservation selectedReservation)
        {

            try
            {
                var temp = GetReservationByCode(selectedReservation.ReservationCode); // get target reservation from List of Reservation

                if (temp != null)
                {
                    temp.Status = "InActive";
                }
                else
                {
                    throw new Exception($"Error occured when get Reservation with code:{selectedReservation.ReservationCode}");
                }
            }
            catch
            {
                throw new Exception($"Error occured when delete Reservation with code:{selectedReservation.ReservationCode}");
            }
            return true;
        }
    }
}
