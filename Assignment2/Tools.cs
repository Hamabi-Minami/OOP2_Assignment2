using Assignment2.Models;

namespace Assignment2;

public class Tools
{
    public static List<Flight> ReadFlights(string filePath)
    {

        List<Flight> flights = new List<Flight>();

        try
        {   
            using (var reader = new StreamReader(filePath))
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
                        Console.WriteLine(values);
                        System.Diagnostics.Debug.WriteLine(values);
                    
                        var flight = new Flight(values[0], values[1], values[2],values[3],values[4], 
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
}