using Applicability.Shared;
using BtgPactual.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BtgPactual.ViewModels;
public partial class ClientsViewModel : ObservableObject
{
    public ObservableCollection<ClientDTO> Clients { get; } = [];

    [ObservableProperty]
    private ClientDTO _selectedClient;

    public ClientsViewModel() =>
        Clients.Add(new() { Name = "Roberto", LastName = "Silva", Age = 25, Address = "Rua A, 123" });

    [RelayCommand]
    public async Task AddCliente(ClientDTO client)
    {
        await App.Current.MainPage.Navigation.PushAsync(new AddClientPage(client, cliente =>
        {
            if (cliente != null)
                Clients.Add(cliente);
        }));
    }

    [RelayCommand]
    public async Task EditCliente(ClientDTO cliente)
    {
        if (cliente != null)
        {
            await App.Current.MainPage.Navigation.PushAsync(new UpdateClientPage(cliente, updatedCliente =>
            {
                if (updatedCliente != null)
                {
                    var index = Clients.IndexOf(cliente);
                    if (index >= 0)
                    {
                        Clients[index] = updatedCliente;
                    }
                }
            }));
        }
    }

    [RelayCommand]
    public async Task DeleteCliente(ClientDTO cliente)
    {
        if (cliente != null)
        {
            if (cliente != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new DeleteClientPage(cliente, deleteClient =>
                {
                    if (deleteClient != null)
                    {
                        Clients.Remove(deleteClient); // Remove o cliente da lista após confirmação de exclusão
                    }
                }));
            }
        }
    }


    [RelayCommand]
    public async Task ConfirmDeleteCliente(ClientDTO cliente)
    {
        if (cliente != null)
        {
            await App.Current.MainPage.Navigation.PushAsync(new DeleteClientPage(cliente, deleteClient =>
            {
                if (deleteClient != null)
                {
                    Clients.Remove(deleteClient); // Remove o cliente da lista após confirmação de exclusão
                }
            }));
        }
    }

}

