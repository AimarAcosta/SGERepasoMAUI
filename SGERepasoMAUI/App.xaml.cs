namespace SGERepasoMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Envolver en NavigationPage para permitir navegación híbrida Blazor -> XAML
            return new Window(new NavigationPage(new MainPage())) { Title = "SGERepasoMAUI" };
        }
    }
}
