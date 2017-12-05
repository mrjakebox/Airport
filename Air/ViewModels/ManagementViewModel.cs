using Air.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Air.ViewModels
{
    class ManagementViewModel : INotifyPropertyChanged
    {
        private Page _home;
        private Page _airlines;
        private Page _aircrafts;
        private Page _airports;
        private Page _countries;
        private Page _cities;
        private Page _schedule;

        private Page _currentPage;
        public Page CurrentPage
        {
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
            get => _currentPage;

        }

        private double _frameOpacity;
        public double FrameOpacity
        {
            set { _frameOpacity = value; OnPropertyChanged("FrameOpacity"); }
            get => _frameOpacity;
        }

        public ManagementViewModel()
        {
            _home = new Home();
            _airlines = new Airlines();
            _aircrafts = new Aircrafts();
            _airports = new Airports();
            _countries = new Countries();
            _cities = new Cities();
            _schedule = new Schedule();
            FrameOpacity = 1;
            CurrentPage = _home;
        }

        public ICommand HomeClick => new RelayCommand(obj => ShowOpacity(_home));
        public ICommand AirlinesClick => new RelayCommand(obj => ShowOpacity(_airlines));
        public ICommand AircraftsClick => new RelayCommand(obj => ShowOpacity(_aircrafts));
        public ICommand AirportsClick => new RelayCommand(obj => ShowOpacity(_airports));
        public ICommand CountriesClick => new RelayCommand(obj => ShowOpacity(_countries));
        public ICommand CitiesClick => new RelayCommand(obj => ShowOpacity(_cities));
        public ICommand ScheduleClick => new RelayCommand(obj => ShowOpacity(_schedule));


        private async void ShowOpacity(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1.0; i>0.0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
                CurrentPage = page;
                for (double i = 1.0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
