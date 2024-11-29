using Applicability.Shared;
using BtgPactual.ViewModels;
using Moq;
namespace BtgTest
{

    public class DeleteClientViewModelTests
    {
        [Fact]
        public async Task DeleteCliente_ShouldInvokeDeleteClientAction()
        {
          
            var client = new ClientDTO { Name = "John", LastName = "Doe", Age = 30, Address = "123 Street" };
            var viewModel = new DeleteClientViewModel(client);
            var deleteClientActionCalled = false;

            viewModel.DeleteClientAction = (deletedClient) =>
            {
                deleteClientActionCalled = true;
                Assert.Equal(client.Name, deletedClient.Name);
            };

            
            var mockPage = new Mock<Page>();
            mockPage.Setup(p => p.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(true); 

           
            await viewModel.DeleteCliente();

           
            Assert.True(deleteClientActionCalled);
        }

        [Fact]
        public async Task DeleteCliente_Cancel_ShouldNotInvokeDeleteClientAction()
        {
           
            var client = new ClientDTO { Name = "John", LastName = "Doe", Age = 30, Address = "123 Street" };
            var viewModel = new DeleteClientViewModel(client);
            var deleteClientActionCalled = false;

            viewModel.DeleteClientAction = (deletedClient) =>
            {
                deleteClientActionCalled = true;
            };

          
            var mockPage = new Mock<Page>();
            mockPage.Setup(p => p.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(false);  

          
            await viewModel.DeleteCliente();

           
            Assert.False(deleteClientActionCalled);
        }
    }

}