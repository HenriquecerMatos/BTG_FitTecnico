using Applicability.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp.Views.Maui.Controls;

namespace BtgPactual.ViewModels;
public partial class MainViewModel : ObservableObject
{

    #region configuração de leyout
    // Controla a visibilidade da área de configuração
    [ObservableProperty]
    private bool _isConfigAreaVisible = false;

    // Número de linhas do grid

    [ObservableProperty]
    private int _gridLinesCount = 5;
    // Cor de fundo do gráfico
    [ObservableProperty]
    private string _backgroundColor = "#000000";
    // Cor do gráfico
    [ObservableProperty]
    private string _graphColor = "#00FF00"; 

    // Lista de cores disponíveis para o Picker
    public List<string> AvailableColors => new List<string> { "#FFFFFF", "#000000", "#0000FF", "#00FF00", "#FF0000", "#00FFFF" };

    // Texto do botão que alterna a área de configurações
    public string ConfigAreaButtonText => IsConfigAreaVisible ? "Fechar Configurações" : "Abrir Configurações";

    /// <summary>
    /// Comando para alternar a visibilidade da área de configuração.
    /// </summary>
    [RelayCommand]
    private void ToggleConfigArea()
    {
        IsConfigAreaVisible = !IsConfigAreaVisible;
    }

    #endregion

    private SKCanvasView _canvasView;

    /// <summary>
    /// Método para registrar o SKCanvasView no ViewModel.
    /// Necessário para forçar a atualização do gráfico.
    /// </summary>
    public void SetCanvasView(SKCanvasView canvasView)
    {
        _canvasView = canvasView;
    }

    private double[] _prices;
    public double[] Prices
    {
        get => _prices;
        set => SetProperty(ref _prices, value);
    }

    [ObservableProperty]
    private double _initialPrice = 100; // Preço inicial padrão.

    [ObservableProperty]
    private double _volatility = 0.02; // Volatilidade padrão.

    [ObservableProperty]
    private double _meanReturn = 0.001; // Retorno médio padrão.

    [ObservableProperty]
    private int _numDays = 100; // Número de dias padrão.

    /// <summary>
    /// Comando para gerar os preços simulados e atualizar o gráfico.
    /// </summary>
    [RelayCommand]
    private async Task GeneratePrices()
    {
        // Validações para garantir que os valores inseridos são positivos
        if (InitialPrice <= 0)
        {
            
            await ShowErrorMessage("O preço inicial deve ser maior que zero.");
            return;
        }

        if (Volatility <= 0)
        {
            
            await ShowErrorMessage("A volatilidade deve ser maior que zero.");
            return;
        }

        if (MeanReturn <= 0)
        {
            
            await ShowErrorMessage("O retorno médio deve ser maior que zero.");
            return;
        }

        if (NumDays <= 0)
        {
           
            await ShowErrorMessage("O número de dias deve ser maior que zero.");
            return;
        }

        
        Prices = Graphics.GenerateBrownianMotion(Volatility, MeanReturn, InitialPrice, NumDays);

        
        _canvasView?.InvalidateSurface();
    }

    /// <summary>
    /// Exibe uma mensagem de erro para o usuário.
    /// </summary>
    private async Task ShowErrorMessage(string message)
    {
        // Exibe um alerta de erro na interface do usuário
        await Application.Current.MainPage.DisplayAlert("Erro", message, "OK");
    }
}
