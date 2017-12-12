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
        private int _planeID;
        private int _airlineID;
        private string _airplaneModel;
        private string _onboardNumber;
        private string _airlineName;

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

        public string AirplaneModel
        {
            get => _airplaneModel;
            set
            {
                _airplaneModel = value;
                OnPropertyChanged("AirplaneModel");
            }
        }

        public string OnboardNumber
        {
            get => _onboardNumber;
            set
            {
                _onboardNumber = value;
                OnPropertyChanged("OnboardNumber");
            }
        }

        public string AirlineName
        {
            get => _airlineName;
            set
            {
                _airlineName = value;
                OnPropertyChanged("AirlineName");
            }
        }
    }
}
