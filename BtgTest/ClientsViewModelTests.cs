using Applicability.Shared;
using BtgPactual.ViewModels;
namespace BtgTest
{
    public class ClientsViewModelTests
    {
        [Fact]
        public async void AddCliente_ShouldAddClientToClientsCollection()
        {
            // Arrange
            var viewModel = new ClientsViewModel();
            var initialCount = viewModel.Clients.Count;

            var newClient = new ClientDTO { Name = "Alice", LastName = "Green", Age = 28, Address = "456 Avenue" };

            // Act
          await  viewModel.AddCliente(newClient);

            // Assert
            Assert.Equal(initialCount + 1, viewModel.Clients.Count);
            Assert.Contains(viewModel.Clients, client => client.Name == newClient.Name);
        }

        [Fact]
        public void EditCliente_ShouldUpdateClientInClientsCollection()
        {
            // Arrange
            var viewModel = new ClientsViewModel();
            var clientToEdit = viewModel.Clients.First();
            var updatedClient = new ClientDTO { Name = "Roberta", LastName = "Silva", Age = 26, Address = "New Street" };

            // Act
            viewModel.EditCliente(clientToEdit);
            clientToEdit.Name = updatedClient.Name;
            clientToEdit.LastName = updatedClient.LastName;
            clientToEdit.Age = updatedClient.Age;
            clientToEdit.Address = updatedClient.Address;

            // Assert
            Assert.Contains(viewModel.Clients, client => client.Name == updatedClient.Name && client.LastName == updatedClient.LastName);
        }

        [Fact]
        public void DeleteCliente_ShouldRemoveClientFromClientsCollection()
        {
            // Arrange
            var viewModel = new ClientsViewModel();
            var clientToDelete = viewModel.Clients.First();

            // Act
            viewModel.DeleteCliente(clientToDelete);

            // Assert
            Assert.DoesNotContain(viewModel.Clients, client => client.Name == clientToDelete.Name);
        }
    }

}