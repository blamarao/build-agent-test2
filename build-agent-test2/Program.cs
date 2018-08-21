using System;

namespace build_agent_test2
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalPosition position = new GlobalPosition("51.9233076-N","4.4794524-W");
            GlobalPosition position2 = new GlobalPosition(51.9233076, 4.4794524);

            Console.WriteLine("Hello World!");

            Console.WriteLine(position.PrintMe());

            position.WhichHemisphere();

            Console.WriteLine(position2.PrintMe());

            position2.WhichHemisphere();

            Console.ReadLine();
        }

        public class GlobalPosition
        {
            //private readonly string _latitude;
            //private readonly string _longitude;
            private readonly (object, object) _coordinates;
                 
            public string Position
            {
                //get => $"{_latitude}-{_longitude}";
                get => $"{_coordinates.Item1}, {_coordinates.Item2}";
            }

            public GlobalPosition(object latitude, object longitude)
            {
                //_latitude = latitude;
                //_longitude = longitude;
                _coordinates = (latitude, longitude);
            }

            public string PrintMe() => $"Your current position is {Position}";

            public void WhichHemisphere()
            {
                if (_coordinates.Item1 is string latitude)
                {
                    Console.WriteLine(Hemisphere(latitude));
                }
                else
                {
                    Console.WriteLine("Could not determine the Hemisphere");
                }

               
                //string Hemisphere() => _latitude.Split('-')[1].Equals("N") ? "Hemisfério Norte" : "Hemisfério Sul"; 
                string Hemisphere (string data) => data.Split('-')[1].Equals("N") ? "North Hemisphere" : "South Hemisphere";
                
            }
        }
    }
}
