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

        // Registra o SKCanvasView no ViewModel
        viewModel.SetCanvasView(canvasView);
    }

    /// <summary>
    /// Evento para desenhar o gráfico no SKCanvasView.
    /// </summary>
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var viewModel = BindingContext as MainViewModel;
        if (viewModel?.Prices == null || viewModel.Prices.Length == 0)
            return;

        var canvas = e.Surface.Canvas;

        // Altura reservada para a escala
        var scaleHeight = 50;

        // Validar a cor de fundo
        SKColor backgroundColor;
        if (!SKColor.TryParse(viewModel.BackgroundColor, out backgroundColor))
        {
            backgroundColor = SKColors.White; // Cor padrão
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

        // Desenhar o grid
        DrawGrid(canvas, width, height, margin, viewModel.GridLinesCount, SKColors.LightGray);

        // Desenhar os eixos
        DrawAxes(canvas, width, height, margin);

        // Determinar a cor do texto com base no brilho da cor de fundo
        SKColor textColor = GetTextColorForBackground(backgroundColor);

        // Desenhar o gráfico
        DrawGraph(
            canvas,
            prices,
            scaleX,
            scaleY,
            margin,
            height,
            minPrice,
            SKColors.Green, //viewModel.GraphColor
            Math.Max(2, Math.Min(width, height) / 200)
        );

        // Desenhar a escala
        DrawScale(canvas, width, height, margin, minPrice, maxPrice, prices, textColor, viewModel.GridLinesCount, viewModel.GridLinesCount);
    }


    private void DrawScale(SKCanvas canvas, int width, int height, int margin,
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

    private void DrawGrid(SKCanvas canvas, int width, int height, int margin, int gridLinesCount, SKColor gridColor)
    {

        // Configuração do pincel para a grid
        var gridPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.LightGray,// viewModel.GridColor,
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

    private void DrawGraph(SKCanvas canvas, double[] prices, float scaleX, float scaleY, int margin, int height, double minPrice, SKColor graphColor, int strokeWidth)
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

    private void DrawAxes(SKCanvas canvas, int width, int height, int margin)
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

    private SKColor GetTextColorForBackground(SKColor backgroundColor)
    {
        // Calcular o brilho relativo usando a fórmula de luminância
        float brightness = (0.2126f * backgroundColor.Red + 0.7152f * backgroundColor.Green + 0.0722f * backgroundColor.Blue) / 255;

        // Se o brilho for baixo (fundo escuro), usa cor clara para o texto (branco)
        // Caso contrário, use cor escura para o texto (preto)
        return brightness < 0.5f ? SKColors.White : SKColors.Black;
    }

}
