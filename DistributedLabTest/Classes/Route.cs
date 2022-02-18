using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLabTest.Classes
{
    internal class Route
    {
        public List<Journey> journeys { get; private set; }

        public Route(Journey startJourney)
        {
            journeys = new List<Journey>() { startJourney };
        }

        public Route(Route route, Journey journey)
        {
            journeys = new List<Journey>(route.journeys);
            journeys.Add(journey);
        }

        public void AddJourney(Journey journey)
        {
            journeys.Add(journey);
        }

        public List<int> GetStations()
        {
            List<int> stations = new List<int>();
            stations.Add(journeys[0].DispatchStation);
            foreach (var journey in journeys)
            {
                stations.Add(journey.ArrivalStation);
            }
            return stations;
        }

        public decimal GetPrice()
        {
            decimal price = 0;
            foreach (var journey in journeys)
            {
                price += journey.Price;
            }
            return price;
        }

        public TimeSpan GetTime()
        {
            TimeSpan time = TimeSpan.Zero;
            for (int i = 0; i < journeys.Count; i++)
            {
                time += GetTimeSpan(journeys[i].ArrivalTime, journeys[i].DispatchTime);
                if (i != journeys.Count - 1)
                    time += GetTimeSpan(journeys[i].DispatchTime, journeys[i+1].ArrivalTime);
            }
            return time;
        }

        public override string ToString()
        {
            string res = String.Format("|{0,14}|{1,14}|{2,14}|{3,14}|{4,14}|{5,14}|\n",
                "Number", "Dispatch st.", "Arrival st.", "Price", "Dispatch Time", "Arrival Time");
            foreach (var item in journeys)
            {
                res += $"{item}\n";
            }
            return res;
        }

        static public TimeSpan GetTimeSpan(DateTime first, DateTime second)
        {
            if (first.TimeOfDay == second.TimeOfDay)
                return TimeSpan.FromHours(24);
            else if (first.TimeOfDay - second.TimeOfDay < TimeSpan.Zero)
                return TimeSpan.FromHours(24) + (first.TimeOfDay - second.TimeOfDay);
            else
                return first.TimeOfDay - second.TimeOfDay;
        }

        public static List<Route> GetAllPosibleRoutes(List<Journey> journeys, List<int> stations)
        {
            var routes = new List<Route>();

            foreach (var item in journeys)
            {
                routes.Add(new Route(item));
            }

            for (int i = 0; i < routes.Count; i++)
            {
                List<int> availableStations = new List<int>(stations);
                List<int> usedStation = new List<int>(routes[i].GetStations());
                for (int j = 0; j < usedStation.Count; j++)
                {
                    availableStations.Remove(usedStation[j]);
                }

                if (availableStations.Count == 0) continue;

                foreach (var item in journeys)
                {
                    if (item.DispatchStation != routes[i].journeys.Last().ArrivalStation) continue;
                    if (!availableStations.Any(x => x == item.ArrivalStation)) continue;
                    routes.Add(new Route(routes[i], item));
                }
                routes.RemoveAt(i);
                i--;
            }

            return routes;
        }

        public static Route GetFastestRoute(List<Journey> journeys, List<int> stations)
        {
            List<Journey> optimizedJorneys = new List<Journey>();

            // Залишаєм тільки найшвидший маршрут з одними і тими самими точками відпралення і прибуття
            for (int i = 0; i < stations.Count; i++)
            {
                for (int j = 0; j < stations.Count; j++)
                {
                    if (i == j) continue;
                    if (journeys.Any(x => x.DispatchStation == stations[i] && x.ArrivalStation == stations[j]))
                    {
                        optimizedJorneys.Add(journeys
                            .Where(x => x.DispatchStation == stations[i] && x.ArrivalStation == stations[j])
                            .OrderBy(y => GetTimeSpan(y.ArrivalTime, y.DispatchTime)).First()
                            );
                    }
                }
            }

            List<Route> routes = Route.GetAllPosibleRoutes(optimizedJorneys, stations);

            return routes.OrderBy(x => x.GetTime()).First();
        }

        public static Route GetCheapestRoute(List<Journey> journeys, List<int> stations)
        {
            List<Journey> optimizedJorneys = new List<Journey>();

            // Залишаєм тільки найдешевший маршрут з одними і тими самими точками відпралення і прибуття
            for (int i = 0; i < stations.Count; i++)
            {
                for (int j = 0; j < stations.Count; j++)
                {
                    if (i == j) continue;
                    if (journeys.Any(x => x.DispatchStation == stations[i] && x.ArrivalStation == stations[j]))
                    {
                        optimizedJorneys.Add(journeys
                            .Where(x => x.DispatchStation == stations[i] && x.ArrivalStation == stations[j])
                            .OrderBy(y => y.Price).First()
                            );
                    }
                }
            }

            List<Route> routes = Route.GetAllPosibleRoutes(optimizedJorneys, stations);

            return routes.OrderBy(x => x.GetPrice()).First();
        }
    }
}
