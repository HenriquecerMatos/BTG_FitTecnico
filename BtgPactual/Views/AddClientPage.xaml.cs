using Applicability.Shared;
using BtgPactual.ViewModels;

namespace BtgPactual.Views
{
    public partial class AddClientPage : ContentPage
    {

        public AddClientPage() { }

        public AddClientPage(ClientDTO client, Action<ClientDTO> onSaveCompleted)
        {
            InitializeComponent();

            BindingContext = new AddClientViewModel(client)
            {
                OnSaveCompleted = clienteAtualizado =>
                {
                    onSaveCompleted(clienteAtualizado);
                    App.Current.MainPage.Navigation.PopAsync();
                }
            };
        }
    }
}
