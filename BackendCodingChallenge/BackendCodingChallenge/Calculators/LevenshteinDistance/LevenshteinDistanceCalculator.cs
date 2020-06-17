using System;
namespace BackendCodingChallenge.Calculators.LevenshteinDistance
{
    public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator
    {
        public int ComputeDistance(string qName, string cityName)
        {
            //Lowercase both string before the algorithm evaluation
            var lowerQName = qName.ToLower();
            var lowerCityName = cityName.ToLower();
            
            //Levenshtein Algorithm
            var qNameLength = lowerQName.Length;
            var cityNameLength = lowerCityName.Length;
            var d = new int[qNameLength + 1, cityNameLength + 1];

            // Step 1
            // returns the maximum length of both names if one is empty
            if (Math.Min(qNameLength, cityNameLength) == 0)
            {
                return Math.Max(cityNameLength, qNameLength);
            }

            // Step 2
            // 2D arrays initialization of same length as both string names
            for (var i = 0; i <= qNameLength; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= cityNameLength; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= qNameLength; i++)
            {
                //Step 4
                for (var j = 1; j <= cityNameLength; j++)
                {
                    // Step 5
                    // character substitution cost: set to 0 when no change is needed and 1 otherwise.
                    var cost = (lowerCityName[j - 1] == lowerQName[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(
                            //character deletion from request string
                            d[i - 1, j] + 1,
                            //new character insertion in city name
                            d[i, j - 1] + 1    
                        ),
                        //substitution
                        d[i - 1, j - 1] + cost 
                        );
                }
            }
            // Step 7
            return d[qNameLength, cityNameLength]; //the last matrix position contains the distance
        }
    }
}
