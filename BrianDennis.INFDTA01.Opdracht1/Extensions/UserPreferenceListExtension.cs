using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Extensions
{
    public static class UserPreferenceListExtension     
    {
        public static bool ContainsMovie(this List<UserPreference> list, int key)
        {
            return list.Count(m => m.MovieId == key) > 0;
        }
    }
}