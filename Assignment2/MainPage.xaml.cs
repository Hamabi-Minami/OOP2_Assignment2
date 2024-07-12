using Assignment2.Models;

namespace Assignment2;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        try
        {
            string content = "nmsl";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = Path.Combine(folderPath, "test.txt");
            File.WriteAllText(path, content);
            
            Console.WriteLine(path);
            Console.WriteLine(path);
            // string path = "/Users/wenhanliu/projects/C#/Assignment2/Assignment2/MyResources/flights.csv";
            //             /Users/wenhanliu/projects/C#/Assignment2/Assignment2/MyResources/
            List<Flight> flights = Tools.ReadFlights(path);
            foreach (var flight in flights)
            {
                flight.ToString();
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}