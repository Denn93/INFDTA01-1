using System.Collections.Specialized;
using System.Configuration;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public static class Configuration
    {
        public static NameValueCollection Targets(string view)
        {
            NameValueCollection coll  = (NameValueCollection) ConfigurationManager.GetSection("targets/" + view);
            return coll;
        }
    }
}