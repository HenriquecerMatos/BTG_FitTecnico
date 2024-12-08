namespace Applicability.Shared
{
    /// <summary>
    /// Responsável por gerar os gráficos
    /// </summary>
    public static class Graphics
    {
        /// <summary>
        /// Gera uma simulação de movimento browniano para uma série de preços.
        /// </summary>
        /// <param name="sigma">A volatilidade do retorno diário.</param>
        /// <param name="mean">O retorno médio diário.</param>
        /// <param name="initialPrice">O preço inicial do ativo.</param>
        /// <param name="numDays">O número de dias para a simulação.</param>
        /// <returns>Um array de double representando os preços simulados ao longo do período.</returns>
        public static double[] GenerateBrownianMotion(double sigma, double mean, double initialPrice, int numDays)
        {
            // Inicializando o gerador de números aleatórios
            Random random = new ();
            double[] prices = new double[numDays];
            prices[0] = initialPrice;

            for (int i = 1; i < numDays; i++)
            {
                // Geração de dois números aleatórios uniformes
                double uniformRandom1 = 1.0 - random.NextDouble();
                double uniformRandom2 = 1.0 - random.NextDouble();

                // Cálculo do número aleatório com distribuição normal
                double standardNormalRandom = Math.Sqrt(-2.0 * Math.Log(uniformRandom1)) * Math.Cos(2.0 * Math.PI * uniformRandom2);

                // Cálculo do retorno diário usando a média e a volatilidade
                double dailyReturn = mean + sigma * standardNormalRandom;

                // Atualização do preço baseado no retorno diário
                prices[i] = prices[i - 1] * Math.Exp(dailyReturn);
            }

            return prices;
        }
    }
}
