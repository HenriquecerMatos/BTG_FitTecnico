using BtgPactual.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace BtgPactual.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Configura o ViewModel
        var viewModel = new MainViewModel();
        BindingContext = viewModel;

        viewModel.SetColor();

        // Registra o SKCanvasView no ViewModel
        viewModel.SetCanvasView(canvasView);
    }

    /// <summary>
    /// Evento para desenhar o gráfico com SKCanvasView
    /// </summary>
    public void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var viewModel = BindingContext as MainViewModel;
        if (viewModel?.Prices == null || viewModel.Prices.Length == 0)
            return;

        var canvas = e.Surface.Canvas;

        // Validar a cor de fundo
        if (!SKColor.TryParse(viewModel.BackgroundColor.ValueHexa, out SKColor backgroundColor))
        {
            backgroundColor = SKColors.White; // Cor padrão
        }

        if (!SKColor.TryParse(viewModel.GraphicColor.ValueHexa, out SKColor graphicColor))
        {
            graphicColor = SKColors.Green; // Cor padrão
        }

        canvas.Clear(backgroundColor);

        var prices = viewModel.Prices;
        var maxPrice = prices.Max();
        double minPrice = prices.Min();

        int width = e.Info.Width;
        int height = e.Info.Height;
        int margin = Math.Min(width, height) / 10;

        // Escalas dinâmicas
        float scaleX = (width - 2 * margin) / (float)(prices.Length - 1);
        float scaleY = (height - 2 * margin) / (float)(maxPrice - minPrice);

        // Desenha a grid
        DrawGrid(canvas, width, height, margin, viewModel.GridLinesCount, SKColors.LightGray);

        // Desenha os eixos
        DrawAxes(canvas, width, height, margin);

        // Determinar a cor do texto com base no brilho da cor de fundo
        SKColor textColor = GetTextColorForBackground(backgroundColor);

        // Desenha o gráfico
        DrawGraph(
            canvas,
            prices,
            scaleX,
            scaleY,
            margin,
            height,
            minPrice,
            graphicColor,
            Math.Max(2, Math.Min(width, height) / 200)
        );

        // Desenhar a escala
        DrawScale(canvas, width, height, margin, minPrice, maxPrice, prices, textColor, viewModel.GridLinesCount, viewModel.GridLinesCount);
    }


    public void DrawScale(SKCanvas canvas, int width, int height, int margin,
    double minPrice, double maxPrice, double[] prices, SKColor Color, int numYLabels = 5, int numXLabels = 5)
    {
        using (var textPaint = new SKPaint
        {
            TextSize = 14,
            IsAntialias = true,
            Color = Color
        })
        {
            // Rótulos do eixo Y
            for (int i = 0; i <= numYLabels; i++)
            {
                float price = (float)(minPrice + i * (maxPrice - minPrice) / numYLabels);
                float y = height - margin - i * ((height - 2 * margin) / numYLabels);
                canvas.DrawText(price.ToString("F2"), 5, y + 5, textPaint);
            }

            // Rótulos do eixo X
            for (int i = 0; i <= numXLabels; i++)
            {
                int day = i * (prices.Length - 1) / numXLabels;
                float x = margin + i * ((width - 2 * margin) / numXLabels);
                canvas.DrawText(day.ToString(), x - 10, height - 5, textPaint);
            }
        }
    }

    /// <summary>
    /// Desenha uma grade para ajudar a visualização dos dados
    /// </summary>
    /// <param name="canvas">canvas</param>
    /// <param name="width">largura</param>
    /// <param name="height">altura</param>
    /// <param name="margin">margem</param>
    /// <param name="gridLinesCount">quantidade de linhas, mesma quantidade na vertical e horizontal</param>
    /// <param name="gridColor">Cor da grade/grid</param>
    private void DrawGrid(SKCanvas canvas, int width, int height, int margin, int gridLinesCount, SKColor gridColor)
    {

        // Configuração da grid
        var gridPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = gridColor,// viewModel.GridColor,
            StrokeWidth = 1,
            IsAntialias = true,
            //PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, 0) // Linhas tracejadas
        };

        // Linhas horizontais da grid
        float ySpacing = (height - 2 * margin) / gridLinesCount;
        for (int i = 0; i <= gridLinesCount; i++)
        {
            float y = height - margin - i * ySpacing;
            canvas.DrawLine(margin, y, width - margin, y, gridPaint);
        }

        // Linhas verticais da grid
        float xSpacing = (width - 2 * margin) / gridLinesCount;
        for (int i = 0; i <= gridLinesCount; i++)
        {
            float x = margin + i * xSpacing;
            canvas.DrawLine(x, margin, x, height - margin, gridPaint);
        }
    }


    /// <summary>
    /// Desenha o gráfico com base nos preços
    /// </summary>
    /// <param name="canvas">Canvas</param>
    /// <param name="prices">Lista de Preços</param>
    /// <param name="scaleX">espaçamento X entre as marcações</param>
    /// <param name="scaleY">espaçamento Y entre as marcações</param>
    /// <param name="margin">Margem</param>
    /// <param name="height">Altura</param>
    /// <param name="minPrice">menor preço</param>
    /// <param name="graphColor">cor do gráfico</param>
    /// <param name="strokeWidth"> espessura da linha do gráfico</param>
    public void DrawGraph(SKCanvas canvas, double[] prices, float scaleX, float scaleY, int margin, int height, double minPrice, SKColor graphColor, int strokeWidth)
    {
        var linePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = graphColor,
            StrokeWidth = strokeWidth
        };

        for (int i = 1; i < prices.Length; i++)
        {
            var x1 = margin + (i - 1) * scaleX;
            var y1 = height - margin - (float)((prices[i - 1] - minPrice) * scaleY);
            var x2 = margin + i * scaleX;
            var y2 = height - margin - (float)((prices[i] - minPrice) * scaleY);

            canvas.DrawLine(x1, y1, x2, y2, linePaint);
        }
    }


    /// <summary>
    /// Desenha os eixos X e Y do gráfico
    /// </summary>
    /// <param name="canvas"> Canvas</param>
    /// <param name="width"> Largura</param>
    /// <param name="height">Altura</param>
    /// <param name="margin">Margem</param>
    public void DrawAxes(SKCanvas canvas, int width, int height, int margin)
    {
        var axisPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 2
        };

        // Desenho dos eixos
        canvas.DrawLine(margin, height - margin, width - margin, height - margin, axisPaint); // Eixo X
        canvas.DrawLine(margin, margin, margin, height - margin, axisPaint); // Eixo Y
    }


    /// <summary>
    /// retorna uma cor de acordo com a cor comparada, importante para trocar a cor de textos de acordo com a cor do fundo
    /// </summary>
    /// <param name="backgroundColor">Com de fundo</param>
    /// <returns></returns>
    public SKColor GetTextColorForBackground(SKColor backgroundColor)
    {
        // Calcular o brilho relativo usando a fórmula de luminância
        float brightness = (0.2126f * backgroundColor.Red + 0.7152f * backgroundColor.Green + 0.0722f * backgroundColor.Blue) / 255;

        // Se o brilho for baixo (fundo escuro), usa cor clara para o texto (branco) e virse versa
        return brightness < 0.5f ? SKColors.White : SKColors.Black;
    }

    /// <summary>
    /// Método chamado ao ter o tamanho da tela alterada 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSizeChanged(object sender, EventArgs e)
    {
        MainViewModel? viewModel = BindingContext as MainViewModel;
        if (viewModel == null) return;

        // Ajusta a largura e altura com base no tamanho disponível
        double availableWidth = this.Width; 
        double availableHeight = this.Height;        

        // Defina proporções para manter o canvas responsivo
        viewModel.CanvasWidth = (availableWidth * 1) - 80; // 100% da largura disponível subtraído o padding
        viewModel.CanvasHeight = availableHeight * 0.6; // 60% da altura disponível
    }
}
