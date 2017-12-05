using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.Models
{
    class AirlineModel: PropertyObservable
    {
        private int _id;
        private string _airlineName;
        private string _airlinePhone;
        private string _airlineAddress;

        public int AirlineID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("AirlineID");
            }
        }
    }
}
