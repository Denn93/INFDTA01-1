using System;
using System.Collections.Generic;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Services;
using BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrianDennis.INFDTA01.Opdracht1.Tests
{
    [TestClass]
    public class AlgorithmTests
    {
        [TestMethod]
        public void ComputeSimilarityWithEuclidean()
        {
            SortedDictionary<int, UserPreference> dataSet = new SortedDictionary<int, UserPreference>
            {
                { 1, new UserPreference { Preferences = new Dictionary<int, float>() {{2, 3.0f}, {3, 4.5f}} } },
                { 2, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 2.0f}, {2, 2.5f}, {3, 1.0f} } } },
                { 3, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.5f}, {2, 3.5f}, {3, 5.0f} } } },
                { 4, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 3.0f}, {2, 3.5f}, {3, 4.5f} } } }
            };

            List<AlgorithmResultListItem> result = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Euclidean, dataSet, "Test").Calculate(1);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result[0].Similarity, 0.59d, 0.1d);
            Assert.AreEqual(result[1].Similarity, 0.67d, 0.1d);
        }

        [TestMethod]
        public void ComputeSimilarityWithPearson()
        {
            SortedDictionary<int, UserPreference> dataSet = new SortedDictionary<int, UserPreference>
            {
                { 1, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.75f}, {2, 4.5f}, {3, 5.0f}, {4, 4.25f}, {5, 4.0f}} } },
                { 2, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.0f}, {2, 3.0f}, {3, 5.0f}, {4, 2.0f}, {5, 1.0f}} } }
            };

            List<AlgorithmResultListItem> result = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, dataSet, "Test").Calculate(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result[0].Similarity, 1.0d, 0.1d);
        }

        [TestMethod]
        public void ComputeSimilarityWithCosine()
        {
            SortedDictionary<int, UserPreference> dataSet = new SortedDictionary<int, UserPreference>
            {
                { 1, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.75f}, {2, 4.5f}, {3, 5.0f}, {4, 4.25f}, {5, 4.0f}} } },
                { 2, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.0f}, {2, 3.0f}, {3, 5.0f}, {4, 2.0f}, {5, 1.0f}} } }
            };

            List<AlgorithmResultListItem> result = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine, dataSet, "Test").Calculate(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result[0].Similarity, 0.935d, 0.1d);
        }

        [TestMethod]
        public void ComputePredictedRating()
        {
            SortedDictionary<int, UserPreference> dataSet = new SortedDictionary<int, UserPreference>
            {
                { 1, new UserPreference { Preferences = new Dictionary<int, float>() } },
                { 2, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.5f}} } },
                { 3, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 5.0f}} } },
                { 4, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 3.5f}} } }
            };

            List<AlgorithmResultListItem> pearsonData = new List<AlgorithmResultListItem>
            {
                new AlgorithmResultListItem {OtherUser = 2, Similarity = 0.5d},
                new AlgorithmResultListItem {OtherUser = 3, Similarity = 0.7d},
                new AlgorithmResultListItem {OtherUser = 4, Similarity = 0.8d}
            };

             PredictingRatingAlgorithm predicting = (PredictingRatingAlgorithm) AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Predictive, dataSet,
                "Test");

            predicting.PearsonListData = pearsonData;
            predicting.Calculate(1);
            Dictionary<int, double> result = predicting.PredictiveRatings;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey(1));
            Assert.AreEqual(result[1], 4.275d, 0.1d);
        }

        [TestMethod]
        public void ComputePredictedRatingsByDeviations()
        {
            SortedDictionary<int, UserPreference> dataSet = new SortedDictionary<int, UserPreference>
            {
                { 1, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 4.0f}, {2, 3.0f}, {3, 4.0f}} } },
                { 2, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 5.0f}, {2, 2.0f}} } },
                { 3, new UserPreference { Preferences = new Dictionary<int, float>() {{2, 3.5f}, {3, 4.0f}} } },
                { 4, new UserPreference { Preferences = new Dictionary<int, float>() {{1, 5.0f}, {3, 3.0f}} } }
            };

            ItemItemDeviationAlgorithm deviation = (ItemItemDeviationAlgorithm) AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Deviation, dataSet, "Test");
            deviation.Calculate();

            Assert.AreEqual(2, ItemItemDeviationAlgorithm.DeviationResultTest[new Tuple<int, int>(1,2)].Item1, 0.1d);
            Assert.AreEqual(1, ItemItemDeviationAlgorithm.DeviationResultTest[new Tuple<int, int>(1,3)].Item1, 0.1d);
            Assert.AreEqual(-0.75, ItemItemDeviationAlgorithm.DeviationResultTest[new Tuple<int, int>(2,3)].Item1, 0.1d);

            ItemItemSlopeOneAlgorithm slopeOne =
                (ItemItemSlopeOneAlgorithm)
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.PredictionDeviation, dataSet, "Test");

            slopeOne.Calculate(2, null);

            Assert.AreEqual(1, slopeOne.SlopeOneResult.Item2.Count);
            Assert.AreEqual(3.375, slopeOne.SlopeOneResult.Item2[3], 0.1d);
        }
    }
}
