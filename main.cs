/*Main program flow file*/
using System;

namespace WimToets
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            string filePath = "towers.csv";

            string[] towersInfo = System.IO.File.ReadAllLines(filePath)[1..];

            var towersList = new Towers.TowerList(towersInfo);

            for (int i = 0; i < towersList.items.Length; i++)
            {
                Console.WriteLine(towersList.items[i].utmCoordinates.Easting);
            }
        }
    }
}
