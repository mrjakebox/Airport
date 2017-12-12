using Air.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Air.Pages.Edit
{
    /// <summary>
    /// Логика взаимодействия для CountriesEdit.xaml
    /// </summary>
    public partial class CountriesEdit : UserControl
    {
        public CountriesEdit(CountryModel country)
        {
            InitializeComponent();
            Country = country;
            CountryOld = country;
            Data.DataContext = Country;
            OldData.DataContext = CountryOld;
            Title.Content = "UPDATE OF COUNTRY DATA";
        }

        public CountriesEdit()
        {
            InitializeComponent();
            Country = new CountryModel();
            Data.DataContext = Country;
            Title.Content = "CREATION OF COUNTRY DATA";
            OldData.Visibility = Visibility.Collapsed;
        }

        public CountryModel CountryOld { get; set; }
        private CountryModel _country;
        public CountryModel Country
        {
            get => _country;
            set
            {
                _country = new CountryModel
                {
                    CountryID = value.CountryID,
                    CountryName = value.CountryName
                };
            }
        }
    }
}
