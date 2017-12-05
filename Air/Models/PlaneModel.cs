using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    public class PlaneModel : INotifyPropertyChanged
    {
        int _id;
        int _airlineID;
        string _airplaneModel;
        string _onboardNumber;
        string _airlineName;

        public int PlaneID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("PlaneID");
            }
        }

        public int AirlineID
        {
            get => _airlineID;
            set
            {
                _id = value;
                OnPropertyChanged("AirlineID");
            }
        }

        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The airplane model must contain only numbers, letters and a hyphen")]
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


        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The onboard number must contain only numbers, letters and a hyphen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The onboard number must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no onboard number")]
        public string OnboardNumber
        {
            get => _onboardNumber;
            set
            {
                _onboardNumber = value;
                OnPropertyChanged("OnboardNumber");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
