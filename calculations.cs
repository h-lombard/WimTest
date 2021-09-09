/*File for distance calculations and frequency assignments */
using System;

namespace Calculations
{
    public class Conversion
    {
        // amount of metres in lattitude/longitude degree
        public const double mDegreesConv = 111000;

        public static double calcDistance(Towers.utmSet pos1, Towers.utmSet pos2) { 
            double xDistance = Math.Abs(pos1.Easting - pos2.Easting);
            double yDistance = Math.Abs(pos1.Northing - pos2.Northing);

            double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            return distance;
        }
    }
}