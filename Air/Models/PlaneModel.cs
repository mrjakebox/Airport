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
    public class PlaneModel : PropertyObservable
    {
        int _planeID;
        int _airlineID;
        string _airplaneModel;
        string _onboardNumber;
        string _airlineName;

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
                _planeID = value;
                OnPropertyChanged("AirlineID");
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


        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The onboard number must contain only numbers, letters and hyphens")]
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
    }
}
