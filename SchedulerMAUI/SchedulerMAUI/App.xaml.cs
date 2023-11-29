namespace SchedulerMAUI;

public partial class App : Application
{
    static SchedulerDatabase database;
    public App()
	{
		InitializeComponent();

		MainPage = new MainPage();
	}

    public static SchedulerDatabase Database
    {
        get
        {
            if (database == null)
            {
                database = new SchedulerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MAUISchedulerDatabase.db3"));
            }
            return database;
        }
    }
}
