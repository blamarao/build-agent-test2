using System;

namespace build_agent_test2
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalPosition position = new GlobalPosition("51.9233076-N","4.4794524-W");

            Console.WriteLine("Hello World!");

            Console.WriteLine(position.PrintMe());

            position.WhichHemisphere();

            Console.ReadLine();
        }

        public class GlobalPosition
        {
            //private readonly string _latitude;
            //private readonly string _longitude;
            private readonly (string, string) _coordinates;
                 
            public string Position
            {
                //get => $"{_latitude}-{_longitude}";
                get => $"{_coordinates.Item1}, {_coordinates.Item2}";
            }

            public GlobalPosition(string latitude, string longitude)
            {
                //_latitude = latitude;
                //_longitude = longitude;
                _coordinates = (latitude, longitude);
            }

            public string PrintMe() => $"Your current position is {Position}";

            public void WhichHemisphere()
            {
                Console.WriteLine(Hemisphere());

                //string Hemisphere() => _latitude.Split('-')[1].Equals("N") ? "Hemisfério Norte" : "Hemisfério Sul"; 
                string Hemisphere() => _coordinates.Item1.Split('-')[1].Equals("N") ? "Hemisfério Norte" : "Hemisfério Sul";
            }
        }
    }
}
