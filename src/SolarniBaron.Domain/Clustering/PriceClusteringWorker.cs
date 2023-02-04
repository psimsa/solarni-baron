namespace SolarniBaron.Domain.Clustering;

internal class PriceClusteringWorker
{
    internal int[] GetClusters(decimal[] prices, int clusterCount)
    {
        var means = prices.Distinct().Take(clusterCount).ToArray();

        var clusters = new List<int>[clusterCount];

        var shouldContinue = true;

        while (shouldContinue)
        {
            for (int i = 0; i < clusterCount; i++)
            {
                clusters[i] = new List<int>();
            }

            for (int i = 0; i < prices.Length; i++)
            {
                int index = GetClosestMean(means, prices[i]);
                clusters[index].Add(i);
            }

            var newMeans = new decimal[clusterCount];
            for (int i = 0; i < clusterCount; i++)
            {
                var values = clusters[i].Select(x => prices[x]).ToArray();
                newMeans[i] = GetMean(values);
            }

            shouldContinue = false;
            for (int i = 0; i < clusterCount; i++)
            {
                if (means[i] != newMeans[i])
                {
                    shouldContinue = true;
                    break;
                }
            }

            means = newMeans;
        }

        var toReturn = new int[prices.Length];
        for (int i = 0; i < clusterCount; i++)
        {
            foreach (int index in clusters[i])
            {
                toReturn[index] = Math.Abs(i - clusterCount + 1);
            }
        }

        return toReturn;
    }

    private static decimal GetMean(decimal[] values)
    {
        decimal sum = values.Sum();
        return sum / values.Length;
    }

    private static int GetClosestMean(decimal[] means, decimal value)
    {
        decimal min = decimal.MaxValue;
        int index = -1;
        for (int i = 0; i < means.Length; i++)
        {
            decimal diff = Math.Abs(means[i] - value);
            if (diff < min)
            {
                min = diff;
                index = i;
            }
        }

        return index;
    }
}
