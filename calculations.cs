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
            foreach (int item in arr)
            {
                Console.Write(item + ",");
            }
            Console.Write('\n');
        }

        public static void printArrI(int[] arr)
        {
            foreach (int item in arr)
            {
                Console.Write(item + ",");
            }
            Console.Write('\n');
        }

        public class edgeList
        {
            public int[,] Edges { get; }

            public int[] VerticeIndexes { get; }

            public edgeList(int[,] edges, int[] indexes)
            {
                Edges = edges;
                VerticeIndexes = indexes;
            }
        }

        public static int[] orderVertices(double[] minDistances)
        {
            int numVertices = minDistances.Length;
            int[] indexes = new int[numVertices];
            
            for (int i = 0; i < numVertices; i++)
            {
                indexes[i] = i;
            }

            Array.Sort(minDistances, indexes);

            return indexes;
        }

        public static edgeList assignEdges(Towers.TowerList towerList)
        {
            int numVertices = towerList.items.Length;
            Towers.Tower[] vertices = towerList.items;

            int[,] edges = new int[numVertices, numVertices];
            double[] minDistances = new double[numVertices];

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

                bool firstMin = true;

                for (int iEdge = 0; iEdge < numEdges; iEdge++)
                {
                    double minDistance = arrayFunctions.findValues.findMin(distances);

                    if (firstMin == true)
                    {
                        firstMin = false;
                        minDistances[iVertice] = minDistance;
                    }

                    int minIndex = Array.IndexOf(distances, minDistance);

                    edges[iVertice, minIndex] = 1;
                    //edges[minIndex, iVertice] = 1;

                    distances[minIndex] = maxDistance;
                }
            }

            edgeList result = new edgeList(edges, orderVertices(minDistances));

            return result;
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

        public static int[] assignFrequencies(edgeList edgeList)
        {
            int numTowers = edgeList.Edges.GetLength(0);
            int[] frequencies = new int[numTowers];
            int[] orderedIndexes = edgeList.VerticeIndexes;
            printArrI(orderedIndexes);

            // assign first tower with edge of frequency range
            frequencies[orderedIndexes[0]] = freqRange[0];

            for (int iVertice = 1; iVertice < numTowers; iVertice++)
            {
                int[] verticeEdges = arrayFunctions.findValues.getIntRow(edgeList.Edges, orderedIndexes[iVertice]);
                int edgeIndex = Array.IndexOf(verticeEdges, 1);
                Console.WriteLine(orderedIndexes[iVertice]);
                if (frequencies[edgeIndex] == 0)
                {
                    frequencies[orderedIndexes[iVertice]] = getRandFreq();
                    
                    Console.WriteLine("Random");
                }
                else
                {
                    frequencies[orderedIndexes[iVertice]] = getFurthestFreq(frequencies[edgeIndex]);
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
