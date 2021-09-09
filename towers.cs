using System;

namespace Towers
{
    public class Tower
    {
        public string ID { get; }
        public float Easting { get; }
        public float Northing { get; }
        public float Longitude { get; }
        public float Lat { get; }

        public Tower(string[] fields)
        {
            ID = fields[0];
            Console.WriteLine(ID);
            Easting = float.Parse(fields[1]);
            Northing = float.Parse(fields[2]);
            Longitude = float.Parse(fields[3]);
            Lat = float.Parse(fields[4]);
        }
    }
    public class TowerList
    {
        public Tower[] items { get; }
        public TowerList(string[] towersData)
        {
            foreach (string towerData in towersData)
            {
                string[] fields = towerData.Split(',');
                var newTower = new Tower(fields);
                //Console.WriteLine(newTower.ID);
            }
        }
    }
}