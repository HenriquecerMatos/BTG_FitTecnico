using Applicability.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.ObjectModel;

namespace BtgPactual.ViewModels;
public partial class MainViewModel : ObservableObject
{
    #region escala/dimencionamento de tela/grafico
    [ObservableProperty]
    private double _canvasWidth;

    [ObservableProperty]
    private double _canvasHeight;
    #endregion

    #region configuração de leyout
    // Controla a visibilidade da área de configuração
    [ObservableProperty]
    private bool _isConfigAreaVisible = false;

    // Número de linhas do grid
    [ObservableProperty]
    private int _gridLinesCount = 8;

    // Lista de cores disponíveis para o Picker   

    public ObservableCollection<ColorDictionary> ColorDics
    {
        get
        {
            return new ObservableCollection<ColorDictionary>
        {
            new("Amarelo", "#FFFF00"),
            new("Azul", "#0000FF"),
            new("Branco", "#FFFFFF"),
            new ("Cinza Claro", "#D3D3D3"),
            new ("Cinza Escuro", "#A9A9A9"),
            new ("Laranja", "#FFA500"),
            new ("Marrom", "#8B4513"),
            new ("Preto", "#000000"),
            new ("Rosa", "#FFC0CB"),
            new ("Verde", "#00FF00"),
            new ("Vermelho", "#FF0000"),
            new ("Violeta", "#8A2BE2")
        };
        }
    }

    // Cor do gráfico  
    [ObservableProperty]
    private ColorDictionary _graphicColor = new("Branco", "#FFFFFF");
    // Cor de fundo do gráfico  
    [ObservableProperty]
    private ColorDictionary _backgroundColor = new("Preto", "#000000");

    // Texto do botão que alterna a área de configurações
    public string ConfigAreaButtonText => IsConfigAreaVisible ? "Fechar Configurações" : "Abrir Configurações";

    /// <summary>
    /// Comando para alternar a visibilidade da área de configuração
    /// </summary>
    [RelayCommand]
    private void ToggleConfigArea()
    {
        IsConfigAreaVisible = !IsConfigAreaVisible;
        OnPropertyChanged(nameof(ConfigAreaButtonText));
    }

    #endregion

    /// <summary>
    /// canvas responsavel pelo grafico 
    /// </summary>
    private SKCanvasView _canvasView;

    /// <summary>
    /// Método para registrar o SKCanvasView no ViewModel
    /// Necessário para forçar a atualização do gráfico
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

    #region propriedades iniciais

    // Preço inicial padrão
    [ObservableProperty]
    private double _initialPrice = 100;
    // Volatilidade padrão
    [ObservableProperty]
    private double _volatility = 0.02;
    // Retorno médio padrão
    [ObservableProperty]
    private double _meanReturn = 0.001;
    // Número de dias padrão
    [ObservableProperty]
    private int _numDays = 100; 
    #endregion

    /// <summary>
    /// Comando para gerar os preços simulados e atualizar o gráfico
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
    /// Exibe uma mensagem de erro para o usuário
    /// </summary>
    private async Task ShowErrorMessage(string message)
    {
        // Exibe um alerta de erro na interface do usuário
        await Application.Current.MainPage.DisplayAlert("Erro", message, "OK");
    }

    internal void SetColor()
    {
        GraphicColor = ColorDics.FirstOrDefault(c => c.NameColor.ToLower() == "verde");
        BackgroundColor = ColorDics.FirstOrDefault(c => c.NameColor.ToLower() == "preto");
    }
}
