using BtgPactual.ViewModels;

namespace BtgPactual.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ClientsViewModel();
        }
    }

}
