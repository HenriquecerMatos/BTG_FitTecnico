using Applicability.Shared;
using BtgPactual.ViewModels;

namespace BtgPactual.Views
{
    public partial class DeleteClientPage : ContentPage
    {
        public DeleteClientPage(ClientDTO cliente, Action<ClientDTO> deleteClient)
        {
            InitializeComponent();
            BindingContext = new DeleteClientViewModel(cliente)
            {
                DeleteClientAction = clienteAtualizado =>
                {
                    deleteClient(clienteAtualizado);
                    App.Current.MainPage.Navigation.PopAsync();
                }
            };
        }
    }
}
