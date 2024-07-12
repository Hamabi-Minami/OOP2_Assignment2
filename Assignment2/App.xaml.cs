namespace Assignment2;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
        LoadMauiAsset();
    }
    
    async Task LoadMauiAsset()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("Flights.csv");
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();
    }
}