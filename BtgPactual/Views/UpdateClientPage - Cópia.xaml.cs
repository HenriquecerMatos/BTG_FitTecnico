using Applicability.Shared;
using BtgPactual.ViewModels;

namespace BtgPactual.Views
{
    public partial class UpdateClientPage : ContentPage
    {

        public UpdateClientPage() { }

        public UpdateClientPage(ClientDTO client, Action<ClientDTO> onSaveCompleted)
        {
            InitializeComponent();

            BindingContext = new UpdateClientViewModel(client)
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
