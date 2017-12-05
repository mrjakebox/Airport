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
    public class PlanesModel : INotifyPropertyChanged
    {
        int _id;
        int _aircompanyID;
        string _planeModel;
        string _planeNumber;
        string _aircompanyName;

        public int PlaneID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("PlaneID");
            }
        }

        public int AircompanyID
        {
            get => _aircompanyID;
            set
            {
                _id = value;
                OnPropertyChanged("AircompanyID");
            }
        }

        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The airplane model must contain only numbers, letters and a hyphen")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The airplane model must contain from 3 to 20 characters")]
        [Required(ErrorMessage = "There is no airplane model")]
        public string PlaneModel
        {
            get => _planeModel;
            set
            {
                _planeModel = value;
                OnPropertyChanged("PlaneModel");
            }
        }


        [RegularExpression(@"^[A-zА-я0-9-]+$", ErrorMessage = "The onboard number must contain only numbers, letters and a hyphen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The onboard number must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no onboard number")]
        public string PlaneNumber
        {
            get => _planeNumber;
            set
            {
                _planeNumber = value;
                OnPropertyChanged("PlaneNumber");
            }
        }

        [RegularExpression(@"^[A-zА-я]+$", ErrorMessage = "The aircompany name must contain only letters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The aircompany name must contain from 3 to 50 characters")]
        [Required(ErrorMessage = "There is no aircompany name")]
        public string AircompanyName
        {
            get => _aircompanyName;
            set
            {
                _aircompanyName = value;
                OnPropertyChanged("AircompanyName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
