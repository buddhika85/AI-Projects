using System;

namespace _5_GA_TSP
{
    public class City
    {
        public int X { get; }
        public int Y { get; }


        public City(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int Proximity(City otherCity) // nearness
        {
            return Proximity(otherCity.X, otherCity.Y);
        }

        private int Proximity(int x, int y)
        {
            var xdiff = X - x;
            var ydiff = Y - y;

            return (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff);       // pythagoras theorem 
        }
    }
}
