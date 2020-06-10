using System;
namespace BackendCodingChallenge.Calculators.LevenshteinDistance
{
    public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator
    {
        public int ComputeDistance(string req, string cityName)
        {
            var reqLength = req.Length;
            var cityNameLength = cityName.Length;
            int[,] d = new int[reqLength + 1, cityNameLength + 1];

            // Step 1
            if (reqLength == 0 || cityNameLength == 0)
            {
                return Math.Min(cityNameLength, reqLength);
            }

            // Step 2
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
                    var cost = (cityName[j - 1] == req[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[reqLength, cityNameLength];
        }
    }
}
