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
        private string _phone;
        private string _address;

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

        public string AirlineName
        {
            get => _airlineName;
            set
            {
                _airlineName = value;
                OnPropertyChanged("AirlineName");
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }
    }
}
