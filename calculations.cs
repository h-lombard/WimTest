/*File for distance calculations and frequency assignments */
using System;

namespace Calculations
{
    public static class Conversion
    {
        // amount of metres in lattitude/longitude degree
        public const double mDegreesConv = 111000;

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
        public static int numEdges = 2;
        public static int[] freqRange = new int[] { 110, 115 };

        public static void printArr(double[] arr)
        {
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }

        public class edgeList
        {
            public int[,] items { get; }
            public int startIndex { get; }

            public edgeList(int[,] itemList, int index)
            {
                items = itemList;
                startIndex = index;
            }
        }

        public static edgeList assignEdges(Towers.TowerList towerList)
        {
            int numVertices = towerList.items.Length;
            Towers.Tower[] vertices = towerList.items;

            int[,] edges = new int[numVertices, numVertices];
            double startDistance = Double.PositiveInfinity;
            int startIndex = 0;

            for (int iVertice = 0; iVertice < numVertices; iVertice++)
            {
                //Console.WriteLine("Vertice {0}", iVertice);
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
                //printArr(edgeHold);
            }

            edgeList result = new edgeList(edges, startIndex);

            return result;
        }

        public static int findFurthestFreqs(int currentFreq, int[] freqRange)
        {
            int furthestFreq = currentFreq;
            int freqDifference = 0;

            for (int i = freqRange[0]; i <= freqRange[1]; i++)
            {
                freqDifference = Math.Abs(i - currentFreq);
                furthestFreq = Math.Abs(furthestFreq - currentFreq) < freqDifference ? i : furthestFreq;
            }

            return furthestFreq;
        }

        public static int[] assignFrequencies(edgeList edges)
        {
            int numTowers = edges.items.GetLength(0);
            int[] frequencies = new int[numTowers];
            Console.WriteLine(freqRange[1] - freqRange[0]);

            // assign frequency to starting vertice
            frequencies[edges.startIndex] = freqRange[0];

            int[] currentEdges = arrayFunctions.findValues.getIntRow(edges.items, edges.startIndex);

            // assign frequencies until all towers are done
            while (Array.IndexOf(frequencies, 0) != -1)
            {
                int nextTowerIndex = Array.IndexOf(currentEdges, 1);
                Console.WriteLine(nextTowerIndex);
                currentEdges[nextTowerIndex] = 0;
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
