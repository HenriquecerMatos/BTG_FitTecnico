using Applicability.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BtgPactual.ViewModels;

public partial class AddClientViewModel : ObservableObject
{
    [ObservableProperty]
    public string _name;

    [ObservableProperty]
    public string _lastName;

    [ObservableProperty]
    public int _age;

    [ObservableProperty]
    public string _address;

    public Action<ClientDTO> OnSaveCompleted { get; set; }

    public AddClientViewModel(ClientDTO cliente)
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
    }
    public AddClientViewModel()
    {

    }

    [RelayCommand]
    public void Save()
    {
        // Validações simples
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(LastName))
        {
            App.Current.MainPage.DisplayAlert("Erro", "Nome e Sobrenome são obrigatórios.", "OK");
            return;
        }

        if (Age <= 0)
        {
            App.Current.MainPage.DisplayAlert("Erro", "Idade deve ser maior que 0.", "OK");
            return;
        }

        var NewClient = new ClientDTO
        {
            Name = Name,
            LastName = LastName,
            Age = Age,
            Address = Address
        };

        OnSaveCompleted?.Invoke(NewClient);
    }

    [RelayCommand]
    public void Cancel()
    {
        // Apenas fecha a tela
        App.Current.MainPage.Navigation.PopAsync();
    }
}

