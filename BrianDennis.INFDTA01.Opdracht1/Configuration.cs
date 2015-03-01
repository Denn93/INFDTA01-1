using System.Configuration;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public static class Configuration
    {
        public static string FilePath
        {
            get { return ConfigurationManager.AppSettings["DatasetPath"]; }
        }

        public static double InitialThresHold
        {
            get { return double.Parse(ConfigurationManager.AppSettings["InitialThresHold"]); }
        }
    }
}