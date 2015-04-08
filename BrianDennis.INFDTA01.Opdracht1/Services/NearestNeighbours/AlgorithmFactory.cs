using System.Collections.Generic;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public static class AlgorithmFactory
    {
        public enum Algorithm
        {
            Euclidean,
            Pearson,
            Cosine,
            Predictive,
            Deviation,
            PredictionDeviation,
            UpdateDeviation
        };

        public static AAlgorithm Build(Algorithm algorithm, UserItemDataSetFactory.DataSets dataSetEnum, string view)
        {
            return Build(algorithm, UserItemDataSetFactory.Build(dataSetEnum), view);
        }

        public static AAlgorithm Build(Algorithm algorithm, SortedDictionary<int, UserPreference> dataSet, string view)
        {
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
                case Algorithm.Deviation:
                    return new ItemItemDeviationAlgorithm(dataSet, view);
                case Algorithm.PredictionDeviation:
                    return new ItemItemSlopeOneAlgorithm(dataSet, view);
                case Algorithm.UpdateDeviation:
                    return new ItemItemSlopeUpdate(dataSet, view);
            }

            return null;
        }
    }
}