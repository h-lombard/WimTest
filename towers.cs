/*File for towers classes*/
using System;

namespace Towers
{
    //class for UTM coordinate set
    public class utmSet
    {
        public double Easting { get; }
        public double Northing { get; }

        public utmSet(double easting, double northing) {
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
    }
}