using System;

namespace build_agent_test2
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalPosition position = new GlobalPosition(51.9233076,4.4794524);

            Console.WriteLine("Hello World!");

            Console.WriteLine(position.PrintMe());

            Console.ReadLine();
        }

        public class GlobalPosition
        {
            private readonly double _latitude;
            private readonly double _longitude;

            public string Position
            {
                get => $"{_latitude}-{_longitude}";
            }

            public GlobalPosition(double latitude, double longitude)
            {
                _latitude = latitude;
                _longitude = longitude;
            }

            public string PrintMe() => $"Your current position is {Position}";
        }
    }
}
