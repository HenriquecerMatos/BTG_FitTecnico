using Xunit;
using Moq;
using BtgPactual.ViewModels;
using Applicability.Shared;

public class AddUpdateClientViewModelTests
{
    [Fact]
    public void Constructor_InitializesProperties_WithClient()
    {
        // Arrange
        var cliente = new ClientDTO
        {
            Name = "John",
            LastName = "Doe",
            Age = 30,
            Address = "123 Main St"
        };

        // Act
        var viewModel = new AddUpdateClientViewModel(cliente);

        // Assert
        Assert.Equal("John", viewModel.Name);
        Assert.Equal("Doe", viewModel.LastName);
        Assert.Equal(30, viewModel.Age);
        Assert.Equal("123 Main St", viewModel.Address);
    }

    [Fact]
    public void Constructor_InitializesProperties_WithoutClient()
    {
        // Act
        var viewModel = new AddUpdateClientViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Equal(string.Empty, viewModel.LastName);
        Assert.Equal(0, viewModel.Age);
        Assert.Equal(string.Empty, viewModel.Address);
    }

    [Fact]
    public void Save_ValidClient_InvokesOnSaveCompleted()
    {
        // Arrange
        var cliente = new ClientDTO
        {
            Name = "John",
            LastName = "Doe",
            Age = 30,
            Address = "123 Main St"
        };
        var viewModel = new AddUpdateClientViewModel(cliente);

        ClientDTO savedClient = null;
        viewModel.OnSaveCompleted = client => savedClient = client;

        // Act
        viewModel.SaveCommand.Execute(null);

        // Assert
        Assert.NotNull(savedClient);
        Assert.Equal("John", savedClient.Name);
        Assert.Equal("Doe", savedClient.LastName);
        Assert.Equal(30, savedClient.Age);
        Assert.Equal("123 Main St", savedClient.Address);
    }

    [Fact]
    public void Save_InvalidClient_ShowsErrorMessage()
    {
        // Arrange
        var viewModel = new AddUpdateClientViewModel
        {
            Name = "",
            LastName = "",
            Age = 0
        };

        bool alertCalled = false;

        // Mockando DisplayAlert
        var appMock = new Mock<App>();
        App.Current = appMock.Object;
        appMock.Setup(x => x.MainPage.DisplayAlert(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        )).Callback(() => alertCalled = true);

        // Act
        viewModel.SaveCommand.Execute(null);

        // Assert
        Assert.True(alertCalled);
    }
}
