using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class FlightModel : PropertyObservable
    {
        private int _flightID;
        private int _airportID;
        private int _planeID;
        private int _airlineID;
        private string _airlineName;
        private string _airplaneModel;
        private string _airportName;
        private string _cityName;
        private string _countryName;
        private bool _flightType;
        private DateTime _dateTimeStart;
        private string _duration;
        private DateTime _dateTimeArrival;
        private int _numOfFlights;
        private DateTime _periodicity;
        private decimal _priceEconom;
        private decimal _priceBusiness;
        private decimal _priceFirst;
        private DateTime _dateTimeStartGMT;
        private DateTime _dateTimeArrivalGMT;
        private string _status;

        public int FlightID
        {
            get => _flightID;
            set
            {
                _flightID = value;
                OnPropertyChanged("FlightID");
            }
        }

        public int AirportID
        {
            get => _airportID;
            set
            {
                _airportID = value;
                OnPropertyChanged("AirportID");
            }
        }

        public int PlaneID
        {
            get => _planeID;
            set
            {
                _planeID = value;
                OnPropertyChanged("PlaneID");
            }
        }

        public int AirlineID
        {
            get => _airlineID;
            set
            {
                _airlineID = value;
                OnPropertyChanged("AirlineID");
            }
        }

        [RegularExpression(@"^[A-zА-я]+$", ErrorMessage = "The airline name must contain only letters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The airline name must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no airline name")]
        public string AirlineName
        {
            get => _airlineName;
            set
            {
                _airlineName = value;
                OnPropertyChanged("AirlineName");
            }
        }

        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The airplane model must contain only numbers, letters and hyphens")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The airplane model must contain from 3 to 20 characters")]
        [Required(ErrorMessage = "There is no airplane model")]
        public string AirplaneModel
        {
            get => _airplaneModel;
            set
            {
                _airplaneModel = value;
                OnPropertyChanged("AirplaneModel");
            }
        }

        [RegularExpression(@"^[A-zА-я-]+$", ErrorMessage = "The airport name must contain only letters and hyphens")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The airport name must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no airport name")]
        public string AirportName
        {
            get => _airportName;
            set
            {
                _airportName = value;
                OnPropertyChanged("AirportName");
            }
        }

        [RegularExpression(@"^[A-zА-я-]+$", ErrorMessage = "The city name must contain only letters and hyphens")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The city name must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no city name")]
        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged("CityName");
            }
        }

        [RegularExpression(@"^[A-zА-я-]+$", ErrorMessage = "The country name must contain only letters and hyphens")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The country name must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no country name")]
        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        public bool FlightType
        {
            get => _flightType;
            set
            {
                _flightType = value;
                OnPropertyChanged("FlightType");
            }
        }

        public DateTime DateTimeStart
        {
            get => _dateTimeStart;
            set
            {
                _dateTimeStart = value;
                OnPropertyChanged("DateTimeStart");
            }
        }

        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public DateTime DateTimeArrival
        {
            get => _dateTimeArrival;
            set
            {
                _dateTimeArrival = value;
                OnPropertyChanged("DateTimeArrival");
            }
        }

        public int NumOfFlights
        {
            get => _numOfFlights;
            set
            {
                _numOfFlights = value;
                OnPropertyChanged("NumOfFlights");
            }
        }

        public DateTime Periodicity
        {
            get => _periodicity;
            set
            {
                _periodicity = value;
                OnPropertyChanged("Periodicity");
            }
        }

        public decimal PriceEconom
        {
            get => _priceEconom;
            set
            {
                _priceEconom = value;
                OnPropertyChanged("PriceEconom");
            }
        }

        public decimal PriceBusiness
        {
            get => _priceBusiness;
            set
            {
                _priceBusiness = value;
                OnPropertyChanged("PriceBusiness");
            }
        }

        public decimal PriceFirst
        {
            get => _priceFirst;
            set
            {
                _priceFirst = value;
                OnPropertyChanged("PriceFirst");
            }
        }

        public DateTime DateTimeStartGMT
        {
            get => _dateTimeStartGMT;
            set
            {
                _dateTimeStartGMT = value;
                OnPropertyChanged("DateTimeStartGMT");
            }
        }

        public DateTime DateTimeArrivalGMT
        {
            get => _dateTimeArrivalGMT;
            set
            {
                _dateTimeArrivalGMT = value;
                OnPropertyChanged("DateTimeArrivalGMT");
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
    }
}
