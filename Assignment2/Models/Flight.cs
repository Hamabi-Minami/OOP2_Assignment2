namespace Assignment2.Models;

public class Flight
{
    public  string FlightCode { get; set; }
    public string AirLine { get; set; }
    
    public string From { get; set; }
    public string To { get; set; }
    public string Day { get; set; }
    public string Time { get; set; }
    
    public int Seats { get; set; }
    public double Cost { get; set; }

    public Flight(string flightCode, string airLine, string From, string To, string day, string time, int seats, double cost)
    {
        this.FlightCode = flightCode;
        this.AirLine = airLine;
        this.From = From;
        this.To = To;
        this.Day = day;
        this.Time = time;
        this.Seats = seats;
        this.Cost = cost;
    }

    public void ToString()
    {
        Console.WriteLine($"Flight Code:{this.FlightCode, -10}\t" +
                          $"AirLine:{this.AirLine, -20}\t" +
                          $"From:{this.From, -10}" +
                          $"To:{this.To, -10}" +
                          $"Day:{this.Day, -10}\t" +
                          $"Time:{this.Time, -10}\t" +
                          $"Seats:{this.Seats, -10}" +
                          $"Cost:{this.Cost, -10}\t");
    }
}