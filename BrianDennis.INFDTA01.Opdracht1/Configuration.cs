using System.Configuration;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public static class Configuration
    {
        public static string UserItemCsvPath
        {
            get { return ConfigurationManager.AppSettings["UserItemCsv"]; }
        }
        
        public static string UserItemEditedCsvPath
        {
            get { return ConfigurationManager.AppSettings["UserItemEditedCsv"]; }
        }

        public static string MovieLensData
        {
            get { return ConfigurationManager.AppSettings["MovieLensData"]; }
        }

        public static double InitialThresHold
        {
            get { return double.Parse(ConfigurationManager.AppSettings["InitialThresHold"]); }
        }
    }
}