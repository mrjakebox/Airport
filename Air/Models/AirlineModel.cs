using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class AirlineModel: PropertyObservable
    {
        private int _airlineID;
        private string _airlineName;
        private string _airlinePhone;
        private string _airlineAddress;

        public AirlineModel() { }

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

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "The airline phone number must contain only numbers")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "The airline phone number must contain 12 characters")]
        [Required(ErrorMessage = "There is no airline phone number")]
        public string AirlinePhone
        {
            get => _airlinePhone;
            set
            {
                _airlinePhone = value;
                OnPropertyChanged("AirlinePhone");
            }
        }

        [RegularExpression(@"^[A-zА-я0-9]+\s*$", ErrorMessage = "The airline address must contain only letters and numbers")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The airline address can not contain more than 100 characters")]
        [Required(ErrorMessage = "There is no airline address")]
        public string AirlineAddress
        {
            get => _airlineAddress;
            set
            {
                _airlineAddress = value;
                OnPropertyChanged("AirlineAddress");
            }
        }
    }
}
