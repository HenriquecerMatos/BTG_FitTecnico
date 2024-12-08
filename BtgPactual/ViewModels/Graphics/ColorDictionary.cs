namespace BtgPactual.ViewModels;
public partial class MainViewModel
{
    /// <summary>
    /// Classe parcial responsável por criar um dicionário simplificado, com o nome da cor e seus Hexa 
    /// </summary>
    public class ColorDictionary
    {
        public string NameColor { get; set; }
        public string ValueHexa { get; set; }
        public ColorDictionary(string nameColor, string valueHexa)
        {
            NameColor = nameColor;
            ValueHexa = valueHexa;
        }
    }
}
