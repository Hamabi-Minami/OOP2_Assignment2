using Assignment2.Models;

namespace Assignment2;

public class Tools
{
    /// <summary>
    /// read flights from csv
    /// the flights.csv is saved at Resources/raw/
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<List<Flight>> ReadFlights(string filePath)
    {

        List<Flight> flights = new List<Flight>();
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

                // Read the remaining lines
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(',');

                        var flight = new Flight(values[0], values[1], values[2], values[3], values[4],
                            values[5], Convert.ToInt32(values[6]), Convert.ToDouble(values[7]));

                        flights.Add(flight);
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

        return flights;
    }

    /// <summary>
    /// multi-keys search, use linq and lambada expression to query.
    /// </summary>
    /// <param name="flights"></param>
    /// <returns></returns>
    public static async Task<List<Flight>> FilterFlights(List<Flight> flights, string keyFrom, string keyTo, string keyDay) {

        var query = flights.AsQueryable();
        if (!string.IsNullOrEmpty(keyFrom) && !keyFrom.Equals("Any"))
        {
            query = query.Where(flight => flight.From == keyFrom);
        }
        if (!string.IsNullOrEmpty(keyTo) && !keyTo.Equals("Any"))
        {
            query = query.Where(flight => flight.To == keyTo);
        }
        if (!string.IsNullOrEmpty(keyDay) && !keyDay.Equals("Any"))
        {
            query = query.Where(flight => flight.Day == keyDay);
        }

        var results = query.ToList();

        return results;
    }
}