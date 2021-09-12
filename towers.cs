/*File for towers classes*/
using System;

namespace Towers
{
    //class for UTM coordinate set
    public class utmSet
    {
        public double Easting { get; }
        public double Northing { get; }

        public utmSet(double easting, double northing)
        {
            Easting = easting;
            Northing = northing;
        }
    }

    //class for geocentric coordinate set
    public class geoSet
    {
        public double Longitude { get; }
        public double Lat { get; }

        public geoSet(double longitude, double lat)
        {
            Longitude = longitude;
            Lat = lat;
        }
    }
    public class Tower
    {
        public string ID { get; }

        public geoSet geoCoordinates { get; }

        public utmSet utmCoordinates { get; }

        public Tower(string[] fields)
        {
            ID = fields[0];
            utmCoordinates = new utmSet(double.Parse(fields[1]), double.Parse(fields[2]));
            geoCoordinates = new geoSet(double.Parse(fields[3]), double.Parse(fields[4]));
        }
    }
    public class TowerList
    {
        public Tower[] items { get; }

        public TowerList(string[] towersData)
        {
            items = new Tower[towersData.Length];
            var itemCount = 0;

            foreach (string towerData in towersData)
            {
                string[] fields = towerData.Split(',');
                var newTower = new Tower(fields);
                items[itemCount] = newTower;
                itemCount++;
            }
        }

        public double[] getAllEast()
        {
            int numTowers = items.Length;
            double[] eastings = new double[numTowers];

            for (int i = 0; i < numTowers; i++)
            {
                eastings[i] = items[i].utmCoordinates.Easting;
            }

            return eastings;
        }

        public double[] getAllNorth()
        {
            int numTowers = items.Length;
            double[] northings = new double[numTowers];

            for (int i = 0; i < numTowers; i++)
            {
                northings[i] = items[i].utmCoordinates.Northing;
            }

            return northings;
        }

        public int[] getFreqPoints(int[] frequencies, int frequency)
        {
            int numItems = frequencies.Length;
            int[] indexes = new int[numItems];
            int indexCounter = 0;

            for (int i = 0; i < numItems; i++)
            {
                if (frequencies[i] == frequency)
                {
                    indexes[indexCounter] = i;
                    indexCounter++;
                }
            }

            return indexes[..indexCounter];
        }

        public void plotFrequencies(int[] frequencies)
        {
            var plot = new ScottPlot.Plot(600, 400);

            var eastings = getAllEast();
            var northings = getAllNorth();

            int[] freqRange = Calculations.graphColor.freqRange;
            int numPossibleFreqs = freqRange[1] - freqRange[0] + 1;

            for (int i = 0; i < numPossibleFreqs; i++)
            {
                int currentFrequency = freqRange[0] + i;
                int[] itemIndexes = getFreqPoints(frequencies, currentFrequency);

                if (itemIndexes.Length != 0)
                {
                    double[] itemEastings = new double[itemIndexes.Length];
                    double[] itemNorthings = new double[itemIndexes.Length];

                    for (int iItem = 0; iItem < itemIndexes.Length; iItem++)
                    {
                        itemEastings[iItem] = eastings[itemIndexes[iItem]];
                        itemNorthings[iItem] = northings[itemIndexes[iItem]];
                    }
                    Console.WriteLine(currentFrequency);
                    plot.AddScatter(itemEastings, itemNorthings, lineWidth: 0, label: currentFrequency.ToString());
                }
            }

            plot.Legend();
            plot.Title("Frequency allocation");
            plot.XLabel("Easting");
            plot.YLabel("Northing");

            plot.SaveFig("FrequencyResult.png");
        }
    }
}