using Applicability.Shared;
using BtgPactual.ViewModels;
using Moq;
namespace BtgTest
{
    public class AddClientViewModelTests
    {
        [Fact]
        public async Task Save_ShouldDisplayAlert_WhenNameIsEmpty()
        {
            var client = new ClientDTO { Name = "", LastName = "Doe", Age = 30, Address = "123 Street" };
            var viewModel = new AddClientViewModel(client);

          
            var mockPage = new Mock<Page>();
            mockPage.Setup(p => p.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(true);

        
            viewModel.Save();

        
            mockPage.Verify(p => p.DisplayAlert("Erro", "Nome e Sobrenome são obrigatórios.", "OK"), Times.Once);
        }

        [Fact]
        public async Task Save_ShouldDisplayAlert_WhenAgeIsZeroOrLess()
        {
            
            var client = new ClientDTO { Name = "John", LastName = "Doe", Age = 0, Address = "123 Street" };
            var viewModel = new AddClientViewModel(client);

          
            var mockPage = new Mock<Page>();
            mockPage.Setup(p => p.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);  // Simulate the alert being shown to the user

         
            viewModel.Save();

           
            mockPage.Verify(p => p.DisplayAlert("Erro", "Idade deve ser maior que 0.", "OK"), Times.Once);
        }

        [Fact]
        public void Save_ShouldInvokeOnSaveCompleted_WhenValidData()
        {
          
            var client = new ClientDTO { Name = "John", LastName = "Doe", Age = 30, Address = "123 Street" };
            var viewModel = new AddClientViewModel(client);

            var saveCompletedCalled = false;
            viewModel.OnSaveCompleted = (newClient) =>
            {
                saveCompletedCalled = true;
                Assert.Equal(client.Name, newClient.Name);
                Assert.Equal(client.LastName, newClient.LastName);
            };

          
            viewModel.Save();

          
            Assert.True(saveCompletedCalled);
        }
    }

}