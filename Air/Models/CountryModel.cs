using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class CountryModel : PropertyObservable
    {
        private int _countryID;
        private string _countryName;

        public int CountryID
        {
            get => _countryID;
            set
            {
                _countryID = value;
                OnPropertyChanged("CountryID");
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
