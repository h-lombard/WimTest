/*File for distance calculations and frequency assignments */
using System;

namespace Calculations
{
    public static class Conversion
    {
        public static double calcDistance(Towers.utmSet pos1, Towers.utmSet pos2)
        {
            double xDistance = Math.Abs(pos1.Easting - pos2.Easting);
            double yDistance = Math.Abs(pos1.Northing - pos2.Northing);

            double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            return distance;
        }
    }

    public static class graphColor
    {
        public static int numEdges = 1;
        public static int[] freqRange = new int[] { 110, 115 };

        public static void printArr(double[] arr)
        {
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }

        public static int[,] assignEdges(Towers.TowerList towerList)
        {
            int numVertices = towerList.items.Length;
            Towers.Tower[] vertices = towerList.items;

            int[,] edges = new int[numVertices, numVertices];
            double startDistance = Double.PositiveInfinity;
            int startIndex = 0;

            for (int iVertice = 0; iVertice < numVertices; iVertice++)
            {
                double[] distances = new double[numVertices];
                Towers.Tower refVertice = vertices[iVertice];

                for (int iDistance = 0; iDistance < numVertices; iDistance++)
                {
                    distances[iDistance] = Conversion.calcDistance(refVertice.utmCoordinates, vertices[iDistance].utmCoordinates);
                }

                // replace 0 distance with max distance to cancel reference tower
                double maxDistance = arrayFunctions.findValues.findMax(distances);
                distances[iVertice] = maxDistance;

                for (int iEdge = 0; iEdge < numEdges; iEdge++)
                {
                    // find minimum distance and add edge between that tower and reference
                    int minIndex = Array.IndexOf(distances, arrayFunctions.findValues.findMin(distances));

                    if (startDistance > distances[minIndex])
                    {
                        startDistance = distances[minIndex];
                        startIndex = iVertice;
                    }

                    edges[iVertice, minIndex] = 1;
                    //edges[minIndex, iVertice] = 1;

                    distances[minIndex] = maxDistance;
                }

                var edgeRow = arrayFunctions.findValues.getIntRow(edges, iVertice);
                var edgeHold = Array.ConvertAll(edgeRow, item => (double)item);
            }

            return edges;
        }

        public static int getFurthestFreq(int currentFreq)
        {
            int furthestFreq = currentFreq;
            int oldDifference = 0;

            for (int i = freqRange[0]; i <= freqRange[1]; i++)
            {
                int newDifference = Math.Abs(i - currentFreq);

                if (newDifference > oldDifference)
                {
                    oldDifference = newDifference;
                    furthestFreq = i;
                }
            }

            return furthestFreq;
        }

        public static int getRandFreq()
        {
            Random random = new Random();
            int frequency = random.Next(freqRange[0], freqRange[1] + 1);

            return frequency;
        }

        public static int[] assignFrequencies(int[,] edges)
        {
            int numTowers = edges.GetLength(0);
            int[] frequencies = new int[numTowers];

            // assign first tower with edge of frequency range
            frequencies[0] = freqRange[0];

            for (int iVertice = 1; iVertice < numTowers; iVertice++)
            {
                int[] verticeEdges = arrayFunctions.findValues.getIntRow(edges, iVertice);
                int edgeIndex = Array.IndexOf(verticeEdges, 1);

                if (frequencies[edgeIndex] == 0)
                {
                    frequencies[iVertice] = getRandFreq();
                }
                else
                {
                    frequencies[iVertice] = getFurthestFreq(frequencies[edgeIndex]);
                }
            }

            return frequencies;
        }
    }
}

namespace arrayFunctions
{
    using System.Linq;

    public static class findValues
    {
        public static double findMax(double[] input)
        {
            return input.Max();
        }

        public static double findMin(double[] input)
        {
            return input.Min();
        }

        public static double[] getRow(double[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public static int[] getIntRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
