using Assignment2.Models;

namespace Assignment2;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
        List<Flight> flights = new List<Flight>();
        //Tools.ReadFlights("C:\\Users\\12206\\Desktop\\OOP2 assignments\\OOP2_Assignment2\\Assignment2\\Resources\\Raw\\flights.csv");

        //LoadMauiAsset(flights);

        
    }
    
    
}