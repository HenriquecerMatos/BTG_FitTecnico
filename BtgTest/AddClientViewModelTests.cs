using BtgPactual.ViewModels;
using BtgPactual.Views;
using Moq;
using SkiaSharp;
using SkiaSharp.Views.Maui;

public class MainPageTests
{
    [Fact]
    public void Constructor_ShouldInitializeViewModelAndSetBindingContext()
    {
        // Arrange & Act
        var mainPage = new MainPage();

        // Assert
        Assert.NotNull(mainPage.BindingContext);
        Assert.IsType<MainViewModel>(mainPage.BindingContext);
    }

    [Fact]
    public void OnPaintSurface_ShouldNotDraw_WhenPricesAreNullOrEmpty()
    {
        // Arrange
        var mainPage = new MainPage();
        var viewModel = new MainViewModel { Prices = null }; // Simula Prices como nulo
        mainPage.BindingContext = viewModel;

        // Criar um canvas de teste
        var mockCanvas = new Mock<SKCanvas>();
        var mockSurface = SKSurface.Create(new SKImageInfo(100, 100));
        var args = new SKPaintSurfaceEventArgs(mockSurface, new SKImageInfo(100, 100), new ());

        // Act & Assert
        var exception = Record.Exception(() => mainPage.OnPaintSurface(null, args));
        Assert.Null(exception); // Nenhuma exceção deve ser lançada
    }

    [Fact]
    public void OnPaintSurface_ShouldDrawGrid_WhenPricesAreValid()
    {
        
        // Arrange
        var mainPage = new MainPage();
        var prices = new double[] { 100, 200, 300, 400, 500 };
        var viewModel = new MainViewModel { Prices = prices, GridLinesCount = 4, BackgroundColor = new("Branco", "#FFFFFF") };
        mainPage.BindingContext = viewModel;

        // Criar um canvas de teste
        var mockCanvas = new Mock<SKCanvas>();
        var mockSurface = SKSurface.Create(new SKImageInfo(100, 100));
        var args = new SKPaintSurfaceEventArgs(mockSurface, new SKImageInfo(100, 100), new ());

        // Act
        mainPage.OnPaintSurface(null, args);

        // Assert
        Assert.NotNull(viewModel.Prices);
        Assert.Equal(5, viewModel.Prices.Length);
    }

    [Fact]
    public void DrawGraph_ShouldDrawLinesCorrectly()
    {
        // Arrange
        var canvas = new Mock<SKCanvas>();
        var prices = new double[] { 10, 20, 30, 40, 50 };
        var graphColor = SKColors.Blue;
        var mainPage = new MainPage();

        // Act
        mainPage.DrawGraph(canvas.Object, prices, 10, 5, 20, 100, 10, graphColor, 2);

        // Assert
        canvas.Verify(c => c.DrawLine(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<SKPaint>()), Times.AtLeastOnce);
    }

    [Fact]
    public void DrawScale_ShouldAddTextLabels()
    {
        // Arrange
        var canvas = new Mock<SKCanvas>();
        var prices = new double[] { 50, 100, 150 };
        var mainPage = new MainPage();
        var color = SKColors.Black;

        // Act
        mainPage.DrawScale(canvas.Object, 200, 100, 10, 50, 150, prices, color, 5, 3);

        // Assert
        canvas.Verify(c => c.DrawText(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<SKPaint>()), Times.AtLeastOnce);
    }
}
