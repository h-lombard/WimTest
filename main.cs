using System;

namespace WimToets
{   
    class MainProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo daar");
            string filePath = "towers.csv";

            string[] towersInfo = System.IO.File.ReadAllLines(filePath)[1..];
            
            var towersList = new Towers.TowerList(towersInfo);
        }
    }
}
