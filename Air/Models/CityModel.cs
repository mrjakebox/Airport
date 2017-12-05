using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class CityModel : PropertyObservable
    {
        private int _cityID;
        private int _countryID;
        private string _cityName;
        private string _countryName;
        private long _population;
        private DateTime _gmt;
        private string _signGMT;
        private string _stringGMT;

        public int CityID
        {
            get => _cityID;
            set
            {
                _cityID = value;
                OnPropertyChanged("CityID");
            }
        }

        public int CountryID
        {
            get => _countryID;
            set
            {
                _countryID = value;
                OnPropertyChanged("CountryID");
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

        public long Population
        {
            get => _population;
            set
            {
                _population = value;
                OnPropertyChanged("Population");
            }
        }

        public DateTime GMT
        {
            get => _gmt;
            set
            {
                _gmt = value;
                OnPropertyChanged("GMT");
            }
        }

        public string SignGMT
        {
            get => _signGMT;
            set
            {
                _signGMT = value;
                OnPropertyChanged("SignGMT");
            }
        }

        public string StringGMT
        {
            get => _stringGMT;
            set
            {
                _stringGMT = value;
                OnPropertyChanged("StringGMT");
            }
        }
    }
}
