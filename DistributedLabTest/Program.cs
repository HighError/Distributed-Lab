using DistributedLabTest.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DistributedLabTest
{
    public class Program
    {
        static List<Journey> journeys = new List<Journey>();
        static List<int> stations = new List<int>();
        static void Main(string[] args)
        {
            if (!GetDataFromCSV())
            {
                Console.WriteLine("CRITICAL ERROR!!! Data file not found.");
            }
            else
            {
                stations = Journey.GetAllStations(journeys);

                Route cheapestRoute = Route.GetCheapestRoute(journeys, stations);
                Console.WriteLine($"\nНайдешевший маршрут: {cheapestRoute.GetPrice()}");
                Console.WriteLine(cheapestRoute);

                Route fastestRoute = Route.GetFastestRoute(journeys, stations);
                Console.WriteLine($"\nНайшвидший маршрут: {cheapestRoute.GetTime().TotalSeconds} секунд");
                Console.WriteLine(cheapestRoute);
            }
            Console.ReadKey();
        }

        static bool GetDataFromCSV()
        {
            string path = "data.csv";
            if (!File.Exists(path)) 
                return false;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (int.TryParse(parts[0], out int number) &&
                    int.TryParse(parts[1], out int dispatchStation) &&
                    int.TryParse(parts[2], out int arrivalStation) &&
                    decimal.TryParse(parts[3].Replace('.', ','), out decimal price) &&
                    DateTime.TryParse(parts[4], out DateTime dispatchTime) &&
                    DateTime.TryParse(parts[5], out DateTime arrivalTime))
                {
                    journeys.Add(new Journey(number, dispatchStation, arrivalStation, price, dispatchTime, arrivalTime));
                }
                else
                {
                    Console.WriteLine($"Corrupt entry found in row '{line}'!!!");
                    Console.WriteLine("Data entry for this row was skipped!");
                }
            }
            return true;
        }
    }
}
