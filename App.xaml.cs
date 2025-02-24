using SoundBrewPOS.Data;

namespace SoundBrewPOS
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;
        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            _databaseService = databaseService;

            Task.Run(async () => await databaseService.InitializeDatabaseAsync())
                    .GetAwaiter().GetResult();          
        }



        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}