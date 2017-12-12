using Air.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для PlanesEdit.xaml
    /// </summary>
    public partial class PlanesEdit : UserControl
    {
        public PlanesEdit(PlaneModel plane, ObservableCollection<AirlineModel> airlineList)
        {
            InitializeComponent();
            Planes = plane;
            PlaneOld = plane;
            Data.DataContext = Planes;
            AirlineName.ItemsSource = airlineList;
            AirlineName.SelectedIndex = airlineList.IndexOf(airlineList.Where(i => i.AirlineID == plane.AirlineID).First());
            OldData.DataContext = PlaneOld;
            Title.Content = "UPDATE OF PLANE DATA";
        }

        public PlanesEdit(ObservableCollection<AirlineModel> airlineList)
        {
            InitializeComponent();
            Planes = new PlaneModel();
            Data.DataContext = Planes;
            AirlineName.ItemsSource = airlineList;
            AirlineName.SelectedIndex = 0;
            Title.Content = "CREATION OF PLANE DATA";
            OldData.Visibility = Visibility.Collapsed;
        }

        public PlaneModel PlaneOld { get; set; }
        private PlaneModel plane;
        public PlaneModel Planes
        {
            get => plane;
            set
            {
                plane = new PlaneModel
                {
                    PlaneID = value.PlaneID,
                    AirlineID = value.AirlineID,
                    AirplaneModel = value.AirplaneModel,
                    OnboardNumber = value.OnboardNumber,
                    AirlineName = value.AirlineName
                };
            }
        }

        private void AirlineName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            plane.AirlineID = (e.AddedItems[0] as AirlineModel).AirlineID;
        }
    }
}
