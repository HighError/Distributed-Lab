using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributedLabTest.Classes
{
    internal class Journey
    {
        public int Number { private set; get; }
        public int DispatchStation { private set; get; }
        public int ArrivalStation { private set; get; }
        public decimal Price { private set; get; }
        public DateTime DispatchTime { private set; get; }
        public DateTime ArrivalTime { private set; get; }

        public Journey(int number, int dispatchStation, int arrivalStation, decimal price, DateTime dispatchTime, DateTime arrivalTime)
        {
            Number = number;
            DispatchStation = dispatchStation;
            ArrivalStation = arrivalStation;
            Price = price;
            DispatchTime = dispatchTime;
            ArrivalTime = arrivalTime;
        }

        public static List<int> GetAllStations(List<Journey> journeys)
        {
            List<int> stations = new List<int>();
            foreach (var item in journeys)
            {
                if (!stations.Any(x => x == item.ArrivalStation))
                {
                    stations.Add(item.ArrivalStation);
                }
                if (!stations.Any(x => x == item.DispatchStation))
                {
                    stations.Add(item.DispatchStation);
                }
            }
            return stations;
        }

        public override string ToString()
        {
            return String.Format("|{0,14}|{1,14}|{2,14}|{3,14}|{4,14}|{5,14}|",
                Number, DispatchStation, ArrivalStation, Price, 
                DispatchTime.ToShortTimeString(), ArrivalTime.ToShortTimeString());
        }
    }
}
