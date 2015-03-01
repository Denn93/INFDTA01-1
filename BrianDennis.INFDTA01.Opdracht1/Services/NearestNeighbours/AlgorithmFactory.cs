namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public static class AlgorithmFactory
    {
        public enum Algorithm
        {
            Euclidean,
            Pearson,
            Cosine
        };

        public static IAlgorithm Build(Algorithm algorithm)
        {
            switch (algorithm)
            {
                    case Algorithm.Euclidean:
                        return new EuclideanAlgorithm();
                    case Algorithm.Pearson:
                        return new PearsonAlgorithm();
                    case Algorithm.Cosine:
                        return new CosineAlgorithm();
            }
  
            return null;
        }
    }
}