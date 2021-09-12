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

            var edges = Calculations.graphColor.assignEdges(towersList);

            var frequencies = Calculations.graphColor.assignFrequencies(edges);
            Console.WriteLine(frequencies.Length);
        }
    }
}
