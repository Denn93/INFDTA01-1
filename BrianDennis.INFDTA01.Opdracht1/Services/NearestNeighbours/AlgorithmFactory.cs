using System.Collections.Generic;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public static class AlgorithmFactory
    {
        public enum Algorithm
        {
            Euclidean,
            Pearson,
            Cosine,
            Predictive
        };

        public static AAlgorithm Build(Algorithm algorithm, UserItemDataSetFactory.DataSets dataSetEnum, string view)
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = UserItemDataSetFactory.Build(dataSetEnum);

            switch (algorithm)
            {
                case Algorithm.Euclidean:
                    return new EuclideanAAlgorithm(dataSet, view);
                case Algorithm.Pearson:
                    return new PearsonAAlgorithm(dataSet, view);
                case Algorithm.Cosine:
                    return new CosineAAlgorithm(dataSet, view);
                case Algorithm.Predictive:
                    return new PredictingRatingAlgorithm(dataSet, view);
            }

            return null;
        }
    }
}