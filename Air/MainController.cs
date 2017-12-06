using Air.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air
{
    public class MainController
    {
        private static MainController _instance;

        public static MainController Instance => _instance ?? (_instance = new MainController());

        private MainController()
        {
            Airlines = new ObservableCollection<AirlineModel>();
            Airports = new ObservableCollection<AirportModel>();
            Cities = new ObservableCollection<CityModel>();
            Countries = new ObservableCollection<CountryModel>();
            Flights = new ObservableCollection<FlightModel>();
            Planes = new ObservableCollection<PlaneModel>();
        }

        public ObservableCollection<AirlineModel> Airlines { get; set; }
        public ObservableCollection<AirportModel> Airports { get; set; }
        public ObservableCollection<CityModel> Cities { get; set; }
        public ObservableCollection<CountryModel> Countries { get; set; }
        public ObservableCollection<FlightModel> Flights { get; set; }
        public ObservableCollection<PlaneModel> Planes { get; set; }

    }
}
