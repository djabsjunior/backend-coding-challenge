using System;
namespace BackendCodingChallenge.Calculators.LevenshteinDistance
{
    public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator
    {
        public int ComputeDistance(string req, string cityName)
        {
            var reqLength = req.Length;
            var cityNameLength = cityName.Length;
            var d = new int[reqLength + 1, cityNameLength + 1];

            // Step 1
            // returns the maximum length of both names if one is empty
            if (Math.Min(reqLength, cityNameLength) == 0)
            {
                return Math.Max(cityNameLength, reqLength);
            }

            // Step 2
            // 2D arrays initialization of same length as both string names
            for (var i = 0; i <= reqLength; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= cityNameLength; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= reqLength; i++)
            {
                //Step 4
                for (var j = 1; j <= cityNameLength; j++)
                {
                    // Step 5
                    // character substitution cost: set to 0 when a a change is needed and 1 otherwise.
                    var cost = (cityName[j - 1] == req[i - 1]) ? 0 : 1;

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
            return d[reqLength, cityNameLength]; //the last matrix position contains the distance
        }
    }
}
