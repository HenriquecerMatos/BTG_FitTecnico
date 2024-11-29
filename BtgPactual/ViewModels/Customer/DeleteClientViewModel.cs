using Applicability.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BtgPactual.ViewModels;

public partial class DeleteClientViewModel : ObservableObject
{
    [ObservableProperty]
    public string _name;

    [ObservableProperty]
    public string _lastName;

    [ObservableProperty]
    public int _age;

    [ObservableProperty]
    public string _address;

    public string WarningMessage => "Atenção: Esta ação é permanente e não pode ser desfeita.";

    public Action<ClientDTO> DeleteClientAction { get; set; }

    public ClientDTO Cliente { get; set; }
    public DeleteClientViewModel(ClientDTO cliente)
    {
        if (cliente != null)
        {
            _name = cliente.Name;
            _lastName = cliente.LastName;
            Age = cliente.Age;
            Address = cliente.Address;
        }
        else
        {
            _name = string.Empty; _lastName = string.Empty; _age = 0; _address = string.Empty;
        }

        Cliente = cliente;
    }

    public DeleteClientViewModel()
    {
            
    }

    [RelayCommand]
    public async Task DeleteCliente()
    {
        if (Cliente != null)
        {
            var confirm = await App.Current.MainPage.DisplayAlert("Confirmação", $"Deseja excluir {Cliente.Name}?", "Sim", "Não");
            if (confirm)
            {
                DeleteClientAction?.Invoke(Cliente);
            }
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        App.Current.MainPage.Navigation.PopAsync();
    }
}

