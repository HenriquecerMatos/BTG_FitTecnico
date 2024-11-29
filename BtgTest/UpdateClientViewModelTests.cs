using Applicability.Shared;
using BtgPactual.ViewModels;
namespace BtgTest
{
    public class UpdateClientViewModelTests
    {
        [Fact]
        public void Save_ValidClient_ShouldInvokeOnSaveCompleted()
        {
            var client = new ClientDTO { Name = "John", LastName = "Doe", Age = 30, Address = "123 Street" };
            var viewModel = new UpdateClientViewModel(client);
            var saveCompletedCalled = false;

            viewModel.OnSaveCompleted = (savedClient) =>
            {
                saveCompletedCalled = true;
                Assert.Equal(client.Name, savedClient.Name);
            };

            viewModel.Save();

            Assert.True(saveCompletedCalled);
        }
    }

}