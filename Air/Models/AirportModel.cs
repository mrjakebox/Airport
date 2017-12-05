using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class AirportModel:PropertyObservable
    {
        private int _airportID;
        private int _cityID;
        private string _airportName;
        private string _cityName;
        private string _countryName;

        public int AirportID
        {
            get => _airportID;
            set
            {
                _airportID = value;
                OnPropertyChanged("AirportID");
            }
        }

        public int CityID
        {
            get => _cityID;
            set
            {
                _cityID = value;
                OnPropertyChanged("CityID");
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
    }
}
