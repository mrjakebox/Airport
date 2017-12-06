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
    /// Логика взаимодействия для AirlinesEdit.xaml
    /// </summary>
    public partial class AirlinesEdit : UserControl
    {
        public AirlinesEdit(AirlineModel airline)
        {
            InitializeComponent();
            Airline = airline;
            Data.DataContext = Airline;      
        }

        private AirlineModel _airline;
        public AirlineModel Airline
        {
            get => _airline;
            set
            {
                _airline = new AirlineModel
                {
                    AirlineID = value.AirlineID,
                    AirlineAddress = value.AirlineAddress,
                    AirlineName = value.AirlineName,
                    AirlinePhone = value.AirlinePhone
                };
            }
        }
    }
}
